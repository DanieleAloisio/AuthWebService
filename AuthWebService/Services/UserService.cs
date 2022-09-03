using AuthWebService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthWebService.Services
{

    public class UserService : IUserService
    {
        private readonly SketchArtDbContext _sketchArtDbContext;
        private readonly AppSettings _appSettings;
        public UserService(SketchArtDbContext sketchArtDbContext, IOptions<AppSettings> appSettings)
        {
            _sketchArtDbContext = sketchArtDbContext;
            _appSettings = appSettings.Value;
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            User utente = await this.GetUser(username);

            if (utente != null)
            {
                if (utente.Password == password)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<User> GetUser(string username)
        {
            return await this._sketchArtDbContext.Users
                .Where(c => c.Username == username)
                .Include(r => r.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetToken(string username)
        {
            User utente = await this.GetUser(username);

            //Creazione del Token Jwt
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            Role userRole = utente.Role;

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, utente.Username));


            claims.Add(new Claim(ClaimTypes.Role, userRole.RoleName));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                //Validità del Token
                Expires = DateTime.UtcNow.AddSeconds(_appSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
