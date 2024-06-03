namespace Course.Models.Imports;
using Npgsql;
public class ImportResultat{

    public string etapeRang{get; set;}
    public string numDossard{get; set;}
    public string nom{get; set;}
    public string genre{get;set;}
    public string dateNaissance{get;set;}
    public string equipe{get;set;}
    public string arrivee{get;set;}

    public ImportResultat(){}
    public ImportResultat( string etapeRang, string numDossard, string nom, string genre, string dateNaissance, string equipe, string arrivee){
        this.etapeRang=etapeRang;
        this.numDossard=numDossard;
        this.nom=nom;
        this.genre=genre;
        this.dateNaissance=dateNaissance;
        this.equipe=equipe;
        this.arrivee=arrivee;
    }

    public static void insertDirectTemporaire(List<ImportResultat> listes,NpgsqlConnection con,NpgsqlTransaction transaction)
    {
        foreach (ImportResultat item in listes)
        {
            string query = "INSERT INTO ImportResultat(rang_etape,num_dossard,nom,genre,date_naissance,equipe, arrivee) VALUES(@rangEtape,@numDossard,@nom,@genre,@dateNaissance,@equipe,@arrivee)";
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con,transaction)){
                cmd.Parameters.AddWithValue("@rangEtape",item.etapeRang);
                cmd.Parameters.AddWithValue("@numDossard",item.numDossard);
                cmd.Parameters.AddWithValue("@nom",item.nom);
                cmd.Parameters.AddWithValue("@genre",item.genre);
                cmd.Parameters.AddWithValue("@dateNaissance",item.dateNaissance);
                cmd.Parameters.AddWithValue("@equipe",item.equipe);
                cmd.Parameters.AddWithValue("@arrivee",item.arrivee);
                cmd.ExecuteNonQuery();
            }  
        }
    }

    
    public static void insertEquipe(NpgsqlConnection con,NpgsqlTransaction transaction)
    {
        string sql="INSERT INTO Utilisateur(nom, email, mot_de_passe, id_profil) SELECT i.equipe, i.equipe||'@gmail.com'"+
                    ", i.equipe, 2 from ImportResultat i ON CONFLICT DO NOTHING ";
                    Console.WriteLine("equipe");
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }

    public static void insertCoureur(NpgsqlConnection con, NpgsqlTransaction transaction)
    {

        string sql="INSERT INTO Coureur(nom, num_dossard, date_naissance, id_genre, id_equipe) SELECT i.nom, i.num_dossard, TO_TIMESTAMP(i.date_naissance, 'DD/MM/YYYY HH24:MI:SS'),"+
                   " g.id_genre, u.id_utilisateur FROM  ImportResultat i LEFT JOIN Genre g ON i.genre = g.intitule LEFT JOIN "+
                   " Utilisateur u ON i.equipe = u.nom ON CONFLICT DO NOTHING";
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }

    

    public static void insertTempsCoureur(NpgsqlConnection con, NpgsqlTransaction transaction)
    {
        
            string sql = "INSERT INTO TempsCoureur(id_etape, id_coureur, heure_arrivee)"+ 
                         "SELECT e.id_etape, c.id_coureur,TO_TIMESTAMP(ir.arrivee, 'DD/MM/YYYY HH24:MI:SS')"+
                         " FROM  ImportResultat ir JOIN Etape e ON ir.rang_etape::integer = e.rang_etape JOIN"+
                         " Coureur c ON ir.num_dossard = c.num_dossard ON CONFLICT DO NOTHING";
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }

    public static void insertPointCoureur(NpgsqlConnection con, NpgsqlTransaction transaction)
    {
    
            string sql = "INSERT INTO PointCoureur(id_temps, points)"+ 
                         "SELECT e.id_etape, c.id_coureur,TO_TIMESTAMP(ir.arrivee, 'DD/MM/YYYY HH24:MI:SS')"+
                         " FROM  ImportResultat ir JOIN Etape e ON ir.rang_etape::integer = e.rang_etape JOIN"+
                         " Coureur c ON ir.num_dossard = c.num_dossard ON CONFLICT DO NOTHING";
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }

    
    public static void nettoyage(NpgsqlConnection con,NpgsqlTransaction transaction)
    {
        string sql="TRUNCATE TABLE ImportResultat";
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }

    
}