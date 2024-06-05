using Npgsql;
namespace Course.Models;

public class Categorie{
    private int _idCategorie;
    private string _intitule;

    public int idCategorie{
        get{ return _idCategorie;} 
        set{
            if (value == null) {
                throw new Exception("La valeur de idCategorie ne peut pas être nulle.");
            } else if (value < 0) {
                throw new Exception("idCategorie invalide");
            } else {
                _idCategorie = value;
            }
        }
    }
    public string intitule{
         get{ return _intitule ;} 
        set{
            if(value.Trim().Length==0 || value==null){
                throw new Exception("L'intitule du Categorie invalide");
            }
            _intitule=value.Trim();
        } 
    }
    public Categorie(){}
    public Categorie(int idCategorie, string intitule){
        this.idCategorie=idCategorie;
        this.intitule=intitule;
    }

    public static Categorie? getById(int id){
        NpgsqlConnection con=null;
        Categorie? g = null;
        bool estValid = false;
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query = "SELECT * FROM categorie WHERE id_categorie="+id;
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idCategorie = reader.GetInt32(0);
                string intitule = reader.GetString(1);
                g = new Categorie(idCategorie, intitule);
            }
             reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return g;
    }

    public  List<Categorie> getAll(NpgsqlConnection con=null){
        List<Categorie> categories = new List<Categorie>();
        Categorie categorie = null;
        bool estValid = false;
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query = "SELECT * FROM categorie";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idCategorie = reader.GetInt32(0);
                string intitule = reader.GetString(1);
                categorie = new Categorie(idCategorie, intitule);
                categories.Add(categorie);
            }
             reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return categories;
    }

}
