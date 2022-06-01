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
    public class Promo2Controller : Controller
    {
        private readonly AppDbContext _context;

        public Promo2Controller(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var promos2 = await _context.Promos2.Where(n => !n.IsDeleted).ToListAsync();
            return View(promos2);
        }
        public async Task<IActionResult> Details(int id)
        {
            var promos2 = await _context.Promos2.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (promos2 is null)
            {
                return NotFound();
            }
            return View(promos2);
        }
        public async Task<IActionResult> Update(int id)
        {
            var promos2 = await _context.Promos2.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (promos2 is null)
            {
                return NotFound();
            }
            return View(promos2);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, string promoRedirectedUrl)
        {
            var promos2 = await _context.Promos2.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (promos2 is null || string.IsNullOrEmpty(promoRedirectedUrl.Trim()))
            {
                return NotFound();
            }
            promos2.RedirectedUrl = promoRedirectedUrl.Trim();
            promos2.UpdatedTime = DateTime.Now;
            _context.Promos2.Update(promos2);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Details), controllerName: nameof(Promo2), routeValues: new { id });
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promo2 promo2)
        {
            if (await isPromo2Exists(promo2.Title1, promo2.Title2, promo2.RedirectedUrl, promo2.ButtonText, promo2.ButtonUrl))
            {
                return Content("Not correct");
            }
            promo2.CreatedTime = DateTime.Now;

            await _context.Promos2.AddAsync(promo2);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: "index", controllerName: nameof(Promo2));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var promo2 = await _context.Promos2.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (promo2 is null)
            {
                return NotFound();
            }
            promo2.IsDeleted = true;
            return View(promo2);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Promo2 promo2)
        {
            var dbPromo2 = await _context.Promos2.Where(n => !n.IsDeleted && n.Id == promo2.Id).FirstOrDefaultAsync();
            if (dbPromo2.RedirectedUrl.ToLower() != dbPromo2.RedirectedUrl.Trim().ToLower())
            {
                return NotFound();
            }
            if (dbPromo2 is null)
            {
                return NotFound();
            }
            dbPromo2.IsDeleted = true;
            _context.Promos2.Update(dbPromo2);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Promo2));
        }
        private async Task<bool> isPromo2Exists(string title1, string title2, string redirectedUrl, string buttonText, string buttonUrl)
        {
            var isExists = await _context.Promos2.AnyAsync(n => n.Title1.ToLower() == title1.Trim().ToLower()
                                                           && n.Title2.ToLower() == title2.Trim().ToLower()
                                                           && n.RedirectedUrl.ToLower() == redirectedUrl.Trim().ToLower()
                                                           && n.ButtonText.ToLower() == buttonText.Trim().ToLower()
                                                           && n.ButtonUrl.ToLower() == buttonUrl.Trim().ToLower());
            return isExists;
        }
    }
}
