using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIS.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly Context _context;
        public HomeController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
               
            return View();
        }

        public async Task<IActionResult> Menu(int sid=-1)
        {
            var user = _context.User.FirstOrDefault(w => w.username == User.Identity.Name);
            var accesses = _context.Access.Where(w => w.userid == user.id).ToList();

            ViewBag.name = user.name;
            List<Action> actions = new List<Action>();
            List<ActionGroup> actionGroups = new List<ActionGroup>();
            List<Samane> samanes = new List<Samane>();

            foreach (var item in accesses)
            {
                var temp = _context.Action.Find(item.actionid).url.Split('/');
                if (temp.Last()!="create"&& temp.Last() != "edit"&&temp.Last() != "delete")
                {
                    actions.Add(_context.Action.Find(item.actionid));
                }
                
                if (!actionGroups.Any(w=>w==_context.ActionGroup.Find(item.actiongroupid)))
                {
                    actionGroups.Add(_context.ActionGroup.Find(item.actiongroupid));
                }
                if (!samanes.Any(w => w == _context.Samane.Find(item.samaneid)))
                {
                    samanes.Add(_context.Samane.Find(item.samaneid));
                }

            }


            if (sid!=-1)
            {
                ViewBag.samane = new SelectList(samanes, "id", "title", _context.Samane.Find(sid));
                ViewBag.action = actions.Where(w=>w.ActionGroup.samaneid==sid);
                ViewBag.actiongroup = actionGroups.Where(w=>w.samaneid==sid);
            }
            else
            {
                ViewBag.samane = new SelectList(samanes, "id", "title");
            }
            
            

            return PartialView();
        }

        
    }
}
