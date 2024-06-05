namespace Course.Models;
using Npgsql;
public class ClassementCategorie{
    private int _rang;
    private Utilisateur _equipe;
    private Categorie _categorie;
    private int _points;

    public Utilisateur equipe{
        get { 
            return _equipe; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre equipe.");
            }
            _equipe = value;
        }
    }

    public int points{
        get { 
            return _points; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre points.");
            }
            _points = value;
        }
    }
    public int rang{
        get { 
            return _rang; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre rang.");
            }
            _rang = value;
        }
    }
    public Categorie categorie{
        get { 
            return _categorie; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre categorie.");
            }
            _categorie = value;
        }
    }

    public ClassementCategorie(){}
    public ClassementCategorie(Utilisateur equipe, Categorie categorie, int points, int rang){
        this.categorie=categorie;
        this.equipe = equipe;
        this.points=points;
        this.rang=rang;
    }

    public List<ClassementCategorie> GetClassementCategorie(NpgsqlConnection con=null){
        bool estValid = true;
        ClassementCategorie? classement = null;
        List<ClassementCategorie> classements = new List<ClassementCategorie>();
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "SELECT * FROM v_classement_equipe_tout_categorie";
                            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                using(NpgsqlDataReader reader = cmd.ExecuteReader()){
                    while (reader.Read())
                    {
                        int idUtilisateur = reader.GetInt32(reader.GetOrdinal("id_utilisateur"));
                        int idCategorie = reader.GetInt32(reader.GetOrdinal("id_categorie"));
                        int points = reader.GetInt32(reader.GetOrdinal("points_equipe"));
                        int rang = reader.GetInt32(reader.GetOrdinal("position"));
                        Utilisateur utilisateur  = Utilisateur.getById(idUtilisateur);
                        Categorie c  = Categorie.getById(idCategorie);
                        classement = new ClassementCategorie(utilisateur, c,points, rang);
                        classements.Add(classement);
                    }
                }
            }
        }catch(Exception e){
            throw e;
        }finally{
            if (estValid == false){
                con.Close();
            }
        }
        return classements;
    }

    public List<ClassementCategorie> GetClassementCategorieFiltree(string filtre, NpgsqlConnection con=null){
        bool estValid = true;
        ClassementCategorie? classement = null;
        List<ClassementCategorie> classements = new List<ClassementCategorie>();
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "SELECT * FROM v_classement_equipe_tout_categorie";
            Console.WriteLine(query);
            if(filtre!=null){
                if(filtre=="1" || filtre=="2"|| filtre=="3"){
                    Console.WriteLine(filtre);
                    query+=" WHERE id_categorie="+filtre;
                }
                else{
                    throw new Exception("Impossible de filtrer la liste en cette fonction");
                }
            }
                Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                using(NpgsqlDataReader reader = cmd.ExecuteReader()){
                    while (reader.Read())
                    {
                        int idUtilisateur = reader.GetInt32(reader.GetOrdinal("id_utilisateur"));
                        int idCategorie = reader.GetInt32(reader.GetOrdinal("id_categorie"));
                        int points = reader.GetInt32(reader.GetOrdinal("points_equipe"));
                        int rang = reader.GetInt32(reader.GetOrdinal("position"));
                        Utilisateur utilisateur  = Utilisateur.getById(idUtilisateur);
                        Categorie c  = Categorie.getById(idCategorie);
                        classement = new ClassementCategorie(utilisateur, c,points, rang);
                        classements.Add(classement);
                    }
                }
            }
        }catch(Exception e){
            throw e;
        }finally{
            if (estValid == false){
                con.Close();
            }
        }
        return classements;
    }
}