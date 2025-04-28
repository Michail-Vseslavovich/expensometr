using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using user_service.infrastructure;
using user_service.domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Net.Http.Headers;
using user_service.application;

namespace user_service.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("/token")]
        public ActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
            issuer: AuthOpt.ISSUER,
                    audience: AuthOpt.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOpt.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOpt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(response);
        }



        [HttpPost]
        public ActionResult CreateNewUser(string name, string email, string password)
        {
            using (var db = new UserContext())
            {
                var user = db.users.FirstOrDefault(x => x.Name == name && x.Email == email);
                if (user == null)
                {
                    db.users.Add(new User { Name = name, Email = email, Password = password, Id = uint.Parse(DateTime.Now.Ticks.ToString()) });
                    db.SaveChanges();
                    return Ok();
                }
                return BadRequest("Такой пользователь уже существует");
            }
        }


        [HttpPut]
        public ActionResult UpdateUserStats(User user)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            int UIserId = JwtDecodeService.GetJwtId(accessToken);
            using (var db = new UserContext())
            {
                var UserToUpd = db.users.FirstOrDefault(u => u.Id == user.Id);
                if (UserToUpd != null)
                {
                    UserToUpd.Email = user.Email;
                    UserToUpd.Password = user.Password;
                    UserToUpd.Name = user.Name;
                    db.users.Update(UserToUpd);
                    db.SaveChanges();
                    return Ok();
                }
                return BadRequest("Такого пользователя нет");
            }
        }



        //Полный шварцмашинен
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            using (var db = new UserContext())
            {
                User? person = db.users.FirstOrDefault(x => x.Name == username && x.Password == password);
                if (person != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, person.Name),
                        new Claim("ID", person.Id.ToString())
                    };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }

                // если пользователя не найдено
                return null;
            }
        }

        
    }
}
    

