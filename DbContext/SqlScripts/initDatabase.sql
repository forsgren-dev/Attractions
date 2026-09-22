
USE [sql-attractions];
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'gstusr')
    EXEC('CREATE SCHEMA gstusr');
GO

CREATE OR ALTER VIEW gstusr.vwDbInfo AS
    SELECT (SELECT COUNT(*) FROM supusr.Users WHERE Seeded = 1) as nrSeededUsers, 
        (SELECT COUNT(*) FROM supusr.Users WHERE Seeded = 0) as nrUnseededUsers,
        (SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 1) as nrSeededAttractions, 
        (SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 0) as nrUnseededAttractions,
        (SELECT COUNT(*) FROM supusr.Addresses WHERE Seeded = 1) as nrSeededAddresses,
        (SELECT COUNT(*) FROM supusr.Addresses WHERE Seeded = 0) as nrUnseededAddresses,
        (SELECT COUNT(*) FROM supusr.Comments WHERE Seeded = 1) as nrSeededComments,
        (SELECT COUNT(*) FROM supusr.Comments WHERE Seeded = 0) as nrUnseededComments,
        (SELECT COUNT(*) FROM supusr.Cities WHERE Seeded = 1) as nrSeededCities,
        (SELECT COUNT(*) FROM supusr.Cities WHERE Seeded = 0) as nrUnseededCities,
        (SELECT COUNT(*) FROM supusr.Countries WHERE Seeded = 1) as nrSeededCountries,
        (SELECT COUNT(*) FROM supusr.Countries WHERE Seeded = 0) as nrUnseededCountries;
GO

CREATE OR ALTER PROC supusr.spDeleteAll
    @seededParam BIT = 1,

    @nrAttractionsAffected INT OUTPUT,
    @nrAddressesAffected INT OUTPUT,
    @nrUsersAffected INT OUTPUT,
    @nrCommentsAffected INT OUTPUT
    
    AS

    SET NOCOUNT ON;

    SELECT  @nrAttractionsAffected = COUNT(*) FROM supusr.Attractions WHERE Seeded = @seededParam;
    SELECT  @nrAddressesAffected = COUNT(*) FROM supusr.Addresses WHERE Seeded = @seededParam;
    SELECT  @nrUsersAffected = COUNT(*) FROM supusr.Users WHERE Seeded = @seededParam;
    SELECT  @nrCommentsAffected = COUNT(*) FROM supusr.Comments WHERE Seeded = @seededParam;

    DELETE FROM supusr.Comments WHERE Seeded = @seededParam;
    DELETE FROM supusr.Attractions WHERE Seeded = @seededParam;
    DELETE FROM supusr.Addresses WHERE Seeded = @seededParam;
    DELETE FROM supusr.Cities WHERE Seeded = @seededParam;
    DELETE FROM supusr.Countries WHERE Seeded = @seededParam;
    DELETE FROM supusr.Users WHERE Seeded = @seededParam;

    --throw our own error
    --;THROW 999999, 'Error occurred in supusr.spDeleteAll', 1

    SELECT * FROM gstusr.vwDbInfo;
GO

CREATE OR ALTER PROC supusr.spDeleteAttraction
    @attractionIdParam UNIQUEIDENTIFIER
AS
    SET NOCOUNT ON;

    DELETE FROM supusr.Comments
    WHERE AttractionDbMAttractionId = @attractionIdParam;

    DELETE FROM supusr.Attractions
    WHERE AttractionId = @attractionIdParam;
GO
