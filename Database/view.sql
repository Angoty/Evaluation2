CREATE OR REPLACE VIEW v_etape_temps_coureur AS (
    SELECT e.*, tc.id_coureur, tc.heure_arrivee
    FROM TempsCoureur tc 
    JOIN Etape e ON tc.id_etape = e.id_etape
);

CREATE OR REPLACE VIEW v_etape_coureur AS (
    SELECT v.*, c.nom, c.num_dossard, c.date_naissance, c.id_genre, c.id_equipe
    FROM v_etape_temps_coureur v 
    JOIN Coureur c ON v.id_coureur = c.id_coureur
);
CREATE OR REPLACE VIEW v_participant AS(
    SELECT count(t.id_coureur)
        FROM Etape e JOIN TempsCoureur t ON e.id_etape=t.id_etape WHERE e.id_etape=1;
);
CREATE OR REPLACE VIEW VueResultats AS (
    SELECT sub.id_temps, sub.id_etape,  sub.heure_arrivee,
           c.*, v_etape_coureur
           CASE WHEN p.id_point IS NULL THEN (sub.position || ' eme') ELSE p.intitule END AS nom_point,
           CASE WHEN p.id_point IS NULL THEN 0 ELSE p.valeur END AS points_attribues,
           sub.position
    FROM (
        SELECT tc.id_temps, tc.id_etape, tc.id_coureur, tc.heure_arrivee,
               DENSE_RANK() OVER (ORDER BY tc.heure_arrivee ASC) AS position
        FROM TempsCoureur tc
    ) AS sub
    LEFT JOIN Point p ON sub.position = p.id_point
    LEFT JOIN Coureur c ON sub.id_coureur = c.id_coureur
);


CREATE OR REPLACE VIEW ClassementEquipes AS(
    SELECT c.id_equipe,
        SUM(CASE WHEN vr.id_coureur IS NULL THEN 0 ELSE vr.points_attribues END) AS total_points_equipe,
        DENSE_RANK() OVER 
            (ORDER BY SUM(CASE WHEN vr.id_coureur IS NULL THEN 0 ELSE vr.points_attribues END) DESC) AS classement
    FROM Coureur c
    LEFT JOIN VueResultats vr ON c.id_coureur = vr.id_coureur
    GROUP BY c.id_equipe
);

CREATE OR REPLACE VIEW VueClassementEtape AS(
    SELECT vr.id_coureur,
        c.id_equipe,
        SUM(vr.points_attribues) AS total_points_coureur,
        DENSE_RANK() OVER (ORDER BY SUM(vr.points_attribues) DESC) AS classement_general
    FROM VueResultats vr
    JOIN Coureur c ON vr.id_coureur = c.id_coureur
    GROUP BY vr.id_coureur, c.id_equipe;
);




SELECT u.nom AS equipe, COUNT(tc.id_temps) AS nombre_de_temps_coureurs
FROM TempsCoureur tc
JOIN Coureur c ON tc.id_coureur = c.id_coureur
JOIN Utilisateur u ON c.id_equipe = u.id_utilisateur
GROUP BY u.nom;


SELECT u.nom AS equipe, COUNT(tc.id_temps) AS nombre_de_temps_coureurs
                    FROM TempsCoureur tc
                    JOIN Coureur c ON tc.id_coureur = c.id_coureur
                    JOIN Utilisateur u ON c.id_equipe = u.id_utilisateur
                    WHERE tc.id_etape = 1
                    GROUP BY u.nom