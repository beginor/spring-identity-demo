package com.example.shirowebdemo;

import org.apache.shiro.codec.Hex;
import org.apache.shiro.crypto.hash.Sha256Hash;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest
class ApplicationTests {

    @Test
    public void contextLoads() {
    }
    
    @Test
    public void hashPassword() {
        var hash = new Sha256Hash("1a2b3c4D");
        var bytes = hash.getBytes();
        var hex = new String(Hex.encode(bytes));
        System.out.println(hex);
        var base64 = hash.toBase64();
        System.out.println(base64);
    }

}
