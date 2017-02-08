# P7
An opinionated way to build Asp.Net core applications

## Development

From a console

```cmd
:> git clone https://github.com/ghstahl/P7
:> cd P7\src\WebApplication5
:> gulp watch
```
This will monitor plugin projects that have static assests and copy them to the main webapp project.
This is only done during developement, as the goal is to have your plugin projects packaged up as nugets.
Your nugets will be versioned and contain your assemblies and static content.

## Build
This is a Visual Studio 2015+ project

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

## Credits
Microsoft OpenSource Asp.Net everything  
A ton of people from stackoverflow.com  
Those that contribute their own time to build great open source solutions.  

