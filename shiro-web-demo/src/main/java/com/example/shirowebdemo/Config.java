package com.example.shirowebdemo;

import javax.sql.DataSource;

import org.apache.shiro.authc.credential.HashedCredentialsMatcher;
import org.apache.shiro.realm.jdbc.JdbcRealm;
import org.apache.shiro.spring.web.config.DefaultShiroFilterChainDefinition;
import org.apache.shiro.spring.web.config.ShiroFilterChainDefinition;
import org.springframework.context.annotation.*;
import org.apache.shiro.realm.Realm;

@Configuration
public class ShiroConfig {
    
    @SuppressWarnings("SpringJavaInjectionPointsAutowiringInspection")
    @Bean
    public Realm realm(DataSource dataSource) {
        var realm = new JdbcRealm();
        realm.setDataSource(dataSource);
        realm.setAuthenticationQuery(
            "select password_hash as password " +
                "from public.aspnet_users where user_name = ?"
        );
        realm.setUserRolesQuery(
            "select r.name as role_name from public.aspnet_roles r " +
                "inner join public.aspnet_user_roles ur on ur.role_id = r.id " +
                "inner join public.aspnet_users u on u.id = ur.user_id " +
                "where u.normalized_user_name = upper(?)"
        );
        realm.setPermissionsLookupEnabled(true);
        realm.setPermissionsQuery(
            "select claim_value as permission from public.aspnet_role_claims rc " +
                "inner join public.aspnet_roles r on r.id = rc.role_id " +
                "and rc.claim_type = 'AppPrivilege' and r.name = ?"
        );
        var matcher = new HashedCredentialsMatcher();
        matcher.setHashAlgorithmName("SHA-256");
        matcher.setStoredCredentialsHexEncoded(false);
        realm.setCredentialsMatcher(matcher);
        return realm;
    }

    @Bean
    public ShiroFilterChainDefinition shiroFilterChainDefinition() {
        var chainDef = new DefaultShiroFilterChainDefinition();
        // chainDef.addPathDefinition("/**", "authc");
        return chainDef;
    }
}
