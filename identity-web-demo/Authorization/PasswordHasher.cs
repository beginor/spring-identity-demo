using Microsoft.AspNetCore.Identity;
using IdentityWebDemo.Entities;

namespace IdentityWebDemo.Authorization {

    public class PasswordHasher : IPasswordHasher<AppUser> {

        public string HashPassword(
            AppUser user,
            string password
        ) {
            throw new System.NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(
            AppUser user,
            string hashedPassword,
            string providedPassword
        ) {
            throw new System.NotImplementedException();
        }
    }

}
