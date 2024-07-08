using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using e_commmerce.Entities.Authentication;

namespace e_commmerce.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ShopContext _context;

        public CategoriesController(ShopContext context)
        {
            _context = context;
        }
        [AdminAuthentication]
        public IActionResult Index()
        {
            var data = _context.Categories.Include(c => c.Products).ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Create(string name)
        {
            Category category = new Category()
            {
                Name = name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok(category);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var data = _context.Categories.FirstOrDefault(x => x.Cid == id);
            if (data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Edit(int id, string name)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Cid == id);
            if (category == null)
            {
                return BadRequest();
            }

            category.Name = name;
            _context.Categories.Update(category);
            _context.SaveChanges();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Cid == id);
            if (category == null)
            {
                return BadRequest();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult CategoriesPartial()
        {
            var categories = _context.Categories.ToList();

            return PartialView("_CategoriesPartial", categories);
        }
    }
}
