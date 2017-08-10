using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface ISliderService
    {
        //Dictionary<int, string> GetSliderIdNames();

        //void AddSlider(KeyValuePair<int, string> slider);

        int CreateSlider(string name);
        
        Slider GetSlider(int id);

        bool RemoveSlider(int id);
        IEnumerable<Slider> GetSliders();
        IEnumerable<Slide> GetSlides(int sliderId);
        int CreateSlide(Slide slide);

        bool EditSlide(Slide slide);

        bool RemoveSlide(int id);

        Slide GetSlide(int id);
    }
}
