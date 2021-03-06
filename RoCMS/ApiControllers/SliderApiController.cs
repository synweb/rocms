﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.Sliders)]
    public class SliderApiController : ApiController
    {
        private readonly ISliderService _sliderService;
        private readonly ILogService _logService;

        public SliderApiController(ISliderService sliderService, ILogService logService)
        {
            _sliderService = sliderService;
            _logService = logService;
        }

        #region Slider

        [HttpGet]
        public ResultModel GetSlider(int id)
        {
            try
            {
                var res = _sliderService.GetSlider(id);
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpGet]
        public ResultModel GetSliders()
        {
            try
            {
                IEnumerable<Slider> res = _sliderService.GetSliders();
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel RemoveSlider(int id)
        {
            bool res =_sliderService.RemoveSlider(id);
            return new ResultModel(res);
        }

        [HttpPost]
        public ResultModel CreateSlider(Slider slider)
        {
            try
            {
                int res = _sliderService.CreateSlider(slider.Name);
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        #endregion

        #region Slide

        [HttpGet]
        public ResultModel GetSlide(int id)
        {
            try
            {
                Slide slide = _sliderService.GetSlide(id);
                return new ResultModel(true, slide);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpGet]
        public ResultModel GetSlides(int sliderId)
        {
            try
            {
                var slides = _sliderService.GetSlides(sliderId);
                return new ResultModel(true, slides);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
            
        [HttpPost]
        public ResultModel EditSlide(Slide slide)
        {
            try
            {

                bool res = _sliderService.EditSlide(slide);
                return new ResultModel(res);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel CreateSlide(Slide slide)
        {
            try
            {


                int res = _sliderService.CreateSlide(slide);
                return res != 0 ? new ResultModel(true, res) : new ResultModel(false);
            }
            catch(Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel RemoveSlide(int id)
        {
            try
            {

                bool res = _sliderService.RemoveSlide(id);
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        #endregion
    }
}
