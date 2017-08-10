using System;
using System.Collections.Generic;
using System.Linq;

namespace RoCMS.Shop.Contract.Models
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
            CreationDate = DateTime.UtcNow;
        }

        public List<CartItem> CartItems { get; set; }

        public Guid CartId { get; set; }

        public System.DateTime CreationDate { get; set; }

        public decimal Summary 
        {
            get
            {
                return Math.Round(CartItems.Sum(item => item.Summary), 0, MidpointRounding.AwayFromZero);
            }
        }

        public int Quantity
        {
            get { return CartItems.Count(); }
        }

        public decimal? TotalDiscount { get; set; }

        public decimal DiscountedSummary 
        {
            get
            {
                return TotalDiscount.HasValue ?
                    Math.Round(Summary - Summary * (TotalDiscount.Value / 100m), 0, MidpointRounding.AwayFromZero)
                    : Summary;
            }
        }
    }
}
