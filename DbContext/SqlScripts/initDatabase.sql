

USE [sql-attractions];
GO

CREATE OR ALTER VIEW supusr.vwIDbInfo AS
    SELECT (SELECT COUNT(*) FROM supusr.Users WHERE Seeded = 1) as nrSeededUsers, 
        (SELECT COUNT(*) FROM supusr.Users WHERE Seeded = 0) as nrUnseededUsers,
        (SELECT COUNT(*) FROM supusr.Cities WHERE Seeded = 1) as nrSeededCities, 
        (SELECT COUNT(*) FROM supusr.Cities WHERE Seeded = 0) as nrUnseededCities,
        (SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 1) as nrSeededAttractions, 
        (SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 0) as nrUnseededAttractions;
GO