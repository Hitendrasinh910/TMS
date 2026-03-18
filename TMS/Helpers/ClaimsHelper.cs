using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TMS.Helpers
{
    public static class ClaimsHelper
    {
        public static int GetCurrentUserId(this ClaimsPrincipal user)
        {
            var idClaim =
                user.FindFirst(ClaimTypes.NameIdentifier) ??
                user.FindFirst("UserId");

            if (idClaim == null || !int.TryParse(idClaim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid or missing UserId claim");

            return userId;
        }

        public static string GetCurrentUserName(this ClaimsPrincipal user)
        {
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
                return string.Empty;

            return user.FindFirst(ClaimTypes.Name)?.Value
                ?? user.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value
                ?? string.Empty;
        }

        public static string GetCurrentUserFullName(this ClaimsPrincipal user)
        {
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
                return string.Empty;

            return user.FindFirst("FullName")?.Value ?? string.Empty;
        }

        public static string GetCurrentUserRole(this ClaimsPrincipal user)
        {
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
                return string.Empty;

            return user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        public static bool IsInRole(this ClaimsPrincipal user, string role)
        {
            return user?.IsInRole(role) ?? false;
        }
        public static int GetCurrentCompanyId(this ClaimsPrincipal user)
        {
            var claim = user?.FindFirst("IDCompany");

            if (claim == null || !int.TryParse(claim.Value, out int companyId))
                throw new UnauthorizedAccessException("Invalid or missing IDCompany claim");

            return companyId;
        }
        public static int GetCurrentLocationId(this ClaimsPrincipal user)
        {
            var claim = user?.FindFirst("IDLocation");

            if (claim == null || !int.TryParse(claim.Value, out int locationId))
                throw new UnauthorizedAccessException("Invalid or missing IDLocation claim");

            return locationId;
        }
    }
}
