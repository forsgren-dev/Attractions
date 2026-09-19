using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Microsoft.Data.SqlClient;
using MySqlConnector;
using Npgsql;
using System.Data;
using System.Data.Common;

using DbContext;
using Configuration;
using Models.DTO;

namespace DbRepos;

public class AdminDbRepos
{
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;
    private readonly AttractionDbRepos _attractionDbRepos;
    private readonly UserDbRepos _userDbRepos;
    private readonly CommentDbRepos _commentDbRepos;

    public async Task SeedAsync(int nrItems)
    {
        await _attractionDbRepos.SeedAsync(nrItems);
        await _userDbRepos.SeedAsync(nrItems);
        await _commentDbRepos.SeedAttractionCommentsAsync();
    }

    public async Task<ResponseItemDto<DbInfoDto>> RemoveSeededAsync()
    {
        const bool seeded = true;

        var connection = _dbContext.Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;

        List<DbParameter> parameters;
        if (connection is MySqlConnection)
        {
            // MySQL parameters - call as stored procedure
            command.CommandText = "supusr_spDeleteAll";
            parameters = new List<DbParameter>
            {
                new MySqlParameter("seededParam", seeded),
                new MySqlParameter("nrAttractionsAffected", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("nrAddressesAffected", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("nrUsersAffected", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
                new MySqlParameter("nrCommentsAffected", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };
        }
        else if (connection is NpgsqlConnection)
        {
             // PostgreSQL parameters - call as function returning table
            command.CommandText = "SELECT * FROM supusr.\"spDeleteAll\"(@seededParam)";
            command.CommandType = CommandType.Text;
            parameters =
            [
                new NpgsqlParameter("seededParam", seeded),
                new NpgsqlParameter("nrAttractionsAffected", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output },
                new NpgsqlParameter("nrAddressesAffected", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output },
                new NpgsqlParameter("nrUsersAffected", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output },
                new NpgsqlParameter("nrCommentsAffected", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output }
            ];
        }
        else
        {
             // SQL Server parameters (default)
            command.CommandText = "supusr.spDeleteAll";
            parameters = new List<DbParameter>
            {
                new SqlParameter("seededParam", seeded),
                new SqlParameter("nrAttractionsAffected", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("nrAddressesAffected", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("nrUsersAffected", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("nrCommentsAffected", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };
        }

        command.Parameters.AddRange(parameters.ToArray());

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        DbInfoDto resultSet = null;

        if (connection is NpgsqlConnection)
        {
            await command.ExecuteScalarAsync();
        }
        else
        {
            using var reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                await reader.ReadAsync();

                resultSet = new DbInfoDto
                {
                    NrSeededUsers = Convert.ToInt32(reader["NrSeededUsers"]),
                    NrUnseededUsers = Convert.ToInt32(reader["NrUnseededUsers"]),
                    NrSeededAttractions = Convert.ToInt32(reader["NrSeededAttractions"]),
                    NrUnseededAttractions = Convert.ToInt32(reader["NrUnseededAttractions"]),
                    NrSeededAddresses = Convert.ToInt32(reader["NrSeededAddresses"]),
                    NrUnseededAddresses = Convert.ToInt32(reader["NrUnseededAddresses"]),
                    NrSeededComments = Convert.ToInt32(reader["NrSeededComments"]),
                    NrUnseededComments = Convert.ToInt32(reader["NrUnseededComments"]),
                    NrCities = Convert.ToInt32(reader["NrCities"]),
                    NrCountries = Convert.ToInt32(reader["NrCountries"])
                };
            }

            await reader.CloseAsync();
        }

        int nrAttractionsAffected = (int)parameters.First(p => p.ParameterName == "nrAttractionsAffected").Value;
        int nrAddressesAffected = (int)parameters.First(p => p.ParameterName == "nrAddressesAffected").Value;
        int nrUsersAffected = (int)parameters.First(p => p.ParameterName == "nrUsersAffected").Value;
        int nrCommentsAffected = (int)parameters.First(p => p.ParameterName == "nrCommentsAffected").Value;

        _logger.LogInformation(
            "Removed seeded data. Attractions: {Attractions}, Addresses: {Addresses}, Users: {Users}, Comments: {Comments}",
            nrAttractionsAffected,
            nrAddressesAffected,
            nrUsersAffected,
            nrCommentsAffected);

        if (resultSet is null)
        {
            return await DbInfo();
        }

        return new ResponseItemDto<DbInfoDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = resultSet
        };
    }

    public async Task<ResponseItemDto<DbInfoDto>> GuestInfoAsync() => await DbInfo();


    private async Task<ResponseItemDto<DbInfoDto>> DbInfo()
    {
        var info = await _dbContext.DbInfoView.FirstAsync();

        return new ResponseItemDto<DbInfoDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif

            Item = info
        };
    }

    public AdminDbRepos(
        ILogger<AdminDbRepos> logger,
        Encryptions encryptions,
        MainDbContext context,
        AttractionDbRepos attractionDbRepos,
        UserDbRepos userDbRepos,
        CommentDbRepos commentDbRepos)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
        _attractionDbRepos = attractionDbRepos;
        _userDbRepos = userDbRepos;
        _commentDbRepos = commentDbRepos;
    }
}
