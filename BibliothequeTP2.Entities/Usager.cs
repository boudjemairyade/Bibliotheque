namespace BibliothequeTP2.Entities;

/// <summary>
/// Représente un usager de la bibliothèque
/// </summary>
public class Usager
{
    public int IdUsager { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Nom} - {Email} - {Telephone}";
    }
}

