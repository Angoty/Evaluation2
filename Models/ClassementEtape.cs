namespace Course.Models;
using Npgsql;
public class ClassementEtape{
    private Utilisateur _equipe;
    private Coureur _coureur;
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

    public ClassementEtape(){}
    public ClassementEtape(Utilisateur equipe, Coureur coureur, int points, int rang){
        this.equipe = equipe;
        this.coureur=coureur;
        this.points=points;
        this.rang=rang;
    }

    public List<ClassementEtape> GetClassementEtape(int id, NpgsqlConnection con=null){
        bool estValid = true;
        ClassementEtape? classement = null;
        List<ClassementEtape> classements = new List<ClassementEtape>();
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "SELECT * FROM v_classement_equipe_Etape2 WHERE id_etape="+id;
                            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                using(NpgsqlDataReader reader = cmd.ExecuteReader()){
                    while (reader.Read())
                    {
                        int idEquipe = reader.GetInt32(reader.GetOrdinal("id_equipe"));
                        int points = reader.GetInt32(reader.GetOrdinal("points"));
                        int idCoureur = reader.GetInt32(reader.GetOrdinal("id_coureur"));
                        int rang = reader.GetInt32(reader.GetOrdinal("position"));
                        Utilisateur c  = Utilisateur.getById(idEquipe);
                        Coureur coureur = Coureur.getById(idCoureur);
                        classement = new ClassementEtape(c, coureur, points, rang);
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