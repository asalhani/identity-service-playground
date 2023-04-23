// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                // identity scope to read all needed user claims:
                new IdentityResource("inspection_profile", new[] {
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.Name,
                    JwtClaimTypes.GivenName,
                    JwtClaimTypes.FamilyName,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Role,
                    "inspection_center_id"})
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {

            };
        }
    }
}
