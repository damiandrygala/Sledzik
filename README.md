# The Tracker
## 1. Description
Software for monitoring the location of objects based on GPS receiver data.
UI views and details are avaible in user guide: [Manual(Tracker)-draft_pl.pdf](https://github.com/damiandrygala/Sledzik/)
## 2. Features
- Definig tracking zones
- Detecting and reporting violations of the assigned zone
- Obtaining data on the location of objects in real time and presenting them on a map relative to assigned zone
- History review of violations and the location of the objects in relation to defined area
## 3. Responsibility
1. Tracker (VUE), mobile app - observed object, the application captures the GPS position and sends data to the server.
2. Monitoring (VUE), web app - user management center for registering an object, defining zones, managing, real-time analysis and history review
3. TrackerApi (.NET), server - communication between the above applications, responsible for data processing, writing and reading in mongoDB and calculates violations