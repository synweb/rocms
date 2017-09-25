using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using RoCMS.App_Start;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.Helpers;
using RoCMS.Base.Infrastructure;
using RoCMS.Base.UnityExtensions;
using RoCMS.Helpers;
using RoCMS.Web.Contract.Infrastructure;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;
using RoCMS.Web.Contract.Models.Security;
using RoCMS.Web.Contract.Services;
using WebApiConfig = RoCMS.App_Start.WebApiConfig;

namespace RoCMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // workaroud для малоизвестного бага дотнета, от которого хренеют все 13 человек, что с ним столкнулись
            // эти две строчки должны стоять в начале AppStart
            // иначе MemoryCache работать не будет
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            SanitizeThreadCulture();

            AreaRegistration.RegisterAllAreas();
            IUnityContainer container = new UnityContainer();
            RegisterControllers(container);

            var riExt = new ResolveInstancesExtension(typeof(CreateOnStartAttribute));

            container.AddExtension(riExt);
            container.LoadConfiguration();

            riExt.ResolveInstances();

            ViewEngines.Engines.Add(new RoViewEngine());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            WebApiConfig.Configure(GlobalConfiguration.Configuration, container);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);



            var uploadedFilesDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles");
            if (!Directory.Exists(uploadedFilesDirPath))
                Directory.CreateDirectory(uploadedFilesDirPath);


            IEnumerable<Type> interfaces = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name.Contains(".Contract"))
                .Where(x => x.GetName().Name.Contains("RoCMS"))
                .SelectMany(x => x.GetExportedTypes().Where(y => y.IsInterface));
            var services = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name.Contains(".Services"))
                .Where(x => x.GetName().Name.Contains("RoCMS"))
                .SelectMany(x => x.GetExportedTypes().Where(y => y.IsClass));
            foreach (var interf in interfaces)
            {
                if (container.IsRegistered(interf))
                {
                    continue;
                }

                var cls = services.Where(x => interf.IsAssignableFrom(x));
                if (cls.Count() == 1)
                {
                    var cl = cls.Single();
                    container.RegisterType(interf, cl, null, new ContainerControlledLifetimeManager());
                }
            }

            //var pluginTypes = from a in assemblies//                  from t in a.GetExportedTypes()
            //                  where typeof(IPlugin).IsAssignableFrom(t)
            //                  select t;

            //            foreach (var t in pluginTypes)
            //                container.RegisterType(typeof(IPlugin), t);

            RouteConfig.RegisterRedirects(RouteTable.Routes);
            ConfigureModules(BundleTable.Bundles, GlobalConfiguration.Configuration, GlobalFilters.Filters, RouteTable.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            RegisterSearch();

            var webConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            try
            {
                foreach(ConnectionStringSettings cstr in webConfiguration.ConnectionStrings.ConnectionStrings)
                {
                    // если есть BasicConnection, значит, RoCMS установлена
                    IsInstalled |= cstr.Name.Equals("BasicConnection");
                    if (IsInstalled)
                        break;
                }
            }
            catch (Exception e)
            {
                if (DemoMode)
                {
                    IsInstalled = true;
                }
                else
                {
                    IsInstalled = false;
                }
            }
        }

        public static bool IsInstalled { get; private set; }
        
        public static void SanitizeThreadCulture()
        {
            var currentCulture = CultureInfo.CurrentCulture;

            // at the top of any culture should be the invariant culture,
            // find it doing an .Equals comparison ensure that we will
            // find it and not loop endlessly
            var invariantCulture = currentCulture;
            while (invariantCulture.Equals(CultureInfo.InvariantCulture) == false)
                invariantCulture = invariantCulture.Parent;

            if (ReferenceEquals(invariantCulture, CultureInfo.InvariantCulture))
                return;

            var thread = Thread.CurrentThread;
            thread.CurrentCulture = CultureInfo.GetCultureInfo(thread.CurrentCulture.Name);
            thread.CurrentUICulture = CultureInfo.GetCultureInfo(thread.CurrentUICulture.Name);
        }

        private void RegisterSearch()
        {
            var searchService = DependencyResolver.Current.GetService<ISearchService>();
            searchService.RegisterRules(typeof (Page), new List<IndexingRule>()
            {
                x =>
                {
                    var item = (Page) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SeachKeyTitle,
                        EntityName = x.GetType().FullName,
                        EntityId = item.HeartId.ToString(),
                        SearchContent = SearchHelper.ToSearchIndexText(item.Title),
                        Title = item.Title,
                        Weight = 2,
                        Url = item.CanonicalUrl,
                        Text = item.MetaDescription ?? SearchHelper.ToSearchIndexText(TextCutHelper.Cut(SearchHelper.ToSearchIndexText(item.Content), 150)),
                    };
                },
                x =>
                {
                    var item = (Page) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SeachKeyContent,
                        EntityName = x.GetType().FullName,
                        EntityId = item.HeartId.ToString(),
                        SearchContent = SearchHelper.ToSearchIndexText(item.Content),
                        Title = item.Title,
                        Weight = 1,
                        Url = item.CanonicalUrl,
                        Text = item.MetaDescription ?? SearchHelper.ToSearchIndexText(TextCutHelper.Cut(SearchHelper.ToSearchIndexText(item.Content), 150)),
                    };
                }
            });
        }

        protected void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture =
                Thread.CurrentThread.CurrentUICulture =
                new CultureInfo("ru-RU");
        }

        protected void Application_PostAuthenticateRequest()
        {
            var httpContext = HttpContext.Current;
            if (User != null && User.Identity is FormsIdentity && User.Identity.IsAuthenticated)
            {
                int userId = GetUserId();
                if (userId == 0)
                {
                    FormsAuthentication.SignOut();
                    return;
                }
                httpContext.User = Thread.CurrentPrincipal = new RoPrincipal(userId, User.Identity);
            }

            if (ActionSessionHelper.SessionStateRequired(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath) || DemoMode)
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }

        }

        private bool DemoMode => AppSettingsHelper.RoCMSDemoMode;
        
        private int GetUserId()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName]; //.ASPXAUTH
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            //var pairs = authTicket.UserData.Split(',');
            //foreach (var pair in pairs)
            //{
            //    var kv = pair.Split(':');
            //}
            var kv = authTicket.UserData.Split(':');
            if (kv[0] == "id")
            {
                return int.Parse(kv[1]);
            }
            //throw new AuthenticationException();

            return 0;
        }


        private void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var httpContext = HttpContext.Current;
            //Айдишник корзины
            string cookieName = ConstantStrings.SessionId;
            HttpCookie requestCookie = httpContext.Request.Cookies[cookieName];
            Guid cartId = requestCookie != null ? Guid.Parse(requestCookie.Value) : Guid.NewGuid();
            var cookie = new System.Web.HttpCookie(cookieName, cartId.ToString());
            cookie.Expires = DateTime.UtcNow.AddHours(AppSettingsHelper.HoursToExpireCartCache);
            httpContext.Response.SetCookie(cookie);

            var sessionService = DependencyResolver.Current.GetService<ISessionValueProviderService>();
            sessionService.Set(ConstantStrings.CartId, cartId);
        }


        private static bool IsCartWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(String.Format("~/{0}/{1}", ConstantStrings.WebApiExecutionPath, "shop/cart"));
        }

        protected void Application_Error()
        {
            Exception error = Server.GetLastError();
            HttpStatusCode code;
            var httpError = error as HttpException;
            if (httpError != null)
            {
                code = (HttpStatusCode)httpError.GetHttpCode();
            }
            else
            {
                code = HttpStatusCode.InternalServerError;
            }

            Guid? errorID = null;

            if (code != HttpStatusCode.NotFound)
            {
                errorID = DependencyResolver.Current.GetService<ILogService>().LogError(error);
            }

            if (!HttpContext.Current.Request.IsLocal)
            {
                Response.Clear();
                Server.ClearError();
                Context.RewritePathToAction("Index", "Error", new { code = code, errorID = errorID });
            }
        }

        void RegisterControllers(IUnityContainer container)
        {
            IEnumerable<Type> controllerTypes = typeof(MvcApplication).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .Where(t => !t.IsAbstract);

            foreach (Type controllerType in controllerTypes)
            {
                container.RegisterType(controllerType);
            }
        }

        private void ConfigureModules(BundleCollection bundleCollection, HttpConfiguration httpConfiguration,
            GlobalFilterCollection globalFilterCollection, RouteCollection routeCollection)
        {



            List<Type> moduleInitializers = new List<Type>();


            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("RoCMS")))
            {
                foreach (Type t in a.GetTypes().Where(t => t.Name.EndsWith("ModuleInitializer")).Where(t => !t.IsAbstract))
                {
                    moduleInitializers.Add(t);
                }
            }

            foreach (Type moduleInitializerType in moduleInitializers)
            {
                IModuleInitializer instance = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);

                instance.Init();
            }

        }

    }

    

}