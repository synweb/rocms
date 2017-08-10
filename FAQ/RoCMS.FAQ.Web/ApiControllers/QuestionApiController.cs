using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using RoCMS.Base.ForWeb.Extensions;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.FAQ.Contract;
using RoCMS.FAQ.Contract.Models;
using RoCMS.FAQ.Contract.Services;
using RoCMS.Helpers;
using RoCMS.Web.Contract.Services;

namespace RoCMS.FAQ.Web.ApiControllers
{
    [AuthorizeResourcesApi(RoCMSResources.FAQ)]
    public class QuestionApiController : ApiController
    {
        private readonly IQuestionService _questionService;

        public QuestionApiController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        public ResultModel Accept(int id)
        {
            try
            {
                _questionService.ModerateQuestion(id, true);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Hide(int id)
        {
            try
            {
                _questionService.ModerateQuestion(id, false);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Delete(int id)
        {
            try
            {
                _questionService.DeleteQuestion(id);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Update(Question data)
        {
            try
            {
                data.RespondentId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                _questionService.UpdateQuestion(data);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel UpdateAndSend(Question data)
        {
            try
            {
                data.RespondentId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                _questionService.UpdateQuestion(data);
                _questionService.SendAnswerToAuthor(data.QuestionId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Create(Question data)
        {
            try
            {
                int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                data.RespondentId = userId;
                data.AuthorId = userId;
                int id = _questionService.CreateQuestion(data);
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Sort(ICollection<int> ids)
        {
            try
            {
                _questionService.SortQuestions(ids);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ResultModel Ask(Question data)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                    data.AuthorId = userId;
                }
                int id = _questionService.CreateQuestion(data);
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}