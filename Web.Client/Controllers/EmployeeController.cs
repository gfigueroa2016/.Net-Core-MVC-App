using System;
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
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.ImagePath = UploadedFile(employee);
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Get));
        }
        private string UploadedFile(Employee employee)
        {
            if (employee.ProfileImage != null)
            {
                string webRootPath = _webHostEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, "wwwroot\\images");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                var uniqueFileName = employee.Id + "_" + employee.ProfileImage.FileName;
                string filePath = Path.Combine(newPath, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                employee.ProfileImage.CopyTo(fileStream);
                return uniqueFileName;
            }
            return null;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Position,ProfileImage")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employeeExists = await _context.Employees.FindAsync(id);
                    if(employeeExists != null)
                    {
                        employeeExists.ImagePath = UploadedFile(employee);
                        employeeExists.Name = employee.Name;
                        employeeExists.Age = employee.Age;
                        employeeExists.Position = employee.Position;
                        _context.Update(employeeExists);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                    
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
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var employeeExists = await _context.Employees.FindAsync(id);
                if (employeeExists != null)
                {
                    _context.Employees.Remove(employeeExists);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Get));
                }
                else
                {
                    return NotFound();
                }
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
