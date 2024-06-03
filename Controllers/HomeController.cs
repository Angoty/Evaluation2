using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Course.Models.Imports;
using Course.Models;
using Course.Utils.Controllers.Sessions;
using Newtonsoft.Json;
namespace Course.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        if (TempData.ContainsKey("Erreur")) ViewBag.ErreurReset = TempData["Erreur"].ToString();
        if (TempData.ContainsKey("Succes")) ViewBag.SuccesReset = TempData["Succes"].ToString();

        string user=HttpContext.Session.GetString("admin");
        Utilisateur utilisateur=JsonConvert.DeserializeObject<Utilisateur>(user);
        List<Etape> etapes = new Etape().getAll();
        return View(new EtapeViewModel(etapes, utilisateur));
    }

    public IActionResult GetCoureursForEtape(int etapeId)
    {
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        List<Coureur> coureurs = new Etape().getAllCoureur(etapeId);
        return Json(coureurs);
    }

    [HttpPost]
    public IActionResult Affectation(IFormCollection form){
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        int idCoureur = int.Parse(form["coureur"]);
        int idEtape = int.Parse(form["etape"]);
        string temps = form["temps"];
        DateTime dateTime = DateTime.Parse(temps);
        new TempsCoureur().update(idEtape, idCoureur, dateTime);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult FinEtape(int etapeFin)
    {
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        new Etape().update(etapeFin);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ClassementEtape(){
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        List<Classement> classements = new Classement().GetClassementEtape();
        return View(classements);
    }

    public IActionResult ClassementEquipe(){
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        List<ClassementEquipe> classements = new ClassementEquipe().GetClassementEquipe();
        return View(classements);
    }

    public IActionResult Reinitialiser()
    {
        try{
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToAction("LoginAdmin", "Auth");
            }
            Reinitialisation.reset();
            TempData["Succes"] = "Réinitialisation terminée";
        }catch(Exception e){
            Console.WriteLine(e.Message);
            TempData["Erreur"] = e.Message;
        }
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ImportPoints(IFormFile etape)
    {
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        if (TempData.ContainsKey("ErreurImport")) ViewBag.ErreurImport = TempData["ErreurImport"].ToString();
        return View("ImportPoints");
    }

    public IActionResult Import(IFormFile etape)
    {
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        if (TempData.ContainsKey("ErreurImport")) ViewBag.ErreurImport = TempData["ErreurImport"].ToString();
        return View("ImportDonnees");
    }

    [HttpPost]
    public IActionResult TraitementImport(IFormFile etape, IFormFile resultat)
    {
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        if (etape == null || etape.Length == 0){
            Console.WriteLine("Aucun fichier selectionne");
            return RedirectToAction("Import","Home");
        }
        if (resultat == null || resultat.Length == 0){
            Console.WriteLine("Aucun fichier selectionne");
            return RedirectToAction("Import","Home");
        }
        List<ImportEtape> data = new List<ImportEtape>();
        List<ImportResultat> resultats = new List<ImportResultat>();
        using (var reader = new StreamReader(etape.OpenReadStream()))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                if(values.Length==6){
                    ImportEtape import=new ImportEtape(values[0],values[1],values[2],values[3],values[4],values[5]);
                    data.Add(import);
                }else{
                    throw new Exception("Nombre de colonne invalide");
                }
            }
        }
        using (var reader = new StreamReader(resultat.OpenReadStream()))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                if(values.Length==7){
                    ImportResultat import=new ImportResultat(values[0],values[1],values[2],values[3],values[4],values[5], values[6]);
                    resultats.Add(import);
                }else{
                    throw new Exception("Le nombre de colonne invalide");
                }
            }
        }
        try{
            ImportEtape.insertTransaction(data, resultats);
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filePath = Path.Combine(directory, Path.GetFileName(etape.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create)){
                etape.CopyTo(stream);
            }

        }catch(Exception e){
            Console.WriteLine(e.Message);
        }
        return RedirectToAction("Import","Home");
    }

    [HttpPost]
    public IActionResult TraitementImportPoints(IFormFile points)
    {
        if (HttpContext.Session.GetString("admin") == null)
        {
            return RedirectToAction("LoginAdmin", "Auth");
        }
        if (points == null || points.Length == 0){
            Console.WriteLine("Aucun fichier selectionne");
            return RedirectToAction("Import","Home");
        }
        List<ImportPoint> data = new List<ImportPoint>();
        Dictionary<int, List<string>> erreursPoint = new Dictionary<int, List<string>>();
        using (var reader = new StreamReader(points.OpenReadStream()))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                Console.WriteLine(values.Length);
                if(values.Length==2){
                    ImportPoint import=new ImportPoint(values[0],values[1]);
                    data.Add(import);
                }else{
                    throw new Exception("Le nombre de colonne invalide");
                }
            }
        }
        try{
            ImportPoint.insertTransaction(data);
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filePath = Path.Combine(directory, Path.GetFileName(points.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create)){
                points.CopyTo(stream);
            }

        }catch(Exception e){
            Console.WriteLine(e.Message);
        }
        return RedirectToAction("ImportPoints","Home");
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
