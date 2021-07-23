using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinStore.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientCompany { get; set; }

        public virtual ICollection<AssignTask> AssignTask { get; set; }
    }
}