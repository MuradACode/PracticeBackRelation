using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBackTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Where(n => !n.IsDeleted).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,
                                                string authorName,
                                                string categoryName,
                                                string name,
                                                string desc,
                                                decimal costPrice,
                                                decimal sellPrice,
                                                string infoText,
                                                bool isNew,
                                                bool isFeatured,
                                                int discount)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();
            if (product is null || string.IsNullOrEmpty(authorName))
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(name))
            {
                return NotFound();
            }
            if (costPrice <= 0 || sellPrice <= 0 || discount <= 0)
            {
                return NotFound();
            }
            product.Author.Fullname = authorName.Trim();
            product.Name = name.Trim();
            product.Description = desc.Trim();
            product.CostPrice = costPrice;
            product.SellPrice = sellPrice;
            product.InfoText = infoText.Trim();
            product.IsNew = isNew;
            product.IsFeatured = isFeatured;
            product.Discount = discount;
            product.UpdatedTime = DateTime.Now;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Details), controllerName: nameof(Product), routeValues: new { id });
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            product.CreatedTime = DateTime.Now;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Product), routeValues: new { id = product.Id });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();
            if (product is null)
            {
                return NotFound();
            }
            product.IsDeleted = true;
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Product product)
        {
            var dbProduct = await _context.Products.Where(n => !n.IsDeleted && n.Id == product.Id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();

            if (dbProduct.Name.ToLower() != product.Name.Trim().ToLower())
            {
                return NotFound();
            }
            if (dbProduct is null)
            {
                return NotFound();
            }
            dbProduct.IsDeleted = true;
            _context.Products.Update(dbProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Product));
        }
        private async Task<bool> isProductExists (int id,
                                                  string authorName,
                                                  string categoryName,
                                                  string name,
                                                  string desc,
                                                  decimal costPrice,
                                                  decimal sellPrice,
                                                  string infoText,
                                                  bool isNew,
                                                  bool isFeatured,
                                                  int discount)
        {
            var isExists = await _context.Products.Include(n => n.Author)
                                                  .Include(n => n.Category)
                                                  .AnyAsync(n => n.Author.Fullname.ToLower() == authorName.Trim().ToLower()
                                                           && n.Category.Name.ToLower() == categoryName.Trim().ToLower()
                                                           && n.Name.ToLower() == name.Trim().ToLower()
                                                           && n.Description.ToLower() == desc.Trim().ToLower()
                                                           && n.CostPrice == costPrice
                                                           && n.SellPrice == sellPrice
                                                           && n.InfoText.ToLower() == infoText.Trim().ToLower()
                                                           && n.IsNew == isNew
                                                           && n.IsFeatured == isFeatured
                                                           && n.Discount == discount);
            return isExists;
        }
    }
}
