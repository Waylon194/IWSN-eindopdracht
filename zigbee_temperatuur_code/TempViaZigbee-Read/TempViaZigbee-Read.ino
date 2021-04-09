// INCLUDES //

#include <ESP8266WiFi.h>
#include <PubSubClient.h>

// ATTRIBUTES //

// MQTT attributes
const char* ssid = "VdR";
const char* password = "vwjapderooij";
const char* mqtt_devicename = "LM35Arduino";
const char* mqtt_server = "plex.shitposts.nl";
const char* mqtt_user = "waylon";
const char* mqtt_pass = "waylon194";
const char* topic = "lm35Temp";

WiFiClient espClient;
PubSubClient client(mqtt_server, 1883, espClient);

// 
const int DELAY_TIME = 10000;

char data[8] = ""; // received string of XBee-nodes, ex.: 1,123
int n = 0;

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
    n = Serial.readBytesUntil('\n',data, 8);
    data[n-1]='\0';
    if(n){ // if valid input
      Serial.println(data);
      client.publish(topic, (char*) data);
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
