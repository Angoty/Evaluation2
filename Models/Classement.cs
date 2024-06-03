namespace Course.Models;
using Npgsql;
public class Classement{
    private Coureur _coureur;
    private int _points;
    private int _rang;

    public Coureur coureur{
        get { 
            return _coureur; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre coureur.");
            }
            _coureur = value;
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

    public Classement(){}
    public Classement(Coureur coureur, int points, int rang){
        this.coureur = coureur;
        this.points=points;
        this.rang=rang;
    }

    public List<Classement> GetClassementEtape(NpgsqlConnection con=null){
        bool estValid = true;
        Classement? classement = null;
        List<Classement> classements = new List<Classement>();
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "SELECT * FROM v_classement_cle";
                            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                using(NpgsqlDataReader reader = cmd.ExecuteReader()){
                    while (reader.Read())
                    {
                        int idCoureur = reader.GetInt32(reader.GetOrdinal("id_coureur"));
                        int total = reader.GetInt32(reader.GetOrdinal("total_points_coureur"));
                        int rang = reader.GetInt32(reader.GetOrdinal("position"));
                        Coureur c  = Coureur.getById(idCoureur);
                        classement = new Classement(c, total, rang);
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