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
    public class Promo1Controller : Controller
    {
        private readonly AppDbContext _context;

        public Promo1Controller(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var promos = await _context.Promos.Where(n => !n.IsDeleted).ToListAsync();
            return View(promos);
        }
        public async Task<IActionResult> Details(int id)
        {
            var promos = await _context.Promos.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (promos is null)
            {
                return NotFound();
            }
            return View(promos);
        }
        public async Task<IActionResult> Update(int id)
        {
            var promos = await _context.Promos.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (promos is null)
            {
                return NotFound();
            }
            return View(promos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, string promoRedirectedUrl)
        {
            var promos = await _context.Promos.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (promos is null)
            {
                return NotFound();
            }
            promos.RedirectedUrl = promoRedirectedUrl.Trim();
            promos.UpdatedTime = DateTime.Now;
            _context.Promos.Update(promos);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Details), controllerName: "promo1", routeValues: new { id });
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promo promo)
        {
            if (await isPromoExists(promo.RedirectedUrl))
            {
                return Content("Not correct");
            }
            promo.CreatedTime = DateTime.Now;

            await _context.Promos.AddAsync(promo);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: "index", controllerName: "promo1");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var promo = await _context.Promos.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (promo is null)
            {
                return NotFound();
            }
            promo.IsDeleted = true;
            return View(promo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Promo promo)
        {
            var dbPromo = await _context.Promos.Where(n => !n.IsDeleted && n.Id == promo.Id).FirstOrDefaultAsync();
            if (dbPromo.RedirectedUrl.ToLower() != dbPromo.RedirectedUrl.Trim().ToLower())
            {
                return NotFound();
            }
            if (dbPromo is null)
            {
                return NotFound();
            }
            dbPromo.IsDeleted = true;
            _context.Promos.Update(dbPromo);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: "promo1");
        }
        private async Task<bool> isPromoExists(string redirectedUrl)
        {
            var isExists = await _context.Promos.AnyAsync(n => n.RedirectedUrl.ToLower() == redirectedUrl.Trim().ToLower());
            return isExists;
        }
    }
}
