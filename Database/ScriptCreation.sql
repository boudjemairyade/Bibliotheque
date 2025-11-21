

USE master;
GO


IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BibliothequeDB')
BEGIN
    CREATE DATABASE BibliothequeDB;
END
GO

USE BibliothequeDB;
GO


IF OBJECT_ID('Emprunts', 'U') IS NOT NULL
    DROP TABLE Emprunts;
GO

IF OBJECT_ID('Livres', 'U') IS NOT NULL
    DROP TABLE Livres;
GO

IF OBJECT_ID('Usagers', 'U') IS NOT NULL
    DROP TABLE Usagers;
GO


CREATE TABLE Usagers (
    IdUsager INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telephone NVARCHAR(20) NOT NULL
);
GO


CREATE TABLE Livres (
    IdLivre INT IDENTITY(1,1) PRIMARY KEY,
    Titre NVARCHAR(200) NOT NULL,
    Auteur NVARCHAR(100) NOT NULL,
    Annee INT NOT NULL,
    ISBN NVARCHAR(20) NOT NULL UNIQUE,
    Categorie NVARCHAR(50) NOT NULL,
    Quantite INT NOT NULL DEFAULT 1 CHECK (Quantite >= 0)
);
GO


CREATE TABLE Emprunts (
    IdEmprunt INT IDENTITY(1,1) PRIMARY KEY,
    DateEmprunt DATETIME NOT NULL DEFAULT GETDATE(),
    DateRetourPrevue DATETIME NOT NULL,
    IdLivre INT NOT NULL,
    IdUsager INT NOT NULL,
    DateRetourReelle DATETIME NULL,
    CONSTRAINT FK_Emprunts_Livres FOREIGN KEY (IdLivre) REFERENCES Livres(IdLivre),
    CONSTRAINT FK_Emprunts_Usagers FOREIGN KEY (IdUsager) REFERENCES Usagers(IdUsager)
);
GO


CREATE INDEX IX_Emprunts_IdUsager ON Emprunts(IdUsager);
CREATE INDEX IX_Emprunts_IdLivre ON Emprunts(IdLivre);
CREATE INDEX IX_Emprunts_DateRetourPrevue ON Emprunts(DateRetourPrevue);
GO

-- Données de test (optionnel)
-- INSERT INTO Usagers (Nom, Email, Telephone) VALUES
-- ('Jean Dupont', 'jean.dupont@email.com', '514-123-4567'),
-- ('Marie Tremblay', 'marie.tremblay@email.com', '514-234-5678');

-- INSERT INTO Livres (Titre, Auteur, Annee, ISBN, Categorie, Quantite) VALUES
-- ('Le Petit Prince', 'Antoine de Saint-Exupéry', 1943, '978-2-07-061275-8', 'Littérature', 5),
-- ('1984', 'George Orwell', 1949, '978-2-07-036822-8', 'Science-Fiction', 3);

PRINT 'Base de données créée avec succès!';
GO

