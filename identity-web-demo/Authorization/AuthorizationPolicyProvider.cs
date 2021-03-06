using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace IdentityWebDemo.Authorization {

    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider {

        public AuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options
        ) : base(options) { }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName) {
            var policy = new AuthorizationPolicyBuilder();
            policy.RequireClaim(Consts.PrivilegeClaimType, policyName);
            return Task.FromResult(policy.Build());
        }

    }

}
