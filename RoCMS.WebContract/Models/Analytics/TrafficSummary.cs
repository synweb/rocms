using System;

namespace RoCMS.Web.Contract.Models.Analytics
{
    public class TrafficSummary
    {
        /// <summary>
        /// Дата окончания диапазона, которому соответствуют данные при выбранном способе группировки.
        /// </summary>
        public DateTime EndDate { get; set; }

        public int Visits { get; set; }

        public int PageViews { get; set; }

        public int Visitors { get; set; }

        public int NewVisitors { get; set; }

        /// <summary>
        /// Процент отказов
        /// </summary>
        public float Denial { get; set; }

        /// <summary>
        /// Средняя глубина просмотра
        /// </summary>
        public float Depth { get; set; }

        /// <summary>
        /// Время, проведённое на сайте
        /// </summary>
        public TimeSpan VisitTime { get; set; }
    }
}
