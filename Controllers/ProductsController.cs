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
        public async Task<IActionResult> Create(string name, IFormFile imageFile, IFormFile imageFile2, IFormFile imageFile3, decimal price, string size, string description, int cateId)
        {
            if (string.IsNullOrEmpty(name) || price <= 0 || cateId <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);

            string imagePath = null;
            string imagePath2 = null;
            string imagePath3 = null;   

            if (imageFile != null)
            {
                imagePath = Path.Combine(uploadsFolder, imageFile.FileName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                imagePath = "/images/" + imageFile.FileName;
            }

            if (imageFile2 != null)
            {
                imagePath2 = Path.Combine(uploadsFolder, imageFile2.FileName);
                using (var fileStream = new FileStream(imagePath2, FileMode.Create))
                {
                    await imageFile2.CopyToAsync(fileStream);
                }
                imagePath2 = "/images/" + imageFile2.FileName;
            }

            if (imageFile3 != null)
            {
                imagePath3 = Path.Combine(uploadsFolder, imageFile3.FileName);
                using (var fileStream = new FileStream(imagePath3, FileMode.Create))
                {
                    await imageFile3.CopyToAsync(fileStream);
                }
                imagePath3 = "/images/" + imageFile3.FileName;
            }

            Product product = new Product()
            {
                Name = name,
                ImagePath = imagePath,
                ImagePath2 = imagePath2,
                ImagePath3 = imagePath3,
                Price = price,
                Size = size,
                Description = description,
                CateId = cateId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, string name,  IFormFile imageFile, IFormFile imageFile2, IFormFile imageFile3, decimal price, string size, string description, int cateId)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return BadRequest();
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);

            string imagePath = product.ImagePath;
            string imagePath2 = product.ImagePath2;
            string imagePath3 = product.ImagePath3;

            if (imageFile != null)
            {
                imagePath = Path.Combine(uploadsFolder, imageFile.FileName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                imagePath = "/images/" + imageFile.FileName;
            }

            if (imageFile2 != null)
            {
                imagePath2 = Path.Combine(uploadsFolder, imageFile2.FileName);
                using (var fileStream = new FileStream(imagePath2, FileMode.Create))
                {
                    await imageFile2.CopyToAsync(fileStream);
                }
                imagePath2 = "/images/" + imageFile2.FileName;
            }

            if (imageFile3 != null)
            {
                imagePath3 = Path.Combine(uploadsFolder, imageFile3.FileName);
                using (var fileStream = new FileStream(imagePath3, FileMode.Create))
                {
                    await imageFile3.CopyToAsync(fileStream);
                }
                imagePath3 = "/images/" + imageFile3.FileName;
            }


            product.Name = name;
            product.ImagePath = imagePath;
            product.ImagePath2 = imagePath2;
            product.ImagePath3 = imagePath3;
            product.Price = price;
            product.Size = size;
            product.Description = description;
            product.CateId = cateId;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
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

        [HttpGet]
        public IActionResult Shop(string orderby, int?[] selectedColors, int? categoryId)
        {
            var categories = _context.Categories.ToList();
            var colors = _context.Colors.ToList();

            ViewData["Categories"] = categories;
            ViewData["Colors"] = colors;
            ViewBag.UserUid = HttpContext.Session.GetString("UserUid");
            ViewBag.IsAdmin = HttpContext.Session.GetInt32("IsAdmin") == 1;

            IQueryable<Product> products = _context.Products.Include(p => p.Cate).AsQueryable();

            if (selectedColors != null && selectedColors.Length > 0)
            {
                products = products.Where(p => p.Color.Name != null && selectedColors.Contains(p.Color.Id));
            }
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CateId == categoryId);
            }

            switch (orderby)
            {
                case "price-lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price-highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "name-ascending":
                    products = products.OrderBy(p => p.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Id);
                    break;
            }

            return View(products.Take(6).ToList());
        }
        [HttpGet]
        public IActionResult FilterPrice(decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<Product> products = _context.Products.Include(p => p.Cate).AsQueryable();

            if (minPrice != null && maxPrice != null)
            {
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            return PartialView("_ProductListPartial", products.Take(6).ToList());
        }

        [HttpPost]
        public IActionResult LoadMore(int skip, int take)
        {
            var products = _context.Products.Skip(skip).Take(take).ToList();
            if (products == null || !products.Any())
            {
                return Json(new { success = false, message = "No more products." });
            }
            return PartialView("_ProductListPartial", products);
        }

        public IActionResult SingleProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
