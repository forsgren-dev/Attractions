
## Assignment in ASP.NET/EF Core by Erik Forsgren


## How to build database (I am using MSSQL) - 

1. Start Docker

2. Fire up the MSSQL server

3. From _scripts (\Attractions\_scripts) run:
    .\database-rebuild-all.ps1 sql-attractions sqlserver docker root ..\AppWebApi

4. Run the SQL-script: 
    \DbContext\SqlScripts\initDatabase.sql

5. Start debugger.

6. Seed database from the api/Admin/SeedDatabase endpoiont in Swagger and take it from there.

## ABOUT THE DB
I based the DB-design on my previous assignment in SQL and the ASP.NET/EF Core on the tutorial code model from SEIDO. In this project I normalized addresses into their own table instead of including them in Attractions. Addresses are indexed by street, postalcode and city to create unique address entities. Howerver, street and postalcode may be null, so multiple attractions can have the same address with only a city and country in them. This is because large nature areas, like Grand Canyon, might not have an actual street address. They do have a closest city and belong to a country tho.   

The seedgenerator has been modiefied for the assignment to create 110 cities and 4 countries when data is seeded. The same amount of users as attractions are seeded. There are seed flags on cities and countries so real addresses use separate city and country records, even when the names match. Deleting seeded data therefore removes seeded cities and countries while preserving real ones.







