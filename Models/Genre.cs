using Npgsql;
namespace Course.Models;

public class Genre{
    private int _idGenre;
    private string _intitule;

    public int idGenre{
        get{ return _idGenre;} 
        set{
            if (value == null) {
                throw new Exception("La valeur de idGenre ne peut pas être nulle.");
            } else if (value < 0) {
                throw new Exception("idGenre invalide");
            } else {
                _idGenre = value;
            }
        }
    }
    public string intitule{
         get{ return _intitule ;} 
        set{
            if(value.Trim().Length==0 || value==null){
                throw new Exception("L'intitule du Genre invalide");
            }
            _intitule=value.Trim();
        } 
    }
    public Genre(){}
    public Genre(int idGenre, string intitule){
        this.idGenre=idGenre;
        this.intitule=intitule;
    }

    public static Genre? getById(int id){
        NpgsqlConnection con=null;
        Genre? g = null;
        bool estValid = false;
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query = "SELECT * FROM genre WHERE id_genre="+id;
            // Console.WriteLine(query);
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idGenre = reader.GetInt32(0);
                string intitule = reader.GetString(1);
                g = new Genre(idGenre, intitule);
            }
             reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return g;
    }

    public  List<Genre> getAll(NpgsqlConnection con){
        List<Genre> genres = new List<Genre>();
        Genre genre = null;
        bool estValid = false;
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query = "SELECT * FROM genre";
            // Console.WriteLine(query);
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idGenre = reader.GetInt32(0);
                string intitule = reader.GetString(1);
                genre = new Genre(idGenre, intitule);
                genres.Add(genre);
            }
             reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return genres;
    }

}
