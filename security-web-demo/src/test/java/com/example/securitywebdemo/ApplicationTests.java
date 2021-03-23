package com.example.securitywebdemo;

import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
class ApplicationTests {

    @Test
    void contextLoads() {
    }
    
    @Test
    void canEncodePassword() {
        var encoder = new Sha256PasswordEncoder();
        var encoded = encoder.encode("1a2b3c$D");
        assertNotNull(encoded);
        System.out.println(encoded);
        assertEquals("VHx1f1pM2bV/FV2bbuWvnbIcdumfLWtM7oeWPBKR6vM=", encoded);
    }

}
