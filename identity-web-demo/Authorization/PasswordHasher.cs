using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using IdentityWebDemo.Entities;

namespace IdentityWebDemo.Authorization {

    public class PasswordHasher : IPasswordHasher<AppUser> {

        public string HashPassword(
            AppUser user,
            string password
        ) {
            return HashPassword(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(
            AppUser user,
            string hashedPassword,
            string providedPassword
        ) {
            var hashed = HashPassword(providedPassword);
            return VerifyHashed(hashed, hashedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }

        private string HashPassword(string password) {
            var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            var result = Convert.ToBase64String(hash);
            return result;
        }

        private bool VerifyHashed(string pass1, string pass2) {
            if (pass1.Length != pass2.Length) {
                return false;
            }
            for (int i = 0; i < pass1.Length; i++) {
                if (pass1[i] != pass2[i]) {
                    return false;
                }
            }
            return true;
        }
    }

}
