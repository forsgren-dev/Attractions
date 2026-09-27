
## Assignment in ASP.NET/EF Core by Erik Forsgren


## How to build database (I am using MSSQL) - 

1. Start Docker

2. Fire up the MSSQL server

3. From _scripts (\Attractions\_scripts) run:
    .\database-rebuild-all.ps1 sql-attractions sqlserver docker root ..\AppWebApi

4. Run the SQL-script adding View and Stored procedure etc: 
    \DbContext\SqlScripts\initDatabase.sql

5. Start debugger.

6. Seed database from the api/Admin/SeedDatabase endpoint in Swagger and take it from there. The default seed value is 1000. This creates 1000 attractions, 1000 users, 110 cities in 4 countries, and between 0 and 20 comments for each attraction. All features that were asked for should be working as expected, I hope. 

## ABOUT THE PROJECT
I based the DB-design on my previous assignment in SQL and the ASP.NET/EF Core structure on the tutorial code model from SEIDO. In this project I normalized addresses into their own table instead of including them in Attractions. This makes the database cleaner because addresses belongs in their own entities. 

Addresses are indexed by street, postalcode and city to create unique address entities. Howerver, street and postalcode may be null, so multiple attractions can have the same address with only a city and country in them. This is because large nature areas, like Grand Canyon, might not have an actual street address. They do have a closest city and belong to a country tho. But there can't be two cities with the same name in the same country with the same seeded flag, This is because I indexed city name, country and seeded together as unique. There can be only one Stockholm in Sweden that is seeded, and one undseeded. But there can also be a Stockholm in the USA. This project allows only one attraction per full unique address. 

All tables except Categories are indexed on the Seeded flag, since this is used for finding what to delete when removing seeded data. The table Attractions is also indexed by AttractionName due to the probability that it will be used to find an attraction, and the table Users is indexed on UserName to force them to be unique. 

Instead of reviews this project has comments. Comments can't live without a user or attraction, so they are deleted when any of these is removed. The only 'many to many' relation is between attractions and categories. All other relations are 'one to zero or many'. 

The seedgenerator has been modified for the assignment to create 110 existing cities in 4 Nordic countries when data is seeded. The same amount of users as attractions are seeded. There are seed flags on cities and countries so real addresses use separate city and country entities, even when the names match. Deleting seeded data therefore removes seeded cities and countries while preserving the ones added manually.

In the Swagger UI there are some endpoints that give the same result. This is because later during the developement I added filters to some endpoints that do the same thing as I had already made individual endpoints for. For example listing attractions that has no comments. This can now be done in the main attraction listing by setting hasComments to false. I kept the old endpoints tho, because this way I can check that the filters are working as they should. ReadAttractions, ReadUsers and ReadComments also have a seeded filter. If seeded is null all data is listed, if seeded is true only seeded test data is listed, and if seeded is false only manually created data is listed.

The Guest/Info endpoint shows an overview of the database content and uses the database view gstusr.vwDbInfo. The Admin/RemoveSeededData endpoint removes seeded test data and uses the stored procedure supusr.spDeleteAll.

I made separe DTO:s for create and update. This is only because I kept forgetting to set the entity id to NULL everytime I created a user, attraction or comment. With a separate create DTO I can leave out the id from the body. With a frontend this would not be an issue and I get the principle of using a combined DTO, so this sollution is purely for my own sanity using Swagger.

## ABOUT ATTRACTION CATEGORIES
I decided that attractions should require at least one category in the API. I imagine with a user-interface I'd have checkboxes or something to select at least one. There would ofcourse be an "Other" category then, but there are no such option in this project. So when creating an attraction, first use the ReadCategories endpoint to list the available category id:s. At least one of them is needed in the create attraction body. I did not include the join table for the relations between Attractions and Categories in the ERD diagram but it is shown in the auto-visualization made by VS Code.




















