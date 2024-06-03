namespace Course.Models;
using Npgsql;
public class TempsCoureur{
    private int _idTemps;
    private Etape _etape;
    private Coureur _coureur;
    private DateTime _heureArrivee;


    
    public int idTemps
    {
        get { return _idTemps; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur de l'idTemps invalide");
            }
            _idTemps = value;
        }
    }
    public Etape? etape{
        get { 
            return _etape; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre etape.");
            }
            _etape = value;
        }
    }

    public Coureur? coureur{
        get { 
            return _coureur; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre coureur.");
            }
            _coureur = value;
        }
    }
    public DateTime heureArrivee {
        get { 
            return _heureArrivee; 
        }set {
            if (value == DateTime.MinValue) {
                _heureArrivee = DateTime.Now;
            }
            _heureArrivee = value;
        } 
    }

    public TempsCoureur(){}
    public TempsCoureur(int id, Etape etape, Coureur coureur){
        this.idTemps=id;
        this.etape=etape;
        this.coureur=coureur;
    }
    public TempsCoureur(int id, Etape etape, Coureur coureur, DateTime heureArrivee){
        this.idTemps=id;
        this.etape=etape;
        this.coureur=coureur;
        this.heureArrivee=heureArrivee;
    }


     public void insert(int etape, int coureur, NpgsqlConnection con = null){
        Console.WriteLine("affiche");
        bool estValid = true;
        try{
            if (con == null){
                con = Connect.connectDB();
                Console.WriteLine("ato indray aaaaaaaaaaaaa");
                estValid = false;
            }
            string query = "INSERT INTO tempscoureur(id_etape, id_coureur) VALUES(@id_etape, @id_coureur)";
            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                cmd.Parameters.AddWithValue("@id_etape",etape);
                cmd.Parameters.AddWithValue("@id_coureur",coureur);
                cmd.ExecuteNonQuery();
            }
        }catch(Exception e){
            throw e;
        }finally{
            if (estValid == false){
                con.Close();
            }
        }
    }

    public void update(int etape, int coureur, DateTime heureArrivee, NpgsqlConnection con = null){
        bool estValid = true;
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "UPDATE TempsCoureur SET heure_arrivee=@heureArrivee WHERE id_etape=@idEtape AND id_coureur=@idCoureur";
            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                cmd.Parameters.AddWithValue("@idEtape",etape);
                cmd.Parameters.AddWithValue("@idCoureur",coureur);
                cmd.Parameters.AddWithValue("@heureArrivee",heureArrivee);
                cmd.ExecuteNonQuery();
            }
        }catch(Exception e){
            throw e;
        }finally{
            if (estValid == false){
                con.Close();
            }
        }
    }

    // public List<TempsCoureur> getAllTempsCoureur(int id, NpgsqlConnection con=null)
    // {
    //     bool estValid = false;
    //     List<TempsCoureur> tempsCoureur = new List<TempsCoureur>();
    //     List<Etape> etapes = new Etape().getAll();
    //     try{
    //         if(con==null){
    //             estValid=true;
    //             con=Connect.connectDB();
    //         }
    //         for(int i =0; i<etapes.Count; i++){    
    //             string query="SELECT * FROM v_etape_coureur WHERE id_etape="+etapes[i].idEtape+" AND id_equipe="+id;
    //             Console.WriteLine(query);
    //             NpgsqlCommand cmd = new NpgsqlCommand(query, con);
    //             NpgsqlDataReader reader = cmd.ExecuteReader();
    //             while (reader.Read())
    //             {
    //                 int idCoureur = reader.GetInt32(reader.GetOrdinal("id_coureur"));
    //                 int idTemps = reader.GetInt32(reader.GetOrdinal("id_temps"));
    //                 int idEtape = reader.GetInt32(reader.GetOrdinal("id_etape"));
    //                 DateTime? heureArrivee = reader.IsDBNull(reader.GetOrdinal("heure_arrivee")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("heure_arrivee"));
    //                 // DateTime heureArrivee = reader.GetDateTime();
    //                 Coureur c = Coureur.getById(idCoureur);
    //                 Etape  e = Etape.getById(idEtape);
    //                 TempsCoureur temps = new TempsCoureur(id, e, c, heureArrivee);
    //                 tempsCoureur.Add(temps);
    //             }
    //             reader.Close();
    //         }
    //     }catch (Exception e){
    //         throw e; 
    //     }finally{
    //         if(estValid) con.Close();
    //     }
    //     return tempsCoureur;
    // }

}