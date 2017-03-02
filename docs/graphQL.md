# GraphQL Support in P7

## GraphQL Viewer
1. Browser to /GraphQLView

**Query**

```graphql
query q($id: String!,$treatment: String!,$culture: String!){
  resource(input: { id: $id, treatment: $treatment,culture: $culture  })
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
  	"treatment":"kva",
  	"culture":"fr-FR"
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
    "resource": [
      {
        "key": "Hello",
        "value": "Bonjour"
      }
    ]
  }
}
```  

or just a query with inputs:
```graphql
{
   resource(input:{id:"p7.main.Resources.Main,p7.main",treatment:"kvo",culture:"fr-FR"})
}
```
