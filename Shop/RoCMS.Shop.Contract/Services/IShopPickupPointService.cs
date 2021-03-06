﻿using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopPickupPointService
    {
        PickupPointInfo GetPickupPoint(int id);
        IList<PickupPointInfo> GetPickupPoints();
        void DeletePickupPoint(int id);
        void UpdatePickupPoint(PickupPointInfo point);
        int CreatePickupPoint(PickupPointInfo point);
    }
}
