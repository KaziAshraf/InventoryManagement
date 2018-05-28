//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChoukashRevamp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Company
    {
        public Company()
        {
            this.Categories = new HashSet<Category>();
            this.Items = new HashSet<Item>();
            this.Roles = new HashSet<Role>();
            this.SubCategories = new HashSet<SubCategory>();
            this.Users = new HashSet<User>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public int superadmin_id { get; set; }
    
        public virtual ICollection<Category> Categories { get; set; }
        public virtual SuperAdmin SuperAdmin { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
