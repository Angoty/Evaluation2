CREATE DATABASE course_a_pied;
\c course_a_pied;
 
CREATE TABLE Genre(
    id_genre SERIAL PRIMARY KEY,
    intitule VARCHAR(10)
);

CREATE TABLE Profil(
    id_profil SERIAL PRIMARY KEY,
    intitule VARCHAR(10)
);

CREATE TABLE Utilisateur(
    id_utilisateur SERIAL PRIMARY KEY,
    nom VARCHAR(30),
    email VARCHAR(40) UNIQUE,
    mot_de_passe VARCHAR(40),
    id_profil INTEGER,
    FOREIGN KEY (id_profil) REFERENCES Profil(id_profil)
);
ALTER TABLE Utilisateur RENAME COLUMN id_admin TO id_utilisateur;

CREATE TABLE Etape(
    id_etape SERIAL PRIMARY KEY,
    intitule VARCHAR(70) UNIQUE,
    nb_coureur INTEGER,
    kilometre DOUBLE PRECISION,
    heure_depart TIMESTAMP,
    rang_etape INTEGER,
    etat INTEGER
);


CREATE TABLE Coureur(
    id_coureur SERIAL PRIMARY KEY,
    nom VARCHAR(30),
    num_dossard VARCHAR(50) UNIQUE,
    date_naissance DATE,
    id_genre INTEGER,
    id_equipe INTEGER,
    FOREIGN KEY (id_genre) REFERENCES Genre(id_genre),
    FOREIGN KEY(id_equipe) REFERENCES Utilisateur(id_utilisateur)
);

CREATE TABLE Categorie(
    id_categorie SERIAL PRIMARY KEY,
    intitule VARCHAR(20)
);

CREATE TABLE CategorieCoureur(
    id_categorie_coureur SERIAL PRIMARY KEY,
    id_categorie INTEGER,
    id_coureur INTEGER,
    FOREIGN KEY(id_categorie) REFERENCES Categorie(id_categorie),
    FOREIGN KEY(id_coureur) REFERENCES Coureur(id_coureur)
);

CREATE TABLE TempsCoureur(
    id_temps SERIAL PRIMARY KEY,
    id_etape INTEGER,
    id_coureur INTEGER,
    heure_arrivee TIMESTAMP,
    FOREIGN KEY (id_etape) REFERENCES Etape(id_etape),
    FOREIGN KEY (id_coureur) REFERENCES Coureur(id_coureur)
);

CREATE TABLE Point(
    id_point SERIAL PRIMARY KEY,
    intitule VARCHAR(30),
    valeur INTEGER
);

CREATE TABLE PointCoureur(
    id_point_coureur SERIAL PRIMARY KEY,
    id_temps INTEGER,
    points INTEGER,
    FOREIGN KEY (id_temps) REFERENCES TempsCoureur(id_temps),
    FOREIGN KEY (points) REFERENCES Point(id_point)
);


CREATE TABLE Classement (
    id_temps INTEGER PRIMARY KEY,
    position INTEGER
);
