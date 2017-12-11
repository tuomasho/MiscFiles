using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdManagerWeb.Models;
using Microsoft.AspNetCore.Authorization;
using CoreAdManagement;
using Microsoft.Extensions.Options;

namespace AdManagerWeb.Controllers
{
    public class HomeController : Controller
    {
        UserModel model = new UserModel();
        private AccountManagementConfig accountManagementConfig;

        public HomeController(IOptions<AccountManagementConfig> options)
        {
            accountManagementConfig = options?.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            var username = User.Identity.Name;
            username = username.Replace("CORE7\\", "");

            var betterManager = new BetterAdManager(accountManagementConfig);
            var user = betterManager.FindUser(username);

            model.DisplayName = user.DisplayName;
            model.Email = user.Email;
            model.EmployeeType = user.EmployeeType;
            model.Title = user.Title;
            model.Username = user.Username;

            return View(model);
        }

        [Authorize]
        public IActionResult Muokkaus()
        {
            var username = User.Identity.Name;
            username = username.Replace("CORE7\\", "");

            var betterManager = new BetterAdManager(accountManagementConfig);
            var user = betterManager.FindUser(username);

            model.DisplayName = user.DisplayName;
            model.Email = user.Email;
            model.EmployeeType = user.EmployeeType;
            model.Title = user.Title;
            model.Username = user.Username;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;

            return View(model);
        }

        //luo yhden uuden käyttäjän
        public IActionResult Lisays(string Enimi, string Snimi, string Password, string Email, string EmployeeType)
        {
            var betterManager = new BetterAdManager(accountManagementConfig);
            string displayName = Enimi + Snimi;
            string vastaus = betterManager.CreateUserAccount(Enimi, Snimi, displayName, Password, Email, EmployeeType);
            ViewData["utuja"] = vastaus; //onnistumis/virheviesti
            return ViewComponent("Lisays");
        }

        //muuttaa käyttäjän käyttäjänimen halutuksi
        public IActionResult Vaihda(string username, string newUsername)
        {
            var betterManager = new BetterAdManager(accountManagementConfig);
            string vastaus = betterManager.UpdateUserAccount(username, newUsername);
            ViewData["utuja"] = vastaus; //onnistumis/virheviesti
            return ViewComponent("Lisays");
        }

        //muuttaa käyttäjän etu -ja sukunimi tietoja
        public IActionResult OmatMuokkaus(string username, string fName, string lName)
        {
            var betterManager = new BetterAdManager(accountManagementConfig);
            string vastaus = betterManager.UpdateOwnAccount(username, fName, lName);
            ViewData["utuja"] = vastaus; //onnistumis/virheviesti
            return ViewComponent("Lisays");
        }
        
        //salasanan vaihto
        public IActionResult Salismuokkaus(string username, string salasana)
        {
            var betterManager = new BetterAdManager(accountManagementConfig);
            string vastaus = betterManager.UpdateUserPassword(username, salasana);
            ViewData["utuja"] = vastaus; //onnistumis/virheviesti
            return ViewComponent("Lisays");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
