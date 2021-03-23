package com.example.securitywebdemo;

import org.springframework.security.crypto.password.PasswordEncoder;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.util.Base64;

public class Sha256PasswordEncoder implements PasswordEncoder {

    @Override
    public String encode(CharSequence rawPassword) {
        try {
            var digest = MessageDigest.getInstance("SHA-256");
            var hash = digest.digest(rawPassword.toString().getBytes(StandardCharsets.UTF_8));
            var result = Base64.getEncoder().encodeToString(hash);
            return result;
        }
        catch (Exception ex) {
            return null;
        }
    }

    @Override
    public boolean matches(CharSequence rawPassword, String encodedPassword) {
        var encoded = encode(rawPassword);
        if (encoded == null) {
            return false;
        }
        return encoded.equals(encodedPassword);
    }
}
