# Application de Gestion de Bibliothèque

## Description

Application de gestion de bibliothèque développée en C# avec SQL Server, utilisant une architecture en couches et le pattern Repository. Cette application permet de gérer les livres, les usagers et les emprunts d'une bibliothèque.

## Architecture

L'application suit une architecture en couches :

- **Bibliotheque.Models** : Entités du domaine (Livre, Usager, Emprunt)
- **Bibliotheque.DataAccess** : Accès aux données avec le pattern Repository (ADO.NET)
- **Bibliotheque.BusinessLogic** : Logique métier et services
- **Bibliotheque.ConsoleApp** : Interface utilisateur console

## Prérequis

- .NET 8.0 SDK ou supérieur
- SQL Server (2019 ou supérieur recommandé)
- Visual Studio 2022 ou VS Code (optionnel)

## Installation

### 1. Création de la base de données

Exécutez le script SQL fourni dans `Database/ScriptCreation.sql` dans SQL Server Management Studio ou via la ligne de commande :

```sql
sqlcmd -S localhost -i Database/ScriptCreation.sql
```

### 2. Configuration de la connexion

Modifiez la chaîne de connexion dans `Bibliotheque.ConsoleApp/Program.cs` si nécessaire :

```csharp
const string connectionString = "Server=localhost;Database=BibliothequeDB;Integrated Security=true;TrustServerCertificate=true;";
```

### 3. Restauration des packages NuGet

```bash
dotnet restore
```

### 4. Compilation

```bash
dotnet build
```

### 5. Exécution

```bash
dotnet run --project Bibliotheque.ConsoleApp
```

## Fonctionnalités

### Gestion des Livres

-  Lister tous les livres
-  Lister les livres disponibles (quantité > 0)
-  Rechercher un livre par ID
-  Ajouter un livre (CRUD - Create)
-  Modifier un livre (CRUD - Update)
-  Supprimer un livre (CRUD - Delete)

### Gestion des Usagers

-  Lister tous les usagers
-  Rechercher un usager par ID
-  Ajouter un usager (CRUD - Create)
-  Modifier un usager (CRUD - Update)
-  Supprimer un usager (CRUD - Delete)

### Gestion des Emprunts

-  Lister tous les emprunts
-  Rechercher un emprunt par ID
-  Créer un emprunt (CRUD - Create)
  - Vérification automatique de la disponibilité du livre
  - Mise à jour automatique de la quantité
-  Modifier un emprunt (CRUD - Update)
-  Supprimer un emprunt (CRUD - Delete)
-  Retourner un livre (mise à jour de DateRetourReelle)
-  Lister les emprunts d'un usager

### Rapports

-  Rapport des emprunts d'un usager
  - Statistiques complètes (total, en cours, en retard, retournés)
  - Liste détaillée de tous les emprunts

## Structure de la base de données

### Table: Livres

| Colonne | Type | Description |

| IdLivre | INT (PK) | Identifiant unique |
| Titre | NVARCHAR(200) | Titre du livre |
| Auteur | NVARCHAR(100) | Auteur du livre |
| Annee | INT | Année de publication |
| ISBN | NVARCHAR(20) | Numéro ISBN (unique) |
| Categorie | NVARCHAR(50) | Catégorie du livre |
| Quantite | INT | Nombre d'exemplaires disponibles |

### Table: Usagers

| Colonne | Type | Description |

| IdUsager | INT (PK) | Identifiant unique |
| Nom | NVARCHAR(100) | Nom complet |
| Email | NVARCHAR(100) | Adresse email (unique) |
| Telephone | NVARCHAR(20) | Numéro de téléphone |

### Table: Emprunts

| Colonne | Type | Description |

| IdEmprunt | INT (PK) | Identifiant unique |
| DateEmprunt | DATETIME | Date d'emprunt |
| DateRetourPrevue | DATETIME | Date de retour prévue |
| DateRetourReelle | DATETIME (nullable) | Date de retour effective |
| IdLivre | INT (FK) | Référence au livre |
| IdUsager | INT (FK) | Référence à l'usager |

## Utilisation

### Menu Principal

L'application démarre avec un menu principal offrant 4 options :

1. **Gestion des Livres** : CRUD complet pour les livres
2. **Gestion des Usagers** : CRUD complet pour les usagers
3. **Gestion des Emprunts** : CRUD complet pour les emprunts
4. **Rapports** : Génération de rapports

### Exemples d'utilisation

#### Ajouter un livre

1. Menu Principal  1 (Gestion des Livres)
2. Option 4 (Ajouter un livre)
3. Saisir les informations demandées

#### Créer un emprunt

1. Menu Principal  3 (Gestion des Emprunts)
2. Option 3 (Ajouter un emprunt)
3. Saisir l'ID du livre et l'ID de l'usager
4. Spécifier la durée d'emprunt (par défaut 14 jours)

#### Générer un rapport

1. Menu Principal  4 (Rapports)
2. Option 1 (Rapport des emprunts d'un usager)
3. Saisir l'ID de l'usager

## Technologies utilisées

- **C#** (.NET 8.0)
- **ADO.NET** (System.Data.SqlClient)
- **SQL Server**
- **Pattern Repository**
- **Architecture en couches**

## Documentation

### Dictionnaire de données

Voir le fichier `Documentation/DictionnaireDonnees.md` pour les détails complets.

### Architecture

Voir le fichier `Documentation/Architecture.md` pour les détails de l'architecture.







