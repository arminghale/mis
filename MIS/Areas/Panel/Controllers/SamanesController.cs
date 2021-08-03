using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MIS;

namespace MIS.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize]
    public class SamanesController : Controller
    {
        private readonly Context _context;

        public SamanesController(Context context)
        {
            _context = context;
        }

        // GET: Panel/Samanes
        [Authorize(Policy = "/panel/samanes")]
        public async Task<IActionResult> Index(string search="")
        {
            ViewBag.policy = "/panel/samanes";
            var context = await Methods.Samanes(_context, User.Identity.Name);
            if (!string.IsNullOrEmpty(search))
            {
                return View(context.Where(w=>w.title.Contains(search)));
            }
            return View(context);
        }


        // GET: Panel/Samanes/Create
        [Authorize(Policy = "/panel/samanes/create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Panel/Samanes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title")] Samane samane)
        {
            if (ModelState.IsValid)
            {
                var samane2 = await _context.Samane.FirstOrDefaultAsync(w => w.title == samane.title);
                if (samane2!=null)
                {
                    ModelState.AddModelError("title", "سیستمی با این عنوان ثبت شده است");
                    return View(samane);
                }
                _context.Add(samane);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(samane);
        }

        // GET: Panel/Samanes/Edit/5
        [Authorize(Policy = "/panel/samanes/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samane = await _context.Samane.FindAsync(id);
            if (samane == null)
            {
                return NotFound();
            }
            return View(samane);
        }

        // POST: Panel/Samanes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title")] Samane samane)
        {
            if (id != samane.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var samane2 =await _context.Samane.FirstOrDefaultAsync(w => w.title == samane.title);
                    if (samane2 != null&&samane!=samane2)
                    {
                        ModelState.AddModelError("title", "سیستمی با این عنوان ثبت شده است");
                        return View(samane);
                    }
                    _context.Update(samane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SamaneExists(samane.id))
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
            return View(samane);
        }

        [Authorize(Policy = "/panel/samanes/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var samane = await _context.Samane.FindAsync(id);
            _context.Samane.Remove(samane);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SamaneExists(int id)
        {
            return _context.Samane.Any(e => e.id == id);
        }
    }
}
