namespace Course.Models;

public class Point{
    private int _idPoint;
    private string? _intitule;
    private int _valeur;

    public int idPoint{
        get{ return _idPoint;} 
        set{
            if (value < 0) {
                throw new Exception("idPoint invalide");
            }
            _idPoint = value;
        }
    }
    public string intitule{
         get{ return _intitule ;} 
        set{
            if(value.Trim().Length==0 || value==null){
                throw new Exception("L'intitule du Point invalide");
            }
            _intitule=value.Trim();
        } 
    }

    public int valeur{
        get{ return _valeur;} 
        set{
            if(value < 0) {
                throw new Exception("valeur invalide");
            }
            _valeur = value;
        }
    }
    public Point(){}
    public Point(int idPoint, string intitule, int valeur){
        this.idPoint=idPoint;
        this.intitule=intitule;
        this.valeur=valeur;
    }
    

}