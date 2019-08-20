using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using VSAND.Data;
using VSAND.Frontend.Filters;
using VSAND.FrontEnd.Filters;

namespace VSAND.Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<VsandContext>(options =>
            {
                options.UseSqlServer(Configuration["DefaultConnection"]);
            });

            services.AddTransient<VSAND.Data.Repositories.IUnitOfWork, VSAND.Data.Repositories.UnitOfWork>();

            // adding SignalR
            services.AddSignalR()
                .AddStackExchangeRedis(Configuration["RedisUrl"], options =>
                {
                    options.Configuration.Password = Configuration["RedisPassword"];
                    options.Configuration.ChannelPrefix = Configuration["ENV"];
                });

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
            services.AddTransient<Services.Email.IEmailSender, Services.Email.EmailSender>();

            // Add services from VSAND.Services
            services.AddTransient<Services.Data.CMS.IConfigService, Services.Data.CMS.ConfigService>();
            services.AddTransient<Services.Data.GameReports.IGameReportService, Services.Data.GameReports.GameReportService>();
            services.AddTransient<Services.Data.Manage.ScheduleYears.IScheduleYearService, Services.Data.Manage.ScheduleYears.ScheduleYearService>();
            services.AddTransient<Services.Data.Schools.ISchoolService, Services.Data.Schools.SchoolService>();
            services.AddTransient<Services.Data.Sports.ISportService, Services.Data.Sports.SportService>();
            services.AddTransient<Services.SlugRouting.ISlugRouting, Services.SlugRouting.SlugRouting>();
            services.AddTransient<Services.Data.Teams.ITeamService, Services.Data.Teams.TeamService>();
            services.AddTransient<Services.Data.Players.IPlayerService, Services.Data.Players.PlayerService>();

            // Setup Stat Aggregation Services
            services.AddTransient<Services.StatAgg.IPlayerStatAggregation, Services.StatAgg.PlayerStatAggregation>();
            services.AddTransient<Services.StatAgg.ITeamStatAggregation, Services.StatAgg.TeamStatAggregation>();
            services.AddTransient<Services.Display.Sports.ISportsDisplayService, Services.Display.Sports.SportsDisplayService>();

            // adding Arc News services
            services.AddTransient<Services.News.INewsService, Services.News.NewsService>();

            services.AddMvc(o =>
            {
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                await next();
            });

            // Best explanation for how to hook in the custom rewriting middleware
            // https://stackoverflow.com/questions/36179304/dynamic-url-rewriting-with-mvc-and-asp-net-core
            app.UseMiddleware<SlugRewritingMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                // TODO: map to something like /Hubs/Provisioning so we don't conflict with other areas / controllers
                // routes.MapHub<ProvisioningHub>("/ProvisioningHub");
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
