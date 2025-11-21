SELECT * FROM Livres;
INSERT INTO Livres (Titre, Auteur, Annee, ISBN, Categorie, Quantite) VALUES
('Le Petit Prince', 'Antoine de Saint-Exupéry', 1943, '9782070612758', 'Littérature', 5),
('1984', 'George Orwell', 1949, '9780451524935', 'Science-Fiction', 3),
('Harry Potter à l''école des sorciers', 'J.K. Rowling', 1997, '9782070584628', 'Fantasy', 7),
('Les Misérables', 'Victor Hugo', 1862, '9782070409181', 'Classique', 2),
('L''Étranger', 'Albert Camus', 1942, '9782070360024', 'Philosophie', 4),
('Sapiens', 'Yuval Noah Harari', 2011, '9780099590088', 'Histoire', 5),
('Le Seigneur des Anneaux', 'J.R.R. Tolkien', 1954, '9780261102385', 'Fantasy', 3),
('Le Seigneur des Anneaux: Deux Tours', 'J.R.R. Tolkien', 1954, '9780261102361', 'Fantasy', 2),
('La Peste', 'Albert Camus', 1947, '9782070360420', 'Roman', 6),
('Cosmos', 'Carl Sagan', 1980, '9780345539434', 'Science', 3);
INSERT INTO Usagers (Nom, Email, Telephone) VALUES
('Jean Dupont', 'jean.dupont@email.com', '514-123-4567'),
('Sarah Lambert', 'sarah.lambert@email.com', '438-555-9999'),
('Mohamed Ali', 'mohamed.ali@email.com', '514-222-3333'),
('Julie Tremblay', 'julie.tremblay@email.com', '438-111-8888'),
('Karim Bahloul', 'karim.bahloul@email.com', '514-321-7645'),
('Emma Roy', 'emma.roy@email.com', '514-678-1234');
SELECT*FROM Usagers ;
INSERT INTO Usagers (Nom, Email, Telephone) VALUES
('Jean Dupont', 'jean.dupont@email.com', '514-123-4567'),
('Sarah Lambert', 'sarah.lambert@email.com', '438-555-9999'),
('Mohamed Ali', 'mohamed.ali@email.com', '514-222-3333'),
('Julie Tremblay', 'julie.tremblay@email.com', '438-111-8888'),
('Karim Bahloul', 'karim.bahloul@email.com', '514-321-7645'),
('Emma Roy', 'emma.roy@email.com', '514-678-1234');
INSERT INTO Usagers (Nom, Email, Telephone) VALUES
('Axil Ameziane', 'axil.ameziane@amazigh.com', '514-101-2020'),
('Tinhinan Ider', 'tinhinan.ider@amazigh.com', '514-303-4040'),
('Massinissa Amellal', 'massinissa.amellal@amazigh.com', '438-505-6060'),
('Dihya Zghana', 'dihya.zghana@amazigh.com', '438-707-8080'),
('Tafsut Ouramdane', 'tafsut.ouramdane@amazigh.com', '514-909-0101'),
('Asirem Aksil', 'asirem.aksil@amazigh.com', '514-616-7171'),
('Yuba Taleb', 'yuba.taleb@amazigh.com', '438-828-9292'),
('Tazdayt Imoula', 'tazdayt.imoula@amazigh.com', '514-333-4444'),
('Ula Ait Messaoud', 'ula.messaoud@amazigh.com', '438-555-6666'),
('Illyas Idir', 'illyas.idir@amazigh.com', '438-777-8888');
SELECT * FROM Usagers;
INSERT INTO Emprunts (DateEmprunt, DateRetourPrevue, IdLivre, IdUsager) VALUES
('2025-01-10', '2025-01-24', 1, 1),
('2025-02-01', '2025-02-15', 3, 2),
('2025-02-05', '2025-02-19', 2, 3),
('2025-03-01', '2025-03-15', 5, 4),
('2025-03-10', '2025-03-24', 7, 1),
('2025-03-12', '2025-03-26', 8, 5),
('2025-03-15', '2025-03-29', 4, 6),
('2025-03-20', '2025-04-03', 9, 3),
('2025-03-22', '2025-04-05', 6, 2),
('2025-03-25', '2025-04-08', 10, 5);
SELECT *FROM Emprunts;
select * From Livres ;
