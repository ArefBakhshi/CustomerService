using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Entity
{
    public class Invoice
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime RegDate { get; set; }

        public bool IsCheckedOut { get; set; }

        public DateTime CheckOutDate { get; set;}
        public Customer Customer { get; set; }

        public User User { get; set; }

        public List<Product> Products { get; set;} = new List<Product>();
    }
}
