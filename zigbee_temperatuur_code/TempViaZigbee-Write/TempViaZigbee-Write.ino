const int lm35DataPin = A1;

void setup()
{
  Serial.begin(9600);
}

void loop()
{
  int rawvoltage = analogRead(lm35DataPin);
  float millivolts = (rawvoltage / 1024.0) * 5000;
  float celsius = millivolts / 10;
  Serial.println(celsius);
  delay(2000);
}
