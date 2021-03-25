package com.example.securitywebdemo;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.*;
import org.springframework.security.config.annotation.method.configuration.EnableGlobalMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.jdbc.JdbcDaoImpl;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.provisioning.InMemoryUserDetailsManager;
import org.springframework.security.provisioning.JdbcUserDetailsManager;

import javax.sql.DataSource;

@Configuration
@EnableWebSecurity()
public class WebSecurityConfig extends WebSecurityConfigurerAdapter {
    
    private DataSource dataSource;
    
    @Autowired
    @SuppressWarnings("SpringJavaInjectionPointsAutowiringInspection")
    public WebSecurityConfig(DataSource dataSource) {
        this.dataSource = dataSource;
        
    }

    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http.authorizeRequests()
            .antMatchers("/", "/home").permitAll()
            .anyRequest().authenticated()
            .and()
            .formLogin()
            .loginPage("/login")
            .permitAll()
            .and()
            .logout()
            .permitAll();
    }
    
    @Bean
    @Override
    protected UserDetailsService userDetailsService() {
        var jdbcDao = new JdbcDaoImpl();
        jdbcDao.setDataSource(dataSource);
        // disable user authorities
        jdbcDao.setEnableAuthorities(false);
        // enable groups
        jdbcDao.setEnableGroups(true);
        jdbcDao.setUsernameBasedPrimaryKey(false);
        jdbcDao.setUsersByUsernameQuery(
            "select user_name as username, password_hash as password, email_confirmed as enabled\n" + 
            "from public.aspnet_users\n" +
            "where normalized_user_name = upper(?);"
        );
        jdbcDao.setGroupAuthoritiesByUsernameQuery(
            "select r.id, r.name as group_name, rc.claim_value as authority\n" +
            "from public.aspnet_roles r\n" +
            "inner join public.aspnet_role_claims rc\n" +
            "    on rc.role_id = r.id and rc.claim_type = 'AppPrivilege'\n" +
            "inner join public.aspnet_user_roles ur on ur.role_id = r.id\n" +
            "inner join public.aspnet_users u on ur.user_id = u.id\n" +
            "    and u.normalized_user_name = upper(?);"
        );
        return jdbcDao;
    }
    
    @Bean
    public PasswordEncoder passwordEncoder() {
        return new Sha256PasswordEncoder();
    }
}
