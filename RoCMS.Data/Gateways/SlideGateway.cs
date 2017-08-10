using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class SlideGateway : BaseGateway
    {
        public int Insert(Slide slide)
        {
            return Exec<int>("[dbo].[Slide_Insert]", slide);
        }

        public void Delete(int slideId)
        {
            Exec("[dbo].[Slide_Delete]", slideId);
        }

        public Slide SelectOne(int slideId)
        {
            return Exec<Slide>("[dbo].[Slide_SelectOne]", slideId);
        }

        public ICollection<Slide> Select(int sliderId)
        {
            return ExecSelect<Slide>("[dbo].[Slide_Select]", sliderId);
        }

        public void Update(Slide slide)
        {
            Exec("[dbo].[Slide_Update]", slide);
        }
    }
}
