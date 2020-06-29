Create new JWTs for test session IDs and a client cert subject.

File format:

```json
[
        {
                "testSessionId": 1,
                "clientCertSubject": "E=russell.hammett@nist.gov, CN=Russ Hammett, OU=ACVTS, O=NIST, L=Gaithersburg, S=Maryland, C=US"
        },
        {
                "testSessionId": 2,
                "clientCertSubject": "E=russell.hammett@nist.gov, CN=Russ Hammett, OU=ACVTS, O=NIST, L=Gaithersburg, S=Maryland, C=US"
        }
]
```

Input the filename with json as above into the application, get new JWTs for the test sessions/cert subjects.

Example run:

```cmd

C:\workspace\gitlab\gen-val\gen-val\code-samples\JwtCreatinator\src\bin\Release\netcoreapp3.1\win-x64\publish>JwtCreatinator.exe
rootDirectory: C:\workspace\gitlab\gen-val\gen-val\code-samples\JwtCreatinator\src\bin\Release\netcoreapp3.1\win-x64\publish\
Bootstrapping application using environment local
This utility can be used to create new JWTs based on a test session and client cert subject.
The format of the file is:

[
        {
                "testSessionId": 1,
                "clientCertSubject": "E=russell.hammett@nist.gov, CN=Russ Hammett, OU=ACVTS, O=NIST, L=Gaithersburg, S=Maryland, C=US"
        },
        {
                "testSessionId": 2,
                "clientCertSubject": "E=russell.hammett@nist.gov, CN=Russ Hammett, OU=ACVTS, O=NIST, L=Gaithersburg, S=Maryland, C=US"
        }
]



-----


Enter the full filepath to a json file in the format shown above.

"C:\workspace\logs\jwtCreatinator\myJson.json"


info: JwtCreatinator.Program[0]
      Attempting to create new JWTs for 2 test sessions.
info: JwtCreatinator.Services.JwtCreatinator[0]
      Create new JWT for test session ID: 10435
info: JwtCreatinator.Services.JwtCreatinator[0]
      Create new JWT for test session ID: 10436
info: JwtCreatinator.Program[0]
      Processed 2 requests.
info: JwtCreatinator.Program[0]
      [
        {
          "testSessionId": 10435,
          "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0c0lkIjoiMTA0MzUiLCJ2c0lkIjoiWzEwNDQ4XSIsInN1YiI6IkU9cnVzc2VsbC5oYW1tZXR0QG5pc3QuZ292LCBDTj1SdXNzIEhhbW1ldHQsIE9VPUFDVlRTLCBPPU5JU1QsIEw9R2FpdGhlcnNidXJnLCBTPU1hcnlsYW5kLCBDPVVTIiwibmJmIjoxNTkzNDU1MjkwLCJleHAiOjE1OTM0NTcwOTAsImlhdCI6MTU5MzQ1NTI5MCwiaXNzIjoiTklTVCBBQ1ZQIExPQ0FMIn0.7pv54hELQkMRyRoJwJxl7SoctnXcrNax1iE1-Ig9L7E"
        },
        {
          "testSessionId": 10436,
          "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0c0lkIjoiMTA0MzYiLCJ2c0lkIjoiWzEwNDQ5XSIsInN1YiI6IkU9cnVzc2VsbC5oYW1tZXR0QG5pc3QuZ292LCBDTj1SdXNzIEhhbW1ldHQsIE9VPUFDVlRTLCBPPU5JU1QsIEw9R2FpdGhlcnNidXJnLCBTPU1hcnlsYW5kLCBDPVVTIiwibmJmIjoxNTkzNDU1MjkwLCJleHAiOjE1OTM0NTcwOTAsImlhdCI6MTU5MzQ1NTI5MCwiaXNzIjoiTklTVCBBQ1ZQIExPQ0FMIn0.rvoD7V_I-qrC-KXAXl2TgEfHWlpEJtgcWJhIIoi700Q"
        }
      ]
info: JwtCreatinator.Program[0]
      New jwts saved to: C:\workspace\logs\jwtCreatinator\20200629_142810_newJwts.json
Press any key to close.
```