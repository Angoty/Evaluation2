namespace Course.Models;
using Npgsql;
public class Utilisateur{
   
    private int _idUtilisateur;
    private string _nom;
    private string _email;
    private string _motDePasse;
    private Profil _profil;


    public int idUtilisateur
    {
        get { return _idUtilisateur; }
        set
        {
            if (value < 0)
            {
                throw new Exception("La valeur de l'idUtilisateur invalide");
            }
            _idUtilisateur = value;
        }
    }
    public string? nom {
       get{
            if (_nom == null){
                throw new Exception("La propriété nom de l'Utilisateur n'a pas été initialisée.");
            }
            return _nom;
        }set{
            if (value == null || value.Trim().Length == 0){
                throw new Exception("Le nom de l'Utilisateur est invalide.");
            }
            _nom = value.Trim();
        }
    }
    public string? email {
       get{
            if (_email == null){
                throw new Exception("La propriété email de l'Utilisateur n'a pas été initialisée.");
            }
            return _email;
        }set{
            if (value == null || value.Trim().Length == 0){
                throw new Exception("Vous devez donnez votre email.");
            }
            _email = value.Trim();
        }
    }

    public string? motDePasse {
       get{
            if (_motDePasse == null){
                throw new Exception("La propriété motDePasse de l'Utilisateur n'a pas été initialisée.");
            }
            return _motDePasse;
        }set{
            if (value == null || value.Trim().Length == 0){
                throw new Exception("Vous devez donner votre mot de passe.");
            }
            _motDePasse = value.Trim();
        }
    }

    public Profil? profil{
        get { 
            return _profil; 
        }set{
            if (value == null){
                throw new Exception("Vous devez donner votre profil.");
            }
            _profil = value;
        }
    }

    public Utilisateur(){}
    public Utilisateur(int id, string nom, string email, string motDePasse, Profil profil){
        this.idUtilisateur=id;
        this.nom=nom;
        this.email=email;
        this.motDePasse=motDePasse;
        this.profil=profil;
    }
    public Utilisateur(int id, string nom, string email){
        this.idUtilisateur=id;
        this.nom=nom;
        this.email=email;
    }


    public Utilisateur? check(int profil, NpgsqlConnection con = null)
    {
        bool estValid = true;
        Utilisateur? utilisateur = null;
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "SELECT * FROM Utilisateur WHERE email='" + this.email + 
                            "' AND mot_de_passe='" + this.motDePasse + "' AND id_profil="+profil;
                            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                using(NpgsqlDataReader reader = cmd.ExecuteReader()){
                    while (reader.Read())
                    {
                        int idUtilisateur = reader.GetInt32(0);
                        string nom = reader.GetString(1);
                        string email = reader.GetString(2);
                        string mdp  = reader.GetString(3);
                        int idProfil = reader.GetInt32(4);
                        Profil p = Profil.getById(idProfil);
                        utilisateur = new Utilisateur(idUtilisateur, nom, email, mdp, p);
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
        return utilisateur;
    }

    public static Utilisateur? getById(int id)
    {
        NpgsqlConnection con = null;
        bool estValid = true;
        Utilisateur? Utilisateur = null;
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "SELECT * FROM Utilisateur WHERE id_utilisateur="+id;
                            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                using(NpgsqlDataReader reader = cmd.ExecuteReader()){
                    while (reader.Read())
                    {
                        int idUtilisateur = reader.GetInt32(0);
                        string nom = reader.GetString(1);
                        string email = reader.GetString(2);
                        string mdp  = reader.GetString(3);
                        int idProfil = reader.GetInt32(4);
                        Profil p = Profil.getById(idProfil);
                        Utilisateur = new Utilisateur(idUtilisateur, nom, email, mdp, p);
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
        return Utilisateur;
    }

    public  List<Utilisateur> getAll(Utilisateur u)
    {
        NpgsqlConnection con = null;
        bool estValid = true;
        Utilisateur? utilisateur = null;
        List<Utilisateur> utilisateurs = new List<Utilisateur>();
        try{
            if (con == null){
                con = Connect.connectDB();
                estValid = false;
            }
            string query = "SELECT * FROM Utilisateur WHERE id_utilisateur="+u.idUtilisateur;
                            Console.WriteLine(query);
            using(NpgsqlCommand cmd = new NpgsqlCommand(query, con)){
                using(NpgsqlDataReader reader = cmd.ExecuteReader()){
                    while (reader.Read())
                    {
                        int idUtilisateur = reader.GetInt32(0);
                        string nom = reader.GetString(1);
                        string email = reader.GetString(2);
                        string mdp  = reader.GetString(3);
                        int idProfil = reader.GetInt32(4);
                        Profil p = Profil.getById(idProfil);
                        utilisateur = new Utilisateur(idUtilisateur, nom, email, mdp, p);
                        utilisateurs.Add(utilisateur);
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
        return utilisateurs;
    }


}