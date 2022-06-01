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
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var features = await _context.Features.Where(n => !n.IsDeleted).ToListAsync();
            return View(features);
        }
        public async Task<IActionResult> Details(int id)
        {
            var features = await _context.Features.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (features is null)
            {
                return NotFound();
            }
            return View(features);
        }
        public async Task<IActionResult> Update(int id)
        {
            var features = await _context.Features.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (features is null)
            {
                return NotFound();
            }
            return View(features);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,
                                                string featureIconUrl,
                                                string featureTitle,
                                                string featureDesc)
        {
            var features = await _context.Features.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (features is null || string.IsNullOrEmpty(featureIconUrl.Trim()))
            {
                return NotFound();
            }
            features.IconUrl = featureIconUrl.Trim();
            features.Title = featureTitle.Trim();
            features.Desc = featureDesc.Trim();
            features.UpdatedTime = DateTime.Now;
            _context.Features.Update(features);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Details), controllerName: nameof(Feature), routeValues: new { id });
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feature feature)
        {
            if (await isFeatureExists(feature.IconUrl, feature.Title, feature.Desc))
            {
                return Content("Not correct");
            }
            feature.CreatedTime = DateTime.Now;

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: "index", controllerName: "feature");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _context.Features.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (feature is null)
            {
                return NotFound();
            }
            feature.IsDeleted = true;
            return View(feature);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Feature feature)
        {
            var dbFeature = await _context.Features.Where(n => !n.IsDeleted && n.Id == feature.Id).FirstOrDefaultAsync();
            if (dbFeature.Title.ToLower() != feature.Title.Trim().ToLower())
            {
                return NotFound();
            }
            if (dbFeature is null)
            {
                return NotFound();
            }
            dbFeature.IsDeleted = true;
            _context.Features.Update(dbFeature);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Feature));
        }
        private async Task<bool> isFeatureExists(string iconUrl, string title, string desc)
        {
            var isExists = await _context.Features.AnyAsync(n => n.IconUrl.ToLower() == iconUrl.Trim().ToLower()
                                                           && n.Title.ToLower() == title.Trim().ToLower()
                                                           && n.Desc.ToLower() == desc.Trim().ToLower());
            return isExists;
        }
    }
}
