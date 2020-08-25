from ptpma.components import PMAUltrasonicSensor, PMALed
from time import sleep
import requests
import json
import config as config
from collections import namedtuple
from munch import *

red_led = PMALed(config.redlight_port)
green_led = PMALed(config.sensor_port)
ultrasonic_sensor = PMAUltrasonicSensor(config.greenlight_port)

while True:
    jsonData = requests.get(config.get_url)
    distance = round(ultrasonic_sensor.distance * 100, 2)
    print(distance)

    chores = jsonData.json()
    newChores = []
    for x in chores:
        chore = munchify(x)
        newChore = munchify(x)
        if chore.zoneId == "Office":
            if distance < 30:
                red_led.on()
                green_led.off()
                newChore.status = 1
            else:
                red_led.off()
                green_led.on()
                newChore.status = 2
        newChores.append(newChore)

    data = json.dumps(newChores)
    r = requests.post(config.post_url, data= data )
    red_led.off()
    green_led.off()
    sleep(5)