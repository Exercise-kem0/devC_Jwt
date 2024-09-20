using AutoMapper;
using devC_Jwt.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using devC_Jwt.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace devC_Jwt.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuthenModel> RegisterAsync(RegisterModel regmodel)
        {
            if (await _userManager.FindByEmailAsync(regmodel.Email) != null)
                return new AuthenModel() { Message = "Email is already Registerd!" };
            if (await _userManager.FindByNameAsync(regmodel.UserName) is not null)
                return new AuthenModel() { Message = "User Name is already Registerd!" };

            var UserToRegister = _mapper.Map<ApplicationUser>(regmodel);
            var isRegisterdResult = await _userManager.CreateAsync(UserToRegister, regmodel.Password);
            if (!isRegisterdResult.Succeeded)
            {
                var RegErrors = string.Empty;
                foreach (var err in isRegisterdResult.Errors)
                {
                    RegErrors += $"{err.Description} | ";
                }
            }
            return new AuthenModel() { Message = "User Name is already Registerd!" };

            //if Registeration succeeded => add default role to registed user
            await _userManager.AddToRoleAsync(UserToRegister, "USER");
        }

        //-------------create JwtSecurityToken Method
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser appUser)
        { //cliams ==> key-value pairs  about a user embedded inside the JWT
            //1-get userClaims - roles
            //2-create of them roleClamis 
            //3-create complete claims from userClaims and roleClaims
            //4-symmerticSecuirtyKey => signingCredentials
            //5-final jwtSecuirtyToken =>claims/signingCredentials / isser / audience / expires

            var userClaims = await _userManager.GetClaimsAsync(appUser);
            var userRoles = await _userManager.GetRolesAsync(appUser);

            var roleClaims = new List<Claim>();
            foreach (var Urole in userRoles)
            {
                roleClaims.Add(new Claim("roles", Urole));
            }

            var finalClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), //Guid => Unique token ID 
                new Claim("uid",appUser.Id), //customizing of claim => User's unique ID
                new Claim(JwtRegisteredClaimNames.Sub,appUser.UserName), //unique identifier of the user
                new Claim(JwtRegisteredClaimNames.Email,appUser.Email)
            }
            .Union(userClaims)
            .Union(roleClaims);
            /*{
                  "jti": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
                  "uid": "123456",
                  "sub": "john_doe",
                  "email": "john.doe@example.com",
                  "roles": "Admin"
                  "roles": "User"
            }*/
            var symmtericSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var finalSignCredentials = new SigningCredentials(symmtericSecKey,SecurityAlgorithms.HmacSha256);

            var finalJwtSecurityToken = new JwtSecurityToken(
                 issuer: _jwt.Issuer,
                 audience:_jwt.Issuer,
                 expires:DateTime.Now.AddDays(_jwt.DurationInDays),
                 claims: finalClaims,
                  signingCredentials: finalSignCredentials
                );

            return finalJwtSecurityToken;
        }
    }
}
