package com.capstone.cloudcontrol.remotepccontrol.dashcontrols;

import org.springframework.web.bind.annotation.*;
import java.io.BufferedReader;
import java.io.File;
import java.io.InputStreamReader;

@RestController
@RequestMapping("/api")
public class DashController {

    // Base directory for DASH CLI
    private static final String DASH_CLI_DIR = "C:\\Program Files\\DASH CLI 7.0\\bin";

    // Endpoint to check the system status
    @GetMapping("/check_status")
    public String checkStatus() {
        return executeDashCommand();
    }

    // Endpoint to start KVM (or any system command for testing)
    @PostMapping("/start_kvm")
    public String startKvm() {
        return executeDashCommand();
    }

    // Method to execute the DASH CLI command
    private String executeDashCommand() {
        try {
            // Use ProcessBuilder to set working directory and execute the command
            ProcessBuilder processBuilder = new ProcessBuilder("cmd.exe", "/c", ".\\dashcli.exe version");
            processBuilder.directory(new File(DASH_CLI_DIR)); // Set working directory
            Process process = processBuilder.start();

            // Read the output
            BufferedReader reader = new BufferedReader(new InputStreamReader(process.getInputStream()));
            StringBuilder output = new StringBuilder();
            String line;
            while ((line = reader.readLine()) != null) {
                output.append(line).append("\n");
            }

            // Read errors, if any
            BufferedReader errorReader = new BufferedReader(new InputStreamReader(process.getErrorStream()));
            StringBuilder errorOutput = new StringBuilder();
            while ((line = errorReader.readLine()) != null) {
                errorOutput.append(line).append("\n");
            }

            int exitCode = process.waitFor();
            if (exitCode == 0) {
                return "Command executed successfully:\n" + output.toString();
            } else {
                return "Command execution failed with exit code " + exitCode + "\nError Output:\n" + errorOutput.toString();
            }
        } catch (Exception e) {
            return "Error executing command: " + e.getMessage();
        }
    }
}
