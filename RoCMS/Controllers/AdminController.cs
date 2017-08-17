using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Controllers
{
    [AuthorizeResources(RoCmsResources.AdminPanel)]
    public class AdminController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly ISettingsService _settingsService;
        private readonly IPageService _pageService;
        private readonly IBlockService _blockService;
        private readonly ISecurityService _securityService;
        private readonly IAlbumService _albumService;
        private readonly IImageService _imageService;
        private readonly ISliderService _sliderService;
        
        private readonly IAnalyticsService _analyticsService;
        private readonly IFileService _fileService;
        private readonly IVideoGalleryService _videoGalleryService;
        private readonly IFormRequestService _formRequestService;
        private readonly IMailService _mailService;
        private readonly IReviewService _reviewService;

        private readonly IOrderFormService _orderFormService;

        public AdminController(IMenuService menuService, ISettingsService settingsService, IPageService pageService,
            IBlockService blockService, ISecurityService securityService, IAlbumService albumService, 
            ISliderService sliderService, IAnalyticsService analyticsService, IFileService fileService, IVideoGalleryService videoGallery, 
            IFormRequestService formRequestService, IMailService mailService, IReviewService reviewService, IImageService imageService, IOrderFormService orderFormService)
        {
            _menuService = menuService;
            _settingsService = settingsService;
            _pageService = pageService;
            _blockService = blockService;
            _securityService = securityService;
            _albumService = albumService;
            _sliderService = sliderService;
            _analyticsService = analyticsService;
            _fileService = fileService;
            _videoGalleryService = videoGallery;
            _formRequestService = formRequestService;
            _mailService = mailService;
            _reviewService = reviewService;
            _imageService = imageService;
            _orderFormService = orderFormService;
        }

        public ActionResult Index()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            ViewBag.Version = $"{version.Major}.{version.Minor}.{version.Build}";
            return View();
        }



        #region Pages
        [AuthorizeResources(RoCmsResources.Pages)]
        public ActionResult Pages()
        {
            var pages = _pageService.GetPagesInfo();
            return PartialView(pages);
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Pages)]
        public ActionResult CreatePage()
        {
            ViewBag.Action = "Create";
            ViewBag.Pages = _pageService.GetPagesInfo();
            return PartialView("_PageEditor");
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Pages)]
        public ActionResult CreatePage(Page page)
        {
            if(!ModelState.IsValid)
            {
                return Json(new ResultModel(false));
            }
            _pageService.CreatePage(page);
            return Json(ResultModel.Success);
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Pages)]
        public ActionResult EditPage(string relativeUrl)
        {
            ViewBag.Action = "Edit";
            ViewBag.Pages = _pageService.GetPagesInfo();
            var page = _pageService.GetPage(relativeUrl);
            return PartialView("_PageEditor", page);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Pages)]
        public ActionResult EditPage(Page page)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ResultModel(false));
            }

            _pageService.UpdatePage(page);
            return Json(ResultModel.Success);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Pages)]
        public ActionResult DeletePage(int pageId)
        {
            _pageService.DeletePage(pageId);
            return Json(ResultModel.Success);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Pages)]
        public string HtmlPreview(string pre)
        {
            pre = ContentRenderHelper.RenderContent(pre);
            return Server.HtmlDecode(pre);
        }

        #endregion

        #region Requests
        [AuthorizeResources(RoCmsResources.Requests)]
        public ActionResult Requests()
        {
            var requests = _formRequestService.GetFormRequests();
            return PartialView(requests);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Requests)]
        public ActionResult DeleteFormRequest(int formRequestId)
        {
            _formRequestService.DeleteFormRequest(formRequestId);
            return Json(ResultModel.Success);
        }

        #endregion

        #region Emails
        [AuthorizeResources(RoCmsResources.Emails)]
        public ActionResult MailMessages()
        {
            var messages = _mailService.GetMessages();
            return PartialView(messages);
        }
        
        [AuthorizeResources(RoCmsResources.Emails)]
        public ActionResult EmailTemplates()
        {
            var templateList = _settingsService.GetEmailTemplateNames();
            return View(templateList);
        }

        [AuthorizeResources(RoCmsResources.Emails)]
        public ActionResult EditEmailTemplate(string id)
        {
            string settingName = "MailTmpl" + id;
            var content = _settingsService.GetSettings<string>(settingName);
            ViewBag.TemplateName = id;
            return View("EditEmailTemplate", (object)content); //(content);
        }




        #endregion

        #region Menus

        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult Menu()
        {
            var menus = _menuService.GetMenus();
            return PartialView(menus);
        }

        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult MenuList()
        {
            var menus = _menuService.GetMenus();
            return PartialView("_MenuList", menus);
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult CreateMenu()
        {
            Menu menu = new Menu()
            { 
                MenuId = -1
            };
            return PartialView("_MenuEditor", menu);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult CreateMenu(Menu menu)
        {
            int id = _menuService.CreateMenu(menu);
            return Json(new ResultModel(true, id));
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult UpdateMenu(int menuId)
        {
            //var menu = _menuService.GetMenu(menuId);
            return PartialView("_MenuEditor");
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult UpdateMenu(Menu menu)
        {
            _menuService.UpdateMenu(menu);
            return Json(ResultModel.Success);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult DeleteMenu(int menuId)
        {
            _menuService.DeleteMenu(menuId);
            return Json(ResultModel.Success);
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult MenuEditor(int Id)
        {
            var menu = _menuService.GetMenu(Id);
            return PartialView("_MenuEditor", menu);
            
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Menus)]
        public ActionResult MenuCreate()
        {           
            return PartialView("_MenuEditor");
        }


        #endregion

        #region Blocks
        [AuthorizeResources(RoCmsResources.Blocks)]
        public ActionResult Blocks()
        {
            var list = _blockService.GetBlocks();
            return PartialView(list);
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Blocks)]
        public ActionResult ViewBlock(int id)
        {
            var block = _blockService.GetBlock(id);
            return PartialView(block);
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Blocks)]
        public ActionResult CreateBlock()
        {
            ViewBag.Action = "Create";
            return PartialView("_BlockEditor");
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Blocks)]
        public ActionResult CreateBlock(Block block)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ResultModel(false, block));
            }
            int id = _blockService.CreateBlock(block);
            return Json(new ResultModel(true, id));
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Blocks)]
        public PartialViewResult EditBlock(int id)
        {
            ViewBag.Action = "Edit";
            var block = _blockService.GetBlock(id);
            return PartialView("_BlockEditor", block);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Blocks)]
        public JsonResult EditBlock(Block block)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ResultModel(false, block));
            }
            //Особенность Kendo Editor
            //block.Content = Server.HtmlDecode(block.Content);
            _blockService.UpdateBlock(block);
            return Json(ResultModel.Success);
        }

        [AuthorizeResources(RoCmsResources.Blocks)]
        public ActionResult DeleteBlock(int id)
        {
            if (id <= 5)
            {
                throw new InvalidOperationException("Блок является системным, его нельзя удалить");
            }

            _blockService.DeleteBlock(id);
            return Json(ResultModel.Success);
        }

        #endregion

        #region Settings
        [AuthorizeResources(RoCmsResources.CommonSettings)]
        public ActionResult CommonConfig()
        {
            Setting model = _settingsService.GetSettings();
            return PartialView(model);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.CommonSettings)]
        public ActionResult SaveSettings(Setting model)
        {
            _settingsService.UpdateSettings(model);
            return Json(ResultModel.Success);
        }

        public class ChangePasswordData { public string EmailPassword { get; set; } }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.CommonSettings)]
        public ActionResult UpdateEmailPassword(ChangePasswordData model)
        {
            _settingsService.UpdateEmailPassword(model.EmailPassword);
            return Json(ResultModel.Success);
        }
        #endregion

        #region Users

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return PartialView("_ChangePassword");
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword, string repeatPassword)
        {
            if (!string.IsNullOrEmpty(oldPassword) &&
                !string.IsNullOrEmpty(newPassword) &&
                !string.IsNullOrEmpty(repeatPassword))
            {
                if (newPassword != repeatPassword)
                {
                    throw new InvalidDataException();
                }
                _securityService.ChangePassword(User.Identity.Name, oldPassword, newPassword);
                return Json(ResultModel.Success);
            }
            return Json(new ResultModel(false));
        }

        [AuthorizeResources(RoCmsResources.Users)]
        public ActionResult Users()
        {
            var users = _securityService.GetUsers();
            return PartialView(users);
        }

        [AuthorizeResources(RoCmsResources.Users)]
        public ActionResult EditUser(int id)
        {
            var user = _securityService.GetUser(id);
            return View("UserEditor",user);
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.Users)]
        public IEnumerable<string> GetUsernames()
        {
            return _securityService.GetUsernames();
        }

        [AuthorizeResources(RoCmsResources.Users)]
        [HttpPost]
        public ActionResult DeleteUser(string username)
        {
            if (username != "admin")
            {
                _securityService.RemoveUser(username);
            }
            return RedirectToAction("Users");
        }

        [AuthorizeResources(RoCmsResources.Users)]
        public ActionResult Register()
        {
            return PartialView("Register");
        }

        public class ChangeUserPasswordData
        {
            public string Password { get; set; }
            public int UserId { get; set; }
        }

        [AuthorizeResources(RoCmsResources.Users)]
        [HttpPost]
        public ActionResult EditUserPassword(ChangeUserPasswordData model)
        {
            _securityService.SetPassword(model.UserId, model.Password);
            return Json(ResultModel.Success);
        }

        #endregion

        #region Sliders

        [AuthorizeResources(RoCmsResources.Sliders)]
        public ActionResult Sliders()
        {
            var sliders = _sliderService.GetSliders();
            return PartialView(sliders);
        }

        [AuthorizeResources(RoCmsResources.Sliders)]
        public ActionResult ConfigureSlider(int id)
        {
            Slider slider = _sliderService.GetSlider(id);
            return PartialView("_SliderEditor", slider);
        }
        
        [HttpPost]
        [AuthorizeResources(RoCmsResources.Sliders)]
        public JsonResult CreateSlider(string name)
        {
            var id = _sliderService.CreateSlider(name);
            return Json(new ResultModel(true, id));
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Sliders)]
        public JsonResult RemoveSlider(int id)
        {
            _sliderService.RemoveSlider(id);
            return Json(ResultModel.Success);
        }

        [AuthorizeResources(RoCmsResources.Sliders)]
        public ActionResult CreateSlide(int id)
        {
            var slide = new Slide() {SliderId = id};
            return PartialView(slide);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Sliders)]
        public JsonResult CreateSlide(Slide slide)
        {
            _sliderService.CreateSlide(slide);
            return Json(ResultModel.Success);
        }

        [AuthorizeResources(RoCmsResources.Sliders)]
        public ActionResult EditSlide(int id)
        {
            Slide slide = _sliderService.GetSlide(id);
            return PartialView("CreateSlide", slide);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Sliders)]
        public JsonResult EditSlide(Slide slide)
        {
            _sliderService.EditSlide(slide);
            return Json(ResultModel.Success);
        }

        #endregion

        #region Albums


        [AuthorizeResources(RoCmsResources.Gallery)]
        public ActionResult Gallery()
        {
            var imageInfos = _imageService.GetAllImageInfos();
            return View(imageInfos);
        }

        [AuthorizeResources(RoCmsResources.Albums)]
        public ActionResult Albums()
        {
            var albums = _albumService.GetAlbums();
            return PartialView(albums);
        }

        [AuthorizeResources(RoCmsResources.Albums)]
        public JsonResult AlbumList()
        {
            var albums = from album in _albumService.GetAlbums()
                         select new IdNamePair<int>() {ID = album.AlbumId, Name = album.Name};
            return Json(albums, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeResources(RoCmsResources.Albums)]
        public ActionResult CreateAlbum()
        {
            return PartialView();
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.Albums)]
        public void CreateAlbum(string title)
        {
            _albumService.CreateAlbum(title, null);
        }
        [AuthorizeResources(RoCmsResources.Albums)]
        public ActionResult ConfigureAlbum(int id)
        {
            ViewBag.AlbumId = id;
            ViewBag.Album = _albumService.GetAlbum(id);
            var model = _albumService.GetAlbumImages(id);
            return PartialView("_AlbumEditor", model);
        }

        //[HttpPost]
        //[AuthorizeResources(RoCmsResources.Albums)]
        //public void AddImageToAlbum(int albumId, string imageId)
        //{
        //    _albumService.AddImageToAlbum(albumId, imageId);
        //}

        //[HttpPost]
        //[AuthorizeResources(RoCmsResources.Albums)]
        //public void RemoveImageFromAlbum(int albumId, string imageId)
        //{
        //    _albumService.RemoveImageFromAlbum(albumId, imageId);
        //}

        #endregion

        #region Analytics

        [AuthorizeResources(RoCmsResources.Analytics)]
        public ActionResult Analytics()
        {
            return View();
        }

        public ActionResult SelectAnalyticsRange()
        {
            return View("_AnalyticsRangeSelect");
        }

        #endregion

        #region Pick Dialogs

        public ActionResult PickBlock()
        {
            var blockIdNames = _blockService.GetBlockIdNames();
            return View("_PickBlock", blockIdNames);
        }

        public ActionResult PickFile()
        {
            List<FileInfo> fileInfos = _fileService.GetFiles();
            return View("_PickFile", fileInfos);
        }

        public ActionResult PickAlbum()
        {
            var albums = _albumService.GetAlbums();
            return View("_PickAlbum", albums);
        }

            #endregion

        #region Videos

        [AuthorizeResources(RoCmsResources.VideoGallery)]
        public ActionResult VideoGallery()
        {
            var albums = _videoGalleryService.GetVideoAlbums();
            return View(albums);
        }

        [AuthorizeResources(RoCmsResources.VideoGallery)]
        public ActionResult ConfigureVideoAlbum(int id)
        {
            ViewBag.AlbumId = id;
            ViewBag.AlbumName = _videoGalleryService.GetVideoAlbumTitle(id);
            var model = _videoGalleryService.GetAlbumVideos(id);
            return PartialView("_VideoAlbumEditor", model);
        }

        #endregion
        
        #region Reviews
        public ActionResult GetReviews(int page = 1, int pgsize = 20)
        {
            int startIndex = (page - 1) * pgsize + 1;
            int totalReviews;
            var reviews = _reviewService.GetReviewPage(startIndex, pgsize, out totalReviews);
            ViewBag.TotalCount = totalReviews;
            ViewBag.Page = page;
            ViewBag.PageSize = pgsize;
            return PartialView(reviews);
        }

        [AuthorizeResources(RoCmsResources.Reviews)]
        [HttpGet]
        public ActionResult EditReview(int id)
        {
            var review = _reviewService.GetReview(id);
            ViewBag.Action = "EditReview";
            return PartialView("_ReviewEditor", review);
        }

        [AuthorizeResources(RoCmsResources.Reviews)]
        [HttpPost]
        public ActionResult EditReview(Review review)
        {
            _reviewService.UpdateReview(review);
            return Json(ResultModel.Success);
        }

        [AuthorizeResources(RoCmsResources.Reviews)]
        [HttpPost]
        public void AcceptReview(int reviewId)
        {
            _reviewService.ModerateReview(reviewId, true);
        }

        [AuthorizeResources(RoCmsResources.Reviews)]
        [HttpPost]
        public void HideReview(int reviewId)
        {
            _reviewService.ModerateReview(reviewId, false);
        }

        [AuthorizeResources(RoCmsResources.Reviews)]
        [HttpPost]
        public void RemoveReview(int id)
        {
            _reviewService.DeleteReview(id);
        }

        [AuthorizeResources(RoCmsResources.Reviews)]
        public ActionResult ShowReview(Review review)
        {
            return PartialView("GetReview", review);
        }

        [AuthorizeResources(RoCmsResources.Reviews)]
        public ActionResult CreateReview()
        {
            ViewBag.Action = "CreateReview";
            return PartialView("_ReviewEditor");
        }

        [AuthorizeResources(RoCmsResources.Reviews)]
        public ActionResult AddReview(Review review)
        {
            _reviewService.CreateReview(review);
            return Json(ResultModel.Success);
        }
        #endregion

        [AuthorizeResourcesApi(RoCmsResources.RedirectToPageRoutes)]
        public ActionResult RedirectToPageRoutes()
        {
            return View("RedirectToPageRoutes");
        }

        #region OrderForms

        [AuthorizeResourcesApi(RoCmsResources.CommonSettings)]
        public ActionResult OrderForms()
        {
            var forms = _orderFormService.GetOrderForms();
            return View(forms);
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.CommonSettings)]
        public ActionResult CreateOrderForm()
        {
            ViewBag.Action = "Create";
            return PartialView("_OrderFormEditor");
        }

        [HttpGet]
        [AuthorizeResources(RoCmsResources.CommonSettings)]
        public ActionResult EditOrderForm(int id)
        {
            ViewBag.Action = "Edit";
            var form = _orderFormService.GetOrderForm(id);
            return PartialView("_OrderFormEditor", form);
        }

        #endregion


    }
}