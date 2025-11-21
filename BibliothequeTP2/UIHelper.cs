namespace BibliothequeTP2;


public static class UIHelper
{
    
    public static void AfficherTitre(string titre)
    {
        var ligne = new string('_', Math.Max(titre.Length + 4, 50));
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(ligne);
        Console.WriteLine($"  {titre.ToUpper()}");
        Console.WriteLine(ligne);
        Console.ResetColor();
        Console.WriteLine();
    }

   
    public static void AfficherSucces(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        
        Console.ResetColor();
        Console.WriteLine(message);
    }

    
    public static void AfficherErreur(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        
        Console.ResetColor();
        Console.WriteLine(message);
    }

    
    public static void AfficherAvertissement(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        
        Console.ResetColor();
        Console.WriteLine(message);
    }

    
    public static void AfficherInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
       
        Console.ResetColor();
        Console.WriteLine(message);
    }

    
    public static void AfficherSeparateur(char caractere = '─', int longueur = 50)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string(caractere, longueur));
        Console.ResetColor();
    }

    
    public static void AfficherOption(int numero, string description)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{numero}.");
        Console.ResetColor();
        Console.WriteLine($" {description}");
    }

   
    public static void AfficherSousOption(string description)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        
        Console.ResetColor();
        Console.WriteLine(description);
    }

   
    public static void AfficherListeVide(string element)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($" Aucun(e) {element} trouvé(e) pour le moment.");
        Console.ResetColor();
        Console.WriteLine();
    }

    
    public static string DemanderSaisie(string libelle, bool obligatoire = true)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{libelle}");
        if (obligatoire)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" *");
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" ");
        Console.ResetColor();
        
        var saisie = Console.ReadLine()?.Trim() ?? string.Empty;
        return saisie;
    }

    
    public static bool DemanderConfirmation(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" {message} (O/N): ");
        Console.ResetColor();
        
        var reponse = Console.ReadLine()?.Trim().ToUpper();
        return reponse == "O" || reponse == "OUI" || reponse == "Y" || reponse == "YES";
    }

   
    public static void AfficherTableau(List<string[]> lignes, string[] enTetes)
    {
        if (lignes == null || lignes.Count == 0)
        {
            Console.WriteLine("Aucune donnée à afficher.");
            return;
        }

        // Calculer les largeurs de colonnes
        var largeurs = new int[enTetes.Length];
        for (int i = 0; i < enTetes.Length; i++)
        {
            largeurs[i] = Math.Max(enTetes[i].Length, 
                lignes.Max(l => l.Length > i ? (l[i]?.Length ?? 0) : 0));
        }

        // Afficher l'en-tête
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("");
        for (int i = 0; i < enTetes.Length; i++)
        {
            Console.Write(new string('_', largeurs[i] + 2));
            if (i < enTetes.Length - 1) Console.Write("");
        }
        Console.WriteLine("");

        Console.Write("");
        for (int i = 0; i < enTetes.Length; i++)
        {
            Console.Write($" {enTetes[i].PadRight(largeurs[i])} ");
        }
        Console.WriteLine();

        Console.Write("");
        for (int i = 0; i < enTetes.Length; i++)
        {
            Console.Write(new string('_', largeurs[i] + 2));
            if (i < enTetes.Length - 1) Console.Write("");
        }
        Console.WriteLine("");
        Console.ResetColor();

       
        foreach (var ligne in lignes)
        {
            Console.Write("");
            for (int i = 0; i < enTetes.Length; i++)
            {
                var valeur = i < ligne.Length ? (ligne[i] ?? "") : "";
                if (valeur.Length > largeurs[i])
                    valeur = valeur.Substring(0, largeurs[i] - 3) + "...";
                Console.Write($" {valeur.PadRight(largeurs[i])} ");
            }
            Console.WriteLine();
        }

        // Afficher le pied
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("");
        for (int i = 0; i < enTetes.Length; i++)
        {
            Console.Write(new string('_', largeurs[i] + 2));
            if (i < enTetes.Length - 1) Console.Write("");
        }
        Console.WriteLine("");
        Console.ResetColor();
    }

    /// <summary>
    /// Attend une action de l'utilisateur
    /// </summary>
    public static void AttendreUtilisateur(string message = "Appuyez sur une touche pour continuer...")
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"{message}");
        Console.ResetColor();
        Console.ReadKey(true);
    }

   
    public static void AfficherBienvenue()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
       
       
        Console.WriteLine("      APPLICATION DE GESTION DE BIBLIOTHÈQUE      ");
       ;
        Console.ResetColor();
        Console.WriteLine();
    }

   
    public static void AfficherAuRevoir()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
       
        Console.WriteLine("                                                       ");
        Console.WriteLine("              Merci d'avoir utilisé notre              ");
        Console.WriteLine("          Application de Gestion de Bibliothèque       ");
        
        Console.WriteLine("                    À bientôt !                    ");
      
        Console.ResetColor();
        Console.WriteLine();
    }
}

