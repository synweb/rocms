using System;
using System.Collections.Generic;
using RoCMS.Base.Models;

namespace RoCMS.Shop.Contract.Models
{
    public class Action
    {
        public Action()
        {
            Goods = new List<IdNamePair<int>>();
            Categories = new List<IdNamePair<int>>();
            Manufacturers = new List<IdNamePair<int>>();
        }

        public int ActionId { get; set; }
        public DateTime? DateOfEnding { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int? Discount { get; set; }

        public bool ShowInSlider { get; set; }

        public bool Active { get; set; }

        public string ImageId { get; set; }

        public List<IdNamePair<int>> Goods { get; set; }
        public List<IdNamePair<int>> Categories { get; set; }
        public List<IdNamePair<int>> Manufacturers { get; set; }
    }
}
