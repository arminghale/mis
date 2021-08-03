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
    public class UsersController : Controller
    {
        private readonly Context _context;

        public UsersController(Context context)
        {
            _context = context;
        }

        // GET: Panel/Users
        [Authorize(Policy = "/panel/users")]
        public async Task<IActionResult> Index(string search="")
        {
            ViewBag.policy = "/panel/users";
            var context =await Methods.DomainUsers(_context, User.Identity.Name);
            if (!string.IsNullOrEmpty(search))
            {
                context = context.Where(w => w.name.Contains(search) || w.username.Contains(search)).ToList();
            }
            return View(context);
        }

        // GET: Panel/Users/Details/5
        [Authorize(Policy = "/panel/users")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Panel/Users/Create
        [Authorize(Policy = "/panel/users/create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.role =await Methods.DomainRoles(_context, User.Identity.Name);
            return View();
        }

        // POST: Panel/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,username,password,name")] User user,int[] roles)
        {
            ViewBag.role =await Methods.DomainRoles(_context, User.Identity.Name);
            if (ModelState.IsValid)
            {
                var user2 =await _context.User.FirstOrDefaultAsync(w => w.username == user.username);
                if (user2!=null)
                {
                    ModelState.AddModelError("username", "کاربری با این نام کاربری ثبت شده است");
                    return View(user);
                }
                var parent = await _context.User.FirstOrDefaultAsync(w => w.username == User.Identity.Name);
                user.parentid = parent.id;
                var saveuser = user;
                _context.Add(saveuser);
                await _context.SaveChangesAsync();

                foreach (var item in roles)
                {
                    _context.UserRoles.Add(new UserRoles { userid=saveuser.id,roleid=item });
                    await _context.SaveChangesAsync();
                }

                if (User.Identity.Name!="admin")
                {
                    var creator = _context.User.FirstOrDefault(w => w.username == User.Identity.Name);
                    foreach (var item in creator.UACCDomains)
                    {
                        _context.UACCDomain.Add(new UACCDomain { userid = saveuser.id, domainvalueid = item.domainvalueid });
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Panel/Users/Edit/5
        [Authorize(Policy = "/panel/users/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user2 = await _context.User.FindAsync(id);
            if (user2 == null)
            {
                return NotFound();
            }
            ViewBag.role =await Methods.DomainRoles(_context, User.Identity.Name);

            return View(user2);
        }

        // POST: Panel/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,username,password,name")] User user, int[] roles)
        {
            ViewBag.role = await Methods.DomainRoles(_context, User.Identity.Name);
            if (id != user.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user2 =await _context.User.FirstOrDefaultAsync(w => w.username == user.username);
                    if (user2 != null&& user.id != user2.id)
                    {
                        ModelState.AddModelError("username", "کاربری با این نام کاربری ثبت شده است");
                        return View(user);
                    }
                    user2.username = user.username;
                    user2.password = user.password;
                    user2.name = user.name;
                    _context.Entry(user2).State=EntityState.Modified;
                    await _context.SaveChangesAsync();

                    var list= await _context.UserRoles.Where(w=>w.userid==user2.id).ToListAsync();
                    foreach (var item in list)
                    {
                        _context.UserRoles.Remove(item);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var item in roles)
                    {
                        _context.UserRoles.Add(new UserRoles { userid = user2.id, roleid = item });
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.id))
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
            return View(user);
        }

        [Authorize(Policy = "/panel/users/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.id == id);
        }
    }
}
