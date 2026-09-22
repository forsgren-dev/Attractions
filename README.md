
- Build database - 

From _scripts (\Attractions\_scripts) run:
    .\database-rebuild-all.ps1 sql-attractions sqlserver docker root ..\AppWebApi

Run SQL-script: 
    \DbContext\SqlScripts\initDatabase.sql

Start debugger.

Seed database from the api/Admin/SeedDatabase endpoiont.

With the built-in seed source, the seeder creates all 110 cities and 4 countries as seeded data. Real addresses use separate city and country records, even when the names match. Removing seeded data also removes seeded cities and countries while preserving real ones.

Complete addresses are unique by street, postal code, city, and country. Street and postal code can still be null, so multiple attractions can have an incomplete address such as no street, no postal code, Stockholm, Sweden.

