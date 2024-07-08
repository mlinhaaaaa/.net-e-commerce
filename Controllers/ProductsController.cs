using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using e_commmerce.Entities.Authentication;

namespace e_commmerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [AdminAuthentication]
        public IActionResult Index()
        {
            var data = _context.Products.Include(p => p.Cate).ToList();
            ViewData["Categories"] = _context.Categories.ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, IFormFile imageFile, decimal price, string size, string description, int cateId)
        {
            string imagePath = null;
            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);
                imagePath = Path.Combine(uploadsFolder, imageFile.FileName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                imagePath = "/images/" + imageFile.FileName;
            }

            Product product = new Product()
            {
                Name = name,
                ImagePath = imagePath,
                Price = price,
                Size = size,
                Description = description,
                CateId = cateId
            };

            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var data = _context.Products.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, IFormFile imageFile, decimal price, string size, string description, int cateId)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return BadRequest();
            }

            string imagePath = product.ImagePath;
            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);
                imagePath = Path.Combine(uploadsFolder, imageFile.FileName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                imagePath = "/images/" + imageFile.FileName;
            }

            product.Name = name;
            product.ImagePath = imagePath;
            product.Price = price;
            product.Size = size;
            product.Description = description;
            product.CateId = cateId;
            _context.Products.Update(product);
            _context.SaveChanges();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return BadRequest();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult ShopList()
        {
            var products = _context.Products.Include(p => p.Cate).ToList();
            return View(products);
        }

    }
}
