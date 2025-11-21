using System.Data;
using System.Data.SqlClient;
using BibliothequeTP2.DAL;
using BibliothequeTP2.Entities;

namespace BibliothequeTP2.DAL.Repositories;


public class EmpruntRepository : IRepository<Emprunt>
{
    private readonly DbConnection _dbConnection;

    public EmpruntRepository(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Emprunt>> GetAllAsync()
    {
        var emprunts = new List<Emprunt>();
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"SELECT e.IdEmprunt, e.DateEmprunt, e.DateRetourPrevue, e.DateRetourReelle, 
                     e.IdLivre, e.IdUsager,
                     l.Titre, l.Auteur, l.Annee, l.ISBN, l.Categorie, l.Quantite,
                     u.Nom, u.Email, u.Telephone
              FROM Emprunts e
              INNER JOIN Livres l ON e.IdLivre = l.IdLivre
              INNER JOIN Usagers u ON e.IdUsager = u.IdUsager
              ORDER BY e.DateEmprunt DESC",
connection);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            emprunts.Add(MapReaderToEmprunt(reader));
        }

        return emprunts;
    }

    public async Task<Emprunt?> GetByIdAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"SELECT e.IdEmprunt, e.DateEmprunt, e.DateRetourPrevue, e.DateRetourReelle, 
                     e.IdLivre, e.IdUsager,
                     l.Titre, l.Auteur, l.Annee, l.ISBN, l.Categorie, l.Quantite,
                     u.Nom, u.Email, u.Telephone
              FROM Emprunts e
              INNER JOIN Livres l ON e.IdLivre = l.IdLivre
              INNER JOIN Usagers u ON e.IdUsager = u.IdUsager
              WHERE e.IdEmprunt = @Id",
connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapReaderToEmprunt(reader);
        }

        return null;
    }

    public async Task<int> CreateAsync(Emprunt emprunt)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"INSERT INTO Emprunts (DateEmprunt, DateRetourPrevue, IdLivre, IdUsager) 
              OUTPUT INSERTED.IdEmprunt
              VALUES (@DateEmprunt, @DateRetourPrevue, @IdLivre, @IdUsager)",
connection);

        command.Parameters.AddWithValue("@DateEmprunt", emprunt.DateEmprunt);
        command.Parameters.AddWithValue("@DateRetourPrevue", emprunt.DateRetourPrevue);
        command.Parameters.AddWithValue("@IdLivre", emprunt.IdLivre);
        command.Parameters.AddWithValue("@IdUsager", emprunt.IdUsager);

        var result = await command.ExecuteScalarAsync();
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<bool> UpdateAsync(Emprunt emprunt)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"UPDATE Emprunts 
              SET DateEmprunt = @DateEmprunt, DateRetourPrevue = @DateRetourPrevue, 
                  DateRetourReelle = @DateRetourReelle, IdLivre = @IdLivre, IdUsager = @IdUsager
              WHERE IdEmprunt = @Id",
connection);

        command.Parameters.AddWithValue("@Id", emprunt.IdEmprunt);
        command.Parameters.AddWithValue("@DateEmprunt", emprunt.DateEmprunt);
        command.Parameters.AddWithValue("@DateRetourPrevue", emprunt.DateRetourPrevue);
        command.Parameters.AddWithValue("@IdLivre", emprunt.IdLivre);
        command.Parameters.AddWithValue("@IdUsager", emprunt.IdUsager);
        
        if (emprunt.DateRetourReelle.HasValue)
            command.Parameters.AddWithValue("@DateRetourReelle", emprunt.DateRetourReelle);
        else
            command.Parameters.AddWithValue("@DateRetourReelle", DBNull.Value);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            "DELETE FROM Emprunts WHERE IdEmprunt = @Id",
connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Emprunt>> GetByUsagerIdAsync(int usagerId)
    {
        var emprunts = new List<Emprunt>();
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"SELECT e.IdEmprunt, e.DateEmprunt, e.DateRetourPrevue, e.DateRetourReelle, 
                     e.IdLivre, e.IdUsager,
                     l.Titre, l.Auteur, l.Annee, l.ISBN, l.Categorie, l.Quantite,
                     u.Nom, u.Email, u.Telephone
              FROM Emprunts e
              INNER JOIN Livres l ON e.IdLivre = l.IdLivre
              INNER JOIN Usagers u ON e.IdUsager = u.IdUsager
              WHERE e.IdUsager = @UsagerId
              ORDER BY e.DateEmprunt DESC",
connection);
        command.Parameters.AddWithValue("@UsagerId", usagerId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            emprunts.Add(MapReaderToEmprunt(reader));
        }

        return emprunts;
    }

    public async Task<bool> ReturnBookAsync(int empruntId)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(
            @"UPDATE Emprunts 
              SET DateRetourReelle = GETDATE()
              WHERE IdEmprunt = @Id AND DateRetourReelle IS NULL",
connection);
        command.Parameters.AddWithValue("@Id", empruntId);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        
        if (rowsAffected > 0)
        {
            var updateQuantiteCommand = new SqlCommand(
                @"UPDATE Livres 
                  SET Quantite = Quantite + 1
                  WHERE IdLivre = (SELECT IdLivre FROM Emprunts WHERE IdEmprunt = @Id)",
    connection);
            updateQuantiteCommand.Parameters.AddWithValue("@Id", empruntId);
            await updateQuantiteCommand.ExecuteNonQueryAsync();
        }

        return rowsAffected > 0;
    }

    public async Task<bool> BorrowBookAsync(Emprunt emprunt)
    {
        using var connection = _dbConnection.CreateConnection();
        await connection.OpenAsync();
        
        using var transaction = connection.BeginTransaction();

        try
        {
          
            var checkCommand = new SqlCommand(
                "SELECT Quantite FROM Livres WHERE IdLivre = @Id",
    connection,
                transaction as SqlTransaction);
            checkCommand.Parameters.AddWithValue("@Id", emprunt.IdLivre);
            
            var quantite = (int?)await checkCommand.ExecuteScalarAsync();
            if (quantite == null || quantite <= 0)
            {
                transaction.Rollback();
                return false;
            }

            
            var empruntCommand = new SqlCommand(
                @"INSERT INTO Emprunts (DateEmprunt, DateRetourPrevue, IdLivre, IdUsager) 
                  OUTPUT INSERTED.IdEmprunt
                  VALUES (@DateEmprunt, @DateRetourPrevue, @IdLivre, @IdUsager)",
    connection,
                transaction as SqlTransaction);

            empruntCommand.Parameters.AddWithValue("@DateEmprunt", emprunt.DateEmprunt);
            empruntCommand.Parameters.AddWithValue("@DateRetourPrevue", emprunt.DateRetourPrevue);
            empruntCommand.Parameters.AddWithValue("@IdLivre", emprunt.IdLivre);
            empruntCommand.Parameters.AddWithValue("@IdUsager", emprunt.IdUsager);

            var idResult = await empruntCommand.ExecuteScalarAsync();
            if (idResult != null)
            {
                emprunt.IdEmprunt = Convert.ToInt32(idResult);
            }

            
            var updateCommand = new SqlCommand(
                "UPDATE Livres SET Quantite = Quantite - 1 WHERE IdLivre = @Id",
    connection,
                transaction as SqlTransaction);
            updateCommand.Parameters.AddWithValue("@Id", emprunt.IdLivre);
            await updateCommand.ExecuteNonQueryAsync();

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

    private static Emprunt MapReaderToEmprunt(IDataReader reader)
    {
        var emprunt = new Emprunt
        {
            IdEmprunt = reader.GetInt32(0),
            DateEmprunt = reader.GetDateTime(1),
            DateRetourPrevue = reader.GetDateTime(2),
            IdLivre = reader.GetInt32(4),
            IdUsager = reader.GetInt32(5)
        };

        
        if (!reader.IsDBNull(3))
        {
            emprunt.DateRetourReelle = reader.GetDateTime(3);
        }

        // Livre
        emprunt.Livre = new Livre
        {
            IdLivre = reader.GetInt32(4),
            Titre = reader.GetString(6),
            Auteur = reader.GetString(7),
            Annee = reader.GetInt32(8),
            ISBN = reader.GetString(9),
            Categorie = reader.GetString(10),
            Quantite = reader.GetInt32(11)
        };

        // Usager
        emprunt.Usager = new Usager
        {
            IdUsager = reader.GetInt32(5),
            Nom = reader.GetString(12),
            Email = reader.GetString(13),
            Telephone = reader.GetString(14)
        };

        return emprunt;
    }
}

