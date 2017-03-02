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





