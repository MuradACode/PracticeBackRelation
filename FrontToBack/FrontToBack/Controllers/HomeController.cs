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
            homeVM.Products = await _context.Products.Include(n => n.Category).Include(n => n.Author).Where(n => !n.IsDeleted).ToListAsync();
            homeVM.FeaturedProducts = await _context.Products.Include(n => n.Category).Include(n => n.Author).Where(n => !n.IsDeleted).Where(n => n.IsFeatured).Take(10).ToListAsync();
            homeVM.DiscountedProducts = await _context.Products.Include(n => n.Category).Include(n => n.Author).Where(n => !n.IsDeleted).Where(n => n.Discount > 0).Take(10).ToListAsync();
            homeVM.NewProducts = await _context.Products.Include(n => n.Category).Include(n => n.Author).Where(n => !n.IsDeleted).Where(n => n.IsNew).Take(10).ToListAsync();

            return View(homeVM);
        }
    }
}
