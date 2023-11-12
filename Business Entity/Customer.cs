using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Entity
{
    public class Customer
    {
        public Customer() 
        {
         IsDeleted = false;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime RegDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<Invoice> Invoices { get; set;} = new List<Invoice>();
        public List<Activity> Activities { get; set; } = new List<Activity>();

    }
}
