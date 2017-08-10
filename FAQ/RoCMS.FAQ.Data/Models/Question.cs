using System;

namespace RoCMS.FAQ.Data.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public DateTime CreationDate { get; set; }
        public string QuestionText { get; set; }
        public int? AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public int? RespondentId { get; set; }
        public string AnswerText { get; set; }
        public bool AnswerSentToAuthor { get; set; }
        public bool Moderated { get; set; }
        public int SortOrder { get; set; }
    }
}
