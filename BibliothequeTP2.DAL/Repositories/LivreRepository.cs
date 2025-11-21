using System.Data;
using System.Data.SqlClient;
using BibliothequeTP2.DAL;
using BibliothequeTP2.Entities;

namespace BibliothequeTP2.DAL.Repositories;


public class LivreRepository : IRepository<Livre>
{
    private readonly DbConnection _dbConnection;

    public LivreRepository(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Livre>> GetAllAsync()
    {
        var livres = new List<Livre>();
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            "SELECT IdLivre, Titre, Auteur, Annee, ISBN, Categorie, Quantite FROM Livres ORDER BY Titre",
            connection);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            livres.Add(MapReaderToLivre(reader));
        }

        return livres;
    }

    public async Task<Livre?> GetByIdAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            "SELECT IdLivre, Titre, Auteur, Annee, ISBN, Categorie, Quantite FROM Livres WHERE IdLivre = @Id",
            connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapReaderToLivre(reader);
        }

        return null;
    }

    public async Task<int> CreateAsync(Livre livre)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"INSERT INTO Livres (Titre, Auteur, Annee, ISBN, Categorie, Quantite) 
              OUTPUT INSERTED.IdLivre
              VALUES (@Titre, @Auteur, @Annee, @ISBN, @Categorie, @Quantite)",
            connection);

        command.Parameters.AddWithValue("@Titre", livre.Titre);
        command.Parameters.AddWithValue("@Auteur", livre.Auteur);
        command.Parameters.AddWithValue("@Annee", livre.Annee);
        command.Parameters.AddWithValue("@ISBN", livre.ISBN);
        command.Parameters.AddWithValue("@Categorie", livre.Categorie);
        command.Parameters.AddWithValue("@Quantite", livre.Quantite);

        var result = await command.ExecuteScalarAsync();
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<bool> UpdateAsync(Livre livre)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"UPDATE Livres 
              SET Titre = @Titre, Auteur = @Auteur, Annee = @Annee, 
                  ISBN = @ISBN, Categorie = @Categorie, Quantite = @Quantite
              WHERE IdLivre = @Id",
            connection);

        command.Parameters.AddWithValue("@Id", livre.IdLivre);
        command.Parameters.AddWithValue("@Titre", livre.Titre);
        command.Parameters.AddWithValue("@Auteur", livre.Auteur);
        command.Parameters.AddWithValue("@Annee", livre.Annee);
        command.Parameters.AddWithValue("@ISBN", livre.ISBN);
        command.Parameters.AddWithValue("@Categorie", livre.Categorie);
        command.Parameters.AddWithValue("@Quantite", livre.Quantite);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            "DELETE FROM Livres WHERE IdLivre = @Id",
            connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Livre>> GetAvailableBooksAsync()
    {
        var livres = new List<Livre>();
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"SELECT l.IdLivre, l.Titre, l.Auteur, l.Annee, l.ISBN, l.Categorie, l.Quantite
              FROM Livres l
              WHERE l.Quantite > 0
              ORDER BY l.Titre",
            connection);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            livres.Add(MapReaderToLivre(reader));
        }

        return livres;
    }

    private static Livre MapReaderToLivre(IDataReader reader)
    {
        return new Livre
        {
            IdLivre = reader.GetInt32(0),
            Titre = reader.GetString(1),
            Auteur = reader.GetString(2),
            Annee = reader.GetInt32(3),
            ISBN = reader.GetString(4),
            Categorie = reader.GetString(5),
            Quantite = reader.GetInt32(6)
        };
    }
}

