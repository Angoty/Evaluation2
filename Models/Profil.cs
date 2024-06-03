namespace Course.Models;
using Npgsql;
public class Profil{
    private int _idProfil;
    private string? _intitule;

    public int idProfil
    {
        get { return _idProfil; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur de l'idProfil ne peut pas être negative");
            }
            _idProfil = value;
        }
    }

    public string? intitule {
       get{
            if (_intitule == null){
                throw new Exception("La propriété intitule du profil n'a pas été initialisée.");
            }
            return _intitule;
        }set{
            if (value == null || value.Trim().Length == 0){
                throw new Exception("Le intitule du profil est invalide.");
            }
            _intitule = value.Trim();
        }
    }


    public Profil(){}
    public Profil(int id, string intitule){
        this.idProfil=id;
        this.intitule=intitule;
    }

    public static Profil? getById(int id){
        NpgsqlConnection con=null;
        Profil? g = null;
        bool estValid = false;
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query = "SELECT * FROM profil WHERE id_profil="+id;
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idProfil = reader.GetInt32(0);
                string intitule = reader.GetString(1);
                g = new Profil(idProfil, intitule);
            }
             reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return g;
    }
}