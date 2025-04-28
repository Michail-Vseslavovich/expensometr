using System.IdentityModel.Tokens.Jwt;

namespace user_service.application
{
    public class JwtDecodeService
    {
        public static JwtSecurityToken Decode(string EncodedJwt)
        {
            var token = "[encoded jwt]";
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken;
        }
        public static int GetJwtId(string EncodedJwt)
        {
            try
            {
                JwtSecurityToken token = JwtDecodeService.Decode(EncodedJwt);
                int.TryParse(token.Claims.First(p => p.Type == "ID").Value, out int userId);
                return userId;
            }
            catch (Exception ex)
            {
                return 0;
                // за это мне дадут по шапке
            }

        }

        public static string? GetJwtClaim(string EncodedJwt, string ClaimName)
        {
            JwtSecurityToken token = JwtDecodeService.Decode(EncodedJwt);
            return token.Claims.FirstOrDefault(p => p.Type == ClaimName)?.ToString();
        }
    }
}
