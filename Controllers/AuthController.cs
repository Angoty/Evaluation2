using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Course.Models;
using Newtonsoft.Json;
namespace Course.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }
    public IActionResult LoginEquipe()
    {
        if (TempData.ContainsKey("ErreurLogin")) ViewBag.ErreurLogin = TempData["ErreurLogin"].ToString();
        ViewBag.Layout="_LayoutLogin";
        return View("Login");
    }
    public IActionResult LoginAdmin()
    {
        if (TempData.ContainsKey("ErrLogAdmin")) ViewBag.ErreurLogAdmin = TempData["ErrLogAdmin"].ToString();
        ViewBag.Layout="_LayoutLogin";
        return View();
    }

    [HttpPost]
    public IActionResult checkLogAdmin(IFormCollection form){
        string email = form["email"];
        string motDePasse = form["mdp"];
        try
        {
            Utilisateur utilisateur = new Utilisateur{email=email, motDePasse=motDePasse}.check(1);
            if(utilisateur!=null){
                var str = JsonConvert.SerializeObject(utilisateur);
                HttpContext.Session.SetString("admin",str);
            }
            else{
                throw new Exception("Vérifiez vos identifiants.");
            }
        }catch(Exception exe){
            TempData["ErrLogAdmin"] = exe.Message;
            return RedirectToAction("LoginAdmin","Auth");    
        }
        return RedirectToAction("Index","Home");
    }

    [HttpPost]
    public IActionResult checkLogEquipe(IFormCollection form){
        string email = form["email"];
        string motDePasse = form["mdp"];
        try
        {
            Utilisateur utilisateur = new Utilisateur{email=email, motDePasse=motDePasse}.check(2);
            if(utilisateur!=null){
                var str = JsonConvert.SerializeObject(utilisateur);
                HttpContext.Session.SetString("equipe",str);
            }
            else{
                throw new Exception("Vérifiez vos identifiants");
            }
        }catch(Exception exe){
            TempData["ErreurLogin"] = exe.Message;
            return RedirectToAction("LoginEquipe","Auth");    
        }
        return RedirectToAction("Index","Equipe");
    }
    public IActionResult Deconnexion(){
        HttpContext.Session.Clear();
        return RedirectToAction("LoginEquipe","Auth");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
