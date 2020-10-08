from ptpma.components import PMAUltrasonicSensor, PMALed
from time import sleep
import requests
import json
import config as config
from munch import *

# Assign Inputs to Objects
red_led = PMALed(config.redlight_port)
green_led = PMALed(config.greenlight_port)
ultrasonic_sensor = PMAUltrasonicSensor(config.sensor_port)
prevDistance = 0

while True:
    distance = round(ultrasonic_sensor.distance * 100, 2)
    print(distance)
    # Check previous distance to see if sensor changed enough to process
    if (abs(distance) - abs(prevDistance) >  10) or (abs(distance) - abs(prevDistance) < -10):
        # Get Chore data and build object
        jsonData = requests.get(config.get_url)
        chores = jsonData.json()['chores']
        newChores = {}
        newChores['assignees'] = jsonData.json()['assignees']
        newChores["chores"] = []
        for x in chores:
            chore = munchify(x)
            newChore = munchify(x)
            # Check distance against configured threshold
            if distance < config.distance_threshold:
                red_led.on()
                green_led.off()
                newChore.status = config.over_threshold_status
            else:
                red_led.off()
                green_led.on()
                newChore.status = config.under_threshold_status
            newChores["chores"].append(newChore)
        data = json.dumps(newChores)
        # Post updated chores to Azure and save previous version
        r = requests.post(config.post_url, data= data )
        prevDistance = distance
    sleep(5)