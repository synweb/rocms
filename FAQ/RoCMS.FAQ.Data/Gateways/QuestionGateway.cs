using RoCMS.Base.Data;
using RoCMS.FAQ.Data.Models;

namespace RoCMS.FAQ.Data.Gateways
{
    public class QuestionGateway: BasicGateway<Question>
    {
        protected override string DefaultScheme => "FAQ";
    }
}
