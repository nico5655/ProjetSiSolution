/*
 Name:		PingPongSketch.ino
 Created:	01/10/2018 11:09:01
 Author:	Megaport
*/
#include <Adafruit_MotorShield.h>
#include <Wire.h>
#include <Servo.h>

const double r = 5;
double speed = 0;
double length = 14;//default length
double angle = 90;//default downAngle
const double k = 4.574e-2;//N.m/A
const double Umax = 11.09;//V
const double I = 0.180;//A
const double R = 18.5;//Ohms
int ballDropperPin = 9;
int bottomAnglePin = 10;
Servo ballDropper;
Servo bottomAngle;
Adafruit_MotorShield AFMS = Adafruit_MotorShield();
Adafruit_DCMotor *leftMotor = AFMS.getMotor(3);
Adafruit_DCMotor *rightMotor = AFMS.getMotor(4);
Adafruit_StepperMotor *tigeStepper = AFMS.getStepper(200, 1);

uint16_t getSteps(double length)
{
	return length;
}

uint8_t toPmw(double speed)
{
	double rds = (speed / r);
	double alpha = (R*I + k * rds) / Umax;
	return (uint8_t)(alpha * 255);
}

void start() {
	Serial.println("starting");
	leftMotor->run(BACKWARD);
	rightMotor->run(BACKWARD);
	delay(50);
	Serial.println("done");
}

void stop() {
	Serial.println("stopping");
	leftMotor->run(RELEASE);
	rightMotor->run(RELEASE);
	delay(50);
	Serial.println("done");
}

void dropBall() {
	Serial.println("dropping ball");
	ballDropper.write(90);
	delay(1000);//time for the ball to go
	ballDropper.write(0);
	Serial.println("done");
}

void setSpeed(double value) {
	if (speed != value)
	{
		uint8_t pmw = toPmw(value);
		Serial.println("Setting speed to " + String(pmw));
		leftMotor->setSpeed(pmw);
		rightMotor->setSpeed(pmw);
		speed = value;
		delay(100);
	}
	Serial.println("done");
}

void setLength(double value) {//longueur tige filetée
	if (length != value)
	{
		uint16_t steps = getSteps(value - length);
		length = value;
		uint8_t direction = FORWARD;
		if (steps < 0)
			direction = BACKWARD;
		steps = abs(steps);
		Serial.println("Setting length to " + String(value));
		tigeStepper->step(steps, direction, DOUBLE);
		delay(5000);
	}
	Serial.println("done");
}

void setAngle(double value) {
	if (angle != value)
	{
		Serial.println("Setting angle to " + String(value));
		angle = value;
		bottomAngle.write(value);
		delay(2000);
	}
	Serial.println("done");
}

void setup() {
	Serial.begin(9600);
	AFMS.begin();
	tigeStepper->setSpeed(30);
	ballDropper.attach(ballDropperPin);
	bottomAngle.attach(bottomAnglePin);
	//init operations
}

void loop() {
	if (Serial.available()) {

		String message = Serial.readStringUntil('=');
		double value = Serial.readStringUntil(';').toDouble();
		message.replace("\r", "");
		message.replace("\n", " ");
		message.replace("\0", "");
		message.replace("\t", "");
		message.replace(" ", "");
		if (message != "")
		{
			Serial.println("received");
			if (message == "speed")
			{
				setSpeed(value);
			}
			else if (message == "angle")
			{
				setAngle(value);
			}
			else if (message == "length")
			{
				setLength(value);//x calculated from bAngle
			}
			else if (message == "start")
			{
				if (value == 1)
					start();
				else
					stop();
			}
			else if (message == "fire")
			{
				dropBall();
			}
			else
			{
				Serial.println("\"" + message + "\" not recognized.");
				Serial.println("done");
			}
		}
	}
}
