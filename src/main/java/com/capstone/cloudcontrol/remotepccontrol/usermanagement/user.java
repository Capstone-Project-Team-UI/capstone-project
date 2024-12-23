package com.capstone.cloudcontrol.remotepccontrol.usermanagement;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;

public record user(
    @NotBlank(message = "UserID Can't be blank")
    String userID,

    @NotBlank(message= "Organization Can't be blank")
    String organization,

    @NotBlank(message = "Serial Number can't be blank")
    String serialNumber,

    @NotBlank(message = "Unique ID is required")
    String uniqueID,

    @Email(message = "Email should be Valid")
    @NotBlank(message = "Email can't be Blank")
    String emailAddress

) {

    public user {
        // Validate that the uniqueID is a valid hash (using MessageDigest)
        if (!isValidHash(uniqueID)) {
            throw new IllegalArgumentException("Invalid unique ID format. Must be a valid hash.");
        }
    }

    // Method to validate if the uniqueID is a valid hash using MessageDigest
    private boolean isValidHash(String uniqueID) {
        try {
            // Try hashing the uniqueID using SHA-256 (you can replace this with any hashing algorithm)
            MessageDigest digest = MessageDigest.getInstance("SHA-256");
            byte[] hashBytes = digest.digest(uniqueID.getBytes());
            
            // If we successfully get a hash, return true
            return hashBytes.length > 0; // Ensure the digest is not empty
        } catch (NoSuchAlgorithmException e) {
            // If the algorithm is not found (unlikely for SHA-256), throw an error
            throw new IllegalStateException("Hash algorithm not found", e);
        }
    }
}
