using System.Collections.Generic;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [Authorize]
    [AuthorizeResourcesApi(RoCmsResources.Blocks)]
    public class BlockApiController : ApiController
    {
        private readonly IBlockService _blockService;

        public BlockApiController(IBlockService blockService)
        {
            _blockService = blockService;
        }

        [HttpGet]
        public IList<Block> Blocks()
        {
            IList<Block> blocks = _blockService.GetBlocks();
            return blocks;
        }

        [AuthorizeResourcesApi(RoCmsResources.DeleteObjects)]
        [HttpPost]
        public ResultModel Delete(int id)
        {
            if (id <= 5)
            {
                return new ResultModel(false, "Блок является системным, его нельзя удалить");
            }

            _blockService.DeleteBlock(id);
            return ResultModel.Success;
        }
    }
}
