using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VSAND.Backend.Filters;
using VSAND.Backend.Policies.Handlers;
using VSAND.Backend.Policies.Requirements;
using VSAND.Data;
using VSAND.Data.Identity;
using VSAND.Services.Email;
using VSAND.Services.Hubs;

namespace VSAND.Backend
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddDbContext<VsandContext>(options => options.UseSqlServer(Configuration["DefaultConnection"]));

            services.AddTransient<VSAND.Data.Repositories.IUnitOfWork, VSAND.Data.Repositories.UnitOfWork>();

            // adding SignalR
            services.AddSignalR()
                .AddStackExchangeRedis(Configuration["RedisUrl"], options =>
                {
                    options.Configuration.Password = Configuration["RedisPassword"];
                    options.Configuration.ChannelPrefix = Configuration["ENV"];
                });

            // adding Hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration["HangfireDbConnection"]));

            if (Configuration["ENV"].Equals("Dev", StringComparison.OrdinalIgnoreCase))
            {
                // adding in-memory cache
                services.AddTransient<Services.Cache.ICache, Services.Cache.InMemoryCache>();
            }
            else
            {
                // adding Redis cache
                services.AddTransient<Services.Cache.ICache, Services.Cache.RedisCache>();
            }

            // adding SMTP service
            services.AddTransient<IEmailSender, EmailSender>();

            // adding our proxied services
            services.AddTransient<Services.Data.IAuditService, Services.Data.AuditService>();
            services.AddTransient<Services.Data.CMS.IConfigService, Services.Data.CMS.ConfigService>();
            services.AddTransient<Services.Data.Manage.Conferences.IConferenceService, Services.Data.Manage.Conferences.ConferenceService>();
            services.AddTransient<Services.Data.Manage.Counties.ICountyService, Services.Data.Manage.Counties.CountyService>();
            services.AddTransient<Services.Data.GameReports.IGameReportService, Services.Data.GameReports.GameReportService>();
            services.AddTransient<Services.Data.Players.IPlayerService, Services.Data.Players.PlayerService>();
            services.AddTransient<Services.Data.Manage.Publications.IPublicationService, Services.Data.Manage.Publications.PublicationService>();
            services.AddTransient<Services.Data.Manage.ScheduleYears.IScheduleYearService, Services.Data.Manage.ScheduleYears.ScheduleYearService>();
            services.AddTransient<Services.Data.Schools.ISchoolService, Services.Data.Schools.SchoolService>();
            services.AddTransient<Services.Data.Sports.ISportService, Services.Data.Sports.SportService>();
            services.AddTransient<Services.Data.Manage.States.IStateService, Services.Data.Manage.States.StateService>();
            services.AddTransient<Services.Data.Teams.ITeamService, Services.Data.Teams.TeamService>();
            services.AddTransient<Services.Data.Manage.Users.IUserService, Services.Data.Manage.Users.UserService>();

            // File Services (Excel, PDF, etc)

            /// DinkToPdf 
            /// Copy native library to root folder of your project. 
            /// From there .NET Core loads native library when native method is called with P/Invoke. 
            /// You can find latest version of native library https://github.com/rdvojmoc/DinkToPdf/tree/master/v0.12.4. 
            /// Select appropriate library for your OS and platform (64 or 32 bit).
            /// Now, wire in as a Singleton
            /// 

            services.AddSingleton(typeof(DinkToPdf.Contracts.IConverter), new DinkToPdf.SynchronizedConverter(new DinkToPdf.PdfTools()));

            services.AddTransient<Services.Files.IFileService, Services.Files.FileService>();
            services.AddTransient<Services.Files.IExcelService, Services.Files.ExcelService>();
            services.AddTransient<Services.Files.IPDFService, Services.Files.PDFService>();
            services.AddTransient<Services.Razor.IRazorViewRenderer, Services.Razor.RazorViewRenderer>();

            // Setup Stat Aggregation Services
            services.AddTransient<Services.StatAgg.IPlayerStatAggregation, Services.StatAgg.PlayerStatAggregation>();
            services.AddTransient<Services.StatAgg.ITeamStatAggregation, Services.StatAgg.TeamStatAggregation>();

            // adding integration services
            services.AddTransient<Services.Integrations.LocalLive.ILocalLiveService, Services.Integrations.LocalLive.LocalLiveService>();

            // adding news services
            services.AddTransient<Services.News.INewsService, Services.News.NewsService>();

            // adding calculation services
            services.AddTransient<Services.Data.Calculations.IPowerPointService, Services.Data.Calculations.PowerPointService>();

            // adding custom policy handlers
            services.AddTransient<IAuthorizationHandler, PermissionHandler>();

            services.AddDefaultIdentity<ApplicationUser>()
                    .AddEntityFrameworkStores<VsandContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logoff";
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

                o.AddPolicy("ApiPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Role, "Access.Api");
                    policy.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme };
                });

                // add custom policies for Appx User Roles
                o.AddPolicy("Admin", policy => policy.RequireClaim("Admin", "true"));
                o.AddPolicy("WriterOrEditor", policy => policy.Requirements.Add(new AnyRoleRequirement(new List<string> { "VSAND.Writer..", "VSAND.Editor.." })));
                o.AddPolicy("EditorOrWorksheet", policy => policy.Requirements.Add(new AnyRoleRequirement(new List<string> { "VSAND.Editor..", "VSAND.Worksheet.." })));
                o.AddPolicy("AdminOrReports", policy => policy.Requirements.Add(new AnyRoleRequirement(new List<string> { "Admin", "VSAND.Report.." })));
                o.AddPolicy("AdminOrCompliance", policy => policy.Requirements.Add(new AnyRoleRequirement(new List<string> { "Admin", "VSAND.Report.Compliance" })));
                o.AddPolicy("EditorOrDataQuality", policy => policy.Requirements.Add(new AnyRoleRequirement(new List<string> { "VSAND.Editor..", "VSAND.Report.DataQuality" })));
                o.AddPolicy("AdminOrGoverningBody", policy => policy.Requirements.Add(new AnyRoleRequirement(new List<string> { "UserFunction.Admin", "UserFunction.GoverningBody" })));

                // query for all Appx User Roles and add a policy for each
                using (var sp = services.BuildServiceProvider())
                {
                    using (var context = sp.GetRequiredService<VsandContext>())
                    {
                        var roles = context.AppxUserRole.Select(r => r.RoleCat + "." + r.RoleName).ToList();
                        foreach (string role in roles)
                        {
                            o.AddPolicy(role, policy => policy.RequireClaim(role, "true"));
                        }
                    }
                }
            });

            services.AddMvc(o =>
            {
                o.Conventions.Add(new VSAND.Backend.Policies.AddAuthorizeFiltersControllerConvention());
                o.Filters.Add<SchoolMasterAccountFilter>();
                o.Filters.Add<SetViewDataFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next();
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24 * 365; // 1 year in cache
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
                }
            });

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // here you can see we make sure it doesn't start with /api, if it does, it'll 404 within .NET if it can't be found
            app.MapWhen(x => x.Request.Path.Value.StartsWith("/game/report", StringComparison.OrdinalIgnoreCase), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                        name: "GameReportSpa",
                        defaults: new { controller = "Game", action = "Report" });
                });
            });


            app.UseSignalR(routes =>
            {
                // TODO: map to something like /Hubs/Provisioning so we don't conflict with other areas / controllers
                routes.MapHub<ProvisioningHub>("/ProvisioningHub");
                routes.MapHub<SchedulingHub>("/SchedulingHub");
            });

            // adding Hangfire
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
            };
        }
    }
}
