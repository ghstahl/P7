# Decentralized Token Management Support

P7 will support a decentralized token management system using the following open source project as the core engine;  
[OpenID Connect and OAuth 2.0 Framework for ASP.NET Core](https://github.com/IdentityServer/IdentityServer4)  

The service being provided is the creation of tokens and the management of those tokens in flight.  

The interesting thing is what is NOT being considered as part of the service, and those are as follows;  
1. Users  
2. Scopes  
3. Claims  

What I have found is that those artifacts tend to be decentralized, especially in large enterprises.  Inside enterprises, there are multiple business units, each with their own concept of what a user is and more importantly what a user has access to.  Basically one business inside an enterprise has nothing to do with the workings of another, and if by coincidence a single user does business with more than one there still should not be an association between the business units.

### What can P7 do for you?  
P7 will manage your OAuth 2.0 tokens, but will not be concerned of what is inside them.   
P7 will let you create arbitrary tokens, where what is inside them is passed into the system as arguments.  
P7 is not here to guard access to your resources, only to provide the inflight managment of tokens.  
Services that use P7 should have their own system of record for users, claims, and scopes that they manage.  It is those services that provide the real resources that clients want.

### Use Cases

#### The ability to create a Resource_Owner Flow type token
I need to be able to create a Resource_Owner token, where I can pass in an arbitrary user, with arbitrary scopes, and abitrary claims.  I need the service to then manage that token whilst in flight.

``` 
Probably should make this an enhanced grant as well.  The username is carried through, but the password can be anything.
http://localhost:7791/connect/token POST
grant_type=password&scope=arbitrary offline_access&client_id=resource-owner-client&client_secret=secret&handler=arbitrary-claims-service&arbitrary-claims={"naguid":"1234abcd","In":"Flames"}&username=rat&password=poison&arbitrary-scopes=A quick brown fox
```
produces
```
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImI5ODQyMzIyYWFiNmRhMWY4OGI2ZDdkYmVhMmY4MTdmIiwidHlwIjoiSldUIn0.eyJuYmYiOjE0ODg0MDUzMjUsImV4cCI6MTQ4ODQwODkyNSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3NzkxIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6Nzc5MS9yZXNvdXJjZXMiLCJhcmJpdHJhcnkiXSwiY2xpZW50X2lkIjoicmVzb3VyY2Utb3duZXItY2xpZW50Iiwic3ViIjoicmF0IiwiYXV0aF90aW1lIjoxNDg4NDA1MzI1LCJpZHAiOiJsb2NhbCIsIm5hZ3VpZCI6IjEyMzRhYmNkIiwiSW4iOiJGbGFtZXMiLCJzY29wZSI6WyJhcmJpdHJhcnkiLCJvZmZsaW5lX2FjY2VzcyIsIkEiLCJxdWljayIsImJyb3duIiwiZm94Il0sImFtciI6WyJwd2QiXX0.AV9xo_a0YC2vSoAgV5sqlSUea2De7iYhwCIneBz_4m2Z1dnuf_XMMJrlZyUj2fg8zvUAtoRl9_epb-jSrYvzeQRqX6c-0jq_gs8emhWscU2X9UCr-KwZJG23WFLu_yHzPpfeoYDwUl8E7P1hdRZC4hol6c6cJrFChA9go5hAcy5pGyeNXTM3iyR3TRDfGT3abqeV1mxrgBA6RxA3i0oPS9_LYpuXbxR7bl-mmMI49hwOpjDJRhmR9EUThBUw51hW0xfMUK9-_Qv2nojjPJUSy7FFOuu2FL1-qsHSZjiIB3JT3dTxWx9RK6PeF75dwMKN0neoKHiAWbFf-eZ2sqTPoQ",
    "expires_in": 3600,
    "token_type": "Bearer",
    "refresh_token": "5ed1395bedc0af8d4faa2e11a9b301e6c6387826c7d1ea35d78f06a908455a48"
}
```
#### The ability to refresh a Resource_Owner Flow type token

taking the refresh_token from above;
```
http://localhost:7791/connect/token POST
grant_type=public_refresh_token&refresh_token=5ed1395bedc0af8d4faa2e11a9b301e6c6387826c7d1ea35d78f06a908455a48&client_id=public-resource-owner-client
```
produces the following;
```
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImI5ODQyMzIyYWFiNmRhMWY4OGI2ZDdkYmVhMmY4MTdmIiwidHlwIjoiSldUIn0.eyJuYmYiOjE0ODg0MDU0NzMsImV4cCI6MTQ4ODQwOTA3MywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3NzkxIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6Nzc5MS9yZXNvdXJjZXMiLCJhcmJpdHJhcnkiXSwiY2xpZW50X2lkIjoicHVibGljLXJlc291cmNlLW93bmVyLWNsaWVudCIsInNjb3BlIjpbImFyYml0cmFyeSJdfQ.pdiEnPT_V5I1I9E5q5fg9k_FPbiwWbQMzhVrhXJEJO5Kkx88GBekpSHq0DkkWP3DKn8gIAHmIduuHO3Pyuu6q379A4axh639ix4Dkmi6gvL0wFlrrt9GvtdmyeQkLchQFIXxTEtzRXHEuGStpSzkxjEDjbP56pEWWbeBTAHvBb52Zb1WuA31uRL1NV1Xb3YFc6gIrql7t88lX0jnr26A0M34VQhBjBkx2zfo67M_r-Wi2YhXRip2UZKPh1mdlgSQ9KzHi_Ah_YeFvDJbffAHCf-zgxF4f6ZkXPFN9bcKERBR3WbspmZhU3RdquzndMvW9kg7T4-vUfoXzDHvL7r0bA",
    "expires_in": 3600,
    "token_type": "Bearer",
    "inner_response": {
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImI5ODQyMzIyYWFiNmRhMWY4OGI2ZDdkYmVhMmY4MTdmIiwidHlwIjoiSldUIn0.eyJuYmYiOjE0ODg0MDU0NzIsImV4cCI6MTQ4ODQwOTA3MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3NzkxIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6Nzc5MS9yZXNvdXJjZXMiLCJhcmJpdHJhcnkiXSwiY2xpZW50X2lkIjoicmVzb3VyY2Utb3duZXItY2xpZW50Iiwic3ViIjoicmF0IiwiYXV0aF90aW1lIjoxNDg4NDA1MzI1LCJpZHAiOiJsb2NhbCIsIm5hZ3VpZCI6IjEyMzRhYmNkIiwiSW4iOiJGbGFtZXMiLCJzY29wZSI6WyJhcmJpdHJhcnkiLCJvZmZsaW5lX2FjY2VzcyIsIkEiLCJxdWljayIsImJyb3duIiwiZm94Il0sImFtciI6WyJwd2QiXX0.ZORVofGs00YvWUWU_dUrNGxVOO2pQebEVkUom8zB2ibpW8Hy6a2dgNnQKZet4op1X7-8LiiW3wXuiQKd5pVgKBjTgGbyAJBoVnzHl2Js1xgxOkm5lKimQNKeVHQ9dW747TxHBHuS-9q7L-CMltfYfUKvsNb1bKHdRpEu1lJ4senKK6fiFLLdklabqIQ7QTopFi3koFpILO4gw9WAVDTnLiIyynl4vY6vdlvHMuGBfwkipjPeu7MTGKdAL5Wu-IJeeEXN1a1NZSO1gXPFed_L3pyRMrZn-ygloL1M-CN_i09ThJVBTpSOsam2sLlO9snd4cOSZ1d7F0FmijA4IbTbyg",
        "expires_in": 3600,
        "token_type": "Bearer",
        "refresh_token": "50dcb1ef15113115a9b1d1ebf3873fabbab4d1a67790e547f765a6559c0f36e6"
    }
}
```



