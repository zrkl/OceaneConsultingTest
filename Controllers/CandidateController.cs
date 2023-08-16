using JobPortal.DBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace JobPortal.Controllers
{
    public class CandidateController : Controller
    {
        AppDbContext dbContext { get; set; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CandidateController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(dbContext.Offer.OrderBy(o => o.Title).ToList());
        }

        [HttpGet]
        public IActionResult Apply(Guid Id)
        {

            return View();
        }
        [HttpPost]
        public IActionResult Apply(CandidatEntity candidate, IFormFile cvFile)
        {
            if (cvFile != null && cvFile.Length > 0)
            {

                string pdfFolder = "PDF"; // Specify the folder name
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, pdfFolder);
                string uniqueFileName = $"{Guid.NewGuid()}_CV_{candidate.Prenom}_{candidate.Nom}." + cvFile.ContentType.Split('/')[1];
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    cvFile.CopyTo(fileStream);
                }
                candidate.CVFilePath = Path.Combine(uniqueFileName);
            }
            candidate.OfferId = candidate.Id;
            candidate.Id = Guid.NewGuid();
            
            dbContext.Add(candidate);
            dbContext.SaveChanges();
            SendMail(candidate.Id);
            return RedirectToAction("Done");

        }
        public void SendMail(Guid CandidateId)
        {
            var candidate = dbContext.Cabdidate.Where(w => w.Id == CandidateId).FirstOrDefault();
            var offer = dbContext.Offer.Where(w => w.Id == candidate.OfferId).FirstOrDefault();


            string formmail = "loutfyelmehdix@gmail.com";
            string formPassword = "twjvkbyofxcqabsn";

            MailMessage message = new MailMessage();
            message.From =new MailAddress(formmail) ;
            message.Subject = "Candidature au poste : "+offer.Title;
            message.To.Add(new MailAddress(candidate.Mail));
            message.Body = $"Bonjour\r\n{candidate.Prenom} {candidate.Nom}, vous avez postulé avec succès pour l'offre {offer.Title}";
            message.IsBodyHtml =false;

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(formmail, formPassword),
                EnableSsl = true,
            };
            smtp.Send(message);
            

        }
        public IActionResult Done()
        {
            return View();
        }
    }
}
