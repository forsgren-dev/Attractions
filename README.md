
Assignment in ASP.NET/EF Core by Erik Forsgren


- How to build database (default is MSSQL) - 

1. Start Docker

2. From _scripts (\Attractions\_scripts) run:
    .\database-rebuild-all.ps1 sql-attractions sqlserver docker root ..\AppWebApi

3. Run the SQL-script: 
    \DbContext\SqlScripts\initDatabase.sql

4. Start debugger.

5. Seed database from the api/Admin/SeedDatabase endpoiont.


I based the DB-design on my previous assignment in SQL. Tables for  

With the built-in seed source, the seeder creates all 110 cities and 4 countries as seeded data. Real addresses use separate city and country records, even when the names match. Removing seeded data also removes seeded cities and countries while preserving real ones.

Complete addresses are unique by street, postal code, city, and country. Street and postal code can still be null, so multiple attractions can have an incomplete address such as no street, no postal code, Stockholm, Sweden.

