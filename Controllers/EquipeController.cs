using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Course.Models;
using Course.Utils.Controllers.Sessions;
using Newtonsoft.Json;
namespace Course.Controllers;

public class EquipeController : Controller
{
    private readonly ILogger<EquipeController> _logger;
    public EquipeController(ILogger<EquipeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("euipe") == null)
        {
            return RedirectToAction("LoginEquipe", "Auth");
        }
        ViewBag.Layout="_LayoutEquipe";
        string user=HttpContext.Session.GetString("equipe");
        Utilisateur utilisateur=JsonConvert.DeserializeObject<Utilisateur>(user);
        List<Etape> etapes = new Etape().getAll();
        List<Coureur> coureurs = new Coureur().getAllById(utilisateur.idUtilisateur);
        return View(new EtapeViewModel(etapes, coureurs, utilisateur));
    }

    [HttpPost]
    public IActionResult Affectation(IFormCollection form){
        if (HttpContext.Session.GetString("euipe") == null)
        {
            return RedirectToAction("LoginEquipe", "Auth");
        }
        int idCoureur = int.Parse(form["coureur"]);
        int idEtape = int.Parse(form["etape"]);
        new TempsCoureur().insert(idEtape, idCoureur);
        return RedirectToAction("Index", "Equipe");
    }

    public IActionResult ClassementEtape(){
        if (HttpContext.Session.GetString("euipe") == null)
        {
            return RedirectToAction("LoginEquipe", "Auth");
        }
        ViewBag.Layout="_LayoutEquipe";
        List<Classement> classements = new Classement().GetClassementEtape();
        return View(classements);
    }

    public IActionResult ClassementEquipe(){
        if (HttpContext.Session.GetString("euipe") == null)
        {
            return RedirectToAction("LoginEquipe", "Auth");
        }
        ViewBag.Layout="_LayoutEquipe";
        List<ClassementEquipe> classements = new ClassementEquipe().GetClassementEquipe();
        return View(classements);
    }

    // public IActionResult ListeCoureurs(){
    //     ViewBag.Layout="_LayoutEquipe";
    //     string user=HttpContext.Session.GetString("equipe");
    //     Utilisateur utilisateur=JsonConvert.DeserializeObject<Utilisateur>(user);
    //     List<Etape> etapes = new Etape().getAll();
    //     List<Coureur> coureurs = new Coureur().getAllById(utilisateur.idUtilisateur);
    //     List<TempsCoureur> temps = new TempsCoureur().getAllTempsCoureur(utilisateur.idUtilisateur);
    //     return View(new EtapeViewModel(etapes, coureurs , temps, utilisateur));
    // }

    // [HttpPost]
    // public IActionResult AffectationCoureur(IFormCollection form){
    //     int idCoureur = int.Parse(form["coureur"]);
    //     int idEtape = int.Parse(form["etape"]);
    //     new TempsCoureur().insert(idEtape, idCoureur);
    //     return RedirectToAction("ListeCoureurs", "Equipe");
    // }
}
