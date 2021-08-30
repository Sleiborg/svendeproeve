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

        private readonly RoleManager<AppUser> roleManager;

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
            }

        }

    }
}
