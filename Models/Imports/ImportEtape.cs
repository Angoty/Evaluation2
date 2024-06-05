namespace Course.Models.Imports;
using Npgsql;
public class ImportEtape{

    public string etape{get; set;}
    public string kilometre{get; set;}
    public string nbCoureur{get; set;}
    public string rangEtape{get;set;}
    public string dateDepart{get;set;}
    public string heureDepart{get;set;}

    public ImportEtape(){}
    public ImportEtape(string etape, string kilometre, string nbCoureur, string rangEtape, string dateDepart, string heureDepart){
        this.etape=etape;
        this.kilometre=kilometre;
        this.nbCoureur=nbCoureur;
        this.rangEtape=rangEtape;
        this.dateDepart=dateDepart;
        this.heureDepart=heureDepart;
    }

    public static void insertDirectTemporaire(List<ImportEtape> listes,NpgsqlConnection con,NpgsqlTransaction transaction)
    {
        foreach (ImportEtape item in listes)
        {
            string query = "INSERT INTO ImportEtape(etape,kilometre, nb_coureur,rang_etape,date_depart,heure_depart) VALUES(@etape,@kilometre,@nbCoureur,@rangEtape,@dateDepart,@heureDepart)";
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con,transaction)){
                cmd.Parameters.AddWithValue("@etape",item.etape);
                cmd.Parameters.AddWithValue("@kilometre",item.kilometre);
                cmd.Parameters.AddWithValue("@nbCoureur",item.nbCoureur);
                cmd.Parameters.AddWithValue("@rangEtape",item.rangEtape);
                cmd.Parameters.AddWithValue("@dateDepart",item.dateDepart);
                cmd.Parameters.AddWithValue("@heureDepart",item.heureDepart);
                cmd.ExecuteNonQuery();
            }  
        }
    }

    
    public static void insertEtape(NpgsqlConnection con,NpgsqlTransaction transaction){
        string sql="INSERT INTO etape(intitule, nb_coureur, kilometre, heure_depart, rang_etape, etat) "+
                    "SELECT i.etape,i.nb_coureur::integer,REPLACE(i.kilometre, ',', '.')::DECIMAL(10, 2),TO_TIMESTAMP(i.date_depart || ' ' || i.heure_depart, 'DD/MM/YYYY HH24:MI:SS')"+
                    ", i.rang_etape::integer,0  from ImportEtape i ON CONFLICT DO NOTHING";
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }
    
    public static void nettoyage(NpgsqlConnection con,NpgsqlTransaction transaction){
        string sql="TRUNCATE TABLE ImportEtape";
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }

    public static void insertTransaction(List<ImportEtape> etapes, List<ImportResultat> res,NpgsqlConnection con=null){
        bool estValid=true;
        NpgsqlTransaction transaction=null;
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            transaction = con.BeginTransaction();
            insertDirectTemporaire(etapes,con,transaction);
            insertEtape(con,transaction);
            nettoyage(con,transaction);

            ImportResultat.insertDirectTemporaire(res,con,transaction);
            ImportResultat.insertEquipe(con,transaction);
            ImportResultat.insertCoureur(con,transaction);
            ImportResultat.insertTempsCoureur(con,transaction);
            ImportResultat.nettoyage(con,transaction);

            transaction.Commit();
        }
        catch (Exception e){
            Console.WriteLine(e.Message);
            transaction.Rollback();
            throw e;
        }finally{
            if (estValid == false) con.Close();
        }
    }



}