﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InsternShip.Data.Policies
{
    public class AuthorizationClaimCustom : TypeFilterAttribute
    {
        public AuthorizationClaimCustom(string claimValue) : base(typeof(AuthorizeClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(string.Empty, claimValue) };
        }
    }
}
