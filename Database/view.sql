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

