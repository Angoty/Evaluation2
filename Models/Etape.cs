namespace Course.Models;
using Npgsql;

public class Etape{
    private int _idEtape;
    private string? _intitule;
    private int _nbCoureur;
    private double _kilometre;
    private DateTime _heureDepart;
    private int _rang;
    private int _etat; 
    private List<Coureur> _coureurs;

    public int idEtape
    {
        get { return _idEtape; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur de l'idEtape ne peut pas être negative");
            }
            _idEtape = value;
        }
    }

    public string? intitule {
       get{
            if (_intitule == null){
                throw new Exception("La propriété intitule de l'etape n'a pas été initialisée.");
            }
            return _intitule;
        }set{
            if (value == null || value.Trim().Length == 0){
                throw new Exception("Le intitule de l'etape est invalide.");
            }
            _intitule = value.Trim();
        }
    }

    public int nbCoureur
    {
        get { return _nbCoureur; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur du nbCoureur ne peut pas être negative");
            }
            _nbCoureur = value;
        }
    }
    public double kilometre
    {
        get { return _kilometre; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur du kilometre ne peut pas être negative");
            }
            _kilometre = value;
        }
    }
    public DateTime heureDepart {
        get { 
            return _heureDepart; 
        }set {
            if (value == DateTime.MinValue) {
                _heureDepart = DateTime.Now;
            }
            _heureDepart = value;
        } 
    }
    public int rang
    {
        get { return _rang; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur du rang ne peut pas être negative");
            }
            _rang = value;
        }
    }
    public int etat
    {
        get { return _etat; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur du etat ne peut pas être negative");
            }
            _etat = value;
        }
    }
    public List<Coureur> coureurs{
        get { 
            return _coureurs; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre coureurs.");
            }
            _coureurs = value;
        }
    }

    public Etape(){}
    
    public Etape(int id, string intitule, int nbCoureur, double kilometre, DateTime depart, int rang, int etat){
        this.idEtape=id;
        this.intitule=intitule;
        this.nbCoureur=nbCoureur;
        this.kilometre=kilometre;
        this.heureDepart=depart;
        this.rang=rang;
        this.etat=etat;
    }


    public List<Etape> getAll(NpgsqlConnection con=null)
    {
        bool estValid = false;
        Etape  etape=null;
        List<Etape> etapes = new List<Etape>();
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query="SELECT * FROM etape WHERE etat=0";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idEtape = reader.GetInt32(reader.GetOrdinal("id_etape"));
                string intitule = reader.GetString(reader.GetOrdinal("intitule"));
                int nbCoureur = reader.GetInt32(reader.GetOrdinal("nb_coureur"));
                double kilometre=reader.GetDouble(reader.GetOrdinal("kilometre"));
                DateTime heure = reader.GetDateTime(reader.GetOrdinal("heure_depart"));
                int rang = reader.GetInt32(reader.GetOrdinal("rang_etape"));
                int etat = reader.GetInt32(reader.GetOrdinal("etat"));
                etape =new Etape(idEtape,  intitule, nbCoureur, kilometre, heure, rang, etat);
                etapes.Add(etape);
            }
            reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return etapes;
    }
    

    public List<Coureur> getAllCoureur(int id, NpgsqlConnection con=null)
    {
        bool estValid = false;
        List<Coureur> coureurs = new List<Coureur>();
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query="SELECT * FROM v_etape_coureur WHERE id_etape="+id;
            Console.WriteLine(query);
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idCoureur = reader.GetInt32(reader.GetOrdinal("id_coureur"));
                Coureur c = Coureur.getById(idCoureur);
                coureurs.Add(c);
            }
            
            reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return coureurs;
    }
    
    public void update(int idEtape, NpgsqlConnection con = null){
        bool estValid = true;
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "UPDATE Etape SET etat=5 WHERE id_etape=@idEtape";
            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                cmd.Parameters.AddWithValue("@idEtape",idEtape);
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

    public static Etape? getById(int id){
        NpgsqlConnection con=null;
        Etape? etape = null;
        bool estValid = false;
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query = "SELECT * FROM etape WHERE id_etape="+id;
            // Console.WriteLine(query);
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idEtape = reader.GetInt32(reader.GetOrdinal("id_etape"));
                string intitule = reader.GetString(reader.GetOrdinal("intitule"));
                int nbCoureur = reader.GetInt32(reader.GetOrdinal("nb_coureur"));
                double kilometre = reader.GetDouble(reader.GetOrdinal("kilometre"));
                DateTime heureDepart = reader.GetDateTime(reader.GetOrdinal("heure_depart"));
                int rang = reader.GetInt32(reader.GetOrdinal("rang_etape"));
                int etat = reader.GetInt32(reader.GetOrdinal("etat"));
                etape = new Etape(idEtape,intitule, nbCoureur, kilometre, heureDepart, rang, etat);
            }
             reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return etape;
    }

    public List<Etape> getAllEtape(NpgsqlConnection con=null)
    {
        bool estValid = false;
        Etape  etape=null;
        List<Etape> etapes = new List<Etape>();
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query="SELECT * FROM etape WHERE etat=5";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idEtape = reader.GetInt32(reader.GetOrdinal("id_etape"));
                string intitule = reader.GetString(reader.GetOrdinal("intitule"));
                int nbCoureur = reader.GetInt32(reader.GetOrdinal("nb_coureur"));
                double kilometre=reader.GetDouble(reader.GetOrdinal("kilometre"));
                DateTime heure = reader.GetDateTime(reader.GetOrdinal("heure_depart"));
                int rang = reader.GetInt32(reader.GetOrdinal("rang_etape"));
                int etat = reader.GetInt32(reader.GetOrdinal("etat"));
                etape =new Etape(idEtape,  intitule, nbCoureur, kilometre, heure, rang, etat);
                etapes.Add(etape);
            }
            reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return etapes;
    }
}