
## Assignment in ASP.NET/EF Core by Erik Forsgren


## How to build database (I am using MSSQL) - 

1. Start Docker

2. Fire up the MSSQL server

3. From _scripts (\Attractions\_scripts) run:
    .\database-rebuild-all.ps1 sql-attractions sqlserver docker root ..\AppWebApi

4. Run the SQL-script: 
    \DbContext\SqlScripts\initDatabase.sql

5. Start debugger.

6. Seed database from the api/Admin/SeedDatabase endpoiont in Swagger and take it from there. All features that were asked for should be working as expected, I hope. 

## ABOUT THE PROJECT
I based the DB-design on my previous assignment in SQL and the ASP.NET/EF Core structure on the tutorial code model from SEIDO. In this project I normalized addresses into their own table instead of including them in Attractions. Addresses are indexed by street, postalcode and city to create unique address entities. Howerver, street and postalcode may be null, so multiple attractions can have the same address with only a city and country in them. This is because large nature areas, like Grand Canyon, might not have an actual street address. They do have a closest city and belong to a country tho. But there can't be two cities with the same name in the same country with the same seeded flag, This is because I indexed city name, country and seeded together as unique. There can be only one Stockholm in Sweden that is seeded, and one undseeded. But there can also be a Stockholm in the USA.

Most of the models are indexed on the seeded flag, since this is 

Instead of reviews this project has comments. Comments can't live without a user or attraction, so they are deleted when any of these is removed. The only 'many to many' relation is between attractions and categories. All other relations are 'one to zero or many'. I did not include the joint table in the ERD diagram but it is shown in the auto-visualization made by VS Code.

The seedgenerator has been modified for the assignment to create 110 existing cities in 4 Nordic countries when data is seeded. The same amount of users as attractions are seeded. There are seed flags on cities and countries so real addresses use separate city and country entities, even when the names match. Deleting seeded data therefore removes seeded cities and countries while preserving the ones added manually.

In the Swagger UI there are some endpoints that give the same result. This is because later during the developement I added filters to some endpoints that do the same thing as I had already made individual endpoints for. For example listing attractions that has no comments. This can now be done in the main attraction listing by setting hasComments to false. I kept the old endpoints tho, because this way I can check that the filters are working as they should.

I made separe DTO:s for create and update. This is only because I kept forgetting to set the entity id to NULL everytime I created a user, attraction or comment. With a separate create DTO I can leave out the id from the body. With a frontend this would not be an issue and I get the principle of using a combined DTO, so this sollution is purely for my own sanity using Swagger.

## ABOUT ATTRACTION CATEGORIES
I decided to have non-nullable categories for attractions. I imagine with a user-interface I'd have checkboxes or something to select at least one. There would ofcourse be an "Other" category then, but there are no such option in this project. So when creating an attraction, first use the ReadCategories endpoint to list the available category id:s. At least one of them is needed in the create attraction body.
















