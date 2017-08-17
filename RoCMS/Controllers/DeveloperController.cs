using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Controllers
{

    [AuthorizeResources(RoCmsResources.Development)]
    public class DeveloperController : Controller
    {
        private readonly IInterfaceStringService _interfaceStringService;

        public DeveloperController(IInterfaceStringService interfaceStringService)
        {
            _interfaceStringService = interfaceStringService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Database");
        }

        [AuthorizeResources(RoCmsResources.Dev_CodeEditor)]
        public ActionResult RobotsTxt()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "robots.txt");
            var text = System.IO.File.ReadAllText(path);
            return View((object)text); // иначе он воспринимает строку как название вьюхи
        }

        [AuthorizeResources(RoCmsResources.Dev_Database)]
        public ActionResult Database()
        {
            var webConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            var connectionString = webConfiguration.ConnectionStrings.ConnectionStrings["BasicConnection"].ConnectionString;
            var model = new DatabaseSettings
            {
                DbDataSource = Regex.Match(connectionString, @"data source=(.+?);", RegexOptions.IgnoreCase).Groups[1].Value,
                DbDatabase = Regex.Match(connectionString, @"initial catalog=(.+?);", RegexOptions.IgnoreCase).Groups[1].Value,
                DbLogin = Regex.Match(connectionString, @"user id=(.+?);", RegexOptions.IgnoreCase).Groups[1].Value,
                DbPassword = Regex.Match(connectionString, @"password=(.+?);", RegexOptions.IgnoreCase).Groups[1].Value
            };
            return View(model);
        }

        [AuthorizeResources(RoCmsResources.Dev_Widgets)]
        public ActionResult Widgets()
        {
            return View();
        }


        [AuthorizeResources(RoCmsResources.Dev_CodeEditor)]
        public ActionResult CodeEditor()
        {
            var dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views"));
            List<FileInfo> views = dir.GetFiles("*.cshtml", SearchOption.AllDirectories).ToList();

            dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin/Views"));
            views.AddRange(dir.GetFiles("*.cshtml", SearchOption.AllDirectories));
            views = views.Except(views.Where(x =>
                !string.IsNullOrEmpty(x.DirectoryName)
                && (x.DirectoryName.Contains("Views\\Admin")
                    || x.DirectoryName.Contains("Views\\Developer")
                    || x.DirectoryName.Contains("Views\\Editor")
                    || x.DirectoryName.Contains("Views\\Install")
                    )
                )).ToList();

            ViewBag.Views = views;

            dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content"));
            var scripts = dir.GetFiles("*.js", SearchOption.AllDirectories).ToList();
            var styles = dir.GetFiles("*.css", SearchOption.AllDirectories).ToList();
            dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin/Content"));
            scripts.AddRange(dir.GetFiles("*.js", SearchOption.AllDirectories));
            styles.AddRange(dir.GetFiles("*.css", SearchOption.AllDirectories));

            scripts = scripts.Except(scripts.Where(x =>
                !string.IsNullOrEmpty(x.DirectoryName)
                && (x.DirectoryName.Contains("admin")
                    || x.DirectoryName.Contains("adminTemplate")
                    || (x.DirectoryName.Contains("vendor") && !x.DirectoryName.Contains("theme"))
                    || x.DirectoryName.Contains("cassette")
                    )
                )).ToList();
            ViewBag.Scripts = scripts;
            styles = styles.Except(styles.Where(x =>
                !string.IsNullOrEmpty(x.DirectoryName)
                && (x.DirectoryName.Contains("admin")
                    || x.DirectoryName.Contains("adminTemplate")
                    || (x.DirectoryName.Contains("vendor") && !x.DirectoryName.Contains("theme"))
                    || x.DirectoryName.Contains("cassette")
                    )
                )).ToList();
            ViewBag.Styles = styles;

            return View();
        }

        [AuthorizeResources(RoCmsResources.Dev_CodeEditor)]
        [HttpGet]

        public ActionResult ViewContent(string path)
        {
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var filePath = Path.Combine(dir.FullName, path.TrimStart('\\'));
            if (System.IO.File.Exists(filePath))
            {
                using (var stream = new StreamReader(filePath))
                {
                    string content = stream.ReadToEnd();
                    return Json(new { content = content }, JsonRequestBehavior.AllowGet);

                }
            }

            return new EmptyResult();
        }

        [AuthorizeResources(RoCmsResources.Dev_CodeEditor)]
        public JsonResult SaveView(string path, string content)
        {
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var filePath = Path.Combine(dir.FullName, path.TrimStart('\\'));
            if (System.IO.File.Exists(filePath))
            {
                using (var stream = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    stream.Write(content);
                    return Json(ResultModel.Success);
                }
            }
            return Json(ResultModel.Error);
        }

        [AuthorizeResources(RoCmsResources.Dev_InterfaceStrings)]
        public ActionResult InterfaceStrings()
        {
            return View();
        }

        [AuthorizeResources(RoCmsResources.Dev_InterfaceStrings)]
        public ActionResult PickInterfaceString()
        {
            var strings = _interfaceStringService.GetStrings();
            return PartialView("_PickInterfaceString", strings);
        }

        [AuthorizeResources(RoCmsResources.Dev_InterfaceStrings)]
        public ActionResult CreateInterfaceString()
        {
            return PartialView("_CreateInterfaceString");
        }

        [AuthorizeResources(RoCmsResources.Development)]
        public ActionResult ViewLogs()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            int dirPatchLenght = path.Length + 1;

            var files = new List<FileInfo>(new DirectoryInfo(path).GetFiles("*.log", SearchOption.AllDirectories))
                                        .OrderByDescending(x => x.Name)
                                        .Select(x => x.FullName.Substring(dirPatchLenght));
            return View("ViewLogs", files);
        }

        [AuthorizeResources(RoCmsResources.Dev_MagicButton)]
        public ActionResult MagicButton()
        {
            return View();
        }
    }
}