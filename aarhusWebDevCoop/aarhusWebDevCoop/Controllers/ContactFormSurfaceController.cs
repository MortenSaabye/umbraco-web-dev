using System.Web.Mvc;
using Umbraco.Web.Mvc;
using System.Net.Mail;
using aarhusWebDevCoop.ViewModels;
using Umbraco.Core.Models;

namespace aarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            ContactForm form = new ContactForm();
            return PartialView("ContactForm", form);
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm form)
        {
            if(!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            MailMessage message = new MailMessage();
            message.To.Add("saabye93@gmail.com");
            message.Subject = form.Subject;
            message.From = new MailAddress(form.Email, form.Name);
            message.Body = form.Message + "\n my email is: " + form.Email;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("saabye93@gmail.com", "vtuntwihiwcbfxew");
                smtp.Send(message);
                TempData["success"] = true;
            }
            IContent comment = Services.ContentService.CreateContent(form.Subject, CurrentPage.Id, "Comment");
            comment.SetValue("Username", form.Name);
            comment.SetValue("email", form.Email);
            comment.SetValue("subject", form.Subject);
            comment.SetValue("message", form.Message);
            Services.ContentService.Save(comment);
                return RedirectToCurrentUmbracoPage();
        }
    }
}