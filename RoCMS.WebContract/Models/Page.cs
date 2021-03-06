﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using RoCMS.Web.Contract.Models.Search;

namespace RoCMS.Web.Contract.Models
{
    [DisplayName(@"Страница")]
    public class Page: Heart, ISearchable
    {
        [AllowHtml]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public string Content { get; set; }

        public IEnumerable<string> SearchIndexKeys {
            get { return new[] {SeachKeyTitle, SeachKeyContent}; }
        }

        public string SeachKeyTitle => nameof(Title);
        public string SeachKeyContent => nameof(Content);        
    }
}
