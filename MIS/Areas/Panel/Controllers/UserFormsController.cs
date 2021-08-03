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
    public class UserFormsController : Controller
    {
        private readonly Context _context;

        public UserFormsController(Context context)
        {
            _context = context;
        }

        // GET: Panel/UserForms
        [Authorize(Policy = "/panel/userforms")]
        public async Task<IActionResult> Index(string search = "")
        {
            var context = await Methods.DomainUsers(_context, User.Identity.Name);
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.name.Contains(search) || w.username.Contains(search)).ToList();
            }
            return View(context);
        }
        [Authorize(Policy = "/panel/userforms")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.forms = await _context.Form.ToListAsync();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int[] forms)
        {

            var user = await _context.User.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    var list = await _context.UserForms.Where(w => w.userid == user.id).ToListAsync();
                    foreach (var item in list)
                    {
                        _context.UserForms.Remove(item);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var item in forms)
                    {
                        _context.UserForms.Add(new UserForms { userid = user.id, formid = item });
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.forms = await _context.Form.ToListAsync();
            return View(user);
        }


        private bool UserFormsExists(int id)
        {
            return _context.UserForms.Any(e => e.id == id);
        }
    }
}
