using JobPortal.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class RecruiterController : Controller
    {
        AppDbContext dbContext { get; set; }
        public RecruiterController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(login login)
        {
            if(login._login != "Admin" && login._password!="12345")
            {
                ViewData["error"] = "login or password is incorecct";
                return View();
            }
            else
            {
                return RedirectToAction("Listoffers");
            }
            
        }
        public IActionResult Listoffers()
        {
            return View(dbContext.Offer.OrderBy(o => o.Title).ToList());
        }
        [HttpGet]
        public IActionResult ListCandidates(Guid Id)
        {
            ViewData["offerId"]= Id.ToString();
            return View(dbContext.Cabdidate.Where(w=>w.OfferId==Id).OrderBy(o => o.Prenom).ToList());
        }
        [HttpPost]
        public IActionResult ListCandidates(string Nom, string Prenom, string Mail, string Telepone, Guid OfferId)
        {
            ViewData["offerId"] = OfferId.ToString();
            var candidate = dbContext.Cabdidate.Where(w => w.OfferId == OfferId);
            if (!String.IsNullOrEmpty(Nom))
            {
                candidate = candidate.Where(x => Nom.Contains(x.Nom));
            }
            if (!String.IsNullOrEmpty(Prenom))
            {
                candidate = candidate.Where(x => Prenom.Contains(x.Prenom));
            }
            if (!String.IsNullOrEmpty(Mail))
            {
                candidate = candidate.Where(x => Mail.Contains(x.Mail));
            }
            if (!String.IsNullOrEmpty(Telepone))
            {
                candidate = candidate.Where(x => Telepone.Contains(x.Telephone));
            }
            return View(candidate);
        }
        public IActionResult DeleteCandidate(Guid id,Guid OfferId)
        {
            ViewData["offerId"] = OfferId.ToString();
            dbContext.Cabdidate.Remove(dbContext.Cabdidate.Where(w=>w.Id==id).FirstOrDefault());
            dbContext.SaveChanges();
            return RedirectToAction("ListCandidates", new {id = OfferId } );
        }
        public class login
        {
            public string _login { get; set; }
            public string _password { get; set; }
        }
    }
}
