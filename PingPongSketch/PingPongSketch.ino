/*
 Name:		PingPongSketch.ino
 Created:	01/10/2018 11:09:01
 Author:	Megaport
*/
#include <MultiStepper.h>
#include <AccelStepper.h>
#include <Adafruit_MotorShield.h>
#include <Wire.h>
#include <Servo.h>


const double r = 5;
double speed = 0;
double length = 26.2;//25.6 and 28.8 default length
double angle = 90;//default downAngle
const double k = 4.574e-2;//N.m/A
const double Umax = 11.09;//V
const double I = 0.180;//A
const double R = 18.5;//Ohms
int ballDropperPin = 9;
Servo ballDropper;
Adafruit_MotorShield AFMS = Adafruit_MotorShield(0x60);
Adafruit_MotorShield AFMSBot = Adafruit_MotorShield(0x61);
Adafruit_DCMotor *leftMotor = AFMS.getMotor(3);
Adafruit_DCMotor *rightMotor = AFMS.getMotor(4);
Adafruit_StepperMotor *tigeStepper = AFMS.getStepper(200, 1);
Adafruit_StepperMotor *bottomStepper = AFMSBot.getStepper(200, 1);
void forwardStep()
{
	bottomStepper->onestep(BACKWARD, DOUBLE);
}
void backwardStep()
{
	bottomStepper->onestep(FORWARD, DOUBLE);
}
AccelStepper bStepper(forwardStep, backwardStep);

uint16_t getSteps(double length)
{
	return (uint16_t)(int(abs(length*10*200)));
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
	ballDropper.write(0);
	delay(300);//time for the ball to go
	ballDropper.write(15);
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
		Serial.println("Setting length to " + String(value) + " steps: " + String(steps) + " direction " + String(value - length));
		uint8_t direction = BACKWARD;
		if (value < length)
		{
			direction = FORWARD;
		}
		length = value;
		tigeStepper->step(steps, direction, DOUBLE);
		tigeStepper->release();
		delay(2000);//5000
	}
	Serial.println("done");
}

void setAngle(double value) {
	if (angle != value)
	{
		Serial.println("Setting angle to " + String(value));
		double pos = value - 90;
		double stepPos = (pos / 1.8) * (6 / 2.3);
		long lStepos = (long)(stepPos);
		double diff = angle - value;
		Serial.println("diff is " + String(diff));
		double diff1= abs(diff / 1.8)*(6/2.3);
		uint8_t direction = FORWARD;
		if (diff < 0)
		{
			direction = BACKWARD;
			Serial.println("changing");
		}
		double diff2 = abs(angle - 90);
		double k = 1 + (diff2 / 17.5)*0.95;
		Serial.println(k);
		uint16_t steps = (uint16_t)(int(diff1));
		Serial.println("change pos from " + String(bStepper.currentPosition()) + " to " + String(stepPos));
		//bStepper.runToNewPosition(lStepos);
		bottomStepper->step(steps, direction, MICROSTEP);
		bottomStepper->release();
		angle = value;
		delay(2000);
	}
	Serial.println("done");
}

void setup() {
	Serial.begin(9600);
	AFMS.begin();
	AFMSBot.begin();
	tigeStepper->setSpeed(40);
	bottomStepper->setSpeed(1);
	ballDropper.attach(ballDropperPin);
	bStepper.setSpeed(1);
	ballDropper.write(15);
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
