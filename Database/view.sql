CREATE OR REPLACE VIEW v_coureur_categorie AS(
    SELECT cc.id_categorie_coureur, c.*
        FROM CategorieCoureur cc JOIN Coureur c ON cc.id_coureur=c.id_coureur
        JOIN Categorie cat ON cc.id_categorie=cat.id_categorie
);

CREATE OR REPLACE VIEW v_etape_temps_coureur AS (
    SELECT e.*, tc.id_temps, tc.id_coureur, tc.heure_arrivee
    FROM TempsCoureur tc 
    JOIN Etape e ON tc.id_etape = e.id_etape
);

CREATE OR REPLACE VIEW v_etape_coureur AS (
    SELECT v.*, c.nom, c.num_dossard, c.date_naissance, c.id_genre, c.id_equipe
    FROM v_etape_temps_coureur v 
    JOIN Coureur c ON v.id_coureur = c.id_coureur
);

CREATE OR REPLACE VIEW v_classement AS (
    SELECT tc.*,
    DENSE_RANK() OVER (PARTITION BY tc.id_etape ORDER BY tc.heure_arrivee ASC) AS position
    FROM TempsCoureur tc 
);
CREATE OR REPLACE VIEW v_coureur_classement AS(
    SELECT v.*, c.id_equipe, p.valeur
        FROM v_classement v JOIN Coureur c ON v.id_coureur=c.id_coureur
        JOIN Point p  ON p.intitule=v.position::VARCHAR(30)
);

CREATE OR REPLACE VIEW v_equipe_classement AS(
    SELECT dense_rank() OVER (ORDER BY SUM(v.valeur) DESC) AS position,
           v.id_equipe,
           SUM(v.valeur) AS points_equipe
    FROM v_coureur_classement v
    GROUP BY v.id_equipe
);

CREATE OR REPLACE VIEW v_classement_general AS(
    SELECT v.id_coureur, 
        SUM(v.valeur) AS total_points_coureur
        FROM v_coureur_classement v
         GROUP BY v.id_coureur
);

CREATE OR REPLACE VIEW v_classement_cle AS (
    SELECT dense_rank() OVER (ORDER BY v.total_points_coureur DESC) AS position,
           v.total_points_coureur,
           c.*
    FROM v_classement_general v 
    JOIN Coureur c ON v.id_coureur = c.id_coureur
);



CREATE OR REPLACE VIEW v_classement_categorie2 AS (
    SELECT tc.id_etape, cc.id_coureur, cc.id_categorie, tc.heure_arrivee,
        DENSE_RANK() OVER (PARTITION BY tc.id_etape, cc.id_categorie ORDER BY tc.heure_arrivee ASC) AS position
    FROM CategorieCoureur cc 
    JOIN TempsCoureur tc ON tc.id_coureur=cc.id_coureur
    Where tc.heure_arrivee is not NULL 
    ORDER BY 
        tc.id_etape, cc.id_categorie, tc.heure_arrivee
);
SELECT * FROM v_classement_categorie2 WHERE id_categorie=2;

CREATE OR REPLACE VIEW v_points_categorie AS(
    SELECT
        cc.*, 
            CASE WHEN p.valeur IS NULL THEN 0 else p.valeur end points
            FROM v_classement_categorie2 cc
            LEFT JOIN Point p ON p.intitule=cc.position::VARCHAR(30)
            JOIN Coureur c ON 

);