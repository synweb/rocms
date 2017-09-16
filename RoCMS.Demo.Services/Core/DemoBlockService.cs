using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Demo.Services.Core
{
    public class DemoBlockService: IBlockService
    {
        private List<Block> _defaultBlocks = new List<Block>();

        public DemoBlockService()
        {
            FillDefaultData();
        }

        private void FillDefaultData()
        {
            try
            {
                var file = "blocks.xml";
                var xs = new XmlSerializer(typeof(List<Block>));
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoData", file);
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    _defaultBlocks = (List<Block>) xs.Deserialize(fs);
                }
            }
            catch
            {
                _defaultBlocks.Add(new Block()
                {
                    BlockId = 1,
                    Title = "Блок первый",
                    Content = "<p>Это первый блок</p>",
                });
                _defaultBlocks.Add(new Block()
                {
                    BlockId = 2,
                    Title = "Блок второй",
                    Content = "<p>Это <strong>второй</strong> блок</p>",
                });
            }

            // Чтобы сохранить список в файл:
            // var xs = new XmlSerializer(_defaultBlocks.GetType());
            // var file = "blocks.xml";
            // string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoData", file);
            // using (FileStream fs = new FileStream(path, FileMode.Create))
            // {
            //     xs.Serialize(fs, _defaultBlocks);
            // }
        }

        private const string BLOCKS_SESSION_KEY = "Blocks";
        private void InitSessionDataIfEmpty(HttpContext ctx)
        {
            if (ctx.Session[BLOCKS_SESSION_KEY] == null)
            {
                ctx.Session[BLOCKS_SESSION_KEY] = _defaultBlocks.ToList();
            }
        }

        private List<Block> GetSessionBlocks(HttpContext ctx)
        {
            return (List<Block>)ctx.Session[BLOCKS_SESSION_KEY];
        }

        public int CreateBlock(Block block)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var blocks = GetSessionBlocks(HttpContext.Current);
            int id = blocks.Max(x => x.BlockId) + 1;
            block.BlockId = id;
            blocks.Add(block);
            return id;
        }

        public Block GetBlock(int id)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var res =
                GetSessionBlocks(HttpContext.Current)
                    .FirstOrDefault(x => x.BlockId == id);
            return res;
        }

        public Block GetBlock(string name)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var res =
                GetSessionBlocks(HttpContext.Current)
                    .FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCulture));
            return res;
        }

        public IList<Block> GetBlocks()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            return GetSessionBlocks(HttpContext.Current);
        }

        public void UpdateBlock(Block block)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var blocks = GetSessionBlocks(HttpContext.Current);
            if (!blocks.Any(x => x.BlockId == block.BlockId))
            {
                throw new ArgumentException("BlockId");
            }
            // старый блок удаляется, новая добавляется
            blocks.RemoveAll(x => x.BlockId == block.BlockId);
            blocks.Add(block);
        }

        public void DeleteBlock(int blockId)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var blocks = GetSessionBlocks(HttpContext.Current);
            if (!blocks.Any(x => x.BlockId == blockId))
            {
                throw new ArgumentException("BlockId");
            }
            blocks.RemoveAll(x => x.BlockId == blockId);
        }

        public IEnumerable<IdNamePair<int>> GetBlockIdNames()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var blocks = GetSessionBlocks(HttpContext.Current);
            return blocks.Select(x => new IdNamePair<int>(x.BlockId, x.Title));
        }
    }
}
