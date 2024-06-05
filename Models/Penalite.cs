namespace Course.Models;
using Npgsql;
public class Penalite{
    
    private int _idPenalite;
    private Etape _etape;
    private Utilisateur _equipe;
    private DateTime _penalite;

    public int idPenalite
    {
        get { return _idPenalite; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur de l'idPenalite invalide");
            }
            _idPenalite = value;
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
    public Utilisateur? equipe{
        get { 
            return _equipe; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre equipe.");
            }
            _equipe = value;
        }
    }
    public DateTime penalite {
        get { 
            return _penalite; 
        }set {
            if (value == DateTime.MinValue) {
                _penalite = DateTime.Today;
            } 
            _penalite = value;
        } 
    }
    public Penalite (){}
    public Penalite (int id, Etape etape, Utilisateur equipe, DateTime penalite){
        this.idPenalite=id;
        this.etape=etape;
        this.equipe=equipe;
        this.penalite=penalite;
    }

    public List<Penalite> getAll(NpgsqlConnection con=null){
        bool estValid = false;
        Penalite  coureur=null;
        List<Penalite> coureurs = new List<Penalite>();
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query="SELECT * FROM penalite";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idPenalite = reader.GetInt32(reader.GetOrdinal("id_penalite"));
                int idEtape = reader.GetInt32(reader.GetOrdinal("id_etape"));
                int idEquipe = reader.GetInt32(reader.GetOrdinal("id_equipe"));
                DateTime penalite = reader.GetDateTime(reader.GetOrdinal("penalite"));
                Utilisateur u = Utilisateur.getById(idEquipe);
                Etape e = Etape.getById(idEtape);
                coureur =new Penalite(idPenalite, e, u, penalite);
                coureurs.Add(coureur);
            }
            reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return coureurs;
    }

    public void insertPenalite(int idEtape, int coureur, DateTime penaliteTemps, NpgsqlConnection con = null){
        bool estValid = true;
        try{
            if (con == null){
                con = Connect.connectDB();
                Console.WriteLine("ato indray aaaaaaaaaaaaa");
                estValid = false;
            }
            string query = "INSERT INTO Penalite(id_etape, id_equipe, penalite) VALUES(@idEtape, @idEquipe, @penaliteTemps)";
            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                cmd.Parameters.AddWithValue("@idEtape",idEtape);
                cmd.Parameters.AddWithValue("@idEquipe",coureur);
                cmd.Parameters.AddWithValue("@penaliteTemps",penaliteTemps);
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

    public void deletePenalite(int idPenalite, NpgsqlConnection con = null){
        bool estValid = true;
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "DELETE FROM Penalite WHERE id_penalite="+idPenalite;
            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
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

    public static Penalite? getById(int id){
        NpgsqlConnection con=null;
        Penalite? g = null;
        bool estValid = false;
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query = "SELECT * FROM penalite WHERE id_penalite="+id;
            Console.WriteLine(query);
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idPenalite = reader.GetInt32(0);
                int idEquipe = reader.GetInt32(1);
                int idUtilisateur = reader.GetInt32(2);
                Etape etape = Etape.getById(idEquipe);
                Utilisateur utilisateur = Utilisateur.getById(idUtilisateur);
                DateTime penalite = reader.GetDateTime(3);
                g = new Penalite(idPenalite, etape, utilisateur, penalite);
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