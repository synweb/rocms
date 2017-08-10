using RoCMS.Base.Data;

namespace RoCMS.Shop.Data.Gateways
{
    public abstract class ShopBasicGateway<T>: BasicGateway<T> where T : class, new()
    {
        protected override string DefaultScheme => "Shop";
    }
}
