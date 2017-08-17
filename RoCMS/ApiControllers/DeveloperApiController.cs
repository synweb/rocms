using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.Web.Contract;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    public class DeveloperApiController : ApiController
    {
        private readonly IDevelopmentService _developmentService;
        private readonly ISecurityService _securityService;
        private readonly ILogService _logService;

        public DeveloperApiController(IDevelopmentService developmentService, ISecurityService securityService, ILogService logService)
        {
            _developmentService = developmentService;
            _securityService = securityService;
            _logService = logService;
        }

        private const string CONNECTION_STRING_TEMPLATE = "data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True;application name=EntityFramework";

        [AuthorizeResourcesApi(RoCmsResources.Development, RoCmsResources.Dev_Database)]
        [HttpPost]
        public ResultModel CheckDB(DatabaseSettings data)
        {
            var newConnectionString = string.Format(CONNECTION_STRING_TEMPLATE, data.DbDataSource, data.DbDatabase, data.DbLogin, data.DbPassword);
            var succeed = _developmentService.CheckDatabaseCredentials(newConnectionString);
            return new ResultModel(succeed);
        }

        [AuthorizeResourcesApi(RoCmsResources.Development, RoCmsResources.Dev_Database)]
        [HttpPost]
        public ResultModel UpdateDBConnections(DatabaseSettings data)
        {
            try
            {
                var newConnectionString = string.Format(CONNECTION_STRING_TEMPLATE, data.DbDataSource, data.DbDatabase, data.DbLogin, data.DbPassword);
                var webConfiguration = WebConfigurationManager.OpenWebConfiguration("~");
                foreach (ConnectionStringSettings connectionString in webConfiguration.ConnectionStrings.ConnectionStrings)
                {
                    if (connectionString.Name.Equals("LocalSqlServer"))
                        continue;
                    if (connectionString.ConnectionString.Contains("provider connection string"))
                    {
                        // строка соединения, созданная EF
                        var parts = connectionString.ConnectionString.Split(new[] { "\"" }, StringSplitOptions.None);
                        parts[1] = newConnectionString;
                        connectionString.ConnectionString = string.Join("\"", parts);
                    }
                    else
                    {
                        // обычная строка соединения
                        connectionString.ConnectionString = newConnectionString;
                    }
                }
                webConfiguration.Save();
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Development, RoCmsResources.Dev_Widgets)]
        [HttpGet]
        public ICollection<Widget> GetWidgets()
        {
            var widgets = new List<Widget>();
            PageRenderHelperConfigurationSection widgetSection = (PageRenderHelperConfigurationSection)ConfigurationManager.GetSection("pageRenderHelper");
            foreach (EmbeddableView rec in widgetSection.EmbeddableViews)
            {
                if(string.IsNullOrEmpty(rec.Pattern))
                    continue;
                widgets.Add(new Widget(rec.Pattern, rec.Path));
            }
            return widgets;
        }
        
        [AuthorizeResourcesApi(RoCmsResources.Development, RoCmsResources.Dev_Widgets)]
        public ResultModel SaveWidgets(ICollection<Widget> widgets)
        {
            try
            {
                if(widgets==null)
                    return new ResultModel(false, "Null collection");
                var webConfiguration = WebConfigurationManager.OpenWebConfiguration("~");
                PageRenderHelperConfigurationSection widgetSection = (PageRenderHelperConfigurationSection)webConfiguration.GetSection("pageRenderHelper");
                widgetSection.EmbeddableViews.Clear();
                foreach (var widget in widgets.OrderBy(x => x.Pattern))
                {
                    widgetSection.EmbeddableViews.Add(new EmbeddableView()
                    {
                        Pattern = widget.Pattern,
                        Path = widget.ViewPath
                    });
                }
                webConfiguration.Save();
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Development, RoCmsResources.Dev_CodeEditor)]
        public ResultModel SaveRobots(SaveRobotsData data)
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "robots.txt");
                using (var stream = new StreamWriter(path, false, Encoding.UTF8))
                {
                    stream.Write(data.Text);
                    return ResultModel.Success;
                }
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        public class SaveRobotsData
        {
            public string Text { get; set; }
        }

        [AuthorizeResourcesApi(RoCmsResources.Dev_MagicButton)]
        public ResultModel EnterDevMode()
        {
            try
            {
                var userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                _securityService.ForbidResource(userId, RoCmsResources.Dev_MagicButton);
                _securityService.GrantResource(userId, RoCmsResources.Development);
                _securityService.GrantResource(userId, RoCmsResources.Dev_CodeEditor);
                _securityService.GrantResource(userId, RoCmsResources.Dev_Database);
                _securityService.GrantResource(userId, RoCmsResources.Dev_InterfaceStrings);
                _securityService.GrantResource(userId, RoCmsResources.Dev_Widgets);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
    }
}