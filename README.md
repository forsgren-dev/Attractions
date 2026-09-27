
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

## ABOUT THE DB
I based the DB-design on my previous assignment in SQL and the ASP.NET/EF Core on the tutorial code model from SEIDO. In this project I normalized addresses into their own table instead of including them in Attractions. Addresses are indexed by street, postalcode and city to create unique address entities. Howerver, street and postalcode may be null, so multiple attractions can have the same address with only a city and country in them. This is because large nature areas, like Grand Canyon, might not have an actual street address. They do have a closest city and belong to a country tho.   

The seedgenerator has been modiefied for the assignment to create 110 cities and 4 countries when data is seeded. The same amount of users as attractions are seeded. There are seed flags on cities and countries so real addresses use separate city and country records, even when the names match. Deleting seeded data therefore removes seeded cities and countries while preserving real ones.

In the Swagger UI there are some endpoints that give the same result. This is because later during the developement I added filters to some endpoints that do the same thing as I had already made individual endpoints for. For example listing attractions that has no comments. This can now be done in the main attraction listing by setting hasComments to false. I kept the old endpoints tho, because this way I can check that the filters are working as they should.

I made separe DTO:s for create and update. This is only because I kept forgetting to set the entity id to NULL everytime I created a user, attraction or comment. With a separate create DTO I can leave out the id from the body. With a frontend this would not be an issue and I get the principle of using a combined DTO, so this sollution is purely for my own sanity using Swagger.















