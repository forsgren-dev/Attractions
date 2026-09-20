
- Build database - 

From _scripts (\Attractions\_scripts) run:
    .\database-rebuild-all.ps1 sql-attractions sqlserver docker root ..\AppWebApi

Run SQL-script: 
    \DbContext\SqlScripts\initDatabase.sql

Start debugger.

Seed database from the api/Admin/SeedDatabase endpoiont.

NOTE: Cities and Contries are not concidered to be seeded, even when filled by the seeder, so they will remain in the database after seeded data deletion. This is due to the fact that these are real cities/contries and deleting them would remove added addresses in these cities/countries.  

