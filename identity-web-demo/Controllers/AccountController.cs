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
    [Route("api/account")]
    public class AccountController {

        private ILogger<AccountController> logger;
        private UserManager<AppUser> userManager;

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<AppUser> userManager
        ) {
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet("")]
        public string[] GetAll() {
            var usernames = from user in userManager.Users
                orderby user.CreateTime
                select user.UserName;
            return usernames.ToArray();
        }
    }

}
