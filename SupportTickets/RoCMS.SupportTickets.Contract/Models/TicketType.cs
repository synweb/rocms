namespace RoCMS.SupportTickets.Contract.Models
{
    public enum TicketType
    {
        /// <summary>
        /// Общий вопрос
        /// </summary>
        Question,
        /// <summary>
        /// Работа с заказом
        /// </summary>
        Order,
        /// <summary>
        /// Конфликт
        /// </summary>
        Conflict,
        /// <summary>
        /// Оспаривание отзыва
        /// </summary>
        ReviewArgument,
        /// <summary>
        /// Технический вопрос
        /// </summary>
        Tech,
        /// <summary>
        /// Платежи
        /// </summary>
        Payments,
        /// <summary>
        /// Предложение по улучшению сервиса
        /// </summary>
        Offer,
        /// <summary>
        /// Мошенничество
        /// </summary>
        Cheating,
        /// <summary>
        /// Прочее
        /// </summary>
        Other
    }
}
