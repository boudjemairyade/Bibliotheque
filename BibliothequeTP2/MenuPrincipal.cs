using BibliothequeTP2.BLL.Services;
using BibliothequeTP2.Entities;

namespace BibliothequeTP2;

public class MenuPrincipal
{
    private readonly LivreService _livreService;
    private readonly UsagerService _usagerService;
    private readonly EmpruntService _empruntService;

    public MenuPrincipal(LivreService livreService, UsagerService usagerService, EmpruntService empruntService)
    {
        _livreService = livreService;
        _usagerService = usagerService;
        _empruntService = empruntService;
    }

    public async Task AfficherMenuAsync()
    {
        while (true)
        {
            UIHelper.AfficherBienvenue();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("                    MENU PRINCIPAL");
            Console.ResetColor();
            UIHelper.AfficherSeparateur();
            Console.WriteLine();
            
            UIHelper.AfficherOption(1, " Gestion des Livres");
            UIHelper.AfficherOption(2, " Gestion des Usagers");
            UIHelper.AfficherOption(3, " Gestion des Emprunts");
            UIHelper.AfficherOption(4, " Rapports");
            UIHelper.AfficherOption(0, " Quitter l'application");
            
            Console.WriteLine();
            UIHelper.AfficherSeparateur();
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Votre choix: ");
            Console.ResetColor();

            var choix = Console.ReadLine();

            try
            {
                switch (choix)
                {
                    case "1":
                        await MenuLivresAsync();
                        break;
                    case "2":
                        await MenuUsagersAsync();
                        break;
                    case "3":
                        await MenuEmpruntsAsync();
                        break;
                    case "4":
                        await MenuRapportsAsync();
                        break;
                    case "0":
                        UIHelper.AfficherAuRevoir();
                        return;
                    default:
                        UIHelper.AfficherErreur("Choix invalide. Veuillez choisir une option du menu.");
                        UIHelper.AttendreUtilisateur();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                UIHelper.AfficherErreur($"Une erreur s'est produite: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"   Détails: {ex.InnerException.Message}");
                    Console.ResetColor();
                }
                UIHelper.AttendreUtilisateur();
            }
        }
    }

    #region Menu Livres

    private async Task MenuLivresAsync()
    {
        while (true)
        {
            Console.Clear();
            UIHelper.AfficherTitre(" GESTION DES LIVRES");
            
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Options disponibles:");
            Console.ResetColor();
            Console.WriteLine();
            
            UIHelper.AfficherOption(1, "Voir tous les livres");
            UIHelper.AfficherOption(2, "Voir les livres disponibles");
            UIHelper.AfficherOption(3, "Rechercher un livre par ID");
            UIHelper.AfficherOption(4, "Ajouter un nouveau livre");
            UIHelper.AfficherOption(5, "Modifier un livre existant");
            UIHelper.AfficherOption(6, "Supprimer un livre");
            UIHelper.AfficherOption(0, "← Retour au menu principal");
            
            Console.WriteLine();
            UIHelper.AfficherSeparateur();
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Votre choix: ");
            Console.ResetColor();

            var choix = Console.ReadLine();

            try
            {
                switch (choix)
                {
                    case "1":
                        await ListerLivresAsync();
                        break;
                    case "2":
                        await ListerLivresDisponiblesAsync();
                        break;
                    case "3":
                        await RechercherLivreAsync();
                        break;
                    case "4":
                        await AjouterLivreAsync();
                        break;
                    case "5":
                        await ModifierLivreAsync();
                        break;
                    case "6":
                        await SupprimerLivreAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nChoix invalide. Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERREUR: {ex.Message}");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
    }

    private async Task ListerLivresAsync()
    {
        Console.Clear();
        UIHelper.AfficherTitre(" LISTE DE TOUS LES LIVRES");

        var livres = await _livreService.GetAllLivresAsync();
        if (!livres.Any())
        {
            UIHelper.AfficherListeVide("livre");
        }
        else
        {
            UIHelper.AfficherInfo($"Nombre total de livres: {livres.Count()}");
            Console.WriteLine();
            
            var lignes = new List<string[]>();
            foreach (var livre in livres)
            {
                lignes.Add(new[]
                {
                    livre.IdLivre.ToString(),
                    livre.Titre,
                    livre.Auteur,
                    livre.Annee.ToString(),
                    livre.Categorie,
                    livre.Quantite.ToString()
                });
            }

            UIHelper.AfficherTableau(lignes, new[] { "ID", "Titre", "Auteur", "Année", "Catégorie", "Quantité" });
            Console.WriteLine();
            UIHelper.AfficherSucces($"Affichage réussi de {livres.Count()} livre(s)!");
        }

        UIHelper.AttendreUtilisateur();
    }

    private async Task ListerLivresDisponiblesAsync()
    {
        Console.Clear();
        UIHelper.AfficherTitre(" LIVRES DISPONIBLES");

        var livres = await _livreService.GetAvailableBooksAsync();
        if (!livres.Any())
        {
            UIHelper.AfficherAvertissement("Aucun livre disponible pour le moment. Tous les exemplaires sont empruntés.");
            Console.WriteLine();
        }
        else
        {
            UIHelper.AfficherSucces($"Il y a {livres.Count()} livre(s) disponible(s) à l'emprunt!");
            Console.WriteLine();
            
            var lignes = new List<string[]>();
            foreach (var livre in livres)
            {
                lignes.Add(new[]
                {
                    livre.IdLivre.ToString(),
                    livre.Titre,
                    livre.Auteur,
                    livre.Quantite.ToString() + " ex."
                });
            }

            UIHelper.AfficherTableau(lignes, new[] { "ID", "Titre", "Auteur", "Exemplaires" });
        }

        UIHelper.AttendreUtilisateur();
    }

    private async Task RechercherLivreAsync()
    {
        Console.Clear();
        UIHelper.AfficherTitre(" RECHERCHER UN LIVRE");
        Console.WriteLine();

        var idStr = UIHelper.DemanderSaisie("ID du livre", true);
        
        if (int.TryParse(idStr, out var id))
        {
            Console.WriteLine();
            UIHelper.AfficherInfo("Recherche en cours...");
            Console.WriteLine();
            
            var livre = await _livreService.GetLivreByIdAsync(id);
            if (livre != null)
            {
                UIHelper.AfficherSucces("Livre trouvé!");
                Console.WriteLine();
                UIHelper.AfficherSeparateur('─', 55);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"  Titre      : {livre.Titre}");
                Console.WriteLine($"  Auteur     : {livre.Auteur}");
                Console.WriteLine($"  Année      : {livre.Annee}");
                Console.WriteLine($"  ISBN       : {livre.ISBN}");
                Console.WriteLine($"  Catégorie  : {livre.Categorie}");
                Console.ForegroundColor = livre.Quantite > 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"  Quantité   : {livre.Quantite} exemplaire(s)");
                Console.ResetColor();
                UIHelper.AfficherSeparateur();
            }
            else
            {
                UIHelper.AfficherErreur($"Aucun livre trouvé avec l'ID {id}.");
            }
        }
        else
        {
            UIHelper.AfficherErreur("L'ID doit être un nombre entier valide.");
        }

        UIHelper.AttendreUtilisateur();
    }

    private async Task AjouterLivreAsync()
    {
        Console.Clear();
        UIHelper.AfficherTitre(" AJOUTER UN NOUVEAU LIVRE");
        Console.WriteLine();
        UIHelper.AfficherInfo("Veuillez remplir les informations suivantes:");
        Console.WriteLine();

        var livre = new Livre();
        
        livre.Titre = UIHelper.DemanderSaisie("Titre du livre", true);
        livre.Auteur = UIHelper.DemanderSaisie("Auteur", true);
        
        var anneeStr = UIHelper.DemanderSaisie("Année de publication", true);
        if (int.TryParse(anneeStr, out var annee))
            livre.Annee = annee;
        else
        {
            UIHelper.AfficherErreur("Année invalide.");
            UIHelper.AttendreUtilisateur();
            return;
        }
        
        livre.ISBN = UIHelper.DemanderSaisie("ISBN", true);
        livre.Categorie = UIHelper.DemanderSaisie("Catégorie", true);
        
        var quantiteStr = UIHelper.DemanderSaisie("Quantité d'exemplaires", false);
        if (string.IsNullOrWhiteSpace(quantiteStr))
            livre.Quantite = 1;
        else if (int.TryParse(quantiteStr, out var quantite))
            livre.Quantite = quantite;
        else
        {
            UIHelper.AfficherErreur("Quantité invalide. Valeur par défaut: 1");
            livre.Quantite = 1;
        }

        try
        {
            Console.WriteLine();
            UIHelper.AfficherInfo("Ajout du livre en cours...");
            await _livreService.CreateLivreAsync(livre);
            Console.WriteLine();
            UIHelper.AfficherSucces($"Livre ajouté avec succès!");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"   ID attribué: {livre.IdLivre}");
            Console.WriteLine($"   Titre: {livre.Titre}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            UIHelper.AfficherErreur($"Impossible d'ajouter le livre: {ex.Message}");
        }

        UIHelper.AttendreUtilisateur();
    }

    private async Task ModifierLivreAsync()
    {
        Console.Clear();
        Console.WriteLine("MODIFIER UN LIVRE");
        Console.WriteLine("_________________\n");
        Console.Write("ID du livre à modifier: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            var livre = await _livreService.GetLivreByIdAsync(id);
            if (livre != null)
            {
                Console.WriteLine($"\nLivre actuel: {livre}\n");
                Console.Write("Nouveau titre (Entrée pour garder): ");
                var titre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(titre))
                    livre.Titre = titre;

                Console.Write("Nouvel auteur (Entrée pour garder): ");
                var auteur = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(auteur))
                    livre.Auteur = auteur;

                Console.Write("Nouvelle année (Entrée pour garder): ");
                var anneeStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(anneeStr) && int.TryParse(anneeStr, out var annee))
                    livre.Annee = annee;

                Console.Write("Nouvel ISBN (Entrée pour garder): ");
                var isbn = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(isbn))
                    livre.ISBN = isbn;

                Console.Write("Nouvelle catégorie (Entrée pour garder): ");
                var categorie = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(categorie))
                    livre.Categorie = categorie;

                Console.Write("Nouvelle quantité (Entrée pour garder): ");
                var quantiteStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(quantiteStr) && int.TryParse(quantiteStr, out var quantite))
                    livre.Quantite = quantite;

                if (await _livreService.UpdateLivreAsync(livre))
                {
                    Console.WriteLine("\nLivre modifié avec succès!");
                }
                else
                {
                    Console.WriteLine("\nErreur lors de la modification.");
                }
            }
            else
            {
                Console.WriteLine("\nLivre non trouvé.");
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task SupprimerLivreAsync()
    {
        Console.Clear();
        Console.WriteLine("SUPPRIMER UN LIVRE");
        Console.WriteLine("__________________\n");
        Console.Write("ID du livre à supprimer: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            Console.Write($"\nÊtes-vous sûr de vouloir supprimer le livre ID {id}? (O/N): ");
            var confirmation = Console.ReadLine();

            if (confirmation?.ToUpper() == "O")
            {
                if (await _livreService.DeleteLivreAsync(id))
                {
                    Console.WriteLine("\nLivre supprimé avec succès!");
                }
                else
                {
                    Console.WriteLine("\nErreur lors de la suppression ou livre non trouvé.");
                }
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    #endregion

    #region Menu Usagers

    private async Task MenuUsagersAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("       GESTION DES USAGERS");
            Console.WriteLine("______________________________________");
            Console.WriteLine();
            Console.WriteLine("1. Lister tous les usagers");
            Console.WriteLine("2. Rechercher un usager par ID");
            Console.WriteLine("3. Ajouter un usager");
            Console.WriteLine("4. Modifier un usager");
            Console.WriteLine("5. Supprimer un usager");
            Console.WriteLine("0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Votre choix: ");

            var choix = Console.ReadLine();

            try
            {
                switch (choix)
                {
                    case "1":
                        await ListerUsagersAsync();
                        break;
                    case "2":
                        await RechercherUsagerAsync();
                        break;
                    case "3":
                        await AjouterUsagerAsync();
                        break;
                    case "4":
                        await ModifierUsagerAsync();
                        break;
                    case "5":
                        await SupprimerUsagerAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nChoix invalide. Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERREUR: {ex.Message}");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
    }

    private async Task ListerUsagersAsync()
    {
        Console.Clear();
        Console.WriteLine("LISTE DES USAGERS");
        Console.WriteLine("_________________\n");

        var usagers = await _usagerService.GetAllUsagersAsync();
        if (!usagers.Any())
        {
            Console.WriteLine("Aucun usager trouvé.");
        }
        else
        {
            foreach (var usager in usagers)
            {
                Console.WriteLine($"ID: {usager.IdUsager} - {usager}");
            }
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task RechercherUsagerAsync()
    {
        Console.Clear();
        Console.WriteLine("RECHERCHER UN USAGER");
        Console.WriteLine("____________________\n");
        Console.Write("ID de l'usager: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            var usager = await _usagerService.GetUsagerByIdAsync(id);
            if (usager != null)
            {
                Console.WriteLine($"\n{usager}");
            }
            else
            {
                Console.WriteLine("\nUsager non trouvé.");
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task AjouterUsagerAsync()
    {
        Console.Clear();
        Console.WriteLine("AJOUTER UN USAGER");
        Console.WriteLine("__________________\n");

        var usager = new Usager();
        Console.Write("Nom: ");
        usager.Nom = Console.ReadLine() ?? string.Empty;
        Console.Write("Email: ");
        usager.Email = Console.ReadLine() ?? string.Empty;
        Console.Write("Téléphone: ");
        usager.Telephone = Console.ReadLine() ?? string.Empty;

        await _usagerService.CreateUsagerAsync(usager);
        Console.WriteLine($"\nUsager ajouté avec succès! ID: {usager.IdUsager}");

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task ModifierUsagerAsync()
    {
        Console.Clear();
        Console.WriteLine("MODIFIER UN USAGER");
        Console.WriteLine("___________________\n");
        Console.Write("ID de l'usager à modifier: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            var usager = await _usagerService.GetUsagerByIdAsync(id);
            if (usager != null)
            {
                Console.WriteLine($"\nUsager actuel: {usager}\n");
                Console.Write("Nouveau nom (Entrée pour garder): ");
                var nom = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nom))
                    usager.Nom = nom;

                Console.Write("Nouvel email (Entrée pour garder): ");
                var email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email))
                    usager.Email = email;

                Console.Write("Nouveau téléphone (Entrée pour garder): ");
                var telephone = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(telephone))
                    usager.Telephone = telephone;

                if (await _usagerService.UpdateUsagerAsync(usager))
                {
                    Console.WriteLine("\nUsager modifié avec succès!");
                }
                else
                {
                    Console.WriteLine("\nErreur lors de la modification.");
                }
            }
            else
            {
                Console.WriteLine("\nUsager non trouvé.");
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task SupprimerUsagerAsync()
    {
        Console.Clear();
        Console.WriteLine("SUPPRIMER UN USAGER");
        Console.WriteLine("___________________n");
        Console.Write("ID de l'usager à supprimer: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            Console.Write($"\nÊtes-vous sûr de vouloir supprimer l'usager ID {id}? (O/N): ");
            var confirmation = Console.ReadLine();

            if (confirmation?.ToUpper() == "O")
            {
                if (await _usagerService.DeleteUsagerAsync(id))
                {
                    Console.WriteLine("\nUsager supprimé avec succès!");
                }
                else
                {
                    Console.WriteLine("\nErreur lors de la suppression ou usager non trouvé.");
                }
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    #endregion

    #region Menu Emprunts

    private async Task MenuEmpruntsAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("       GESTION DES EMPRUNTS");
            Console.WriteLine("______________________________________");
            Console.WriteLine();
            Console.WriteLine("1. Lister tous les emprunts");
            Console.WriteLine("2. Rechercher un emprunt par ID");
            Console.WriteLine("3. Ajouter un emprunt");
            Console.WriteLine("4. Modifier un emprunt");
            Console.WriteLine("5. Supprimer un emprunt");
            Console.WriteLine("6. Retourner un livre");
            Console.WriteLine("7. Lister les emprunts d'un usager");
            Console.WriteLine("0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Votre choix: ");

            var choix = Console.ReadLine();

            try
            {
                switch (choix)
                {
                    case "1":
                        await ListerEmpruntsAsync();
                        break;
                    case "2":
                        await RechercherEmpruntAsync();
                        break;
                    case "3":
                        await AjouterEmpruntAsync();
                        break;
                    case "4":
                        await ModifierEmpruntAsync();
                        break;
                    case "5":
                        await SupprimerEmpruntAsync();
                        break;
                    case "6":
                        await RetournerLivreAsync();
                        break;
                    case "7":
                        await ListerEmpruntsUsagerAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nChoix invalide. Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERREUR: {ex.Message}");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
    }

    private async Task ListerEmpruntsAsync()
    {
        Console.Clear();
        Console.WriteLine("LISTE DES EMPRUNTS");
        Console.WriteLine("___________________\n");

        var emprunts = await _empruntService.GetAllEmpruntsAsync();
        if (!emprunts.Any())
        {
            Console.WriteLine("Aucun emprunt trouvé.");
        }
        else
        {
            foreach (var emprunt in emprunts)
            {
                Console.WriteLine(emprunt);
                Console.WriteLine();
            }
        }

        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task RechercherEmpruntAsync()
    {
        Console.Clear();
        Console.WriteLine("RECHERCHER UN EMPRUNT");
        Console.WriteLine("_____________________\n");
        Console.Write("ID de l'emprunt: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            var emprunt = await _empruntService.GetEmpruntByIdAsync(id);
            if (emprunt != null)
            {
                Console.WriteLine($"\n{emprunt}");
            }
            else
            {
                Console.WriteLine("\nEmprunt non trouvé.");
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task AjouterEmpruntAsync()
    {
        Console.Clear();
        Console.WriteLine("AJOUTER UN EMPRUNT");
        Console.WriteLine("___________________\n");

        var emprunt = new Emprunt
        {
            DateEmprunt = DateTime.Now
        };

        Console.Write("ID du livre: ");
        if (!int.TryParse(Console.ReadLine(), out var idLivre))
        {
            Console.WriteLine("\nID invalide.");
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
            return;
        }
        emprunt.IdLivre = idLivre;

        Console.Write("ID de l'usager: ");
        if (!int.TryParse(Console.ReadLine(), out var idUsager))
        {
            Console.WriteLine("\nID invalide.");
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
            return;
        }
        emprunt.IdUsager = idUsager;

        Console.Write("Nombre de jours d'emprunt (défaut: 14): ");
        var joursStr = Console.ReadLine();
        var jours = string.IsNullOrWhiteSpace(joursStr) ? 14 : int.Parse(joursStr);
        emprunt.DateRetourPrevue = emprunt.DateEmprunt.AddDays(jours);

        await _empruntService.CreateEmpruntAsync(emprunt);
        Console.WriteLine($"\nEmprunt créé avec succès! ID: {emprunt.IdEmprunt}");

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task ModifierEmpruntAsync()
    {
        Console.Clear();
        Console.WriteLine("MODIFIER UN EMPRUNT");
        Console.WriteLine("___________________\n");
        Console.Write("ID de l'emprunt à modifier: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            var emprunt = await _empruntService.GetEmpruntByIdAsync(id);
            if (emprunt != null)
            {
                Console.WriteLine($"\nEmprunt actuel: {emprunt}\n");
                Console.Write("Nouveau nombre de jours (Entrée pour garder): ");
                var joursStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(joursStr) && int.TryParse(joursStr, out var jours))
                {
                    emprunt.DateRetourPrevue = emprunt.DateEmprunt.AddDays(jours);
                }

                if (await _empruntService.UpdateEmpruntAsync(emprunt))
                {
                    Console.WriteLine("\nEmprunt modifié avec succès!");
                }
                else
                {
                    Console.WriteLine("\nErreur lors de la modification.");
                }
            }
            else
            {
                Console.WriteLine("\nEmprunt non trouvé.");
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task SupprimerEmpruntAsync()
    {
        Console.Clear();
        Console.WriteLine("SUPPRIMER UN EMPRUNT");
        Console.WriteLine("_____________________\n");
        Console.Write("ID de l'emprunt à supprimer: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            Console.Write($"\nÊtes-vous sûr de vouloir supprimer l'emprunt ID {id}? (O/N): ");
            var confirmation = Console.ReadLine();

            if (confirmation?.ToUpper() == "O")
            {
                if (await _empruntService.DeleteEmpruntAsync(id))
                {
                    Console.WriteLine("\nEmprunt supprimé avec succès!");
                }
                else
                {
                    Console.WriteLine("\nErreur lors de la suppression ou emprunt non trouvé.");
                }
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task RetournerLivreAsync()
    {
        Console.Clear();
        Console.WriteLine("RETOURNER UN LIVRE");
        Console.WriteLine("___________________\n");
        Console.Write("ID de l'emprunt: ");

        if (int.TryParse(Console.ReadLine(), out var id))
        {
            if (await _empruntService.ReturnBookAsync(id))
            {
                Console.WriteLine("\nLivre retourné avec succès!");
            }
            else
            {
                Console.WriteLine("\nErreur lors du retour ou emprunt non trouvé/déjà retourné.");
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private async Task ListerEmpruntsUsagerAsync()
    {
        Console.Clear();
        Console.WriteLine("LISTE DES EMPRUNTS D'UN USAGER");
        Console.WriteLine("_______________________________\n");
        Console.Write("ID de l'usager: ");

        if (int.TryParse(Console.ReadLine(), out var idUsager))
        {
            var emprunts = await _empruntService.GetEmpruntsByUsagerAsync(idUsager);
            if (!emprunts.Any())
            {
                Console.WriteLine("\nAucun emprunt trouvé pour cet usager.");
            }
            else
            {
                foreach (var emprunt in emprunts)
                {
                    Console.WriteLine(emprunt);
                    Console.WriteLine();
                }
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    #endregion

    #region Menu Rapports

    private async Task MenuRapportsAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("            RAPPORTS");
            Console.WriteLine("____________________________________");
            Console.WriteLine();
            Console.WriteLine("1. Rapport des emprunts d'un usager");
            Console.WriteLine("0. Retour au menu principal");
            Console.WriteLine();
            Console.Write("Votre choix: ");

            var choix = Console.ReadLine();

            try
            {
                switch (choix)
                {
                    case "1":
                        await GenererRapportEmpruntsAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nChoix invalide. Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERREUR: {ex.Message}");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
    }

    private async Task GenererRapportEmpruntsAsync()
    {
        Console.Clear();
        Console.WriteLine("RAPPORT DES EMPRUNTS D'UN USAGER");
        Console.WriteLine("________________________________\n");
        Console.Write("ID de l'usager: ");

        if (int.TryParse(Console.ReadLine(), out var idUsager))
        {
            var rapport = await _empruntService.GenerateEmpruntReportAsync(idUsager);
            
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("     RAPPORT D'EMPRUNTS");
            Console.WriteLine("________________________________");
            Console.WriteLine();
            Console.WriteLine($"Usager: {rapport.Usager.Nom}");
            Console.WriteLine($"Email: {rapport.Usager.Email}");
            Console.WriteLine($"Téléphone: {rapport.Usager.Telephone}");
            Console.WriteLine();
            Console.WriteLine("Statistiques:");
            Console.WriteLine($"  - Nombre total d'emprunts: {rapport.NombreTotal}");
            Console.WriteLine($"  - Emprunts en cours: {rapport.NombreEnCours}");
            Console.WriteLine($"  - Emprunts en retard: {rapport.NombreRetard}");
            Console.WriteLine($"  - Emprunts retournés: {rapport.NombreRetournes}");
            Console.WriteLine();
            Console.WriteLine($"Date de génération: {rapport.DateGeneration:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
            Console.WriteLine("Détails des emprunts:");
            Console.WriteLine("---------------------");

            if (!rapport.Emprunts.Any())
            {
                Console.WriteLine("Aucun emprunt trouvé.");
            }
            else
            {
                foreach (var emprunt in rapport.Emprunts)
                {
                    Console.WriteLine(emprunt);
                    Console.WriteLine();
                }
            }
        }
        else
        {
            Console.WriteLine("\nID invalide.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    #endregion
}

