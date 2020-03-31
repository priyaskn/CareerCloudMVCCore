using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareerCloudMVCCore2.Models;
using CareerCloudMVCCore2.Models.ViewModel;

namespace CareerCloudMVCCore2.Controllers
{
    public class CompanyProfilesController : Controller
    {
        private readonly JOB_PORTAL_DBContext _context;

        public CompanyProfilesController(JOB_PORTAL_DBContext context)
        {
            _context = context;
        }

        // GET: CompanyProfiles
        public async Task<IActionResult> Index()
        {
            var companyProfiles = await _context.CompanyProfiles
                .Include(cd=>cd.CompanyDescriptions)
                .ToListAsync();

           

            return View(companyProfiles);
        }

        // GET: CompanyProfiles/Details
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfiles = await _context.CompanyProfiles
                .Include(a=>a.CompanyLocations)
                .Include(a=>a.CompanyDescriptions)
                .Include(a=>a.CompanyJobs).ThenInclude(a => a.CompanyJobSkills)
                .Include(a=>a.CompanyJobs).ThenInclude(a=> a.CompanyJobsDescriptions)
                .Include(a=>a.CompanyJobs).ThenInclude(a=> a.CompanyJobEducations)
                .FirstOrDefaultAsync(m => m.Id == id);

            

            if (companyProfiles == null)
            {
                return NotFound();
            }

            return View(companyProfiles);
        }

        // GET: CompanyProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationDate,CompanyWebsite,ContactPhone,ContactName,CompanyLogo")] CompanyProfiles companyProfiles)
        {
            if (ModelState.IsValid)
            {
                companyProfiles.Id = Guid.NewGuid();
                _context.Add(companyProfiles);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Create), "CompanyDescriptions", new { id = companyProfiles.Id });
            }
            return View(companyProfiles);
        }

        // GET: CompanyProfiles/Edit
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfiles = await _context.CompanyProfiles.FindAsync(id);
            if (companyProfiles == null)
            {
                return NotFound();
            }
            return View(companyProfiles);
        }

        // POST: CompanyProfiles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,RegistrationDate,CompanyWebsite,ContactPhone,ContactName,CompanyLogo,TimeStamp")] CompanyProfiles companyProfiles)
        {
            if (id != companyProfiles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyProfiles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyProfilesExists(companyProfiles.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(companyProfiles);
        }

        // GET: CompanyProfiles/Delete
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfiles = await _context.CompanyProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyProfiles == null)
            {
                return NotFound();
            }

            return View(companyProfiles);
        }

        // POST: CompanyProfiles/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyProfiles = await _context.CompanyProfiles.FindAsync(id);
            _context.CompanyProfiles.Remove(companyProfiles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyProfilesExists(Guid id)
        {
            return _context.CompanyProfiles.Any(e => e.Id == id);
        }
    }
}
