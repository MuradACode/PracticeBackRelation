using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM();

            homeVM.Sliders = await _context.Sliders.Where(n => !n.IsDeleted).ToListAsync();
            homeVM.Features = await _context.Features.Where(n => !n.IsDeleted).ToListAsync();
            homeVM.Promos = await _context.Promos.Where(n => !n.IsDeleted).ToListAsync();
            homeVM.Promos2 = await _context.Promos2.Where(n => !n.IsDeleted).ToListAsync();
            homeVM.Categories = await _context.Categories.Where(n => !n.IsDeleted).ToListAsync();
            homeVM.Products = await _context.Products.Include(n => n.Category).Where(n => !n.IsDeleted).ToListAsync();

            return View(homeVM);
        }
    }
}
