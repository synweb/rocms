using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class SliderGateway : BaseGateway
    {
        public int Insert(Slider slider)
        {
            return Exec<int>("[dbo].[Slider_Insert]", slider);
        }

        public void Delete(int sliderId)
        {
            Exec("[dbo].[Slider_Delete]", sliderId);
        }

        public Slider SelectOne(int sliderId)
        {
            return Exec<Slider>("[dbo].[Slider_SelectOne]", sliderId);
        }

        public ICollection<Slider> Select()
        {
            return ExecSelect<Slider>("[dbo].[Slider_Select]");
        }

        public void Update(Slider slider)
        {
            Exec("[dbo].[Slider_Update]", slider);
        }
    }
}
