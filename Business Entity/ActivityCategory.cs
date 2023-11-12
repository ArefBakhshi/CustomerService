using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Entity
{
    public class ActivityCategory
    { 
        public int Id { get; set; } 
        public string CategoryName { get; set; }

        public List<Activity> Activities { get; set; } = new List<Activity>();
    }
}
