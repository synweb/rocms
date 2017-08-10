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
using RoCMS.Web.Contract;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.Development)]
    public class DeveloperApiController : ApiController
    {
        private readonly IDevelopmentService _developmentService;

        public DeveloperApiController(IDevelopmentService developmentService)
        {
            _developmentService = developmentService;
        }

        private const string CONNECTION_STRING_TEMPLATE = "data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True;application name=EntityFramework";

        [AuthorizeResourcesApi(RoCmsResources.Dev_Database)]
        [System.Web.Mvc.HttpPost]
        public ResultModel CheckDB(DatabaseSettings data)
        {
            var newConnectionString = string.Format(CONNECTION_STRING_TEMPLATE, data.DbDataSource, data.DbDatabase, data.DbLogin, data.DbPassword);
            var succeed = _developmentService.CheckDatabaseCredentials(newConnectionString);
            return new ResultModel(succeed);
        }

        [AuthorizeResourcesApi(RoCmsResources.Dev_Database)]
        [System.Web.Mvc.HttpPost]
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
                return new ResultModel(e);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Dev_Widgets)]
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
        
        [AuthorizeResourcesApi(RoCmsResources.Dev_Widgets)]
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
                return new ResultModel(e);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Dev_CodeEditor)]
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
                return new ResultModel(e);
            }
        }

        public class SaveRobotsData
        {
            public string Text { get; set; }
        }
    }
}