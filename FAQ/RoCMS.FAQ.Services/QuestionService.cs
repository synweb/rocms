using System;
using System.Collections.Generic;
using System.Transactions;
using AutoMapper;
using RoCMS.FAQ.Contract.Models;
using RoCMS.FAQ.Contract.Services;
using RoCMS.FAQ.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.FAQ.Services
{
    public class QuestionService: BaseFAQSerivce, IQuestionService
    {
        private readonly QuestionGateway _questionGateway = new QuestionGateway();
        private readonly IMailService _mailService;
        private readonly IRazorEngineService _razorEngineService;

        public QuestionService(IMailService mailService, IRazorEngineService razorEngineService)
        {
            _mailService = mailService;
            _razorEngineService = razorEngineService;
        }

        public ICollection<Question> GetQuestions()
        {
            var dataRes = _questionGateway.Select();
            var res = Mapper.Map<ICollection<Question>>(dataRes);
            return res;
        }

        public Question GetQuestion(int id)
        {
            var dataRes = _questionGateway.SelectOne(id);
            var res = Mapper.Map<Question>(dataRes);
            return res;
        }

        public void ModerateQuestion(int id, bool isModerated)
        {
            var rec = _questionGateway.SelectOne(id);
            rec.Moderated = isModerated;
            _questionGateway.Update(rec);
        }

        public void DeleteQuestion(int id)
        {
            _questionGateway.Delete(id);
        }

        public void UpdateQuestion(Question rec)
        {
            var dataRec = Mapper.Map<Data.Models.Question>(rec);
            _questionGateway.Update(dataRec);
        }

        public void SendAnswerToAuthor(int questionId)
        {
            var dataRec = _questionGateway.SelectOne(questionId);
            if (string.IsNullOrEmpty(dataRec.AuthorEmail))
            {
                throw new ArgumentException("Empty email", nameof(dataRec.AuthorEmail));
            }
            var msg = new MailMsg();
            msg.Subject = "Ответ на вопрос";
            msg.Receiver = dataRec.AuthorEmail;
            msg.Body = _razorEngineService.RenderEmailMessage("FAQAnswerMessage", Mapper.Map<Question>(dataRec));
            _mailService.Send(msg);
            dataRec.AnswerSentToAuthor = true;
            _questionGateway.Update(dataRec);
        }

        public int CreateQuestion(Question rec)
        {
            var dataRec = Mapper.Map<Data.Models.Question>(rec);
            int id = _questionGateway.Insert(dataRec);
            return id;
        }

        public void SortQuestions(ICollection<int> ids)
        {
            int order = 0;
            foreach (var id in ids)
            {
                var rec = _questionGateway.SelectOne(id);
                rec.SortOrder = order++;
                _questionGateway.Update(rec);
            }
        }
    }
}
