using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MIS;
using Newtonsoft.Json;

namespace MIS.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize]
    public class DomainValuesController : Controller
    {
        private readonly Context _context;

        public DomainValuesController(Context context)
        {
            _context = context;
        }

        // GET: Panel/DomainValues
        [Authorize(Policy ="/panel/domainvalues")]
        public async Task<IActionResult> Index(string search="",int did=-1)
        {
            ViewBag.policy = "/panel/domainvalues";
            ViewData["domainid"] = new SelectList(await Methods.Domains(_context, User.Identity.Name), "id", "title",did);
            var context =await Methods.DomainValues(_context,User.Identity.Name);
            if (did!=-1)
            {
                context = context.Where(w => w.domainid == did).ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.value.Contains(search) || w.Domain.title.Contains(search)).ToList();
            }
            return View(context);
        }


        // GET: Panel/DomainValues/Create
        [Authorize(Policy = "/panel/domainvalues/create")]
        public async Task<IActionResult> Create()
        {
            ViewData["domainid"] = new SelectList(await Methods.Domains(_context, User.Identity.Name), "id", "title");
            return View();
        }

        // POST: Panel/DomainValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,domainid,value")] DomainValue domainValue,int[] subdomain)
        {
            if (ModelState.IsValid)
            {
                var value = domainValue;
                _context.Add(value);
                await _context.SaveChangesAsync();
                foreach (var item in subdomain)
                {
                    await _context.SubDomainValue.AddAsync(new SubDomainValue { domainvalueid = item, domainvalue2id = value.id });
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["domainid"] = new SelectList(await Methods.Domains(_context, User.Identity.Name), "id", "title", domainValue.domainid);
            return View(domainValue);
        }

        // GET: Panel/DomainValues/Edit/5
        [Authorize(Policy = "/panel/domainvalues/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domainValue = await _context.DomainValue.FindAsync(id);
            var subid = await _context.SubDomainValue.FirstOrDefaultAsync(w => w.domainvalue2id == domainValue.id);
            if (domainValue == null)
            {
                return NotFound();
            }
            ViewData["domainid"] = new SelectList(await Methods.Domains(_context, User.Identity.Name), "id", "title", domainValue.domainid);
            if (subid!=null)
            {
                ViewData["domain2id"] = new SelectList(await Methods.Domains(_context, User.Identity.Name), "id", "title", subid.DomainValue.domainid);
                ViewBag.parent = await _context.DomainValue.Where(w => w.domainid == subid.DomainValue.domainid).ToListAsync();
                ViewBag.parentvalue = await _context.SubDomainValue.Where(w => w.domainvalue2id == domainValue.id).ToListAsync();
            }
            else
            {
                ViewData["domain2id"] = new SelectList(await Methods.Domains(_context, User.Identity.Name), "id", "title");
            }
            return View(domainValue);
        }

        // POST: Panel/DomainValues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,domainid,value")] DomainValue domainValue, int[] subdomain)
        {
            if (id != domainValue.id)
            {
                return NotFound();
            }
            var subid = await _context.SubDomainValue.FirstOrDefaultAsync(w => w.domainvalue2id == domainValue.id);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domainValue);
                    await _context.SaveChangesAsync();
                    var list = await _context.SubDomainValue.Where(w => w.domainvalue2id == id).ToListAsync();
                    foreach (var item in list)
                    {
                        _context.SubDomainValue.Remove(item);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var item in subdomain)
                    {
                        _context.SubDomainValue.Add(new SubDomainValue { domainvalueid = item, domainvalue2id = id });
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomainValueExists(domainValue.id))
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
            ViewData["domainid"] = new SelectList(await Methods.Domains(_context, User.Identity.Name), "id", "title", domainValue.domainid);
            ViewData["domain2id"] = new SelectList(await Methods.Domains(_context, User.Identity.Name), "id", "title", subid.id);
            return View(domainValue);
        }

        [Authorize(Policy = "/panel/domainvalues/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var domainValue = await _context.DomainValue.FindAsync(id);
            _context.DomainValue.Remove(domainValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> domainvalue(int did)
        {
            var context = await Methods.DomainValues(_context,User.Identity.Name);
            context = context.Where(w => w.domainid == did).ToList();
            return PartialView(context);
            //JsonSerializerSettings setting = new JsonSerializerSettings();
            //setting.MaxDepth = 1;
            //var json = JsonConvert.SerializeObject(context, setting);
            //return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        private bool DomainValueExists(int id)
        {
            return _context.DomainValue.Any(e => e.id == id);
        }
    }
}
