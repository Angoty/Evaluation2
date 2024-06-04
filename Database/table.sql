CREATE DATABASE course_a_pied2;
\c course_a_pied2;
 
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
    intitule VARCHAR(70),
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
    intitule VARCHAR(30) UNIQUE,
    valeur INTEGER
);


CREATE TABLE ImportEtape(
    etape VARCHAR(200),
    kilometre VARCHAR(200),
    nb_coureur VARCHAR(200),
    rang_etape VARCHAR(200),
    date_depart VARCHAR(200),
    heure_depart VARCHAR(200)
);


CREATE TABLE ImportResultat(
    rang_etape VARCHAR(200),
    num_dossard VARCHAR(200),
    nom VARCHAR(200),
    genre VARCHAR(200),
    date_naissance VARCHAR(200),
    equipe VARCHAR(200),
    arrivee VARCHAR(200)
);

CREATE TABLE ImportPoint(
    classement VARCHAR(200),
    points VARCHAR(200)
);

CREATE TABLE Penalite(
    id_penalite SERIAL PRIMARY KEY,
    id_etape INTEGER,
    id_equipe INTEGER,
    penalite TIMESTAMP
);

CREATE OR REPLACE FUNCTION update_etat_etape()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE Etape
    SET etat = 5
    WHERE id_etape IN (SELECT DISTINCT id_etape FROM TempsCoureur);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_etat_etape_trigger
AFTER INSERT ON TempsCoureur
FOR EACH STATEMENT
EXECUTE FUNCTION update_etat_etape();

CREATE OR REPLACE VIEW v_age_coureurs AS
SELECT
    c.id_coureur,
    c.nom AS nom_coureur,
    c.date_naissance,
    c.id_genre,
    EXTRACT(YEAR FROM age(current_date, c.date_naissance)) AS age
FROM
    Coureur c;

CREATE OR REPLACE VIEW vue_categorie_coureur AS
SELECT
    v.id_coureur,
    v.nom_coureur,
    v.date_naissance,
    g.intitule AS genre,
    CASE
        WHEN g.intitule = 'M' THEN 'Homme'
        ELSE 'Femme'
    END AS categorie
FROM
    v_age_coureurs v
INNER JOIN Genre g ON v.id_genre = g.id_genre;

CREATE OR REPLACE VIEW v_categorie_junior AS
SELECT
    v.id_coureur,
    v.nom_coureur,
    v.date_naissance,
    g.intitule AS genre,
    CASE
        WHEN v.age<18  THEN 'Junior'
    END AS categorie
FROM
    v_age_coureurs v 
JOIN Genre g ON v.id_genre = g.id_genre;


INSERT INTO CategorieCoureur (id_categorie, id_coureur)
SELECT
    c.id_categorie,
    v.id_coureur
FROM
    vue_categorie_coureur v
INNER JOIN
    Categorie c ON v.categorie = c.intitule;


INSERT INTO CategorieCoureur (id_categorie, id_coureur)
SELECT
    c.id_categorie,
    v.id_coureur
FROM
    v_categorie_junior v
INNER JOIN
    Categorie c ON v.categorie = c.intitule;



CREATE OR REPLACE VIEW v_classement_general_equipe AS (
    SELECT
        dense_rank() OVER (ORDER BY SUM(v.points_equipe) DESC) AS position,
        v.nom_equipe,
        SUM(v.points_equipe) AS total_points_equipe
    FROM
        v_equipe_classement v
    GROUP BY
        v.nom_equipe
);


CREATE OR REPLACE VIEW v_temps_coureur_etape AS (
    SELECT c.nom AS nom_coureur,
           e.intitule AS intitule_etape,
           tc.heure_arrivee AS temps_arrivee
    FROM Coureur c
    CROSS JOIN Etape e
    LEFT JOIN TempsCoureur tc ON c.id_coureur = tc.id_coureur AND e.id_etape = tc.id_etape
);

CREATE OR REPLACE VIEW v_Age_Coureurs AS
SELECT
    c.id_coureur,
    c.nom AS nom_coureur,
    c.date_naissance,
    EXTRACT(YEAR FROM age(current_date, c.date_naissance)) AS age
FROM
    Coureur c;


CREATE OR REPLACE VIEW v_classement_equipe_tout_categorie AS (
    SELECT
        dense_rank() OVER (ORDER BY SUM(v.valeur) DESC) AS position,
        u.nom AS nom_equipe,
        u.id_utilisateur,
        c.intitule AS categorie,
        c.id_categorie,
        SUM(v.valeur) AS points_equipe
    FROM
        v_coureur_classement v
    JOIN
        Utilisateur u ON v.id_equipe = u.id_utilisateur
    JOIN
        CategorieCoureur cc ON v.id_coureur = cc.id_coureur
    JOIN
        Categorie c ON cc.id_categorie = c.id_categorie
    GROUP BY
        u.nom, c.intitule, u.id_utilisateur, c.id_categorie  ORDER BY u.nom ASC
);  

-- SELECT
--     dense_rank() OVER (ORDER BY SUM(v.points_equipe) DESC) AS position,
--     v.nom_equipe,
--     v.categorie,
--     SUM(v.points_equipe) AS total_points_equipe
-- FROM
--     v_equipe_classement2 v
-- GROUP BY
--     v.nom_equipe, v.categorie;


-- SELECT
--     dense_rank() OVER (ORDER BY SUM(v.points_equipe) DESC) AS position,
--     v.nom_equipe,
--     SUM(v.points_equipe) AS total_points_equipe
-- FROM
--     v_equipe_classement2 v
-- WHERE
--     v.categorie = 'Homme'
-- GROUP BY
--     v.nom_equipe;


-- SELECT
--     dense_rank() OVER (ORDER BY CASE WHEN SUM(v.points_equipe) IS NULL THEN 0 ELSE SUM(v.points_equipe) END DESC) AS position,
--     u.nom AS nom_equipe,
--     CASE WHEN SUM(v.points_equipe) IS NULL THEN 0 ELSE SUM(v.points_equipe) END AS total_points_equipe
-- FROM
--     Utilisateur u
-- LEFT JOIN
--     v_equipe_classement2 v ON u.id_utilisateur = v.id_utilisateur AND v.categorie = 'Homme'
-- GROUP BY
--     u.nom;


