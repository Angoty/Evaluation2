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

CREATE OR REPLACE VIEW v_classement_etape AS (
    SELECT dense_rank() OVER (ORDER BY v.total_points_coureur DESC) AS position,
           v.total_points_coureur,
           c.*
    FROM v_classement_general v 
    JOIN Coureur c ON v.id_coureur = c.id_coureur
);



CREATE OR REPLACE VIEW v_classement_equipe_Etape1 AS(
    SELECT  e.intitule,e.kilometre,e.heure_depart,e.rang_etape,t.*
    FROM Etape e 
        LEFT JOIN TempsCoureur t ON e.id_etape=t.id_etape
);

CREATE OR REPLACE VIEW v_classement_equipe_Etape1 AS(
    SELECT 
        DENSE_RANK() OVER (PARTITION BY id_etape ORDER BY heure_arrivee ASC) AS position, 
        c.*
        FROM v_etape_coureur c
);

CREATE OR REPLACE VIEW v_classement_equipe_Etape2 AS
SELECT 
    v.*,
    COALESCE(p.valeur, 0) AS points
FROM v_classement_equipe_Etape1 v
LEFT JOIN point p ON p.intitule = v.position::VARCHAR(30);

-- CREATE OR REPLACE VIEW v_classement_equipe_Etape2 AS(
--     SELECT 
--         v.*,p.valeur points
--         FROM v_classement_equipe_Etape1 v
--         JOIN point p ON p.intitule=v.position::VARCHAR(30) 
-- );

CREATE OR REPLACE VIEW v_equipe_gagnante AS(
    SELECT * FROM v_equipe_classement LIMIT 1
);

CREATE OR REPLACE VIEW v_coureur_categorie1 AS(
    SELECT c.id_categorie,v.*
        FROM v_coureur_categorie v 
        JOIN CategorieCoureur c ON c.id_categorie_coureur=v.id_categorie_coureur
);

CREATE OR REPLACE VIEW v_classement_categorie3 AS(
    SELECT v.id_equipe,V.id_categorie,sum(total_points_coureur) total_points,
        DENSE_RANK() OVER (PARTITION BY id_categorie ORDER BY sum(total_points_coureur) ASC) AS position
        FROM v_coureur_categorie1 v 

        JOIN v_classement_general v1 ON v1.id_coureur=v.id_coureur
        GROUP BY  v.id_equipe,V.id_categorie
) 
SELECT v.*, c.id_categorie
FROM v_etape_coureur v 
    JOIN CategorieCoureur c ON c.id_coureur=v.id_coureur

SELECT id_equipe,cc.id_categorie,sum(total_points_coureur)
    FROM v_classement_general v
    JOIN CategorieCoureur cc ON cc.id_coureur=v.id_coureur 
    JOIN Coureur c on c.id_coureur=v.id_coureur
GROUP BY id_equipe,cc.id_categorie