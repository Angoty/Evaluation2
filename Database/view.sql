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

CREATE OR REPLACE VIEW v_classement_cle AS (
    SELECT dense_rank() OVER (ORDER BY v.points_equipe DESC) AS position,
           v.points_equipe,
           u.*
    FROM v_equipe_classement v 
    JOIN Utilisateur u ON v.id_coureur = u.id_utilisateur
);



CREATE OR REPLACE VIEW v_temps_coureur_etape AS (
    SELECT c.nom AS nom_coureur,
           e.intitule AS intitule_etape,
           tc.heure_arrivee AS temps_arrivee
    FROM Coureur c
    CROSS JOIN Etape e
    LEFT JOIN TempsCoureur tc ON c.id_coureur = tc.id_coureur AND e.id_etape = tc.id_etape
);
