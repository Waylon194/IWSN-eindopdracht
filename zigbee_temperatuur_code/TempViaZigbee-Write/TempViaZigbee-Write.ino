const int lm35DataPin = A1;

const int DELAY_TIME = 2500;

void setup()
{
  Serial.begin(9600);
}

void loop()
{
  int rawvoltage = analogRead(lm35DataPin);
  //Serial.println(rawvoltage);
  
  float millivolts = (rawvoltage / 1024.0) * 5000;
  float celsius = millivolts / 10;
  Serial.println(celsius);
  delay(DELAY_TIME);
}
