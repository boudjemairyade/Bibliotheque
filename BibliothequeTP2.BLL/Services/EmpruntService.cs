using BibliothequeTP2.DAL;
using BibliothequeTP2.DAL.Repositories;
using BibliothequeTP2.Entities;

namespace BibliothequeTP2.BLL.Services;


public class EmpruntService
{
    private readonly EmpruntRepository _empruntRepository;
    private readonly LivreRepository _livreRepository;
    private readonly UsagerRepository _usagerRepository;

    public EmpruntService(DbConnection dbConnection)
    {
        _empruntRepository = new EmpruntRepository(dbConnection);
        _livreRepository = new LivreRepository(dbConnection);
        _usagerRepository = new UsagerRepository(dbConnection);
    }

    public async Task<IEnumerable<Emprunt>> GetAllEmpruntsAsync()
    {
        return await _empruntRepository.GetAllAsync();
    }

    public async Task<Emprunt?> GetEmpruntByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID de l'emprunt doit être supérieur à 0", nameof(id));

        return await _empruntRepository.GetByIdAsync(id);
    }

    public async Task<Emprunt> CreateEmpruntAsync(Emprunt emprunt)
    {
        ValidateEmprunt(emprunt);

        
        var livre = await _livreRepository.GetByIdAsync(emprunt.IdLivre);
        if (livre == null)
            throw new InvalidOperationException($"Le livre avec l'ID {emprunt.IdLivre} n'existe pas");

        
        var usager = await _usagerRepository.GetByIdAsync(emprunt.IdUsager);
        if (usager == null)
            throw new InvalidOperationException($"L'usager avec l'ID {emprunt.IdUsager} n'existe pas");

        
        if (livre.Quantite <= 0)
            throw new InvalidOperationException("Le livre n'est pas disponible (quantité = 0)");

       
        var success = await _empruntRepository.BorrowBookAsync(emprunt);
        if (!success)
            throw new InvalidOperationException("Impossible de créer l'emprunt. Le livre n'est peut-être pas disponible.");

        return emprunt;
    }

    public async Task<bool> UpdateEmpruntAsync(Emprunt emprunt)
    {
        ValidateEmprunt(emprunt);

        if (emprunt.IdEmprunt <= 0)
            throw new ArgumentException("L'ID de l'emprunt doit être supérieur à 0", nameof(emprunt));

        return await _empruntRepository.UpdateAsync(emprunt);
    }

    public async Task<bool> DeleteEmpruntAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID de l'emprunt doit être supérieur à 0", nameof(id));

        return await _empruntRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Emprunt>> GetEmpruntsByUsagerAsync(int usagerId)
    {
        if (usagerId <= 0)
            throw new ArgumentException("L'ID de l'usager doit être supérieur à 0", nameof(usagerId));

        return await _empruntRepository.GetByUsagerIdAsync(usagerId);
    }

    public async Task<bool> ReturnBookAsync(int empruntId)
    {
        if (empruntId <= 0)
            throw new ArgumentException("L'ID de l'emprunt doit être supérieur à 0", nameof(empruntId));

        return await _empruntRepository.ReturnBookAsync(empruntId);
    }

    public async Task<EmpruntReport> GenerateEmpruntReportAsync(int usagerId)
    {
        var usager = await _usagerRepository.GetByIdAsync(usagerId);
        if (usager == null)
            throw new InvalidOperationException($"L'usager avec l'ID {usagerId} n'existe pas");

        var emprunts = await _empruntRepository.GetByUsagerIdAsync(usagerId);
        
        return new EmpruntReport
        {
            Usager = usager,
            Emprunts = emprunts.ToList(),
            DateGeneration = DateTime.Now
        };
    }

    private static void ValidateEmprunt(Emprunt emprunt)
    {
        if (emprunt.IdLivre <= 0)
            throw new ArgumentException("L'ID du livre doit être supérieur à 0", nameof(emprunt));

        if (emprunt.IdUsager <= 0)
            throw new ArgumentException("L'ID de l'usager doit être supérieur à 0", nameof(emprunt));

        if (emprunt.DateRetourPrevue <= emprunt.DateEmprunt)
            throw new ArgumentException("La date de retour prévue doit être postérieure à la date d'emprunt", nameof(emprunt));
    }
}


public class EmpruntReport
{
    public Usager Usager { get; set; } = null!;
    public List<Emprunt> Emprunts { get; set; } = new();
    public DateTime DateGeneration { get; set; }

    public int NombreTotal => Emprunts.Count;
    public int NombreEnCours => Emprunts.Count(e => !e.EstRetourne);
    public int NombreRetard => Emprunts.Count(e => e.EstEnRetard);
    public int NombreRetournes => Emprunts.Count(e => e.EstRetourne);
}

