using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Controllers
{
    [AllowAnonymous]
    public class InstallController : Controller
    {
        private readonly IInstallService _installService;

        public InstallController(IInstallService installService)
        {
            _installService = installService;
        }

        public ActionResult Index()
        {
            if (MvcApplication.IsInstalled)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult Install(InstallSettings data)
        {
            if (MvcApplication.IsInstalled)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (string.IsNullOrEmpty(data.DbDataSource)
                || string.IsNullOrEmpty(data.DbDatabase)
                || string.IsNullOrEmpty(data.DbLogin)
                || string.IsNullOrEmpty(data.DbPassword)
                || string.IsNullOrEmpty(data.AdminLogin)
                || string.IsNullOrEmpty(data.AdminPassword)
                || string.IsNullOrEmpty(data.RootUrl))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            data.RootUrl = data.RootUrl.Trim('/', '\\', ' ', '\t');
            data.AdminPassword = data.AdminPassword.Trim();
            data.AdminLogin = data.AdminLogin.Trim();
            data.DbPassword = data.DbPassword.Trim();
            data.DbLogin = data.DbLogin.Trim();
            data.DbDatabase = data.DbDatabase.Trim();
            data.DbDataSource = data.DbDataSource.Trim();

            var baseConectionString = GetConnectionString(data.DbDataSource, data.DbDatabase, data.DbLogin, data.DbPassword);
            var dbCheck = _installService.CheckDatabase(baseConectionString);
            if(!dbCheck)
                return new HttpUnauthorizedResult("db");


            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstallData",
                "Connections.mssql.template");
            var template = System.IO.File.ReadAllText(templatePath);
            var config = template.Replace("%host%", data.DbDataSource)
                .Replace("%password%", data.DbPassword)
                .Replace("%username%", data.DbLogin)
                .Replace("%db%", data.DbDatabase);

            var connectionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Connections.config");
            System.IO.File.WriteAllText(connectionsPath, config, Encoding.UTF8);

            var sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstallData",
                "RoCMS.Database_Create.sql");
            _installService.Install(sqlPath, baseConectionString, data.AdminLogin, data.AdminPassword, data.RootUrl);
            try
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            finally
            {
                System.Web.HttpRuntime.UnloadAppDomain();
            }
        }

        private string GetConnectionString(string dbDataSource, string dbDatabase, string dbLogin, string dbPassword)
        {
            const string CONNECTION_STRING_TEMPLATE = "data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True;application name=EntityFramework";
            var connectionString = string.Format(CONNECTION_STRING_TEMPLATE, dbDataSource, dbDatabase, dbLogin, dbPassword);
            return connectionString;
        }

        public class InstallSettings
        {
            public string DbDataSource { get; set; }
            public string DbDatabase { get; set; }
            public string DbLogin { get; set; }
            public string DbPassword { get; set; }
            public string AdminLogin { get; set; }
            public string AdminPassword { get; set; }
            public string RootUrl { get; set; }
        }
    }
}