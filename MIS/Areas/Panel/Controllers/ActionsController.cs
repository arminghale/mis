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
    public class ActionsController : Controller
    {
        private readonly Context _context;

        public ActionsController(Context context)
        {
            _context = context;
        }

        // GET: Panel/Actions
        [Authorize(Policy = "/panel/actions")]
        public async Task<IActionResult> Index(string search = "", int sid = -1, int agid = -1)
        {
            ViewBag.policy = "/panel/actions";
            ViewBag.sid = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title", sid);
            var context = await Methods.Actions(_context, User.Identity.Name);
            if (sid != -1)
            {
                context = context.Where(w => w.ActionGroup.samaneid == sid).ToList();
                var list = await Methods.ActionGroups(_context, User.Identity.Name);
                ViewBag.agid = new SelectList(list.Where(w => w.samaneid == sid), "id", "title", agid);
            }
            if (agid != -1)
            {
                context = context.Where(w => w.actiongroupid == agid).ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.title.Contains(search) || w.url.Contains(search)).ToList();
            }
            return View(context);
        }

        // GET: Panel/Actions/Create
        [Authorize(Policy = "/panel/actions/create")]
        public async Task<IActionResult> Create()
        {
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title");
            return View();
        }

        // POST: Panel/Actions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( string url, int actiongroupid, int sid,string title)
        {
            var action = new Action() { url = url, actiongroupid = actiongroupid, title = title };
            if (ModelState.IsValid)
            {
                _context.Add(action);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title",sid);
            if (sid!=-1)
            {
                var list = await Methods.ActionGroups(_context, User.Identity.Name);
                ViewData["actiongroupid"] = new SelectList(list.Where(w=>w.samaneid==sid), "id", "title", action.actiongroupid);
            }
            return View(action);
        }

        // GET: Panel/Actions/Edit/5
        [Authorize(Policy = "/panel/actions/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var action = await _context.Action.FindAsync(id);
            if (action == null)
            {
                return NotFound();
            }
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title", action.ActionGroup.samaneid);
            var list = await Methods.ActionGroups(_context, User.Identity.Name);
            ViewData["actiongroupid"] = new SelectList(list.Where(w => w.samaneid == action.ActionGroup.samaneid), "id", "title", action.actiongroupid);
            return View(action);
        }

        // POST: Panel/Actions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string url,string title,int actiongroupid, int sid)
        {
            var action = _context.Action.Find(id);
            action.title = title;
            action.url = url;
            action.actiongroupid = actiongroupid;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(action);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActionExists(action.id))
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
            ViewData["samaneid"] = new SelectList(await Methods.Samanes(_context, User.Identity.Name), "id", "title", action.ActionGroup.samaneid);
            var list = await Methods.ActionGroups(_context, User.Identity.Name);
            ViewData["actiongroupid"] = new SelectList(list.Where(w => w.samaneid == action.ActionGroup.samaneid), "id", "title", action.actiongroupid);
            return View(action);
        }



        [Authorize(Policy = "/panel/actions/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var action = await _context.Action.FindAsync(id);
            _context.Action.Remove(action);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> actiongroups(int sid)
        {
            var list = await Methods.ActionGroups(_context, User.Identity.Name);
            var context = list.Where(w => w.samaneid == sid);
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.MaxDepth = 1;
            var json = JsonConvert.SerializeObject(context, setting);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }
        private bool ActionExists(int id)
        {
            return _context.Action.Any(e => e.id == id);
        }
    }
}
