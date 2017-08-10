using System.Collections;
using System.Collections.Generic;
using RoCMS.FAQ.Contract.Models;

namespace RoCMS.FAQ.Contract.Services
{
    public interface IQuestionService
    {
        ICollection<Question> GetQuestions();
        Question GetQuestion(int id);
        void ModerateQuestion(int id, bool isModerated);
        void DeleteQuestion(int id);
        void UpdateQuestion(Question rec);
        void SendAnswerToAuthor(int questionId);
        int CreateQuestion(Question rec);
        void SortQuestions(ICollection<int> ids);
    }
}
