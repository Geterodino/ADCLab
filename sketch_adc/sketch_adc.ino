#include <Wire.h>
#include <Adafruit_ADS1015.h>
Adafruit_ADS1115 ads; 

int trash;
 int16_t a;
 int16_t b;
 int serial;
 
 byte GetNS(int iNum, int iRadix = 10)
{
    byte nCount = 0;
    do
    {
        nCount++;
        iNum = iNum / iRadix;
    }while (0 != iNum);
    return nCount;
}
void CrashWrite(int16_t num, byte count  )
{
  
  for( byte i=0; i<count;i++)
  {
    Serial.write(num%10);
    num=num/10;
  }
}
void DataOut( int16_t adc0=0,int16_t adc1=0,int16_t adc2=0,int16_t adc3=0)
{
         byte count;
         count=GetNS(adc0);
         Serial.write(count);
         CrashWrite(adc0, count);
         count=GetNS(adc1);
         Serial.write(count);
         CrashWrite(adc1, count);
         count=GetNS(adc2);
         Serial.write(count);
         CrashWrite(adc2, count);
         count=GetNS(adc3);
         Serial.write(count);
         CrashWrite(adc3, count);
       
         a=millis();
         count=GetNS(a-b);
         Serial.write(count);
         CrashWrite(a-b, count);
     
  }
void setup() {
  // put your setup code here, to run once:
  Serial.begin(38400);

   ads.setGain(GAIN_TWOTHIRDS);  // 2/3x gain +/- 6.144V  1 bit = 0.1875mV (default)
   //ads.setGain(GAIN_ONE);        // 1x gain   +/- 4.096V  1 bit = 0.125mV
   //ads.setGain(GAIN_TWO);          // 2x gain   +/- 2.048V  1 bit = 0.0625mV
  // ads.setGain(GAIN_FOUR);       // 4x gain   +/- 1.024V  1 bit = 0.03125mV
  // ads.setGain(GAIN_EIGHT);      // 8x gain   +/- 0.512V  1 bit = 0.015625mV
  // ads.setGain(GAIN_SIXTEEN);    // 16x gain  +/- 0.256V  1 bit = 0.0078125mV
 
  ads.begin();

}
int16_t adc0=0, adc1=0, adc2=0, adc3=0;
int state=0;
void loop() {
  // put your main code here, to run repeatedly:
  
 
  
   if( Serial.available()>0 ){ 
   serial= Serial.read()-'0';
   /*trash=Serial.read();
   trash=0;*/
   
   switch(serial)
   {
    case 0:
    Serial.println("ADCv2.0");
    break;
    case 1:
     state = 0;
    while(state<1)
    {
      
      b=millis();
      //delay(50);
      adc0 = ads.readADC_SingleEnded(0);
      //adc1 = ads.readADC_SingleEnded(1);
      //adc2 = ads.readADC_SingleEnded(2);
      //adc3 = ads.readADC_SingleEnded(3);

       DataOut(adc0);
     state= Serial.read()-'0';
    }
    break;
    
    case 2:
    state = 0;
    while(state<1)
    {
      
      b=millis();
      //delay(50);
      adc0 = ads.readADC_SingleEnded(0);
      adc1 = ads.readADC_SingleEnded(1);
      //adc2 = ads.readADC_SingleEnded(2);
      //adc3 = ads.readADC_SingleEnded(3);

        DataOut(adc0,adc1);
     state= Serial.read()-'0';
    }
    break;
    
    case 3:
    state = 0;
   
      while(state<1)
    {
      
      
      b=millis();
      //delay(50);
      adc0 = ads.readADC_SingleEnded(0);
      adc1 = ads.readADC_SingleEnded(1);
      adc2 = ads.readADC_SingleEnded(2);
      //adc3 = ads.readADC_SingleEnded(3);

        DataOut(adc0,adc1,adc2);
     state= Serial.read()-'0';
    }
    
    break;
    
    case 4:
    state = 0;
     while(state<1)
    {
      
      
      b=millis();
      //delay(50);
      adc0 = ads.readADC_SingleEnded(0);
      adc1 = ads.readADC_SingleEnded(1);
      adc2 = ads.readADC_SingleEnded(2);
      adc3 = ads.readADC_SingleEnded(3);

        DataOut(adc0,adc1,adc2,adc3);
     state= Serial.read()-'0';
    }
    break;
   
    }
    }
 
//   Serial.flush();
   
   
  }
  
