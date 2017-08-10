using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using RoCMS.Base;
using RoCMS.Base.Helpers;
using RoCMS.Helpers;
using RoCMS.Logging;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Services;
using RoCMS.Web.Contract.Models;

using RoCMS.Web.Contract.Services;
using RoCMS.Web.Services;
using Image = System.Drawing.Image;

namespace SynWeb.Teaworld.Import
{
    class Program
    {
        private static List<ImportGoodsItem> _goods = new List<ImportGoodsItem>();
        private static AlbumService _albumService;
        private static IShopService _shopService;
        private static IImageService _imageService;
        private static IPageService _pageService;        
        private static ILogService _logService;

        public class AltShopService : ShopContextService
        {
            

            public void RemoveHtmlFromDescriptions()
            {
                using (var db = Context)
                {
                    foreach (var goodsItem in db.GoodsSet)
                    {
                        string descr = goodsItem.Description;
                        descr =  Regex.Replace(descr, HTML_TAG_PATTERN, string.Empty, RegexOptions.Singleline);
                        descr = HttpUtility.HtmlDecode(descr);
                        if (descr.Contains("<") || descr.Contains("&"))
                        {
                            Console.WriteLine(descr);
                        }
                        else
                        {
                            goodsItem.Description = descr;
                        }
                        
                    }
                    db.SaveChanges();
                }
            }

            protected override int CacheExpirationInMinutes
            {
                get { throw new NotImplementedException(); }
            }

            public void FillSearchDescriptions()
            {
                IList<GoodsItem> goods;
                int[] ids;
                using (var db = Context)
                {
                    ids = db.GoodsSet.Select(g => g.GoodsId).ToArray();
                }
                ids = ids.Where(x => x > 2000).ToArray();
                foreach (var id in ids)
                {
                    try
                    {
                        using (var db = Context)
                        {
                            var goodsItem = db.GoodsSet.Find(id);
                            goodsItem.SearchDescription = SearchHelper.ToSearchIndexText(goodsItem.HtmlDescription);
                            db.SaveChanges();

                        }
                        Console.WriteLine("GoodsId: {0} of {1}", id, ids.Length);
                    }
                    catch { }
                }

//                foreach (var item in goods)
//                {
//                    _shopService.UpdateGoods(item);
//                }
            }
        }




        private static void UpdatePageSearchContent()
        {
            IList<PageInfo> pageInfos = _pageService.GetPagesInfo();
            IList<Page> pages = new List<Page>();
            foreach (var pageInfo in pageInfos)
            {
                pages.Add(_pageService.GetPage(pageInfo.RelativeUrl));
            }
            foreach (var page in pages)
            {
                _pageService.UpdatePage(page);
            }
        }

        static void Main(string[] args)
        {
            InitUnity();
            InitServices();
            //FillGoods();
            //ExportGoodsToDB();

            //var service = new AltShopService();
            //service.RemoveHtmlFromDescriptions();
            //service.FillSearchDescriptions();
            //Console.WriteLine("Goods ok!");
            //UpdatePageSearchContent();

            //ExportImages();
            //ExportThumbs();
            CreateThumbs();

            Console.WriteLine("Everything ok!");
            Console.ReadLine();
        }

        static void CreateThumbs()
        {
            const string IMAGES_DIR = "Images";

            const string THUMBS_DIR = "Thumbnails";
            if (!Directory.Exists(THUMBS_DIR))
            {
                Directory.CreateDirectory(THUMBS_DIR);
            }

            var jpgs = Directory.GetFiles(IMAGES_DIR, "*.jpg", SearchOption.AllDirectories);
            var gifs = Directory.GetFiles(IMAGES_DIR, "*.gif", SearchOption.AllDirectories);
            var pngs = Directory.GetFiles(IMAGES_DIR, "*.png", SearchOption.AllDirectories);

            foreach (var jpg in jpgs)
            {
                CreateThumbnail(jpg, "jpg");
            }

            foreach (var gif in gifs)
            {
                CreateThumbnail(gif, "gif");
            }

            foreach (var png in pngs)
            {
                CreateThumbnail(png, "png");
            }
        }

        private static void CreateThumbnail(string path, string extension)
        {
            Image thumb = Image.FromFile(path);
            int thumbnailHeight = 200;
            int thumbnailWidth = 300;

            if ((float)thumb.Height <= thumbnailHeight && (float)thumb.Width <= thumbnailWidth)
            {
                Image temp = new Bitmap(thumb.Width, thumb.Height);

                using (var g = Graphics.FromImage(temp))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(thumb, 0, 0, temp.Width, temp.Height);
                }
                thumb = (Image)temp.Clone();
            }
            else
            {

                // Масштабирование по высоте
                if ((float)thumb.Height > thumbnailHeight)
                {
                    float scale = thumbnailHeight / (float)thumb.Height;
                    Image temp = new Bitmap((int)(thumb.Width * scale), thumbnailHeight);

                    using (var g = Graphics.FromImage(temp))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(thumb, 0, 0, temp.Width, temp.Height);
                    }
                    thumb = (Image)temp.Clone();
                }

                // Масштабирование по ширине
                if ((float)thumb.Width > thumbnailWidth)
                {
                    float scale = thumbnailWidth / (float)thumb.Width;
                    Image temp = new Bitmap(thumbnailWidth, (int)(thumb.Height * scale));

                    using (var g = Graphics.FromImage(temp))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(thumb, 0, 0, temp.Width, temp.Height);
                    }
                    thumb = (Image)temp.Clone();
                }
            }
            ImageFormat format;
            switch (extension)
            {
                case "jpg":
                    format = ImageFormat.Jpeg;
                    break;
                case "gif":
                    format = ImageFormat.Gif;
                    break;
                case "png":
                    format = ImageFormat.Png;
                    break;
                default:
                    throw new ArgumentException("extension");
            }
            string thumbPath = path.Replace("Images", "Thumbnails");
            var dirPath = String.Join("\\", thumbPath.Split('\\').Where(x => !x.Contains(".")).ToArray());
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            thumb.Save(thumbPath, format);
        }

        //private static void ExportImages()
        //{
        //    const string IMAGES_DIR = "Images";
        //    if (!Directory.Exists(IMAGES_DIR))
        //    {
        //        Directory.CreateDirectory(IMAGES_DIR);
        //    }

        //    var imageIds = _imageService.GetAllImageInfos().Select(x => x.ImageId);
        //    foreach (var imageId in imageIds)
        //    {
        //        string first = imageId.Substring(0, 1).ToLower();
        //        string second = imageId.Substring(1, 1).ToLower();
        //        var img = _imageService.GetImage(imageId);
        //        if (!Directory.Exists(Path.Combine(IMAGES_DIR, first)))
        //        {
        //            Directory.CreateDirectory(Path.Combine(IMAGES_DIR, first));
        //        }
        //        string finalDir = Path.Combine(IMAGES_DIR, first, second);
        //        if (!Directory.Exists(finalDir))
        //        {
        //            Directory.CreateDirectory(finalDir);
        //        }

        //        //const string MIMETYPE_JPG = "image/jpeg";
        //        //const string MIMETYPE_PNG = "image/png";
        //        //const string MIMETYPE_GIF = "image/gif";
        //        string extension = img.ContentType.Replace(@"image/", "").Replace("jpeg", "jpg");
        //        string fname = imageId.ToLower() + "." + extension;
        //        File.WriteAllBytes(Path.Combine(finalDir, fname), img.Content);
        //    }

        //}


        private static void InitServices()
        {
            //_shopService = new ShopService();
            //_imageService = new ImageService(_logService);
            //_albumService = new AlbumService(_imageService, _logService);
            //_pageService = new PageService();
            //_articleService = new ArticleService(_pageService);
            //_logService = new LogService();
        }

        private static void ExportGoodsToDB()
        {
            
            const string albumName = "Импортированные товары";
            int albumId;
            if (_albumService.GetAlbums().Any(x => x.Name.Equals(albumName)))
            {
                albumId = _albumService.GetAlbums().First(x => x.Name.Equals(albumName)).AlbumId;
            }
            else
            {
                albumId = _albumService.CreateAlbum(albumName);
            }


            foreach (var goodsItem in _goods)
            {
                var i = new GoodsItem()
                {
                    Article = goodsItem.Article,
                    HtmlDescription = goodsItem.Description,
                    //Description = RemoveHtml(goodsItem.Description),
                    Categories = new[] {new IdNamePair<int>() {ID = goodsItem.CategoryId}},
                    Name = goodsItem.Name
                };

                i.Description = "";
                throw new NotImplementedException("Description");

                try
                {
                    i.Price = int.Parse(goodsItem.Price);
                }
                catch
                {
                    if (goodsItem.Price.Contains(" за 100 г."))
                    {
                        decimal p = decimal.Parse(goodsItem.Price.Replace(" за 100 г.", ""));
                        i.Price = p;
                        //Пачка
                        i.Packs.Add(new GoodsPack() {PackInfo = new Pack() {PackId = 1}});
                        i.Packs.Add(new GoodsPack() {PackInfo = new Pack() {PackId = 2}});
                        i.Packs.Add(new GoodsPack() {PackInfo = new Pack() {PackId = 3}});
                        i.Packs.Add(new GoodsPack() {PackInfo = new Pack() {PackId = 4}});
                    }
                    if (goodsItem.Price.Contains(" за 250 г."))
                    {
                        decimal p = decimal.Parse(goodsItem.Price.Replace(" за 250 г.", ""));
                        p = Decimal.Divide(p, 2.5m);
                        i.Price = p;
                        //Пачка
                        i.Packs.Add(new GoodsPack() {PackInfo = new Pack() {PackId = 1}});
                        i.Packs.Add(new GoodsPack() {PackInfo = new Pack() {PackId = 2}});
                        i.Packs.Add(new GoodsPack() {PackInfo = new Pack() {PackId = 3}});
                        i.Packs.Add(new GoodsPack() {PackInfo = new Pack() {PackId = 4}});
                    }
                }

                try
                {
                    var img = new RoCMS.Web.Contract.Models.Image();
                    var fStream = File.OpenRead(goodsItem.PhotoUrl);
                    img.Size = fStream.Length;
                    byte[] bytes = new byte[fStream.Length];
                    fStream.Read(bytes, 0, (int) fStream.Length);

                    const string MIMETYPE_JPG = "image/jpeg";
                    const string MIMETYPE_PNG = "image/png";

                    string fileType = goodsItem.PhotoUrl.Split('.').Last();
                    img.Content = bytes;
                    img.ContentType = fileType == "jpg" ? MIMETYPE_JPG : MIMETYPE_PNG;
                    var imageId = _imageService.UploadImage(img, goodsItem.PhotoUrl);
                    _albumService.AddImageToAlbum(albumId, imageId);

                    i.MainImageId = imageId;
                }
                catch
                {
                    Console.WriteLine("Failed to find image for {0}", i.Name);
                }
                try
                {
                    _shopService.CreateGoods(i);
                }
                catch
                {
                    Console.WriteLine("Failed to add {0}", i.Name);
                }
            }
        }

        const string HTML_TAG_PATTERN = "<.*?>";

        private static void InitUnity()
        {
            IUnityContainer container = new UnityContainer();
            //RegisterControllers(container);
            container.LoadConfiguration();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }


        private static void FillGoods()
        {
            XDocument doc = XDocument.Load("../../goods.xml");

            XNode node = doc.Root.FirstNode;
            while (node.NextNode != null) //<Worksheet>
            {
                node = node.NextNode;
            }
            node = ((XElement)node).FirstNode; //<Table>
            var rows = ((XElement)node).Elements().Where(x => x.Name.LocalName == "Row");
            Console.WriteLine(rows.Count());

            foreach (var row in rows)
            {
                var item = new ImportGoodsItem();

                var n = (XElement)row.FirstNode; //<Cell> 
                var data = (XElement)n.FirstNode; //<Data>
                item.Id = Convert.ToInt32(data.Value);

                n = (XElement)n.NextNode;
                data = (XElement)n.FirstNode;
                item.Name = data.Value;

                n = (XElement)n.NextNode;
                data = (XElement)n.FirstNode;
                item.Article = data.Value;

                n = (XElement)n.NextNode;
                data = (XElement)n.FirstNode;
                item.Description = data.Value;

                n = (XElement)n.NextNode;
                data = (XElement)n.FirstNode;
                try
                {
                    item.CategoryId = Convert.ToInt32(data.Value);
                }
                catch (FormatException)
                {
                    continue;

                }
                n = (XElement)n.NextNode;
                data = (XElement)n.FirstNode;
                item.Price = data.Value;

                n = (XElement)n.NextNode;
                data = (XElement)n.FirstNode;
                item.PhotoUrl = Path.Combine("C:\\", data.Value.Replace('/', '\\'));

                _goods.Add(item);

            }
        }
    }


    struct ImportGoodsItem
    {
        public int Id;
        public string Name;
        public string Article;
        public string Description;
        public int CategoryId;
        public string Price;
        public string PhotoUrl;
    }
}
