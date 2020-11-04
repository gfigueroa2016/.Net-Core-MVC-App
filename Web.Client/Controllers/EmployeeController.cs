﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.Client.Data;
using Web.Client.Extensions;
using Web.Client.Models;

namespace Web.Client.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            var employees = from e in _context.Employees
                            select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.Name.Contains(searchString));
            }
            int pageSize = 10;
            return View(await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(employee);
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Get));
        }
        private string UploadedFile(Employee employee)
        {
            string uniqueFileName = null;

            if (employee.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + employee.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                employee.ProfileImage.CopyTo(fileStream);
            }
            return uniqueFileName;
        }

        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Employees.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Position,ProfileImage")] Employee employee)
        {
            if (id != employee.Id.ToString())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(employee);
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                        return NotFound();
                }
                return RedirectToAction("Get");
            }
            return View(employee);
        }

        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return RedirectToAction(nameof(Get));
            }
            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Get));
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(Get));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
