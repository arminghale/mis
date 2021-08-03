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
    public class RACCsController : Controller
    {
        private readonly Context _context;

        public RACCsController(Context context)
        {
            _context = context;
        }

        // GET: Panel/RACCs
        [Authorize(Policy = "/panel/raccs")]
        public async Task<IActionResult> Index(string search = "")
        {
            var context = await Methods.DomainRoles(_context,User.Identity.Name);
            if (User.Identity.Name != "admin")
            {
                context = context.Where(w => w.title != "admin").ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.title.Contains(search)).ToList();
            }
            return View(context);
        }


        // GET: Panel/RACCs/Edit/5
        [Authorize(Policy = "/panel/raccs")]
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
            var context = await Methods.ActionGroups(_context, User.Identity.Name);
            var actions = await Methods.Actions(_context, User.Identity.Name);
            foreach (var item in context)
            {
                var list = item.Actions.ToList();
                foreach (var item2 in list)
                {
                    if (!actions.Any(w => w == item2))
                    {
                        item.Actions.Remove(item2);
                    }
                }
            }
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title");
            ViewBag.actiongroup = context;
            ViewBag.actions = await _context.RACC.Where(w => w.roleid == role.id).ToListAsync();

            return View(role);
        }

        // POST: Panel/RACCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] access)
        {
            var role = await _context.Role.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    var list = await _context.RACC.Where(w => w.roleid == role.id).ToListAsync();
                    foreach (var item in list)
                    {
                        _context.RACC.Remove(item);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var item in access)
                    {
                        _context.RACC.Add(new RACC { roleid = role.id, actionid = int.Parse(item.Split('-')[0]), type = bool.Parse(item.Split('-')[1]) });
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }


            //
            var user2 = _context.User.FirstOrDefault(w => w.username == User.Identity.Name);
            var accesses = _context.Access.Where(w => w.userid == user2.id).ToList();
            List<Action> actions = new List<Action>();
            List<ActionGroup> actionGroups = new List<ActionGroup>();
            foreach (var item in accesses)
            {
                actions.Add(_context.Action.Find(item.actionid));
                if (!actionGroups.Any(w => w == _context.ActionGroup.Find(item.actiongroupid)))
                {
                    var temp = await _context.ActionGroup.FindAsync(item.actiongroupid);
                    actionGroups.Add(temp);
                }

            }
            foreach (var item in actionGroups)
            {
                var list = item.Actions.ToList();
                foreach (var item2 in list)
                {
                    if (!actions.Any(w => w == item2))
                    {
                        item.Actions.Remove(item2);
                    }
                }
            }

            if (User.Identity.Name == "admin")
            {
                ViewBag.actiongroup = await _context.ActionGroup.Include(w => w.Actions).ToListAsync();
                ViewBag.actions = await _context.RACC.Where(w => w.roleid == role.id).ToListAsync();
            }
            else
            {
                ViewBag.actiongroup = actionGroups;
                ViewBag.actions = await _context.RACC.Where(w => w.roleid == role.id).ToListAsync();
            }
            return View(role);
        }

        private bool RACCExists(int id)
        {
            return _context.RACC.Any(e => e.id == id);
        }
    }
}
