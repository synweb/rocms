﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Data.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
    }
}
