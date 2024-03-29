﻿using System;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Shop.Contract.Models
{
    public class Manufacturer:Heart
    {
        public System.Guid Guid { get; set; }
        public string Name { get; set; }
        public string LogoImageId { get; set; }
        public string Description { get; set; }

        public int? CountryId { get; set; }

        public IdNamePair<int> Country { get; set; }
    }
}
