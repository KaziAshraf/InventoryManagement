using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ChoukashRevamp.Models;

namespace ChoukashRevamp.Views.Validation
{
    public class ValidationClass
    {
        public static void ValidateTextWithSpace(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Z]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        public static bool ValidateEmailID(string text)
        {
            Regex regex = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            return regex.IsMatch(text);
        }

       
        public static bool UniqueTestforEmail(int id, string email, string model) 
        {
            bool doesExist = true;
            using (var ctx = new Choukash_Revamp_DemoEntities())
            {
                switch (model)
                {
                    case "User":
                        var user = ctx.Users.Where(a => a.email == email).SingleOrDefault<User>();
                        if (user != null && user.id != id)
                        {
                            doesExist = false;
                        }
                        else
                            doesExist = true;
                        break;
                    case "SuperAdmin":
                        var sa = ctx.SuperAdmins.Where(a => a.email == email).SingleOrDefault<SuperAdmin>();
                        if (sa != null && sa.id != id)
                        {
                            doesExist = false;
                        }
                        else
                            doesExist = true;
                        break;
                    case "Company":
                        var company = ctx.Companies.Where(a => a.email == email).SingleOrDefault<Company>();
                        if (company != null && company.id != id)
                        {
                            doesExist = false;
                        }
                        else
                            doesExist = true;
                        break;
                    default:
                        break;
                }
            }
            return doesExist;
        }

        public static bool UniqueTestforEmail(string email, string model)
        {
            bool doesExist = true;
            using (var ctx = new Choukash_Revamp_DemoEntities())
            {
                switch (model)
                {
                    case "User":
                        var user = ctx.Users.Where(a => a.email == email).SingleOrDefault<User>();
                        if (user != null)
                        {
                            doesExist = false;
                        }
                        else
                            doesExist = true;
                        break;
                    case "SuperAdmin":
                        var sa = ctx.SuperAdmins.Where(a => a.email == email).SingleOrDefault<SuperAdmin>();
                        if (sa != null)
                        {
                            doesExist = false;
                        }
                        else
                            doesExist = true;
                        break;
                    case "Company":
                        var company = ctx.Companies.Where(a => a.email == email).SingleOrDefault<Company>();
                        if (company != null)
                        {
                            doesExist = false;
                        }
                        else
                            doesExist = true;
                        break;
                    default:
                        break;
                }
            }
            return doesExist;
        }

        public static bool ValidatePassword(string password) {

            if (password.Count() < 4 || password.Count() > 19)
            {
                return false;
            }
            else
                return true;
        }
    }

    
}
