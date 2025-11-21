using System;
using System.Threading;
using BibliothequeTP2.BLL.Services;
using BibliothequeTP2.DAL;   

namespace BibliothequeTP2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Bibliothèque TP2 - Gestion complète";

            var dbConnection = new DbConnection();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Connexion à la base de données en cours...");
            Console.ResetColor();

            if (!dbConnection.TestConnection())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERREUR : Impossible de se connecter à localhost\\RIYADE");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Connexion réussie !");
            Console.ResetColor();
            Thread.Sleep(800);

           
            var livreService = new LivreService(dbConnection);
            var usagerService = new UsagerService(dbConnection);
            var empruntService = new EmpruntService(dbConnection);

            
            var menu = new MenuPrincipal(livreService, usagerService, empruntService);
            await menu.AfficherMenuAsync();
        }
    }
}