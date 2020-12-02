#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MimeKit.Text;

namespace Shop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // contact sent confirmation 
        public async Task<IActionResult> ContactUsConfirmation()
        {
            return View();
        } 
        public async Task<IActionResult> ContactUsNotConfirmation()
        {
            return View();
        }
// order sent confirmation 
        public async Task<IActionResult> OrderNotSentConfirmation()
        {
            return View();
        } 
        public async Task<IActionResult> OrderSentConfirmation()
        {
            return View();
        }

        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }
       public async Task<IActionResult> SendCustomerMessage(CustumerMessage custumerMessage)
       {
           try
           {
               var smt = new Mysmtp();
               var message = new MimeMessage();
               message.From.Add(new MailboxAddress(smt.user, smt.email));
               message.To.Add(new MailboxAddress(custumerMessage.Name, custumerMessage.Email));
               message.Subject = custumerMessage.Subject;
               message.Body = new TextPart(TextFormat.Html)
               {
                
                   Text = "<p>"+"<b>"+"customer Contact "+"<b/>"+"<p/>"+
                       "<p>"+"Name:  "+custumerMessage.Name+"<p/>" +
                          
                       "<p>"+"<b>"+"Contact:  "+"<b/>"+custumerMessage.Wnumber +"<p/>"+
                       "<p>"+"<b>"+"Email:  "+"<b/>"+custumerMessage.Email +"<p/>"+
                       "<p><b>Message:<b/><p/>"+
                       "<p>"+custumerMessage.Message +"<p/>" 
               };
               using var client = new MailKit.Net.Smtp.SmtpClient();
               await client.ConnectAsync(smt.SmtpServer, smt.Port, false);
 
               //SMTP server authentication
               await client.AuthenticateAsync(smt.email, smt.Password);
 
               await client.SendAsync(message);
 
               await client.DisconnectAsync(true);
           }
           catch (Exception )
           {
               return RedirectToAction("ContactUsConfirmation", "Products");
           }
           return RedirectToAction("ContactUsConfirmation", "Products");
       }
      
       public async Task<IActionResult> SendProductOrder(Buyproduct buyproduct)
       {
           try
           {
               var smt = new Mysmtp();
               var message = new MimeMessage();
               message.From.Add(new MailboxAddress(smt.user, smt.email));
               message.To.Add(new MailboxAddress(smt.OfficialName, smt.OfficialEmail));
               message.Subject = "Product Order";
               message.Body = new TextPart(TextFormat.Html)
               {
                
                   Text = "<p>"+"<b>New order from:<b/>  "+"<b>"+buyproduct.Name+"<b/>"+"<p/>"+
                          "<p>"+"product:  "+"<b>"+buyproduct.ProductName+"<b/>"+"<p/>"+
                           "<p>"+"Quantity:  "+"<b>"+buyproduct.Quantity+"<b/>"+"<p/>"+
                          "<p>"+"Contact:  "+buyproduct.MobileNumber+"<p/>" +
                          "<p>"+"Address: "+buyproduct.Address+"<p/>" +
                          "<p>"+"City: "+buyproduct.City+"<p/>" +
                          "<p>"+"CompanyName:  "+buyproduct.CompanyName+"<p/>" +
                          "<p>"+"SARID:  "+buyproduct.SarId+"<p/>" 
                           

               };
               using var client = new MailKit.Net.Smtp.SmtpClient();
               await client.ConnectAsync(smt.SmtpServer, smt.Port, false);
 
               //SMTP server authentication
               await client.AuthenticateAsync(smt.email, smt.Password);
 
               await client.SendAsync(message);
 
               await client.DisconnectAsync(true);
           }
           catch (Exception )
           {
             
               return RedirectToAction("OrderNotSentConfirmation", "Products");
           }
           return RedirectToAction("OrderSentConfirmation", "Products");
       }






        // Get: All  shop page
        public async Task<IActionResult> Shop(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return View(await _context.Products.ToListAsync());
            }

            var prod = _context.Products.Where(
                p => p.Name.ToLower().Contains(name.ToLower())
                     || p.BriefDescription.ToLower().Contains(name.ToLower())
                     || p.FullDescription.ToLower().Contains(name.ToLower()));
            return !prod.Any() ? View(await _context.Products.ToListAsync()) : View(await prod.ToListAsync());
        }
         // buy product
        public async Task<IActionResult> Buy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ProductId,Name,Code,BriefDescription,FullDescription,Price,Brand,ImagePath,ImageFile")] Product product)
        {
            if (!ModelState.IsValid) return View(product);

            var path = Path.Combine(_env.WebRootPath, "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileExtention = Path.GetExtension(product.ImageFile.FileName);
            var myUniqueFileName = string.Concat( DateTime.UtcNow.ToString("yymmddssfff"),fileExtention);
            await using (var stream = new FileStream(Path.Combine(path, myUniqueFileName), FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(stream);
            }

            product.ImagePath =  string.Concat("Images\\",myUniqueFileName);
            _context.Add(product);
            await _context.SaveChangesAsync();
            ModelState.Clear();
            return RedirectToAction(nameof(Index));
 
           
        
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Code,BriefDescription,FullDescription,Price,Brand,ImagePath,ImageFile")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(product);
            if (product.ImageFile == null)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else{
                // delete old image 

                var wwwPath = this._env.WebRootPath;

                var path = Path.Combine(this._env.WebRootPath, "Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var de = string.Concat(wwwPath,"\\", product.ImagePath);
                if (!System.IO.File.Exists(de))
                {
                    return NotFound("File not found");
                      
                }
                System.IO.File.Delete(de);

                // upload new image
                var fileExtention = Path.GetExtension(product.ImageFile.FileName);
                var myUniqueFileName = string.Concat( DateTime.UtcNow.ToString("yymmddssfff"),fileExtention);
                await using (var stream = new FileStream(Path.Combine(path, myUniqueFileName), FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }


                try
                {
                    product.ImagePath =  string.Concat("Images/",myUniqueFileName);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    ModelState.Clear();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            var wwwPath = this._env.WebRootPath;

            var path = Path.Combine(this._env.WebRootPath, "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var de = string.Concat(wwwPath,"\\", product.ImagePath);
            if (!System.IO.File.Exists(de))
            {
                return NotFound("File not found");
                      
            }
            System.IO.File.Delete(de);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
