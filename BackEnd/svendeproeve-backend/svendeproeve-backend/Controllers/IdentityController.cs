using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using svendeproeve_backend.Data;
using svendeproeve_backend.Models;
using svendeproeve_backend.Models.User;

namespace svendeproeve_backend.Controllers
{
    /// Login controller for userlogin.
    [Area("Identity")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class IdentityController : ControllerBase
    {
        /// Sign manager for app user

        private readonly SignInManager<AppUser> signInManager;

        /// Usermanager for app user

        private readonly UserManager<AppUser> userManager;

        /// <summary>
        /// Rolemanager to mage the userroles.
        /// </summary>

        private readonly RoleManager<AppRole> roleManager;

        /// Json app configuration

        private readonly IConfiguration configuration;

        private readonly Databasedcontext databasedcontext;

        /// Constroller with usermanager, ilogger and signinmanager.
        
        public IdentityController(Databasedcontext databasedcontext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.databasedcontext = databasedcontext;
        }

        /// Login for user.
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromForm] UserSignIndDto dto)
        {
            // Check for is all data is set.
            if (!ModelState.IsValid)
                return StatusCode(400, "Email or passwordd is missing!");

            // Make new instanze of appuser.
            AppUser appUser = await userManager.FindByIdAsync(dto.Email).ConfigureAwait(false);

            if (appUser is null)
                return BadRequest("User not exsists");

            // Login with user data and password and get result of signin manager.
            var result = await signInManager.CheckPasswordSignInAsync(appUser, dto.Password, true).ConfigureAwait(false);

            // If signin is a success.
            if (result.Succeeded)
            {
                var expies = DateTime.Now.AddMinutes(1);
                var accessToken = GenerateAccessToken(appUser, expies);
                string refeshToken = string.Empty;

                var appRefreshToken = await databasedcontext.AppRefreshTokens.FirstOrDefaultAsync(i => i.AppUserId.Equals(appUser.Id));

                if (appRefreshToken is null)
                {
                    refeshToken = GenerateRefreshToken();
                    databasedcontext.AppRefreshTokens.Add(new AppRefreshToken { AppUserId = appUser.Id, RefreshToken = refeshToken });
                    await databasedcontext.SaveChangesAsync();
                }
                else
                    refeshToken = appRefreshToken.RefreshToken;

                var roles = await userManager.GetRolesAsync(appUser);

                return new JsonResult(new LoginResultDto { Roles = roles, AccessToken = accessToken, Expires = expies, RefreshToken = refeshToken });
            }

            // Requere a two factor authcation code.
            if (result.RequiresTwoFactor)
                return StatusCode(401, "You need a twofactor authcation code");

            //if sign faliur.
            return StatusCode(401, "Email or password was not corect.");
        }

        //signup a user.
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto dto)
        {
            //Check for is all userdata is set.
            if (!ModelState.IsValid)
                return StatusCode(400, "Some userdata is missing.");

            //Make new user object from.
            AppUser appUser = new AppUser { UserName = dto.UserName, Email = dto.Email };

            // Make user by usermanager and get result.
            var result = await userManager.CreateAsync(appUser, dto.Password);

            // If user is created.
            if (result.Succeeded)
                return StatusCode(201);

            return StatusCode(400, "Wrong userdata or user exits.");
        }

        /// Get new token by refreshtoken.
        /// <param name="refreshtoken"></param>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string refreshtoken)
        {
             var appRefreshToken = databasedcontext.AppRefreshTokens.Include(i => i.AppUser).FirstOrDefault(i => i.RefreshToken.Equals(refreshtoken));

            if (appRefreshToken is null)
                return StatusCode(401, "Refreshtoken is not valid.");

            var expires = DateTime.Now.AddMinutes(1);
            var accessToken = GenerateAccessToken(appRefreshToken.AppUser, expires);

            var roles = await userManager.GetRolesAsync(appRefreshToken.AppUser);

            return new JsonResult(new RefreshTokenResultDto { Roles = roles, Accesstoken = accessToken, Expires = expires });
        }

        ///Delete a user by Id.
        ///<param name="id"></param>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            //AppUser bu Id.
            AppUser appUser = await userManager.FindByIdAsync(id);

            //Delete user by appuser.
            await userManager.DeleteAsync(appUser);

            //Return status 200
            return Ok();
        }

        // TODO: Make dto class for this.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult>ChangePassword(string id, [FromBody] UserChangePasswordDto dto)
        {
            // Get the appuser by id.
            AppUser appUser = await userManager.FindByIdAsync(id);

            // Check for is user exits.
            if (appUser is null)
                return NotFound("User not found");

            // Set the new password for app user.
            appUser.PasswordHash = userManager.PasswordHasher.HashPassword(appUser, dto.Password);
            await userManager.UpdateAsync(appUser);

            return Ok();
        }

        /// Change user by id and userdata
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult>EditUser(string id, [FromBody] UserChangeDto dto)
        {
            // Check for is data is valid.
            if (!ModelState.IsValid)
                return BadRequest("Data not valid.");

            // Get the user by id;
            AppUser appUser = await userManager.FindByIdAsync(id);

            // Change the user data from dto class.
            appUser.Email = dto.Email;
            appUser.UserName = dto.UserName;

            // Change user and get result.
            var result = await userManager.UpdateAsync(appUser);

            // User change a success.
            if (result.Succeeded)
                return Ok();

            // Some thing work with chaning af the user data.
            return BadRequest("Wrong userdata.");
        }

        /// Get user by id.
        /// <param name="id"></param>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult>GetUser(string id)
        {
            //Get user by id.
            AppUser appUser = await userManager.FindByIdAsync(id);

            if (appUser is null)
                return NotFound("User not found.");

            // Manek init new get user dto class obj and return it as json by defult api controller repsonse.
            return new JsonResult(new GetUserDto { Id = appUser.Id, UserName = appUser.UserName, Email = appUser.Email });
        }

        ///Get User data from auticatet user.
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var claims = User.Claims;

            if (claims is null)
                return NotFound("Could not find your user data.");

            var user = new GetUserDto { Id = claims.FirstOrDefault(i => i.Type.Equals("Id")).Value, Email = claims.FirstOrDefault(i => i.Type.Equals("Email")).Value, UserName = claims.FirstOrDefault(i => i.Type.Equals("UserName")).Value };

            return new JsonResult(user);
        }

        // Get all users
        [HttpGet]
        [Authorize]
        public IQueryable<GetUserDto> GetUsers()
        {
            return from u in userManager.Users select new GetUserDto { Id = u.Id, Email = u.Email, UserName = u.UserName };
        }

        /// Get roles infor from a specifik role.
        /// <param name="role"></param>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult>GetRole(string role)
        {
            var appRole = await roleManager.FindByNameAsync(role);

            if (appRole is null)
                return NotFound("Role not exsits");

            return new JsonResult(role);
        }

        /// Create the role.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult>CreateRole(string role)
        {
            if (await roleManager.RoleExistsAsync(role))
                return BadRequest("Role exsists.");

            AppRole appRole = new AppRole { Name = role };

            var result = await roleManager.CreateAsync(appRole);

            if (!result.Succeeded)
                return BadRequest();

            return Ok();
        }

        /// Delete role.
        /// <param name="id"></param>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult>DeleteRole(string id)
        {
            AppRole appRole = await roleManager.FindByIdAsync(id);

            var result = await roleManager.DeleteAsync(appRole);

            if (result.Succeeded)
                return Ok();

            return BadRequest();
        }

        /// Reomve a role from user.
        /// <param name="id"></param>
        /// <param name="role"></param>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult>RemoveRole(string id, string role)
        {
            AppUser appUser = await userManager.FindByIdAsync(id);

            var result = await userManager.RemoveFromRoleAsync(appUser, role);

            if (result.Succeeded)
                return Ok();

            return BadRequest("Fail to remove the role from user.");
        }

        /// Add a role to user.
        /// <param name="userid"></param>
        /// <param name="role"></param>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRole(string id, string role)
        {
            // Check for is a parameter is missing.
            if (role is null || id is null)
                return BadRequest("Some parameters is missing");
            // Check for is role exsists.
            if (!await roleManager.RoleExistsAsync(role))
                return BadRequest("Role not exists.");

            //Get user by id
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound("User not found.");

            if (await userManager.IsInRoleAsync(user, role))
                return BadRequest($"User are in role { role}");

            var result = await userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                return BadRequest("Fail to add the role to user.");

            return CreatedAtAction(nameof(GetRole), role, "Role created.");
        }


        /// Check for is authcated user has a specifik role
        /// <param name="role"></param>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> HasRole(string role)
        {
            var userid = User.Claims.FirstOrDefault(i => i.Type.Equals("Id")).Value;

            var appUser = await userManager.FindByIdAsync(userid);

            var hasRole = await userManager.IsInRoleAsync(appUser, role);

            return new JsonResult(hasRole);
        }

        /// Set roles to a user.
        /// <param name="userid"></param>
        /// <param name="dtos"></param>
        [Authorize]
        [HttpPut("{userid}")]
        public async Task<IActionResult> SetRole(String userid, [FromBody] ICollection<GetUserRolesDto> dtos)
        {
            //Check for is data is valid.
            if (!ModelState.IsValid)
                return BadRequest("Data not valied.");

            var appUser = await userManager.FindByIdAsync(userid);
            var appUserRoles = await userManager.GetRolesAsync(appUser);

            var result = await userManager.RemoveFromRolesAsync(appUser, dtos.Where(i => !i.HasRole && appUserRoles.Any(e => e.Contains(i.Name))).Select(i => i.Name));

            if (!result.Succeeded)
                return StatusCode(500, "fail to remove roles");

            result = await userManager.AddToRolesAsync(appUser, dtos.Where(i => i.HasRole && !appUserRoles.Any(e => e.Contains(i.Name))).Select(i => i.Name));

            if (result.Succeeded)
                return BadRequest("Fail to add roles.");

            return Ok();
        }

        /// Get list of roles the user have.
        /// <param name="userid"></param>
        [Authorize]
        [HttpGet("{userid}")]
        public async Task<IActionResult>GetRoles(string userid)
        {
            var appUser = await userManager.FindByIdAsync(userid);

            var appUserRoles = await userManager.GetRolesAsync(appUser);

            var dtos = from role in roleManager.Roles select new GetUserRolesDto { Name = role.Name, HasRole = appUserRoles.Contains(role.Name) };

            return new JsonResult(dtos);
        }

        /// Get all roles
        [HttpGet]
        [Authorize]
        public IQueryable<GetRolesDto> GetRoles()
        {
            return from role in roleManager.Roles select new GetRolesDto { Id = role.Id, Name = role.Name };
        }

        /// Generate a refresstoken for appuser.
        /// /// <param name="appUser"></param>
        private string GenerateRefreshToken()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string refreshToken = string.Empty;

            Random ran = new Random();
            for (int i = 0; i < 64; i++)
                refreshToken += chars[ran.Next(0, (chars.Length - 1))];

            return refreshToken;
        }

        /// Generate a access token for a user.
        private string GenerateAccessToken(AppUser appUser, DateTime expires)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("Id", appUser.Id),
                        new Claim("UserName", appUser.UserName),
                        new Claim("Email",appUser.Email)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
