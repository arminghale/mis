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
    public class RolesController : Controller
    {
        private readonly Context _context;

        public RolesController(Context context)
        {
            _context = context;
        }

        // GET: Panel/Roles
        [Authorize(Policy = "/panel/roles")]
        public async Task<IActionResult> Index(string search="")
        {
            ViewBag.policy = "/panel/roles";
            var context =await Methods.DomainRoles(_context, User.Identity.Name);
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.title.Contains(search)).ToList();
            }
            return View(context);
        }


        // GET: Panel/Roles/Create
        [Authorize(Policy = "/panel/roles/create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Panel/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title")] Role role)
        {
            if (ModelState.IsValid)
            {
                var role2 = await _context.Role.FirstOrDefaultAsync(w => w.title == role.title);
                if (role2!=null)
                {
                    ModelState.AddModelError("title","نقشی با این عنوان ثبت شده است.");
                }
                var saverole = role;
                _context.Add(saverole);
                await _context.SaveChangesAsync();

                if (User.Identity.Name!="admin")
                {
                    var creator = _context.User.FirstOrDefault(w => w.username == User.Identity.Name);
                    foreach (var item in creator.UserRoles)
                    {
                        foreach (var item2 in item.Role.RACCDomains)
                        {
                            _context.RACCDomain.Add(new RACCDomain {roleid=saverole.id,domainvalueid=item2.domainvalueid });
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Panel/Roles/Edit/5
        [Authorize(Policy = "/panel/roles/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Panel/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title")] Role role)
        {
            if (id != role.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var role2 =await _context.Role.FirstOrDefaultAsync(w => w.title == role.title);
                    if (role2 != null&&role!=role2)
                    {
                        ModelState.AddModelError("title", "نقشی با این عنوان ثبت شده است.");
                    }
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.id))
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
            return View(role);
        }

        [Authorize(Policy = "/panel/roles/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.Role.FindAsync(id);
            _context.Role.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.id == id);
        }
    }
}
