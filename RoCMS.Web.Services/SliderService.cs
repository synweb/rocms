using RoCMS.Base.Helpers;
using RoCMS.Web.Contract.Services;
using System.Collections.Generic;
using AutoMapper;
using RoCMS.Data.Gateways;
using Slide = RoCMS.Data.Models.Slide;
using Slider = RoCMS.Data.Models.Slider;

namespace RoCMS.Web.Services
{
    class SliderService : BaseCoreService, ISliderService
    {
        private readonly SliderGateway _sliderGateway = new SliderGateway();
        private readonly SlideGateway _slideGateway = new SlideGateway();
        
        protected override int CacheExpirationInMinutes
        {
            get
            {
                return AppSettingsHelper.HoursToExpireCartCache * 60;
            }
        }
        #region ISliderService
        public int CreateSlide(Contract.Models.Slide slide)
        {
            return _slideGateway.Insert(Mapper.Map<Slide>(slide));
        }

        public int CreateSlider(string name)
        {
            return _sliderGateway.Insert(new Slider() { Name = name });
        }

        public bool EditSlide(Contract.Models.Slide slide)
        {
            try
            {
                _slideGateway.Update(Mapper.Map<Slide>(slide));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Contract.Models.Slide GetSlide(int id)
        {
            return Mapper.Map<Contract.Models.Slide>(_slideGateway.SelectOne(id));
        }

        public Contract.Models.Slider GetSlider(int id)
        {
            return Mapper.Map<Contract.Models.Slider>(_sliderGateway.SelectOne(id));
        }

        public IEnumerable<Contract.Models.Slider> GetSliders()
        {
            var sliders = _sliderGateway.Select();
            var list = Mapper.Map<ICollection<Contract.Models.Slider>>(sliders);
            return list;
        }

        public IEnumerable<Contract.Models.Slide> GetSlides(int sliderId)
        {
            var slides = _slideGateway.Select(sliderId);
            var list = Mapper.Map<ICollection<Contract.Models.Slide>>(slides);
            return list;
        }

        public bool RemoveSlide(int id)
        {
            try
            {
                _slideGateway.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveSlider(int id)
        {
            try
            {
                _sliderGateway.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
