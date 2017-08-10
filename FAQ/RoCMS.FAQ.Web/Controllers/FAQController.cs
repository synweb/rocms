using System.Linq;
using System.Web.Mvc;
using RoCMS.Base.Models;
using RoCMS.FAQ.Contract.Services;

namespace RoCMS.FAQ.Web.Controllers
{
    public class FAQController : Controller
    {
        private readonly IQuestionService _questionService;

        public FAQController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        //public ActionResult List()
        //{
        //    var questions = _questionService.GetQuestions().Where(x => x.Moderated && !string.IsNullOrEmpty(x.AnswerText)).ToList();
        //    return PartialView("_QuestionList", questions);
        //}
    }
}
