// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace VSAND.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // OpenID Connect hybrid flow client (MVC)
                new Client
                {
                    ClientId = "vsand.backend",
                    ClientName = "VSAND Web Backend",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("2484D86C575C572645599428700A6897BAF42AE4D31290AD3AE665A48ED596E6".Sha256())
                    },

                    RedirectUris =
                    {
                        "https://localhost:44362/account/logincallback",
                        "https://identity.uat.njschoolsports.com/account/logincallback"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44362",
                        "https://identity.uat.njschoolsports.com"
                    },
                    LogoUri = "https://localhost:44362/images/NJaffiliatedSL-logo-260x52.jpg",

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },

                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },

                // Resource Owner Authentication (username and password)
                new Client
                {
                    ClientId = "vsand.backend.ro",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("63E46F9A8F68F04C86B5F44EE280D6C3BA866DBAC1CC3D049C17AD63E65FF6AB".Sha256())
                    }
                }
            };
        }
    }
}