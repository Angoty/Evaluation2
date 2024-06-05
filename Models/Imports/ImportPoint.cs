namespace Course.Models.Imports;
using Npgsql;
public class ImportPoint{

    public string classement{get; set;}
    public string valeur{get; set;}

    public ImportPoint(){}
    public ImportPoint(string classement, string valeur){
        this.classement=classement;
        this.valeur=valeur;
    }

    public static void insertDirectTemporaire(List<ImportPoint> listes,NpgsqlConnection con,NpgsqlTransaction transaction)
    {
        foreach (ImportPoint item in listes)
        {
            string query = "INSERT INTO ImportPoint(classement, points) VALUES(@classement,@valeur)";
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con,transaction)){
                cmd.Parameters.AddWithValue("@classement",item.classement);
                cmd.Parameters.AddWithValue("@valeur",item.valeur);
                cmd.ExecuteNonQuery();
            }  
        }
    }

    
    public static void insertPoint(NpgsqlConnection con,NpgsqlTransaction transaction){
        string sql="INSERT INTO point(intitule, valeur) "+
                    "SELECT i.classement,i.points::integer from ImportPoint i ON CONFLICT DO NOTHING";
        Console.WriteLine(sql);
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }
    
    public static void nettoyage(NpgsqlConnection con,NpgsqlTransaction transaction){
        string sql="TRUNCATE TABLE ImportPoint";
        using (NpgsqlCommand command=new NpgsqlCommand(sql,con,transaction))
        {
            command.ExecuteNonQuery();
        }
    }

    public static void insertTransaction(List<ImportPoint> points, NpgsqlConnection con=null){
        bool estValid=true;
        NpgsqlTransaction transaction=null;
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            transaction = con.BeginTransaction();
            insertDirectTemporaire(points,con,transaction);
            insertPoint(con,transaction);
            nettoyage(con,transaction);
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