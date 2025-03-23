
char byf = ' ';

void setup() {
  pinMode(22, OUTPUT);
  digitalWrite(22, LOW);


  Serial3.begin(9600);
  Serial2.begin(9600);
  Serial.begin(9600);

}

void loop() {
  if (Serial.available())
    if (Serial.read() == '@'){
      digitalWrite(22, HIGH);
      delay(3000);
      digitalWrite(22, LOW);
    }
  

  if (Serial2.available()){
    byf = Serial2.read();

    if (byf == 2) 
      Serial.print("+");
    else if (byf == 3) {
        Serial.print(";");
        delay(1000);
        while(Serial2.available())
          byf = Serial2.read();
      }
    else 
      Serial.print((char)byf);
  }

  if (Serial3.available()){
    byf = Serial3.read();

    if (byf == 2) 
      Serial.print("+");
    else if (byf == 3) {
        Serial.print(";");
        delay(1000);
        while(Serial3.available())
          byf = Serial3.read();
      }
    else 
      Serial.print((char)byf);    
  }

}