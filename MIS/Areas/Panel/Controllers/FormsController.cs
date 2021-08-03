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
    [Authorize(Policy = "/panel/forms")]
    public class FormsController : Controller
    {
        private readonly Context _context;

        public FormsController(Context context)
        {
            _context = context;
        }

        // GET: Panel/Forms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Form.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,name")] Form form)
        {
            if (ModelState.IsValid)
            {
                _context.Add(form);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(form);
        }

        public async Task<IActionResult> Filled(int id)
        {
            var form = await _context.Form.FindAsync(id);
            List<string> field = new List<string>();
            foreach (var item in form.Fields.OrderBy(w => w.radif))
            {
                if (item.type != "label" && item.type != "button")
                {
                    if (item.title != null)
                    {
                        item.title = item.title.Replace(' ', '_');
                        field.Add(item.title);
                    }
                    else
                    {
                        field.Add("Field_" + item.id);
                    }
                }
            }
            if (form.SubForms1.Any(w => w.type == 0))
            {
                foreach (var item in form.SubForms1.Where(w => w.type == 0))
                {
                    foreach (var item2 in item.Form2.Fields)
                    {
                        if (item2.type != "label" && item2.type != "button")
                        {
                            field.Add(item2.title.Replace(' ', '_'));
                        }

                    }
                }
            }

            List<string> subforms = new List<string>();
            if (form.SubForms1.Any(w => w.type == 1))
            {
                foreach (var item in form.SubForms1.Where(w => w.type == 1))
                {
                    subforms.Add(item.Form2.title);
                }
            }
            ViewBag.subform = subforms;
            ViewBag.fields = field;
            var result = SqlMethods.Get(_context, form);
            ViewBag.formid = id;
            return View(result);
        }

        public async Task<IActionResult> SubFilled(int ID,string subf,int formid)
        {
            var form = await _context.Form.FindAsync(formid);
            var subform = await _context.Form.FirstOrDefaultAsync(w=>w.title==subf);
            List<string> field = new List<string>();
            foreach (var item in subform.Fields.OrderBy(w => w.radif))
            {
                if (item.type != "label" && item.type != "button")
                {
                    if (item.title != null)
                    {
                        item.title = item.title.Replace(' ', '_');
                        field.Add(item.title);
                    }
                    else
                    {
                        field.Add("Field_" + item.id);
                    }
                }
            }
            string subformname = "";
            if (form.SubForms1.FirstOrDefault(w => w.form2id == subform.id).title!=null)
            {
                subformname = form.SubForms1.FirstOrDefault(w => w.form2id == subform.id).title;
            }
            else
            {
                subformname = form.name + "_" + subform.name;
            }
           

            ViewBag.fields = field;
            var result = SqlMethods.SubFormGet(_context, form,subform, subformname, ID);
            return View(result);
        }

        [Authorize(Policy = "/panel/forms/fillindex")]
        public async Task<IActionResult> FillIndex()
        {
            var user = _context.User.FirstOrDefault(w => w.username == User.Identity.Name);
            var list = await _context.Form.Where(w => w.UserForms.Any(e => e.userid == user.id)).ToListAsync();
            return View(list);
        }
        [Authorize(Policy = "/panel/forms/fillindex")]
        public async Task<IActionResult> Fill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = await _context.Form.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            ViewBag.radif = form.Fields.Count() + form.SubForms1.Count();
            return View(form);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = await _context.Form.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            ViewBag.radif = form.Fields.Count() + form.SubForms1.Count();
            return View(form);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var form = await _context.Form.FindAsync(id);
            _context.Form.Remove(form);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> AddSubForm(int formid)
        {
            var list = await _context.Form.Where(w => w.id != formid).ToListAsync();
            ViewBag.forms = new SelectList(list, "id", "title");
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> AddSubForm(SubForm subForm)
        {
            var f = subForm;
            _context.SubForm.Add(f);
            await _context.SaveChangesAsync();

            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.MaxDepth = 1;
            var json = JsonConvert.SerializeObject(f, setting);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }
        public async Task<IActionResult> DetailsSubForm(int id)
        {
            var form = await _context.SubForm.FindAsync(id);
            var context = _context.Form.Find(form.form2id).Fields;
            //JsonSerializerSettings setting = new JsonSerializerSettings();
            //setting.MaxDepth = 1;
            var json = JsonConvert.SerializeObject(context);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }
        public async Task<IActionResult> DeleteSubForm(int id)
        {
            var form = await _context.SubForm.FindAsync(id);
            var list = await _context.Field.Where(w => w.formid == form.form1id).ToListAsync();
            foreach (var item in list)
            {
                if (item.radif > form.radif)
                {
                    item.radif--;
                    _context.Field.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            var list2 = await _context.SubForm.Where(w => w.form1id == form.form1id).ToListAsync();
            foreach (var item in list2)
            {
                if (item.radif > form.radif)
                {
                    item.radif--;
                    _context.SubForm.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            _context.SubForm.Remove(form);
            await _context.SaveChangesAsync();
            return Ok();
        }




        public async Task<IActionResult> CreateField()
        {
            var list = await Methods.Domains(_context, User.Identity.Name);
            ViewBag.domains = new SelectList(list, "id", "title");
            return PartialView();
        }
        [HttpPost]
        public async Task<int> CreateField(Field field, string[] subfields)
        {
            var f = field;
            switch (field.type)
            {
                case "0":
                    f.type = "input";
                    break;
                case "1":
                    f.type = "number";
                    break;
                case "2":
                    f.type = "dateshamsi";
                    break;
                case "3":
                    f.type = "datemiladi";
                    break;
                case "4":
                    f.type = "textarea";
                    break;
                case "5":
                    f.type = "label";
                    break;
                case "6":
                    f.type = "dropdown";
                    break;
                case "7":
                    f.type = "multidropdown";
                    break;
                case "8":
                    f.type = "button";
                    break;
                case "9":
                    f.type = "checkbox";
                    break;
                case "10":
                    f.type = "password";
                    break;
            }
            _context.Field.Add(f);
            await _context.SaveChangesAsync();
            foreach (var item in subfields)
            {
                _context.SubField.Add(new SubField { fieldid = f.id, value = item });
                await _context.SaveChangesAsync();
            }
            return f.id;
        }

        public async Task<IActionResult> EditField(int id)
        {
            var list = await Methods.Domains(_context, User.Identity.Name);
            ViewBag.domains = new SelectList(list, "id", "title");
            var field = await _context.Field.FindAsync(id);

            ViewBag.isdomain = 0;
            if (field.SubFields != null)
            {
                if (field.SubFields.Count() != 0)
                {
                    ViewBag.isdomain = 1;
                }
                foreach (var item in field.SubFields)
                {
                    var temp = _context.DomainValue.FirstOrDefault(w => w.value == item.value);
                    if (temp != null)
                    {
                        ViewBag.isdomain = 2;
                        ViewBag.domains = new SelectList(list, "id", "title", temp.domainid);
                        ViewBag.khodedomain = temp.Domain.DomainValues;
                    }
                }
            }



            return PartialView(field);
        }
        [HttpPost]
        public async Task<int> EditField(Field field, string[] subfields)
        {
            var f = field;
            switch (field.type)
            {
                case "0":
                    f.type = "input";
                    break;
                case "1":
                    f.type = "number";
                    break;
                case "2":
                    f.type = "dateshamsi";
                    break;
                case "3":
                    f.type = "datemiladi";
                    break;
                case "4":
                    f.type = "textarea";
                    break;
                case "5":
                    f.type = "label";
                    break;
                case "6":
                    f.type = "dropdown";
                    break;
                case "7":
                    f.type = "multidropdown";
                    break;
                case "8":
                    f.type = "button";
                    break;
                case "9":
                    f.type = "checkbox";
                    break;
                case "10":
                    f.type = "password";
                    break;
            }
            _context.Field.Update(f);
            await _context.SaveChangesAsync();

            var list = await _context.SubField.Where(w => w.fieldid == field.id).ToListAsync();

            if (list != null)
            {
                foreach (var item in list)
                {
                    _context.SubField.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }


            foreach (var item in subfields)
            {
                _context.SubField.Add(new SubField { fieldid = f.id, value = item });
                await _context.SaveChangesAsync();
            }
            return f.id;
        }
        public async Task<IActionResult> DeleteField(int id)
        {
            var form = await _context.Field.FindAsync(id);
            var list = await _context.Field.Where(w => w.formid == form.formid).ToListAsync();
            foreach (var item in list)
            {
                if (item.radif > form.radif)
                {
                    item.radif--;
                    _context.Field.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            var list2 = await _context.SubForm.Where(w => w.form1id == form.formid).ToListAsync();
            foreach (var item in list2)
            {
                if (item.radif > form.radif)
                {
                    item.radif--;
                    _context.SubForm.Update(item);
                    await _context.SaveChangesAsync();
                }
            }

            _context.Field.Remove(form);
            await _context.SaveChangesAsync();
            return Ok();
        }
        public async Task<IActionResult> domainvalues(int id)
        {
            var list = await Methods.DomainValues(_context, User.Identity.Name);
            var context = list.Where(w => w.domainid == id).ToList();
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.MaxDepth = 1;
            var json = JsonConvert.SerializeObject(context, setting);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }




        private bool FormExists(int id)
        {
            return _context.Form.Any(e => e.id == id);
        }



        public async Task<IActionResult> SqlCommand(int id, string method)
        {
            var form = await _context.Form.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            bool result;
            switch (method)
            {
                case "Create":
                    result = await SqlMethods.Create(_context, form);
                    break;
                case "Drop":
                    result = await SqlMethods.Drop(_context, form);
                    break;
            }
            return Ok(new { message = "با موفقیت انجام شد." });

        }
        [HttpPost]
        public async Task<IActionResult> SqlCommandInsertFK(int formid, string id, List<string> parameters)
        {
            var form = _context.SubForm.Find(int.Parse(id.Split('-')[1])).Form2;
            List<int> fk = new List<int>();
            if (parameters.Count() > form.Fields.Count())
            {
                int up = form.Fields.Count();
                for (int i = 0; i < parameters.Count(); i += up)
                {
                    var temp = parameters.GetRange(i, up);
                    var result = await SqlMethods.InsertFK(_context, form, temp);
                    fk.Add(SqlMethods.GetLastID(_context, form));
                }
            }
            else
            {
                var result = await SqlMethods.InsertFK(_context, form, parameters);
                fk.Add(SqlMethods.GetLastID(_context, form));
            }
            return Ok(new { message = fk });

        }
        [HttpPost]
        public async Task<IActionResult> SqlCommandInsert(int formid, List<string> parameters, List<string> fk)
        {
            var form = _context.Form.Find(formid);
            foreach (var item in fk)
            {
                var temp = _context.SubForm.Find(int.Parse(item.Split('-')[1]));
                if (temp.type == 0)
                {
                    parameters.Add(item.Split('-')[2]);
                }
            }
            var result = await SqlMethods.InsertFK(_context, form, parameters, true);
            int pk = SqlMethods.GetLastID(_context, form);
            foreach (var item in fk)
            {

                var temp = _context.SubForm.Find(int.Parse(item.Split('-')[1]));
                if (temp.type == 1)
                {
                    string name = "";
                    temp.Form1.name = temp.Form1.name.Replace(' ', '_');
                    temp.Form2.name = temp.Form2.name.Replace(' ', '_');
                    temp.title = temp.title.Replace(' ', '_');
                    if (string.IsNullOrEmpty(temp.title))
                    {
                        name = temp.Form1.name + "_" + temp.Form2.name;
                    }
                    else
                    {
                        name = temp.title;
                    }
                    List<string> pk_fk = new List<string>();
                    pk_fk.Add(pk.ToString());
                    pk_fk.Add(item.Split('-')[2]);
                    var result2 = await SqlMethods.InsertFK(_context, form, pk_fk, false, name, temp.id);
                }
            }

            return Ok(new { message = "با موفقیت درج شد" });

        }
    }
}
