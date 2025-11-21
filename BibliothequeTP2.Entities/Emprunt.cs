namespace BibliothequeTP2.Entities;

/// <summary>
/// Représente un emprunt de livre par un usager
/// </summary>
public class Emprunt
{
    public int IdEmprunt { get; set; }
    public DateTime DateEmprunt { get; set; }
    public DateTime DateRetourPrevue { get; set; }
    public DateTime? DateRetourReelle { get; set; }
    public int IdLivre { get; set; }
    public int IdUsager { get; set; }

    // Propriétés de navigation (optionnel)
    public Livre? Livre { get; set; }
    public Usager? Usager { get; set; }

    public bool EstRetourne => DateRetourReelle.HasValue;
    public bool EstEnRetard => !EstRetourne && DateTime.Now > DateRetourPrevue;

    public override string ToString()
    {
        var livreInfo = Livre != null ? Livre.Titre : $"Livre ID: {IdLivre}";
        var usagerInfo = Usager != null ? Usager.Nom : $"Usager ID: {IdUsager}";
        var statut = EstRetourne ? "Retourné" : (EstEnRetard ? "En retard" : "En cours");
        return $"Emprunt #{IdEmprunt} - {livreInfo} par {usagerInfo} - Emprunt: {DateEmprunt:yyyy-MM-dd} - Retour prévu: {DateRetourPrevue:yyyy-MM-dd} - Statut: {statut}";
    }
}

