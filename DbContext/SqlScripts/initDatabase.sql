
USE [sql-attractions];
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'gstusr')
    EXEC('CREATE SCHEMA gstusr');
GO

CREATE OR ALTER VIEW gstusr.vwDbInfo AS
    SELECT (SELECT COUNT(*) FROM supusr.Users WHERE Seeded = 1) as nrSeededUsers, 
        (SELECT COUNT(*) FROM supusr.Users WHERE Seeded = 0) as nrUnseededUsers,
        (SELECT COUNT(*) FROM supusr.Cities) as nrCities,
        (SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 1) as nrSeededAttractions, 
        (SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 0) as nrUnseededAttractions;
GO

CREATE OR ALTER PROC supusr.spDeleteAll
    @seededParam BIT = 1,

    @nrFriendsAffected INT OUTPUT,
    @nrAddressesAffected INT OUTPUT,
    @nrPetsAffected INT OUTPUT,
    @nrQuotesAffected INT OUTPUT
    
    AS

    SET NOCOUNT ON;

    SELECT  @nrFriendsAffected = COUNT(*) FROM supusr.Friends WHERE Seeded = @seededParam;
    SELECT  @nrAddressesAffected = COUNT(*) FROM supusr.Addresses WHERE Seeded = @seededParam;
    SELECT  @nrPetsAffected = COUNT(*) FROM supusr.Pets WHERE Seeded = @seededParam;
    SELECT  @nrQuotesAffected = COUNT(*) FROM supusr.Quotes WHERE Seeded = @seededParam;

    DELETE FROM supusr.Friends WHERE Seeded = @seededParam;
    DELETE FROM supusr.Addresses WHERE Seeded = @seededParam;
    DELETE FROM supusr.Pets WHERE Seeded = @seededParam;
    DELETE FROM supusr.Quotes WHERE Seeded = @seededParam;

    --throw our own error
    --;THROW 999999, 'Error occurred in supusr.spDeleteAll', 1

    SELECT * FROM gstusr.vwDbInfo;
GO
