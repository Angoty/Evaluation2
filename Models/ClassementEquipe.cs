namespace Course.Models;
using Npgsql;
public class ClassementEquipe{
    private Utilisateur _equipe;
    private int _points;
    private int _rang;

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

    public ClassementEquipe(){}
    public ClassementEquipe(Utilisateur equipe, int points, int rang){
        this.equipe = equipe;
        this.points=points;
        this.rang=rang;
    }

    public List<ClassementEquipe> GetClassementEquipe(NpgsqlConnection con=null){
        bool estValid = true;
        ClassementEquipe? classement = null;
        List<ClassementEquipe> classements = new List<ClassementEquipe>();
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "SELECT * FROM v_equipe_classement";
                            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                using(NpgsqlDataReader reader = cmd.ExecuteReader()){
                    while (reader.Read())
                    {
                        int idEquipe = reader.GetInt32(reader.GetOrdinal("id_equipe"));
                        int total = reader.GetInt32(reader.GetOrdinal("points_equipe"));
                        int rang = reader.GetInt32(reader.GetOrdinal("position"));
                        Utilisateur c  = Utilisateur.getById(idEquipe);
                        classement = new ClassementEquipe(c, total, rang);
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