using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace e_commmerce.Controllers
{
    public class NewsController : Controller
    {
        private readonly ShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsController(ShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: News
        public async Task<IActionResult> News(int? page, int pageSize = 3)
        {
            int pageNumber = page ?? 1;

            var newsItems = await _context.News
                                        .Include(p => p.Cate)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            var totalNewsCount = await _context.News.CountAsync();

            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalNewsCount / (double)pageSize);

            return View(newsItems);
        }

        public  IActionResult Index()
        {
            return View( _context.News.Include(n => n.Cate).ToList());
        }
        public IActionResult NewsDetails(int id)
        {
            var news = _context.News
                               .Include(n => n.Cate)
                               .FirstOrDefault(x => x.IdN == id);

            if (news == null)
            {
                return NotFound();
            }

            ViewData["Category"] = news.Cate;

            return View(news);
        }

        public IActionResult GetCategories()
        {
            var categories = _context.Categories.Select(c => new { Name = c.Name }).ToList();
            return Json(categories);
        }


        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.IdN == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();

            ViewData["Categories"] = categories;

            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile Image, string Title, string Description, DateTime TimeCreate, int CateId)
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Description) || TimeCreate == default || CateId == 0)
            {
                ModelState.AddModelError("", "All fields are required.");
                return View();
            }

            string imagePath = null;
            if (Image != null && Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "anh");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                imagePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }
                imagePath = "/anh/" + uniqueFileName;
            }

            var news = new News
            {
                Title = Title,
                Image = imagePath,
                Description = Description,
                TimeCreate = TimeCreate,
                CateId = CateId,
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = _context.Categories.ToList();
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile Image, string Title, string Description, DateTime TimeCreate, int CateId)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imagePath = news.Image; // Bảo toàn đường dẫn ảnh hiện tại

                    // Xử lý upload ảnh nếu có ảnh mới được cung cấp
                    if (Image != null && Image.Length > 0)
                    {
                        // Xóa ảnh hiện tại nếu cần thiết
                        if (!string.IsNullOrEmpty(news.Image))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, news.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload ảnh mới
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "anh");
                        Directory.CreateDirectory(uploadsFolder);
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                        string newImagePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(newImagePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(fileStream);
                        }
                        imagePath = "/anh/" + uniqueFileName; // Cập nhật đường dẫn ảnh mới
                    }

                    // Cập nhật các thuộc tính của tin tức
                    news.Title = Title;
                    news.Image = imagePath;
                    news.Description = Description;
                    news.TimeCreate = TimeCreate;
                    news.CateId = CateId;

                    _context.Update(news);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.IdN))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            

            return View(news); // Trả về view chỉnh sửa nếu ModelState không hợp lệ
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.IdN == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.IdN == id);
        }
    }
}
