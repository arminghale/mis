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
    public class ActionGroupsController : Controller
    {
        private readonly Context _context;

        public ActionGroupsController(Context context)
        {
            _context = context;
        }

        // GET: Panel/ActionGroups
        [Authorize(Policy = "/panel/actiongroups")]
        public async Task<IActionResult> Index(string search="",int sid=-1)
        {
            ViewBag.policy = "/panel/actiongroups";
            ViewBag.sid = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title", sid);
            var context =await Methods.ActionGroups(_context,User.Identity.Name);
            if (sid!=-1)
            {
                context = context.Where(w => w.samaneid == sid).ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.title.Contains(search)).ToList();
            }
            return View(context);
        }

        [Authorize(Policy = "/panel/actiongroups/create")]
        public async Task<IActionResult> Create()
        {
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context,User.Identity.Name), "id", "title");
            return View();
        }

        // POST: Panel/ActionGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,samaneid,title")] ActionGroup actionGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actionGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title",actionGroup.samaneid);
            return View(actionGroup);
        }

        // GET: Panel/ActionGroups/Edit/5
        [Authorize(Policy = "/panel/actiongroups/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionGroup = await _context.ActionGroup.FindAsync(id);
            if (actionGroup == null)
            {
                return NotFound();
            }
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title", actionGroup.samaneid);
            return View(actionGroup);
        }

        // POST: Panel/ActionGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,samaneid,title")] ActionGroup actionGroup)
        {
            if (id != actionGroup.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actionGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActionGroupExists(actionGroup.id))
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
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title", actionGroup.samaneid);
            return View(actionGroup);
        }

        [Authorize(Policy = "/panel/actiongroups/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var actionGroup = await _context.ActionGroup.FindAsync(id);
            _context.ActionGroup.Remove(actionGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActionGroupExists(int id)
        {
            return _context.ActionGroup.Any(e => e.id == id);
        }
    }
}
