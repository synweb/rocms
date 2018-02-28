using System;
using System.Linq;
using System.Text;

namespace RoCMS.Shop.Contract.Models
{
    public class MassPriceChangeTask
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public MassPriceChangeTaskState State { get; set; }
        public string Comment { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class MassPriceChange
    {
        public GoodsFilter Filter { get; set; }
        public bool Increase { get; set; }

        /// <summary>
        /// Значение в процентах
        /// </summary>
        public decimal Value { get; set; }

        public string Comment { get; set; }

        public string GetDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Цены ");
            sb.Append(Increase ? "увеличены " : "уменьшены ");
            sb.Append(String.Format("на {0}% ", Value));

            if (!Filter.ManufacturerIds.Any() && !Filter.CategoryIds.Any())
            {
                sb.Append("во всех товарах.");
            }
            else
            {
                if (Filter.ManufacturerIds.Any())
                {
                    sb.Append("в производителях: ");
                    foreach (int id in Filter.ManufacturerIds)
                    {
                        sb.Append(id);
                        if (id != Filter.ManufacturerIds.Last())
                        {
                            sb.Append(", ");
                        }
                    }
                    if (Filter.CategoryIds.Any())
                    {
                        sb.Append(" и ");
                    }
                }
                if (Filter.CategoryIds.Any())
                {
                    sb.Append("в категориях: ");
                    sb.Append(string.Join(", ", Filter.CategoryIds.SelectMany(x => x)));
                }
                sb.Append(".");
            }
            return sb.ToString();
        }
    }

    public enum MassPriceChangeTaskState 
    {
        Processed,
        Completed,
        Error
    }
}
