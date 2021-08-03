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
    public class UACCDomainsController : Controller
    {
        private readonly Context _context;

        public UACCDomainsController(Context context)
        {
            _context = context;
        }

        // GET: Panel/UACCDomains
        [Authorize(Policy ="/panel/uaccdomains")]
        public async Task<IActionResult> Index(string search="")
        {
            var context = await Methods.DomainUsers(_context, User.Identity.Name);
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.name.Contains(search) || w.username.Contains(search)).ToList();
            }
            return View( context);
        }

        // GET: Panel/UACCDomains/Edit/5
        [Authorize(Policy = "/panel/uaccdomains")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uACCDomain = await _context.User.FindAsync(id);
            if (uACCDomain == null)
            {
                return NotFound();
            }
            var t = await Methods.Domains(_context, User.Identity.Name);
            var e = await Methods.DomainValues(_context, User.Identity.Name);
            ViewBag.domain = t.Where(w => w.IsAccess == true);
            ViewBag.domainvalue = e;
            return View(uACCDomain);
        }

        // POST: Panel/UACCDomains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id,int[] subdomain)
        {
            var uACCDomain = await _context.User.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    var list = await _context.UACCDomain.Where(w => w.userid == id).ToListAsync();
                    foreach (var item in list)
                    {
                        _context.UACCDomain.Remove(item);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var item in subdomain)
                    {
                        _context.UACCDomain.Add(new UACCDomain { userid = id, domainvalueid = item });
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var t = await Methods.Domains(_context, User.Identity.Name);
            ViewBag.domain = t.Where(w => w.IsAccess == true);
            return View(uACCDomain);
        }


        private bool UACCDomainExists(int id)
        {
            return _context.UACCDomain.Any(e => e.id == id);
        }
    }
}
