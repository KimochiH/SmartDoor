/* Sweep
 by BARRAGAN <http://barraganstudio.com>
 This example code is in the public domain.

 modified 8 Nov 2013
 by Scott Fitzgerald
 http://www.arduino.cc/en/Tutorial/Sweep
*/

#include <Servo.h>
#include <SPI.h>
#include <Ethernet.h>

Servo myservo;  // create servo object to control a servo
// twelve servo objects can be created on most boards
bool isOpen = false;
bool isOn = false;
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED  };
char server[] = "smartdoorapi20160609040037.azurewebsites.net";
char inString[32]; // string for incoming serial data
int stringPos = 0; // string index counter
boolean startRead = false; // is reading?

IPAddress ip(192, 168, 207, 200);

EthernetClient client;

void setupEthernet() {
  Serial.begin(9600);
    //Serial.println("ROK");
    Ethernet.begin(mac, ip);
  
  delay(3000);
} 

void mocua(){
    myservo.write(90);
 }

 void dongcua(){
      myservo.write(0);
}



String httpRequest() {
  
    Serial.println("KOK");  
    if (client.connect(server, 80)) {
      Serial.println("connecting...");    
      client.println("GET /api/data HTTP/1.0");
      client.println("Host: smartdoorapi20160609040037.azurewebsites.net");    
      client.println("Connection: close");
      client.println();
      Serial.println(readPage()); 
      return readPage();
        client.stop();     
    } else {    
      Serial.println("connection failed");
    }
}

String readPage()
{
    stringPos = 0;
    memset( &inString, 0, 32 ); //clear inString memory

    while(true){
    if (client.available()) {
      char c = client.read();
      if (c == '<' ) { //'<' is our begining character
          startRead = true; //Ready to start reading the part 
          }else if(startRead){
              if(c != '>'){ //'>' is our ending character
                inString[stringPos] = c;
                stringPos ++;
              }else{          
                startRead = false;
                client.stop();
                client.flush();
                return inString;
              }
          }
    }
  }
}

void setup() {
  myservo.attach(9);  // attaches the servo on pin 9 to the servo object
  
  setupEthernet();
}

void loop() {

String dataReceive;
    dataReceive = httpRequest();
Serial.println("test");
    if (dataReceive == "0")
      isOn = false;
    if (dataReceive == "1")
      isOn = true;
    Serial.println(isOn);
    
    delay(1000);
   
  
    if (isOpen && isOn )
    {
     mocua();
       delay(1500);
      isOpen = false;
     }
     else
     {
     

        dongcua();
      delay(1500);
      isOpen = true; 
     }
      
}

