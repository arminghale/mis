using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public static class Methods
    {

        public static async Task<List<Role>> DomainRoles(Context _context, string username)
        {
            if (username != "admin")
            {
                var user = _context.User.FirstOrDefault(w => w.username == username);
                var list = await _context.Role.ToListAsync();
                var roles = new List<Role>();
                foreach (var item in list)
                {
                    bool flag = false;
                    foreach (var item2 in item.RACCDomains)
                    {
                        if (user.UACCDomains.Any(w => w.domainvalueid == item2.domainvalueid) && !user.UserRoles.Any(w => w.roleid == item.id))
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        roles.Add(item);
                    }
                }
                return roles;
            }
            else
            {
                return _context.Role.ToList();
            }
        }

        public static async Task<List<User>> DomainUsers(Context _context, string username)
        {
            if (username != "admin")
            {
                var user = _context.User.FirstOrDefault(w => w.username == username);
                var list = await _context.User.Where(w=>w.id!=user.parentid).ToListAsync();
                var users = new List<User>();
                foreach (var item in list)
                {
                    bool flag = false;
                    bool flag2 = false;
                    foreach (var item2 in item.UACCDomains)
                    {
                        if (user.UACCDomains.Any(w => w.domainvalueid == item2.domainvalueid) && item.username != "admin")
                        {
                            foreach (var item3 in user.UserRoles)
                            {
                                if (item.UserRoles.Any(w => w.roleid == item3.roleid))
                                {
                                    flag2 = true;
                                }
                            }
                            if (!flag2)
                            {
                                flag = true;
                            }

                        }
                    }
                    if (flag)
                    {
                        users.Add(item);
                    }
                }
                return users;
            }
            else
            {
                return _context.User.ToList();
            }
        }

        public static async Task<List<Domain>> Domains(Context _context, string username)
        {
            if (username != "admin")
            {
                var user = _context.User.FirstOrDefault(w => w.username == username);
                var list = await _context.Domain.ToListAsync();
                var domains = new List<Domain>();
                foreach (var item in list)
                {
                    if (user.UACCDomains.Any(w => w.DomainValue.Domain.id == item.id))
                    {
                        domains.Add(item);
                    }

                }
                return domains;
            }
            else
            {
                return _context.Domain.ToList();
            }
        }

        public static async Task<List<DomainValue>> DomainValues(Context _context, string username)
        {
            if (username != "admin")
            {
                var user = _context.User.FirstOrDefault(w => w.username == username);
                var list = await _context.DomainValue.ToListAsync();
                var domains = new List<DomainValue>();
                foreach (var item in list)
                {
                    if (user.UACCDomains.Any(w => w.domainvalueid == item.id))
                    {
                        domains.Add(item);
                    }

                }
                return domains;
            }
            else
            {
                return _context.DomainValue.ToList();
            }
        }

        public static async Task<List<Samane>> Samanes(Context _context, string username)
        {
            if (username != "admin")
            {
                var user = _context.User.FirstOrDefault(w => w.username == username);
                var accesses =await _context.Access.Where(w => w.userid == user.id).ToListAsync();

                List<Samane> samanes = new List<Samane>();

                foreach (var item in accesses)
                {
                    if (!samanes.Any(w => w == _context.Samane.Find(item.samaneid)))
                    {
                        samanes.Add(_context.Samane.Find(item.samaneid));
                    }

                }
                return samanes;
            }
            else
            {
                return _context.Samane.ToList();
            }
        }

        public static async Task<List<ActionGroup>> ActionGroups(Context _context, string username)
        {
            if (username != "admin")
            {
                var user = _context.User.FirstOrDefault(w => w.username == username);
                var accesses = await _context.Access.Where(w => w.userid == user.id).ToListAsync();

                List<ActionGroup> actionGroups = new List<ActionGroup>();

                foreach (var item in accesses)
                {
                    if (!actionGroups.Any(w => w == _context.ActionGroup.Find(item.actiongroupid)))
                    {
                        actionGroups.Add(_context.ActionGroup.Find(item.actiongroupid));
                    }

                }
                return actionGroups;
            }
            else
            {
                return _context.ActionGroup.ToList();
            }
        }

        public static async Task<List<Action>> Actions(Context _context, string username)
        {
            if (username != "admin")
            {
                var user = _context.User.FirstOrDefault(w => w.username == username);
                var accesses = await _context.Access.Where(w => w.userid == user.id).ToListAsync();

                List<Action> actions = new List<Action>();

                foreach (var item in accesses)
                {
                    actions.Add(_context.Action.Find(item.actionid));

                }
                return actions;
            }
            else
            {
                return _context.Action.ToList();
            }
        }

    }
}
