using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Bakery.Data;
using Bakery.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bakery.Pages
{
    public class OrderModel : PageModel
    {
        private readonly BakeryDataContext _context;

        public OrderModel(BakeryDataContext context)
        {
            _context = context;
        }


        [Required(ErrorMessage ="Your Email address is required")]
        [BindProperty]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Your shipping address is required")]
        [BindProperty]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Order quantity is required")]
        [BindProperty]
        public int OrderQty { get; set; } = 1;

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid) return Page();

            Product = await _context.Products.FindAsync(id);

            //Enter your Hotmail credentials for UserName/Password and a "From" address for the e-mail
            var SmtpUserName = "";
            var SmtpPassword = "";
            var From = "";

            if (string.IsNullOrEmpty(SmtpUserName) || string.IsNullOrEmpty(SmtpPassword) || string.IsNullOrEmpty(From))
            {
                TempData["NoEmail"] = 1;
                return RedirectToPage("OrderSuccess");
            }
            else
            {
                try
                {
                    await SendMailAsync(SmtpUserName,SmtpPassword,From);
                    Response.Redirect("~/OrderSuccess");
                }
                catch
                {
                    ModelState.AddModelError ("CustomerEmail", "There was an error and your order could not be processed at this time");
                }
            }

            return Page();
        }

        private async Task SendMailAsync(string smtpUserName, string smtpPassword, string from)
        {
            string body = BuildEmailBody();

            //SMTP Configuration for Hotmail
            var SmtpServer = "smtp.live.com";
            var SmtpPort = 25;
            var EnableSsl = false;

            SmtpClient client = new SmtpClient(SmtpServer, SmtpPort);
            client.Credentials = new NetworkCredential
            {
                UserName = smtpUserName,
                Password = smtpPassword
            };
            client.EnableSsl = EnableSsl;
            try
            {
                await client.SendMailAsync(from, CustomerEmail, "Fourth Coffee - New Order", body);
            }
            catch
            {
                throw;
            }
        }

        private string BuildEmailBody()
        {
            var body = $"Thank you, we have received your order for {OrderQty} unit(s) of {Product.Name} !<br/>";
            var orderShipping = ShippingAddress;
            var customerEmail = CustomerEmail;
            if (!string.IsNullOrEmpty(orderShipping))
            {
                //Replace carriage returns with HTML breaks for HTML mail
                var formattedOrder = orderShipping.Replace("\r\n", "<br/>");
                body += "Your address is: <br/>" + formattedOrder + "<br/>";
            }
            body += "Your total is $" + Product.Price * Convert.ToDecimal(OrderQty) + ".<br/>";
            body += "We will contact you if we have questions about your order.  Thanks!<br/>";
            return body;
        }
    }
}