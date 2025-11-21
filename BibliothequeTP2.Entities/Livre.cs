namespace BibliothequeTP2.Entities;

/// <summary>
/// Représente un livre dans la bibliothèque
/// </summary>
public class Livre
{
    public int IdLivre { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Auteur { get; set; } = string.Empty;
    public int Annee { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public int Quantite { get; set; }

    public override string ToString()
    {
        return $"{Titre} par {Auteur} ({Annee}) - ISBN: {ISBN} - Catégorie: {Categorie} - Quantité: {Quantite}";
    }
}

