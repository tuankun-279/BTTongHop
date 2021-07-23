using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WinStore.Models
{
    public class Model_Project_Context:DbContext
    {
        public Model_Project_Context() : base("AssignTask") { }
        //DbSet<Project> Projects { get; set; }
        //DbSet<Client> Clients { get; set; }
        //DbSet<Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<WinStore.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<WinStore.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<WinStore.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<WinStore.Models.AssignTask> AssignTasks { get; set; }
    }
}