using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Microsoft.AspNetCore.Hosting;

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

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // Get: All  shop page
        public async Task<IActionResult> Shop()
        {

            return View(await _context.Products.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
