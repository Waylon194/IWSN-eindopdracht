// INCLUDES //

#include <ESP8266WiFi.h>
#include <PubSubClient.h>

// ATTRIBUTES //

// MQTT attributes
const char* ssid = "Uplink_Vincent";
const char* password = "UnifyVWJAP_23";
const char* mqtt_devicename = "LM35Arduino";
const char* mqtt_server = "plex.shitposts.nl";
const char* mqtt_user = "waylon";
const char* mqtt_pass = "waylon194";
const char* topic = "lm35Temp";

WiFiClient espClient;
PubSubClient client(mqtt_server, 1883, espClient);

// 
const int DELAY_TIME = 10000;

char data[5] = ""; // received string of XBee-nodes, ex.: 12.34

// METHODS //

void setup_wifi() {
  Serial.print((String)mqtt_devicename + " Attempting to connect to ");
  Serial.println(ssid);
  
  WiFi.persistent(false);
  
  WiFi.begin(ssid, password);
  Serial.print((String)mqtt_devicename + " Connecting");
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(1000);
  }
  Serial.println("");
  Serial.println((String)mqtt_devicename + " Connected to WiFi!");
  Serial.println((String)mqtt_devicename + " IP address: ");
  Serial.println(WiFi.localIP());
}

void setup_mqtt() {
  if (client.connect(mqtt_devicename, mqtt_user, mqtt_pass)) {
    client.setCallback(callback);
    client.publish(topic, "Connected!");
  }
  else {
    Serial.println((String)mqtt_devicename + " Attempting to reconnect in 5 seconds");
  }
}

void callback(char* topic, byte* payload, unsigned int length) {
  payload[length] = '\0';
  String message = (char*)payload;
}

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  setup_wifi();
  setup_mqtt();
}

void loop() {
  if (Serial.available()){                                         
   // length of '6' is required for the 5 characters (12345 => "24.14") + the escape character ('\0')    
   // which gets added at a later moment                            
    int n = Serial.readBytesUntil('\n', data, 6); 
    data[n-1]='\0'; // <- here the escape character gets added to ensure a proper char* type is build
    if(n){ // if valid input
      Serial.println(data);
      client.publish(topic, data);
    }
  }

  if (!client.connected()) {
    Serial.println((String)mqtt_devicename + " MQTT disconnected");
    setup_mqtt();
    delay(5000);
  }
  client.loop();

  delay(DELAY_TIME);    
}
