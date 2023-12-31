﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Entity
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public List<Invoice> invoices { get; set; } = new List<Invoice>();

        public bool IsDeleted { get; set; }


    }
}
