using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Clinic.Api.Security;

public sealed class RolesClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
        {
            return Task.FromResult(principal);
        }

        if (identity.Claims.Any(c => c.Type == ClaimTypes.Role))
        {
            return Task.FromResult(principal);
        }

        foreach (var claim in identity.FindAll("roles"))
        {
            if (string.IsNullOrWhiteSpace(claim.Value)) continue;

            foreach (var role in claim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }

        return Task.FromResult(principal);
    }
}
