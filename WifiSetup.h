#ifndef WifiSetup_h
#define WifiSetup_h

#include "application.h"

void WifiSetup() {
  int bytes = Serial.available();
  if (!bytes) {
    Serial.print(".");
    return;
  }

  int auth = Serial.read();
  Serial.read(); // empty bit
  String ssid = Serial.readStringUntil('\0');
  Serial.println(ssid);Serial.println();
  String password = Serial.readStringUntil('\0');
  Serial.println(password);Serial.println();
  Serial.println("Connecting");

  Spark.disconnect();
  WiFi.clearCredentials();   // if you only want one set of credentials stored
  WiFi.setCredentials(ssid, password, auth);

  Spark.connect();

  while(WiFi.connecting()) {
    Spark.process();
  }

  if (Spark.connected()) {
    Serial.println("Connected");
  } else {
    Serial.println("Could not connect");
  }
}

#endif
