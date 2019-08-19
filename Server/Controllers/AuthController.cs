using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;

        private readonly ApplicationDbContext _context;

        public object SecuritAlgorithms { get; private set; }

        public AuthController( UserManager<ApplicationUser> manager)
        {
            userManager = manager;
    }


        [HttpGet]
        public string[] GetUsers()
        {
            return userManager.Users.Select(s=>s.UserName).ToArray();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user =await  userManager.FindByNameAsync(model.username);
            if (user!=null && await userManager.CheckPasswordAsync(user,model.password))
            {
                var claims = new[]
                {
                    new Claim (JwtRegisteredClaimNames.Sub,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                };

                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));
                var token = new JwtSecurityToken(
                    issuer: "http://xo.rs", audience: "http://xo.rs", expires:DateTime.UtcNow.AddMonths(1),
                    claims:claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signinKey,SecurityAlgorithms.HmacSha256));
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    userName = user.UserName,
                    expiration = token.ValidTo,
                    plants = user.AccesiblePlants,
                    locations = user.AccesibleLocations,
                    auth = user.Authority,
                }) ;
            }

            return Unauthorized();
         
        }


    }
}