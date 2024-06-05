namespace Course.Models;
using Npgsql;

public class Coureur{
    private int _idCoureur;
    private string? _nom;
    private string? _numDossard;
    private DateTime _dateNaissance;
    private Genre _genre;
    private Utilisateur _equipe;

     public int idCoureur
    {
        get { return _idCoureur; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur de l'idCoureur invalide");
            }
            _idCoureur = value;
        }
    }
    public string? nom {
       get{
            if (_nom == null){
                throw new Exception("La propriété nom du coureur n'a pas été initialisée.");
            }
            return _nom;
        }set{
            if (value == null || value.Trim().Length == 0){
                throw new Exception("Le nom du coureur est invalide.");
            }
            _nom = value.Trim();
        }
    }

    public string? numDossard {
       get{
            if (_numDossard == null){
                throw new Exception("La propriété numDossard du coureur n'a pas été initialisée.");
            }
            return _numDossard;
        }set{
            if (value == null || value.Trim().Length == 0){
                throw new Exception("Le numDossard du coureur est invalide.");
            }
            _numDossard = value.Trim();
        }
    }
    public DateTime dateNaissance {
        get { 
            return _dateNaissance; 
        }set {
            if (value == DateTime.MinValue) {
                _dateNaissance = DateTime.Today;
            } else if (value.Date > DateTime.Today) {
                throw new Exception("Votre date de naissance n'est pas valide");
            } else {
                _dateNaissance = value;
            }
        } 
    }

    public Genre genre{
        get { 
            return _genre; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre genre.");
            }
            _genre = value;
        }
    }

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

    public Coureur(){}
    public Coureur(int id, string nom, string num, DateTime date, Genre genre, Utilisateur equipe){
        this.idCoureur=id;
        this.nom=nom;
        this.numDossard=num;
        this.dateNaissance=date;
        this.genre=genre;
        this.equipe=equipe;
    }

    public List<Coureur> getAll(NpgsqlConnection con=null)
    {
        bool estValid = false;
        Coureur  coureur=null;
        List<Coureur> coureurs = new List<Coureur>();
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query="SELECT * FROM coureur";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idCoureur = reader.GetInt32(reader.GetOrdinal("id_coureur"));
                string nom = reader.GetString(reader.GetOrdinal("nom"));
                string num = reader.GetString(reader.GetOrdinal("num_dossard"));
                DateTime date = reader.GetDateTime(reader.GetOrdinal("date_naissance"));
                int idGenre = reader.GetInt32(reader.GetOrdinal("id_genre"));
                int idEquipe = reader.GetInt32(reader.GetOrdinal("id_equipe"));
                Genre g = Genre.getById(idGenre);
                Utilisateur u = Utilisateur.getById(idEquipe);
                coureur =new Coureur(idCoureur,  nom, num, date, g, u);
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


    public List<Coureur> getAllById(int id, NpgsqlConnection con=null)
    {
        bool estValid = false;
        Coureur  coureur=null;
        List<Coureur> coureurs = new List<Coureur>();
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query="SELECT * FROM coureur WHERE id_equipe="+id;
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idCoureur = reader.GetInt32(reader.GetOrdinal("id_coureur"));
                string nom = reader.GetString(reader.GetOrdinal("nom"));
                string num = reader.GetString(reader.GetOrdinal("num_dossard"));
                DateTime date = reader.GetDateTime(reader.GetOrdinal("date_naissance"));
                int idGenre = reader.GetInt32(reader.GetOrdinal("id_genre"));
                int idEquipe = reader.GetInt32(reader.GetOrdinal("id_equipe"));
                Genre g = Genre.getById(idGenre);
                Utilisateur u = Utilisateur.getById(idEquipe);
                coureur =new Coureur(idCoureur,  nom, num, date, g, u);
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


    public static Coureur getById(int id, NpgsqlConnection con=null)
    {
        bool estValid = false;
        Coureur  coureur=null;
        try{
            if(con==null){
                estValid=true;
                con=Connect.connectDB();
            }
            string query="SELECT * FROM coureur WHERE id_coureur="+id;
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idCoureur = reader.GetInt32(reader.GetOrdinal("id_coureur"));
                string nom = reader.GetString(reader.GetOrdinal("nom"));
                string num = reader.GetString(reader.GetOrdinal("num_dossard"));
                DateTime date = reader.GetDateTime(reader.GetOrdinal("date_naissance"));
                int idGenre = reader.GetInt32(reader.GetOrdinal("id_genre"));
                int idEquipe = reader.GetInt32(reader.GetOrdinal("id_equipe"));
                Genre g = Genre.getById(idGenre);
                Utilisateur u = Utilisateur.getById(idEquipe);
                coureur =new Coureur(idCoureur,  nom, num, date, g, u);
            }
            reader.Close();
        }catch (Exception e){
            throw e; 
        }finally{
            if(estValid) con.Close();
        }
        return coureur;
    }

    public static void GenererCategories(NpgsqlConnection con, NpgsqlTransaction transaction)
    {
        string sql = "INSERT INTO CategorieCoureur (id_categorie, id_coureur)"+
                     " SELECT c.id_categorie, v.id_coureur FROM vue_categorie_coureur v INNER JOIN"+
                     " Categorie c ON v.categorie = c.intitule;";
        using (NpgsqlCommand command = new NpgsqlCommand(sql, con, transaction))
        {
            command.ExecuteNonQuery();
        }
    }


    public static void ok(NpgsqlConnection con=null){
        bool estValid=true;
        NpgsqlTransaction transaction=null;
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            transaction = con.BeginTransaction();
            GenererCategories(con,transaction);
            transaction.Commit();
        }
        catch (Exception e){
            Console.WriteLine(e.Message);
            transaction.Rollback();
            throw e;
        }finally{
            if (estValid == false) con.Close();
        }
    }


}