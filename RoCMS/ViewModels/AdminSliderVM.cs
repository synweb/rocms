using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoCMS.Web.Contract.Models;

namespace RoCMS.ViewModels
{
    public class AdminSliderVM
    {
        public Slider Slider { get; set; }
        public IEnumerable<Slide> Slides { get; set; } 
    }
}