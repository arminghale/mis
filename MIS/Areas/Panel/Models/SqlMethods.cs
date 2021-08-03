using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MIS
{
    public static class SqlMethods
    {
        public static async Task<bool> Create(Context _context, Form form)
        {
            form.name = form.name.Replace(' ', '_');
            var commandText = "CREATE TABLE " + form.name + " (";
            commandText += "ID int IDENTITY(1,1) NOT NULL";
            foreach (var item in form.Fields.OrderBy(w => w.radif))
            {
                if (item.type != "label" && item.type != "button")
                {
                    commandText += ", " + GetType(item);
                }
            }

            commandText += ", PRIMARY KEY (ID)";
            foreach (var item in form.SubForms1.OrderBy(w => w.radif))
            {
                if (item.type == 0)
                {
                    if (string.IsNullOrEmpty(item.title))
                    {
                        item.Form2.name = item.Form2.name.Replace(' ', '_');
                        commandText += ", FK_" + item.Form2.name + " int FOREIGN KEY REFERENCES " + item.Form2.name + "(ID) ON DELETE CASCADE";
                    }
                    else
                    {
                        item.Form2.name = item.Form2.name.Replace(' ', '_');
                        item.title = item.title.Replace(' ', '_');
                        commandText += ", FK_" + item.title + " int FOREIGN KEY REFERENCES " + item.Form2.name + "(ID) ON DELETE CASCADE";
                    }
                }
            }

            commandText += ");";
            await _context.Database.ExecuteSqlRawAsync(commandText);

            foreach (var item in form.SubForms1)
            {
                if (item.type == 1)
                {
                    item.Form1.name = item.Form1.name.Replace(' ', '_');
                    item.Form2.name = item.Form2.name.Replace(' ', '_');
                    
                    if (string.IsNullOrEmpty(item.title))
                    {
                        commandText = "CREATE TABLE " + item.Form1.name + "_" + item.Form2.name + " (";
                    }
                    else
                    {
                        item.title = item.title.Replace(' ', '_');
                        commandText = "CREATE TABLE " + item.title + " (";
                    }

                    commandText += "ID int IDENTITY(1,1) NOT NULL";
                    commandText += ", PRIMARY KEY (ID)";
                    commandText += ", FK_" + item.Form1.name + " int FOREIGN KEY REFERENCES " + item.Form1.name + "(ID) ON DELETE CASCADE";
                    commandText += ", FK_" + item.Form2.name + " int FOREIGN KEY REFERENCES " + item.Form2.name + "(ID) ON DELETE CASCADE";


                    commandText += ");";
                    await _context.Database.ExecuteSqlRawAsync(commandText);
                }
            }
            return true;
        }
        public static async Task<bool> Drop(Context _context, Form form)
        {
            var commandText = "DROP TABLE " + form.name + " ;";
            foreach (var item in form.SubForms1)
            {
                if (item.type == 1)
                {
                    item.Form1.name = item.Form1.name.Replace(' ', '_');
                    item.Form2.name = item.Form2.name.Replace(' ', '_');
                    
                    if (string.IsNullOrEmpty(item.title))
                    {
                        
                        commandText = "DROP TABLE " + item.Form1.name + "_" + item.Form2.name + " ;";
                    }
                    else
                    {
                        item.title = item.title.Replace(' ', '_');
                        commandText = "DROP TABLE " + item.title + " ;";
                    }
                    await _context.Database.ExecuteSqlRawAsync(commandText);
                }
            }

            form.name = form.name.Replace(' ', '_');
            commandText = "DROP TABLE " + form.name + " ;";
            await _context.Database.ExecuteSqlRawAsync(commandText);


            return true;
        }
        public static List<List<string>> Get(Context _context, Form form)
        {
            int count = form.Fields.Where(w=>w.type!="label"&&w.type!="button").Count();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                if (form.SubForms1.Any(w => w.type == 0))
                {
                    command.CommandText = "SELECT ";
                    command.CommandText += form.name + ".ID ,";
                    foreach (var item in form.Fields)
                    {
                        if (item.type != "label" && item.type != "button")
                        {
                            command.CommandText += form.name + "." + item.title.Replace(' ', '_') + " , ";
                        }

                    }
                    foreach (var item in form.SubForms1.Where(w => w.type == 0))
                    {
                        foreach (var item2 in item.Form2.Fields)
                        {
                            if (item2.type != "label" && item2.type != "button")
                            {
                                command.CommandText += item.Form2.name + "." + item2.title.Replace(' ', '_') + " , ";
                            }
                            count++;

                        }
                    }
                    command.CommandText = command.CommandText.Remove(command.CommandText.Length - 2);
                    command.CommandText += "From " + form.name;
                    foreach (var item in form.SubForms1.Where(w => w.type == 0))
                    {
                        if (item.type == 0)
                        {
                            if (string.IsNullOrEmpty(item.title))
                            {
                                item.Form2.name = item.Form2.name.Replace(' ', '_');
                                command.CommandText += " INNER JOIN " + item.Form2.name + " ON " + form.name +
                                    ".FK_" + item.Form2.name + " =" + item.Form2.name + ".ID";
                            }
                            else
                            {
                                item.Form2.name = item.Form2.name.Replace(' ', '_');
                                item.title = item.title.Replace(' ', '_');
                                command.CommandText += " INNER JOIN " + item.Form2.name + " ON " + form.name +
                                    ".FK_" + item.title + " =" + item.Form2.name + ".ID";
                            }
                        }
                        
                    }
                }
                else
                {
                    command.CommandText = "SELECT * From " + form.name;
                }

                command.CommandType = CommandType.Text;

                _context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<List<string>>();

                    while (result.Read())
                    {
                        var temp = new List<string>();

                        for (int i = 0; i <= count; i++)
                        {
                            temp.Add(result.GetValue(i).ToString());
                        }
                        entities.Add(temp);
                    }

                    return entities;
                }
            }
        }
        public static List<List<string>> SubFormGet(Context _context, Form form,Form subform,string subformname,int ID)
        {
            int count = subform.Fields.Where(w => w.type != "label" && w.type != "button").Count();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT ";
                command.CommandText += subform.name + ".ID ,";
                foreach (var item in subform.Fields)
                {
                    if (item.type != "label" && item.type != "button")
                    {
                        command.CommandText += subform.name + "." + item.title.Replace(' ', '_') + " , ";
                    }

                }
                command.CommandText = command.CommandText.Remove(command.CommandText.Length - 2);
                command.CommandText += "From " + subformname;
                command.CommandText += " INNER JOIN " + form.name + " ON " + form.name + ".ID =" + subformname + ".FK_" + form.name;
                command.CommandText += " INNER JOIN " + subform.name + " ON " + subform.name + ".ID =" + subformname + ".FK_" + subform.name;
                command.CommandText += " WHERE ( "+form.name+".ID = "+ID+" )";
                command.CommandType = CommandType.Text;

                _context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<List<string>>();

                    while (result.Read())
                    {
                        var temp = new List<string>();

                        for (int i = 0; i <= count; i++)
                        {
                            temp.Add(result.GetValue(i).ToString());
                        }
                        entities.Add(temp);
                    }

                    return entities;
                }
            }
        }
        public static async Task<bool> InsertFK(Context _context, Form form, List<string> parameters, bool fk = false, string pk_fk = null, int subformid = 0)
        {
            string commandText = "";
            if (string.IsNullOrEmpty(pk_fk))
            {
                commandText = "INSERT INTO " + form.name + " ( ";
                foreach (var item in form.Fields.OrderBy(w => w.radif))
                {
                    if (item.type != "button" && item.type != "label")
                    {
                        item.title = item.title.Replace(' ', '_');
                        commandText += item.title + ", ";
                    }

                }
                if (fk)
                {
                    foreach (var item in form.SubForms1.OrderBy(w => w.radif))
                    {
                        if (item.type == 0)
                        {
                            if (string.IsNullOrEmpty(item.title))
                            {
                                item.Form2.name = item.Form2.name.Replace(' ', '_');
                                commandText += " FK_" + item.Form2.name + ", ";
                            }
                            else
                            {
                                item.Form2.name = item.Form2.name.Replace(' ', '_');
                                item.title = item.title.Replace(' ', '_');
                                commandText += " FK_" + item.title + ", ";
                            }
                        }
                    }
                }
                commandText = commandText.Remove(commandText.Length - 2);
                commandText += " ) VALUES ( ";
                foreach (var item in parameters)
                {
                    commandText += "N'" + item + "', ";
                }
                commandText = commandText.Remove(commandText.Length - 2);
                commandText += " )";
            }
            else
            {
                commandText = "INSERT INTO " + pk_fk + " ( ";
                foreach (var item in form.SubForms1.Where(w => w.id == subformid))
                {
                    commandText += "FK_" + item.Form1.name;
                    commandText += " , FK_" + item.Form2.name;
                }
                commandText += " ) VALUES ( ";
                foreach (var item in parameters)
                {
                    commandText += "N'" + item + "', ";
                }
                commandText = commandText.Remove(commandText.Length - 2);
                commandText += " )";
            }

            await _context.Database.ExecuteSqlRawAsync(commandText);
            return true;
        }
        public static int GetLastID(Context _context, Form form)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select top 1 * from " + form.name + " order by ID DESC ";
                command.CommandType = CommandType.Text;

                _context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    int entities = 0;

                    while (result.Read())
                    {
                        entities = result.GetInt32(0);
                    }

                    return entities;
                }
            }
        }
        private static string GetType(Field field)
        {
            string result = "";
            if (field.title != null)
            {
                field.title = field.title.Replace(' ', '_');
                result = field.title + " ";
            }
            else
            {
                result = "Field_"+field.id + " ";
            }
            switch (field.type)
            {
                case "input":
                    result += "NVARCHAR(150)";
                    break;
                case "number":
                    result += "int";
                    break;
                case "dateshamsi":
                    result += "NVARCHAR(30)";
                    break;
                case "datemiladi":
                    result += "datetime2(7)";
                    break;
                case "textarea":
                    result += "NVARCHAR(MAX)";
                    break;
                case "checkbox":
                    result += "bit";
                    break;
                case "password":
                    result += "NVARCHAR(50)";
                    break;
                case "dropdown":
                    result += "NVARCHAR(MAX)";
                    break;
                case "multidropdown":
                    result += "NVARCHAR(MAX)";
                    break;
            }
            if (!field.isnull)
            {
                result += " NOT NULL";
            }
            return result;
        }
    }
}
