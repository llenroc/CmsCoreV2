using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CmsCoreV2.Data;
using CmsCoreV2.Models;
using Microsoft.AspNetCore.Hosting;
using SaasKit.Multitenancy;
using Z.EntityFramework.Plus;
using Microsoft.AspNetCore.Authorization;

namespace CmsCoreV2.Areas.CmsCore.Controllers
{
    [Authorize(Roles = "ADMIN,GALLERY")]
    [Area("CmsCore")]
    public class GalleriesController : ControllerBase
    {
        private IHostingEnvironment env;
        public GalleriesController(IHostingEnvironment _env, ITenant<AppTenant> tenant, ApplicationDbContext context) : base(context, tenant)
        { 
            this.env = _env;
        }

        // GET: CmsCore/Galleries
        public async Task<IActionResult> Index()
        {
            return View(await _context.SetFiltered<Gallery>().Where(x => x.AppTenantId == tenant.AppTenantId).ToListAsync());
        }

        // GET: CmsCore/Galleries/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Galleries
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // GET: CmsCore/Galleries/Create
        public IActionResult Create()
        {
            var gallery = new Gallery();
            gallery.CreatedBy = User.Identity.Name ?? "username";
            gallery.CreateDate = DateTime.Now;
            gallery.UpdatedBy = User.Identity.Name ?? "username";
            gallery.UpdateDate = DateTime.Now;
            return View(gallery);
        }

        // POST: CmsCore/Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsPublished,Id,CreateDate,CreatedBy,UpdateDate,UpdatedBy,AppTenantId")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                gallery.CreatedBy = User.Identity.Name ?? "username";
                gallery.CreateDate = DateTime.Now;
                gallery.UpdatedBy = User.Identity.Name ?? "username";
                gallery.UpdateDate = DateTime.Now;
                gallery.AppTenantId = tenant.AppTenantId;

                _context.Add(gallery);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(gallery);
        }

        // GET: CmsCore/Galleries/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Galleries.SingleOrDefaultAsync(m => m.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }

        // POST: CmsCore/Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,IsPublished,Id,CreateDate,CreatedBy,UpdateDate,UpdatedBy,AppTenantId")] Gallery gallery)
        {
            if (id != gallery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    gallery.UpdatedBy = User.Identity.Name ?? "username";
                    gallery.UpdateDate = DateTime.Now;
                    gallery.AppTenantId = tenant.AppTenantId;
                    _context.Update(gallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryExists(gallery.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(gallery);
        }

        // GET: CmsCore/Galleries/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallery = await _context.Galleries
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // POST: CmsCore/Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var gallery = await _context.Galleries.SingleOrDefaultAsync(m => m.Id == id);
            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool GalleryExists(long id)
        {
            return _context.Galleries.Any(e => e.Id == id);
        }
    }
}
