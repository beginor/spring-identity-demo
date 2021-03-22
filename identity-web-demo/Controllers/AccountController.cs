using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityWebDemo.Entities;
using IdentityWebDemo.Models;

namespace IdentityWebDemo.Controllers {

    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase {

        private ILogger<AccountController> logger;
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
        ) {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("")]
        public async Task<ActionResult> Test() {
            var user = await userManager.FindByNameAsync("admin");
            if (user == null) {
                user = new AppUser {
                    UserName = "admin",
                    Email = "admin@local.com",
                    EmailConfirmed = true,
                    PhoneNumber = "13400000000",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true
                };
                await userManager.CreateAsync(user);
                await userManager.AddPasswordAsync(user, "1a2b3c$D");
            }
            else {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, token, "1a2b3c$D");
            }
            return Ok(user.UserName);
        }

        [HttpGet("info")]
        public string GetInfo() {
            return User.Identity.IsAuthenticated
                ? User.Identity.Name
                : "anonymous";
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginModel model) {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null) {
                return NotFound("Not found!");
            }
            var isValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (isValid) {
                await signInManager.SignInAsync(user, model.RememberMe);
                return Ok(model.Username);
            }
            return "Invalid User!";
        }
    }

}
