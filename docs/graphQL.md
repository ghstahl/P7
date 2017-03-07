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

### The Blog Store
#### Insert via Mutation

```graphql
mutation q($input: BlogMutationInput! ){
  blog(input:   $input )
}
```
**Mutation Variables**
```graphql
{
   "input":   {
		"metaData": {
			"category": "c0",
			"version": "1.0.0.0"
		},
		"categories": ["c10", "c20"],
		"tags": ["t10", "t20"],
		"data": "This is my blog",
		"timeStamp": "2027-03-07T14:55:03Z",
		"title": "My Title",
		"summary": "My Summary",
		"id": "7f5ebe45-47dd-424a-a871-88e3b38f5e33"
	}
}
```  
**Result**
```graphql
{
  "data": {
    "blog": true
  }
}
```
