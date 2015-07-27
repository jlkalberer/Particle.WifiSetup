# Particle.WifiSetup

This is a windows app used to change your WiFi settings over a usb.

Here is a sample on getting this to work.

```c
#include "application.h"
#include "WifiSetup.h"

void setup(void) {
    Serial.begin(115200);

    WifiSetup();
}

void loop(void) {
  WifiSetup();

  // TODO
}
```

Then plug your Spark Core or Photon into USB and run the WPF app.

***This requires the Spark Core or Photon drivers to work***
In the future I will attempt to integrate installing the drivers into this app.