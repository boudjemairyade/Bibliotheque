using BibliothequeTP2.DAL;
using BibliothequeTP2.DAL.Repositories;
using BibliothequeTP2.Entities;

namespace BibliothequeTP2.BLL.Services;


public class LivreService
{
    private readonly LivreRepository _repository;

    public LivreService(DbConnection dbConnection)
    {
        _repository = new LivreRepository(dbConnection);
    }

    public async Task<IEnumerable<Livre>> GetAllLivresAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Livre?> GetLivreByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID du livre doit être supérieur à 0", nameof(id));

        return await _repository.GetByIdAsync(id);
    }

    public async Task<Livre> CreateLivreAsync(Livre livre)
    {
        ValidateLivre(livre);

        var id = await _repository.CreateAsync(livre);
        livre.IdLivre = id;
        return livre;
    }

    public async Task<bool> UpdateLivreAsync(Livre livre)
    {
        ValidateLivre(livre);

        if (livre.IdLivre <= 0)
            throw new ArgumentException("L'ID du livre doit être supérieur à 0", nameof(livre));

        return await _repository.UpdateAsync(livre);
    }

    public async Task<bool> DeleteLivreAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID du livre doit être supérieur à 0", nameof(id));

        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Livre>> GetAvailableBooksAsync()
    {
        return await _repository.GetAvailableBooksAsync();
    }

    private static void ValidateLivre(Livre livre)
    {
        if (string.IsNullOrWhiteSpace(livre.Titre))
            throw new ArgumentException("Le titre du livre est requis", nameof(livre));

        if (string.IsNullOrWhiteSpace(livre.Auteur))
            throw new ArgumentException("L'auteur du livre est requis", nameof(livre));

        if (string.IsNullOrWhiteSpace(livre.ISBN))
            throw new ArgumentException("L'ISBN du livre est requis", nameof(livre));

        if (livre.Annee < 0 || livre.Annee > DateTime.Now.Year + 1)
            throw new ArgumentException("L'année du livre n'est pas valide", nameof(livre));

        if (string.IsNullOrWhiteSpace(livre.Categorie))
            throw new ArgumentException("La catégorie du livre est requise", nameof(livre));

        if (livre.Quantite < 0)
            throw new ArgumentException("La quantité ne peut pas être négative", nameof(livre));
    }
}

