using System.Collections.Generic;

namespace RoCMS.Web.Contract.Models
{
    public class Menu
    {
        public Menu()
        {
            Items = new List<MenuItem>();
        }

        public int MenuId { get; set; }
        public string Name { get; set; }

        public List<MenuItem> Items { get; set; }

        public IEnumerable<MenuItem> FlatItems()
        {
            foreach (var item in Items)
            {
                yield return item;
                foreach (var child in item.FlatItems())
                {
                    yield return child;
                }
            }
            
        }
    }
}
