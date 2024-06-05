namespace Course.Models
{
    public class EtapeViewModel
    {
        public List<Etape> etapes { get; set; }
        public Utilisateur utilisateur {get; set;}
        public List<Coureur> coureurs {get; set;}
        public List<TempsCoureur> temps {get; set;}
        public List<Utilisateur> equipes {get; set;}
        public List<ClassementEtape> classementEtape{get; set;}
        public Etape etape{get; set;}

        public EtapeViewModel(List<Etape> etapes,Utilisateur utilisateur){
            this.etapes=etapes;
            this.utilisateur=utilisateur;
        } 
        public EtapeViewModel(List<Etape> etapes,List<Coureur> coureurs, Utilisateur utilisateur){
            this.etapes=etapes;
            this.utilisateur=utilisateur;
            this.coureurs=coureurs;
        } 

        public EtapeViewModel(List<Etape> etapes,List<TempsCoureur> temps, Utilisateur utilisateur){
            this.etapes=etapes;
            this.utilisateur=utilisateur;
            this.temps=temps;
        } 
        public EtapeViewModel(List<Etape> etapes,List<Coureur> coureurs,List<TempsCoureur> temps, Utilisateur utilisateur){
            this.etapes=etapes;
            this.coureurs=coureurs;
            this.utilisateur=utilisateur;
            this.temps=temps;
        } 
        public EtapeViewModel(List<Etape> etapes,List<Utilisateur> equipes){
            this.etapes=etapes;
            this.equipes=equipes;
        }
        public EtapeViewModel(Etape etape,List<ClassementEtape> classement){
            this.etape=etape;
            this.classementEtape=classement;
        }

    }
}
