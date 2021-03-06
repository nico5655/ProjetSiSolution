/*
 Name:		TestSketch.ino
 Created:	15/10/2018 13:06:33
 Author:	Megaport
*/

#include <AccelStepper.h>
#include <Adafruit_MotorShield.h>
#include <Wire.h>
#include <Servo.h>

const uint8_t aimSpeed = 10;//vitesse en RPM du aimControlStepper
const uint8_t leftStepType = DOUBLE;
const uint8_t rightStepType = DOUBLE;


Adafruit_MotorShield AFMStop = Adafruit_MotorShield();
Adafruit_StepperMotor *leftShoutStepper = AFMStop.getStepper(200, 1);
Adafruit_DCMotor *myMotor = AFMStop.getMotor(3);
Adafruit_DCMotor *myMotor1 = AFMStop.getMotor(4);
Servo servo = Servo();

volatile byte half_revolutions;
double rps;
unsigned long timeold;
void magnet_detect()//This function is called whenever a magnet/interrupt is detected by the arduino
{
	half_revolutions++;
	Serial.println("detect");
}

// the setup function runs once when you press reset or power the board
void setup() {
	//AFMSbot.begin();
	AFMStop.begin();
	Serial.begin(9600);
	//servo.attach(10);
	//TWBR = ((F_CPU / 400000l) - 16) / 2;
	leftShoutStepper->setSpeed(6);
}
int value = 50;
int previous = 0;
// the loop function runs over and over again until power down or reset
void loop() {
	//Serial.println("turning begin");
	leftShoutStepper->step(50, FORWARD, DOUBLE);
	//servo.write(90);
	delay(1000);
	leftShoutStepper->step(50, BACKWARD, DOUBLE);
	//servo.write(0);
	delay(1000);
	/*float t1 = millis();
	leftShoutStepper->step(20000, FORWARD, DOUBLE);
	float t2 = millis();
	float t = (t2 - t1) / 1000;
	float rps = (100 / t);
	Serial.println("Turning ended in " + String(t) + "s, speed: " + String(rps) + "rps");
	delay(5000);*/
	/*if (millis() - timeold >= 1000
	{
		rps = (half_revolutions / ((millis() - timeold) / 1000.0));
		Serial.println(String(value) + " ;" + String(rps));
		timeold = millis();
		half_revolutions = 0;
	}*/
}
