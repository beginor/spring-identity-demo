using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityWebDemo.Entities;

namespace IdentityWebDemo.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class AccountController {

        private UserManager<AppUser> userManager;

        public AccountController(UserManager<AppUser> userManager) {
            this.userManager = userManager;
        }

        [HttpGet("")]
        public string[] GetAll() {
            var usernames = from user in userManager.Users
                select user.UserName;
            return usernames.ToArray();
        }
    }

}
