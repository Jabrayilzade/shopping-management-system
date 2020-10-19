using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingDB.Helpers
{
    public class Roles
    {
        public const string Admin = "admin";
        public const string Superadmin = "superadmin";
        private const string Manager = "manager";
        private const string Moderator = "moderator";
        private const string Advisor = "advisor";
        public const string Customer = "customer";

        public static AuthorizationPolicy AdminRole()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }

        public static AuthorizationPolicy SuperadminRole()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Superadmin).Build();
        }

        public static AuthorizationPolicy ManagerRole()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Manager).Build();
        }

        public static AuthorizationPolicy ModeratorRole()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Moderator).Build();
        }

        public static AuthorizationPolicy AdvisorRole()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Advisor).Build();
        }

        public static AuthorizationPolicy CustomerRole()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Customer).Build();
        }
    }
}
