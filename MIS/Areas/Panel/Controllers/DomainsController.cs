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
    public class DomainsController : Controller
    {
        private readonly Context _context;

        public DomainsController(Context context)
        {
            _context = context;
        }

        // GET: Panel/Domains
        [Authorize(Policy = "/panel/domains")]
        public async Task<IActionResult> Index(string search="")
        {
            ViewBag.policy = "/panel/domains";
            var context = await Methods.Domains(_context,User.Identity.Name);
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.title.Contains(search)).ToList();
            }
            return View(context);
        }


        [Authorize(Policy = "/panel/domains/create")]
        // GET: Panel/Domains/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Panel/Domains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,IsAccess")] Domain domain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(domain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(domain);
        }

        // GET: Panel/Domains/Edit/5
        [Authorize(Policy = "/panel/domains/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domain = await _context.Domain.FindAsync(id);
            if (domain == null)
            {
                return NotFound();
            }
            return View(domain);
        }

        // POST: Panel/Domains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,IsAccess")] Domain domain)
        {
            if (id != domain.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomainExists(domain.id))
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
            return View(domain);
        }

        [Authorize(Policy = "/panel/domains/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var domain = await _context.Domain.FindAsync(id);
            _context.Domain.Remove(domain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DomainExists(int id)
        {
            return _context.Domain.Any(e => e.id == id);
        }
    }
}
