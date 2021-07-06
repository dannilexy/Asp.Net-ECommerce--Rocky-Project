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
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get Index Action
        public IActionResult Index()
        {
            var applications = _context.ApplicationType.ToList();
            return View(applications);
        }

        //Get Create Action
        public IActionResult Create()
        {
            return View();
        }


        //Post Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationType application)
        {
            if (ModelState.IsValid)
            {
                _context.ApplicationType.Add(application);
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
            var Appli = _context.ApplicationType.FirstOrDefault(c => c.Id == id);
            if (Appli == null)
            {
                return NotFound();
            }
            else
            {
                return View(Appli);
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
            var application = _context.ApplicationType.FirstOrDefault(c => c.Id == id);
            if (application == null)
            {
                return NotFound();
            }
            else
            {
                _context.ApplicationType.Remove(application);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

        }

    }
}
