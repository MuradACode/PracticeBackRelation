using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBackTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.Where(n => !n.IsDeleted).ToListAsync();
            return View(sliders);
        }
        public async Task<IActionResult> Details(int id)
        {
            var slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (slider is null)
            {
                return NotFound();
            }
            return View(slider);
        }
        public async Task<IActionResult> Update(int id)
        {
            var slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (slider is null)
            {
                return NotFound();
            }
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, string sliderTitle1)
        {
            var slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (slider is null || string.IsNullOrEmpty(sliderTitle1.Trim()))
            {
                return NotFound();
            }
            slider.Title1 = sliderTitle1.Trim();
            slider.UpdatedTime = DateTime.Now;
            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Details), controllerName: nameof(slider), routeValues: new { id });
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create(Slider slider)
        {
            slider.CreatedTime = DateTime.Now;
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: "index", controllerName: "slider");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            if (slider is null)
            {
                return NotFound();
            }
            slider.IsDeleted = true;
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Slider slider)
        {
            var dbSlider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == slider.Id).FirstOrDefaultAsync();
            if (dbSlider.Title1.ToLower() != slider.Title1.Trim().ToLower())
            {
                return NotFound();
            }
            if (dbSlider is null)
            {
                return NotFound();
            }
            dbSlider.IsDeleted = true;
            _context.Sliders.Update(dbSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Slider));
        }
        private async Task<bool> isSliderExists(string title)
        {
            var isExists = await _context.Sliders.AnyAsync(n => n.Title1.ToLower() == title.Trim().ToLower());
            return isExists;
        }
    }
}
