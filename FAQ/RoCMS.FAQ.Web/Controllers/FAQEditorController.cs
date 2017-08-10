using System.Collections.Generic;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.FAQ.Contract;
using RoCMS.FAQ.Contract.Models;
using RoCMS.FAQ.Contract.Services;

namespace RoCMS.FAQ.Web.Controllers
{
    [AuthorizeResources(RoCMSResources.FAQ)]
    public class FAQEditorController : Controller
    {
        private readonly IQuestionService _questionService;

        public FAQEditorController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [MvcSiteMapNode(Title = "Вопросы", ParentKey = "AdminHome", Key = "Questions", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""FAQ"", ""iconClass"" : ""fa-question-circle""}")]
        public ActionResult Questions()
        {
            ICollection<Question> list = _questionService.GetQuestions();
            return View(list);
        }

        public ActionResult EditQuestion(int id)
        {
            var question = _questionService.GetQuestion(id);
            ViewBag.Action = "Edit";
            return View("QuestionEditor", question);
        }
        public ActionResult CreateQuestion()
        {
            ViewBag.Action = "Create";
            var question = new Question();
            return View("QuestionEditor", question);
        }
    }
}
