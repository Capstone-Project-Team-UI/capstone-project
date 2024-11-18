// src/main/java/com/example/webrtc/SignalMessage.java
package com.example.webrtc;

public class SignalMessage {
    private String type;
    private String sdp;

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getSdp() {
        return sdp;
    }

    public void setSdp(String sdp) {
        this.sdp = sdp;
    }
}
