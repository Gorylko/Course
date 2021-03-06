﻿using Shop.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Shop.Web.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        public User Seller { get; set; }

        public User Customer { get; set; }

        public string Address { get; set; }

        public int ProductId { get; set; }

        public DateTime Date { get; set; }
    }
}