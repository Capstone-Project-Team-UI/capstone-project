package com.capstone.cloudcontrol.remotepccontrol.usermanagement;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.web.bind.annotation.*;
import jakarta.validation.Valid;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/users")
public class UserManagementController {

    // Logger instance for logging
    private static final Logger logger = LoggerFactory.getLogger(UserManagementController.class);

    // Autowired JdbcTemplate for direct database interaction
    @Autowired
    private JdbcTemplate jdbcTemplate;

    // GET endpoint: Return all users from the database
    @GetMapping
    public List<user> getAllusers() {
        logger.info("Received request to fetch all users from the database");

        // Query to fetch all users from the Users table
        List<user> users = jdbcTemplate.query(
            "SELECT * FROM Users",
            (rs, rowNum) -> new user(
                rs.getString("userID"),
                rs.getString("organization"),
                rs.getString("serialNumber"),
                rs.getString("uniqueID"),
                rs.getString("emailAddress")
            )
        );
        logger.info("Fetched {} users from the database", users.size());
        return users;
    }

    // POST endpoint: Add a new user after verifying serial number and organization exist
    @PostMapping
    public String adduser(@Valid @RequestBody user newUser) {
        logger.info("Received request to add a new user with ID: {}", newUser.userID());

        // Query to check if the serial number and organization exist in the VerifiedUsers table
        String query = "SELECT COUNT(*) FROM VerifiedUsers WHERE registeredDevice = ? AND organization = ?";
        int count = jdbcTemplate.queryForObject(query, new Object[]{newUser.serialNumber(), newUser.organization()}, Integer.class);

        if (count > 0) {
            logger.info("Serial number and organization exist. Proceeding to add the user.");
            // If the serial number and organization exist, insert the new user into the Users table
            String insertQuery = "INSERT INTO Users (userID, organization, serialNumber, uniqueID, emailAddress) VALUES (?, ?, ?, ?, ?)";
            jdbcTemplate.update(insertQuery, newUser.userID(), newUser.organization(), newUser.serialNumber(), newUser.uniqueID(), newUser.emailAddress());
            logger.info("User with ID: {} added successfully.", newUser.userID());
            return "User added successfully!";
        } else {
            logger.warn("Error: Serial Number and Organization must exist in the VerifiedUsers table for user with ID: {}", newUser.userID());
            return "Error: Serial Number and Organization must exist in the VerifiedUsers table!";
        }
    }

    // PUT endpoint: Update an existing user
    @PutMapping("/{userID}")
    public String updateuser(@PathVariable String userID, @Valid @RequestBody user updatedUser) {
        logger.info("Received request to update user with ID: {}", userID);

        // Query to check if the user exists in the Users table
        String query = "SELECT COUNT(*) FROM Users WHERE userID = ?";
        int count = jdbcTemplate.queryForObject(query, new Object[]{userID}, Integer.class);

        if (count > 0) {
            logger.info("User with ID: {} found. Proceeding to update.", userID);
            // If user exists, update the user's information in the Users table
            String updateQuery = "UPDATE Users SET organization = ?, serialNumber = ?, uniqueID = ?, emailAddress = ? WHERE userID = ?";
            jdbcTemplate.update(updateQuery, updatedUser.organization(), updatedUser.serialNumber(), updatedUser.uniqueID(), updatedUser.emailAddress(), userID);
            logger.info("User with ID: {} updated successfully.", userID);
            return "User updated successfully!";
        } else {
            logger.warn("User with ID: {} not found in the database.", userID);
            return "User not found!";
        }
    }
}
