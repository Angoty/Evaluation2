namespace Course.Models
{
    public class ClassementCategorieViewModel
    {
        public List<Categorie> categories { get; set; }
        public List<ClassementCategorie> classements{get; set;}
        public string filtre {get; set;}


        public ClassementCategorieViewModel(){}
        public ClassementCategorieViewModel(List<Categorie> categories, List<ClassementCategorie> classements, string filtre){
            this.categories=categories;
            this.classements=classements;
            this.filtre=filtre;
        } 
    }
}
