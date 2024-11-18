// src/main/java/com/example/webrtc/SignalController.java
package com.example.webrtc;

import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/device")
public class SignalController {

    @PostMapping("/connect")
    public String connectDevice(@RequestBody DeviceInfo deviceInfo) {
        System.out.println("Device connected with IP: " + deviceInfo.getIp() + 
                           " and Network Info: " + deviceInfo.getNetworkInfo());
        return "Device connected";
    }

    @PostMapping("/sdp")
    public String receiveSdp(@RequestBody SignalMessage signalMessage) {
        System.out.println("Received SDP Type: " + signalMessage.getType());
        System.out.println("Received SDP: " + signalMessage.getSdp());
        return "SDP Received";
    }

    @PostMapping("/ice")
    public String receiveIceCandidate(@RequestBody IceCandidate iceCandidate) {
        System.out.println("Received ICE Candidate: " + iceCandidate.getCandidate());
        return "ICE Candidate Received";
    }
}
