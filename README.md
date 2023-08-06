ZoomEase
======================
Introduction:
-------------
This C# program facilitates the scheduling of Zoom meetings using the Zoom API. The user can enter the meeting details, including the topic, password, start time, and duration, and the program will create a meeting on Zoom. After successfully creating the meeting, the program returns the Join and Start URLs for the meeting.

Dependencies:
-------------
- System
- System.Globalization
- System.Net.Http
- System.Text
- System.Text.RegularExpressions
- System.Threading.Tasks
- Newtonsoft.Json

Dependencies that you will have to install:
-------------
- Newtonsoft.Json

How to install newtonsoft.Json (without visual studio):
-------------
1. Go to https://www.newtonsoft.com/json
2. Click on the download button

How to install newtonsoft.Json (with visual studio):
-------------
1. Open visual studio code
2. Open the project
3. On the right side under the "solution explorer" right click on the "solution 'program_name'"
4. Click on the "Manage NuGet Packages for Solution..."
5. Click on Browse
6. Search "Newtonsoft.Json"
7. Then check off "Project"
8. Then hit install

Setup:
------
1. You will need the following to run the code:
    - Refresh Token from PostMan (Hard coded)
    - Client ID from Zoom App Management Tab (linked to josh's account, can be changed if you create your own API, you will get these values and will be able to change them easily)
    - Client Secret from Zoom App Management Tab
2. These values are hardcoded in the sample, but it's recommended to obtain them securely or use a more secure method like environment variables.

Usage:
------
1. Run the program.
2. Enter the topic for the meeting.
3. Input the password for the meeting. Note: Password cannot have more than 10 characters.
4. Specify the start time in this format (YYYY-MM-DDTHH:MM:SSZ).
5. Provide the meeting duration in minutes.
6. If all details are valid, the program will return the Join and Start URLs for the created Zoom meeting.

Validations:
------------
- The password length is checked to ensure it doesn't exceed 10 characters.
- The start time format is validated against the format (YYYY-MM-DDTHH:MM:SSZ).

API Interactions:
-----------------
The program interacts with the Zoom API to:
1. Refresh the Access Token.
2. Fetch existing meetings for the user.
3. Post and create a new meeting based on user input.

Error Handling:
---------------
- Errors are printed to the console in cases like invalid inputs, failed API requests, or token refresh failures.

Scalability:
---------------
- Client-Server Model.

Deployment:
---------------
- Comand-line tool
- 

Note:
-----
Please remember that storing sensitive data like refresh tokens, client ID, and client secret in the code is not secure. Always consider using secure methods like environment variables or secrets management tools.
