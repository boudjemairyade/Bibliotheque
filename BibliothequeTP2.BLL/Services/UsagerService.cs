using BibliothequeTP2.DAL;
using BibliothequeTP2.DAL.Repositories;
using BibliothequeTP2.Entities;

namespace BibliothequeTP2.BLL.Services;


public class UsagerService
{
    private readonly UsagerRepository _repository;

    public UsagerService(DbConnection dbConnection)
    {
        _repository = new UsagerRepository(dbConnection);
    }

    public async Task<IEnumerable<Usager>> GetAllUsagersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Usager?> GetUsagerByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID de l'usager doit être supérieur à 0", nameof(id));

        return await _repository.GetByIdAsync(id);
    }

    public async Task<Usager> CreateUsagerAsync(Usager usager)
    {
        ValidateUsager(usager);

        var id = await _repository.CreateAsync(usager);
        usager.IdUsager = id;
        return usager;
    }

    public async Task<bool> UpdateUsagerAsync(Usager usager)
    {
        ValidateUsager(usager);

        if (usager.IdUsager <= 0)
            throw new ArgumentException("L'ID de l'usager doit être supérieur à 0", nameof(usager));

        return await _repository.UpdateAsync(usager);
    }

    public async Task<bool> DeleteUsagerAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID de l'usager doit être supérieur à 0", nameof(id));

        return await _repository.DeleteAsync(id);
    }

    private static void ValidateUsager(Usager usager)
    {
        if (string.IsNullOrWhiteSpace(usager.Nom))
            throw new ArgumentException("Le nom de l'usager est requis", nameof(usager));

        if (string.IsNullOrWhiteSpace(usager.Email))
            throw new ArgumentException("L'email de l'usager est requis", nameof(usager));

        if (string.IsNullOrWhiteSpace(usager.Telephone))
            throw new ArgumentException("Le téléphone de l'usager est requis", nameof(usager));

        
        if (!usager.Email.Contains('@') || !usager.Email.Contains('.'))
            throw new ArgumentException("L'email n'est pas dans un format valide", nameof(usager));
    }
}

