package com.example.shirowebdemo.api;

import com.example.shirowebdemo.models.LoginInfo;
import org.apache.shiro.SecurityUtils;
import org.apache.shiro.authc.AuthenticationException;
import org.apache.shiro.authc.UsernamePasswordToken;
import org.springframework.beans.factory.annotation.*;
import org.springframework.web.bind.annotation.*;

@RestController()
@RequestMapping("/api/account")
public class AccountController {
    
    @Autowired
    public AccountController() { }
    
    @GetMapping("/info")
    public String getInfo() {
        var user = SecurityUtils.getSubject();
        return user.isAuthenticated()
            ? user.getPrincipal().toString()
            : "anonymous";
    }
    
    @PostMapping("/login")
    public String login(
        @RequestBody LoginInfo info
    ) {
        try {
            var user = SecurityUtils.getSubject();
            var token = new UsernamePasswordToken(
                info.getUsername(),
                info.getPassword(),
                info.isRememberMe()
            );
            user.login(token);
            return user.getPrincipal().toString();
        }
        catch (AuthenticationException ex) {
            return ex.getMessage();
        }
    }
}
