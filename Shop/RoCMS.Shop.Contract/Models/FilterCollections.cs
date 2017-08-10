using System.Collections.Generic;
using RoCMS.Base.Models;

namespace RoCMS.Shop.Contract.Models
{
    public class FilterCollections
    {
        public FilterCollections()
        {
            Countries = new List<IdNamePair<int>>();
            Manufacturers = new List<IdNamePair<int>>();
            Packs = new List<IdNamePair<int>>();
            SpecValues = new Dictionary<Spec, IList<string>>();
        }

        public ICollection<IdNamePair<int>> Countries { get; set; }
        public ICollection<IdNamePair<int>> Manufacturers { get; set; }
        public ICollection<IdNamePair<int>> Packs { get; set; }
        public IDictionary<Spec, IList<string>> SpecValues { get; set; }
    }
}
