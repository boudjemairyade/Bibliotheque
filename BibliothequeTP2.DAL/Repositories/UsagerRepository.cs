using System.Data;
using System.Data.SqlClient;
using BibliothequeTP2.DAL;
using BibliothequeTP2.Entities;

namespace BibliothequeTP2.DAL.Repositories;


public class UsagerRepository : IRepository<Usager>
{
    private readonly DbConnection _dbConnection;

    public UsagerRepository(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Usager>> GetAllAsync()
    {
        var usagers = new List<Usager>();
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            "SELECT IdUsager, Nom, Email, Telephone FROM Usagers ORDER BY Nom",
            connection);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            usagers.Add(MapReaderToUsager(reader));
        }

        return usagers;
    }

    public async Task<Usager?> GetByIdAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            "SELECT IdUsager, Nom, Email, Telephone FROM Usagers WHERE IdUsager = @Id",
            connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapReaderToUsager(reader);
        }

        return null;
    }

    public async Task<int> CreateAsync(Usager usager)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"INSERT INTO Usagers (Nom, Email, Telephone) 
              OUTPUT INSERTED.IdUsager
              VALUES (@Nom, @Email, @Telephone)",
            connection);

        command.Parameters.AddWithValue("@Nom", usager.Nom);
        command.Parameters.AddWithValue("@Email", usager.Email);
        command.Parameters.AddWithValue("@Telephone", usager.Telephone);

        var result = await command.ExecuteScalarAsync();
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<bool> UpdateAsync(Usager usager)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"UPDATE Usagers 
              SET Nom = @Nom, Email = @Email, Telephone = @Telephone
              WHERE IdUsager = @Id",
            connection);

        command.Parameters.AddWithValue("@Id", usager.IdUsager);
        command.Parameters.AddWithValue("@Nom", usager.Nom);
        command.Parameters.AddWithValue("@Email", usager.Email);
        command.Parameters.AddWithValue("@Telephone", usager.Telephone);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            "DELETE FROM Usagers WHERE IdUsager = @Id",
            connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    private static Usager MapReaderToUsager(IDataReader reader)
    {
        return new Usager
        {
            IdUsager = reader.GetInt32(0),
            Nom = reader.GetString(1),
            Email = reader.GetString(2),
            Telephone = reader.GetString(3)
        };
    }
}

