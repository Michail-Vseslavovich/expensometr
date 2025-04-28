using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace user_service.domain
{
    public class AuthOpt
    {

        public const string ISSUER = "bezdarnost"; 
        public const string AUDIENCE = "ia"; 
        const string KEY = "ochenbezobasnikluch";   
        public const int LIFETIME = 10; //Долгожитель
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }


    }
}
