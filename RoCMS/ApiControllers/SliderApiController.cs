using System;
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
    [System.Web.Http.Authorize]
    [AuthorizeResourcesApi(RoCmsResources.Sliders)]
    public class SliderApiController : ApiController
    {
        private readonly ISliderService _sliderService;

        public SliderApiController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        #region Slider

        [HttpGet]
        public ResultModel GetSlider(int id)
        {
            var res = _sliderService.GetSlider(id);
            return new ResultModel(true, res);
        }

        [HttpGet]
        public ResultModel GetSliders()
        {
            IEnumerable<Slider> res = _sliderService.GetSliders();
            return new ResultModel(true, res);
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
            int res = _sliderService.CreateSlider(slider.Name);
            return new ResultModel(true, res);
        }

        #endregion

        #region Slide

        [HttpGet]
        public ResultModel GetSlide(int id)
        {
            Slide slide = _sliderService.GetSlide(id);
            return new ResultModel(true, slide);
        }

        [HttpGet]
        public ResultModel GetSlides(int sliderId)
        {
            var slides = _sliderService.GetSlides(sliderId);
            return new ResultModel(true, slides);
        }
            
        [HttpPost]
        public ResultModel EditSlide(Slide slide)
        {
            bool res = _sliderService.EditSlide(slide);
            return new ResultModel(res);
        }

        [HttpPost]
        public ResultModel CreateSlide(Slide slide)
        {
            try
            {


                int res = _sliderService.CreateSlide(slide);
                return res != 0 ? new ResultModel(true, res) : new ResultModel(false);
            }
            catch
            {
                return new ResultModel(false);
            }
        }

        [HttpPost]
        public ResultModel RemoveSlide(int id)
        {
            bool res = _sliderService.RemoveSlide(id);
            return new ResultModel(true, res);
        }

        #endregion
    }
}
