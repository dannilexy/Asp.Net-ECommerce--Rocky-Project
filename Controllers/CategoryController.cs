using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockyProject.Data;
using RockyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockyProject.Controllers
{
    [Authorize(Roles =WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult>  Index()
        {
            var Categories = _context.Category.ToList();
            return View(Categories);
        }

        //Get Create Action
        public IActionResult Create()
        {
            return View();
        }


        //Post Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(ModelState);
            }
        }

        //Get Edit Action
        public IActionResult Edit(int id)
        {
            var Category = _context.Category.FirstOrDefault(c => c.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            else
            {
                return View(Category);
            }
           
        }

        //Post Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(ModelState);
            }
        }


        //Get Delete Action
        public async Task<IActionResult> Delete(int id)
        {
            var Category = _context.Category.FirstOrDefault(c => c.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            else
            {
                _context.Category.Remove(Category);
              await  _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

        }

    }
}
