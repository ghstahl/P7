# GraphQL Support in P7
Built into P7 is GraphQL support using the following open source project;  
[graphql-dotnet](https://github.com/graphql-dotnet/graphql-dotnet)  
Admittedly this was the first one I looked at, and at the time of usage it was pretty complete.  What drew me to it was that it had a nice way to apply security on the incoming requests. 

P7 provides a way to easily implement new graphQL endpoints.  Currently I am sweeping the project for implementations and registering them all with the DI, but I think I am going to require folks to register their implementations themselves.  Securing access to these queries is done via configuration, and that work is currently in progress.

P7 provides a stock graphQL implementation for fetching strings out of the ASP.NET RESX files.  There is also a custom REST api to fetch the same data, but where we can migrate away from REST to graphQL we should do it.  
[GraphQL resource implementation](../src/P7.Globalization)


## GraphQL Viewer
1. Browser to /GraphQLView

**Query**

### Query for browser culture
```graphql
query q($id: String!,$treatment: String! ){
  resource(input: { id: $id, treatment: $treatment })
}
```
**Query Variables**
```graphql
{
    "id": "p7.main.Resources.Main,p7.main",
  	"treatment":"kva"
}
or
{
    "id": "p7.main.Resources.Main,p7.main",
    "treatment":"kvo" 
}
```  
**Result**
```graphql
{
  "data": {
    "resource": [
      {
        "key": "Hello",
        "value": "Hello"
      }
    ]
  }
}
or
{
  "data": {
    "resource": {
      "Hello": "Hello"
    }
  }
}
```  

### Query for any culture

```graphql
query q($id: String!,$treatment: String!,$culture: String!){
  resource(input: { id: $id, treatment: $treatment,culture: $culture  })
}
```
**Query Variables**
```graphql
{
    "id": "p7.main.Resources.Main,p7.main",
  	"treatment":"kva",
  	"culture":"fr-FR"
}
or
{
    "id": "p7.main.Resources.Main,p7.main",
  	"treatment":"kvo",
  	"culture":"fr-FR"
}
```  

**Result**
```graphql
{
  "data": {
    "resource": [
      {
        "Key": "Hello",
        "Value": "Bonjour"
      }
    ]
  }
}
or
{
  "data": {
    "resource": {
      "Hello": "Bonjour"
    }
  }
}
```  
