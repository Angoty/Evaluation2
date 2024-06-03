INSERT INTO Genre(intitule) VALUES ('M'), ('F');

INSERT INTO Profil(intitule) 
                        VALUES ('Admin'),
                           ('Equipe');

INSERT INTO Utilisateur(nom, email, mot_de_passe, id_profil)
VALUES ('Angoty Fitia', 'angotyrabarijaona@gmail.com', 'angoty', 1);
--        ('Equipe A', 'equipe.a@gmail.com', 'equipeA', 2),
--        ('Equipe B', 'equipe.b@gmail.com', 'equipeB', 2),
--        ('Equipe C', 'equipe.c@gmail.com', 'equipe3', 2);

-- INSERT INTO Etape(intitule, nb_coureur, kilometre, heure_depart, rang_etape, etat)
-- VALUES ('Etape 1 de Betsizaraina', 3, 42, '2024-06-01 08:03:06', 1, 0),
--        ('Etape 2 d''Ampasimbe', 2, 10, '2024-06-01 08:03:06', 2, 0),
--        ('Etape 3 d''Itasy', 1, 5, '2024-06-01 08:03:06', 3, 0);

-- INSERT INTO Categorie(intitule) VALUES ('Homme'), ('Femme'), ('Senior'), ('Junior');

-- INSERT INTO Coureur(nom, num_dossard, date_naissance, id_genre, id_equipe)
-- VALUES ('Lova', 'COUR001', '2005-01-01', 1, 2),
--        ('Sabrina', 'COUR002', '2005-02-02', 2, 2),
--        ('Faniry', 'COUR003', '2005-03-03', 1, 2),
--        ('Justin', 'COUR004', '2005-04-04', 1, 3),
--        ('Vero', 'COUR005', '2005-05-05', 2, 3),
--        ('Haja', 'COUR006', '2005-06-06', 1, 3);

-- INSERT INTO CategorieCoureur(id_categorie, id_coureur) VALUES (1, 1);

-- INSERT INTO Point(intitule, valeur) VALUES ('1 er', 10), ('2 eme', 6), ('3 eme', 4), ('4 eme', 2), ('5 eme', 1);



