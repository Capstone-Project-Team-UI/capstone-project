package com.capstone.cloudcontrol.remotepccontrol.login;

import io.jsonwebtoken.*;
import jakarta.servlet.http.Cookie;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.Date;


@RestController
@RequestMapping("/")
public class login {

    private final String SECRET = "secret_key";

    public static void main(String[] args) {
        SpringApplication.run(login.class, args);
    }

    // --- Hardcoded User ---
    static class User {
        private Long id;
        private String username;
        private String email;
        private String password;
        private String company;

        public User(Long id, String username, String email, String password, String company) {
            this.id = id;
            this.username = username;
            this.email = email;
            this.password = password;
            this.company = company;
        }

        public Long getId() { return id; }
        public String getUsername() { return username; }
        public String getEmail() { return email; }
        public String getPassword() { return password; }
        public String getCompany() { return company; }
    }

    // --- DTO ---
    static class LoginRequest {
        private String username;
        private String password;

        public String getUsername() { return username; }
        public void setUsername(String username) { this.username = username; }
        public String getPassword() { return password; }
        public void setPassword(String password) { this.password = password; }
    }

    // --- JWT Utilities ---
    private String generateToken(Long userId, String company) {
        return Jwts.builder()
                .setSubject(userId + "," + company)
                .setIssuedAt(new Date())
                .setExpiration(new Date(System.currentTimeMillis() + 86400000))
                .signWith(SignatureAlgorithm.HS256, SECRET)
                .compact();
    }

    private Long extractUserId(String token) {
        String subject = Jwts.parser().setSigningKey(SECRET).parseClaimsJws(token).getBody().getSubject();
        return Long.parseLong(subject.split(",")[0]);
    }

    private String extractCompany(String token) {
        String subject = Jwts.parser().setSigningKey(SECRET).parseClaimsJws(token).getBody().getSubject();
        return subject.split(",")[1];
    }

    private final User hardcodedUser = new User(1L, "rory", "rory@trit.com", "password123", "Team Remote IT");

    @GetMapping("/login")
    public ResponseEntity<String> loginInfo() {
        return ResponseEntity.ok("Send POST request to /login with JSON body containing username and password.");
    }

    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody LoginRequest request, HttpServletResponse response) {
        if (!request.getUsername().equals(hardcodedUser.getUsername()) ||
            !request.getPassword().equals(hardcodedUser.getPassword())) {
            return ResponseEntity.status(401).body("Invalid credentials");
        }

        String token = generateToken(hardcodedUser.getId(), hardcodedUser.getCompany());
        Cookie cookie = new Cookie("token", token);
        cookie.setHttpOnly(true);
        cookie.setPath("/");
        cookie.setMaxAge(86400);
        response.addCookie(cookie);

        return ResponseEntity.ok(hardcodedUser);
    }

    @GetMapping("/me")
    public ResponseEntity<?> getUserFromToken(@CookieValue("token") String token) {
        Long userId = extractUserId(token);
        String company = extractCompany(token);

        if (!userId.equals(hardcodedUser.getId()) || !company.equals(hardcodedUser.getCompany())) {
            return ResponseEntity.status(403).body("Unauthorized");
        }

        return ResponseEntity.ok(hardcodedUser);
    }

    @PostMapping("/logout")
    public ResponseEntity<?> logout(HttpServletResponse response) {
        Cookie cookie = new Cookie("token", null);
        cookie.setHttpOnly(true);
        cookie.setPath("/");
        cookie.setMaxAge(0);
        response.addCookie(cookie);
        return ResponseEntity.ok("Logged out");
    }
} 
