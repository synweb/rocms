using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;
using JetBrains.Annotations;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Helpers;
using RoCMS.Helpers.Formatters;
using UnityDependencyResolver = Unity.WebApi.UnityDependencyResolver;

namespace RoCMS.App_Start
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration config, IUnityContainer unityContainer)
        {
            config.DependencyResolver = new UnityDependencyResolver(unityContainer);
            var index = config.Formatters.IndexOf(config.Formatters.JsonFormatter);
            config.Formatters[index] = new JsonCamelCaseFormatter();

        }

        public static void Register(HttpConfiguration config)
        {
            WebApiConfigHelper.ApiRoute(config.Routes, "user/current/info/get", "UserApi", "GetCurrentUserInfo");
            WebApiConfigHelper.ApiRoute(config.Routes, "user/password/createTicket/{email}", "UserApi", "CreateTicket");
            WebApiConfigHelper.ApiRoute(config.Routes, "user/password/set", "UserApi", "SetPassword");
            WebApiConfigHelper.ApiRoute(config.Routes, "users/get", "UserApi", "GetUsers");
            WebApiConfigHelper.ApiRoute(config.Routes, "user/{name}/delete", "UserApi", "DeleteUser");
            WebApiConfigHelper.ApiRoute(config.Routes, "user/resources/update", "UserApi", "UpdateResources");
            WebApiConfigHelper.ApiRoute(config.Routes, "user/profile/update", "UserApi", "UpdateProfile");

            WebApiConfigHelper.ApiRoute(config.Routes, "menu/{id}/get", "MenuApi", "Get");
            WebApiConfigHelper.ApiRoute(config.Routes, "menu/{id}/delete", "MenuApi", "Delete");
            WebApiConfigHelper.ApiRoute(config.Routes, "menu/create", "MenuApi", "Create");
            WebApiConfigHelper.ApiRoute(config.Routes, "menu/update", "MenuApi", "Update");
            
            WebApiConfigHelper.ApiRoute(config.Routes, "review/{id}/delete", "ReviewApi", "Delete");
            WebApiConfigHelper.ApiRoute(config.Routes, "review/create", "ReviewApi", "Create");
            WebApiConfigHelper.ApiRoute(config.Routes, "review/createbyadmin", "ReviewApi", "CreateByAdmin");
            WebApiConfigHelper.ApiRoute(config.Routes, "review/update", "ReviewApi", "Update");
            WebApiConfigHelper.ApiRoute(config.Routes, "review/{id}/accept", "ReviewApi", "Accept");
            WebApiConfigHelper.ApiRoute(config.Routes, "review/{id}/hide", "ReviewApi", "Hide");

            WebApiConfigHelper.ApiRoute(config.Routes, "block/{id}/delete", "BlockApi", "Delete");
            WebApiConfigHelper.ApiRoute(config.Routes, "block/blocks/get", "BlockApi", "Blocks");

            WebApiConfigHelper.ApiRoute(config.Routes, "page/pages/get", "PageApi", "Pages");
            WebApiConfigHelper.ApiRoute(config.Routes, "page/{id}/copy", "PageApi", "CopyPage");

            WebApiConfigHelper.ApiRoute(config.Routes, "slider/sliders/get", "SliderApi", "GetSliders");
            WebApiConfigHelper.ApiRoute(config.Routes, "slider/{id}/get", "SliderApi", "GetSlider");
            WebApiConfigHelper.ApiRoute(config.Routes, "slider/{id}/delete", "SliderApi", "RemoveSlider");
            WebApiConfigHelper.ApiRoute(config.Routes, "slider/create", "SliderApi", "CreateSlider");

            WebApiConfigHelper.ApiRoute(config.Routes, "slide/slides/{sliderId}/get", "SliderApi", "GetSlides");
            WebApiConfigHelper.ApiRoute(config.Routes, "slide/{id}/get", "SliderApi", "GetSlide");
            WebApiConfigHelper.ApiRoute(config.Routes, "slide/{id}/update", "SliderApi", "EditSlide");
            WebApiConfigHelper.ApiRoute(config.Routes, "slide/{id}/delete", "SliderApi", "RemoveSlide");
            WebApiConfigHelper.ApiRoute(config.Routes, "slide/create", "SliderApi", "CreateSlide");
            
            WebApiConfigHelper.ApiRoute(config.Routes, "message/send/order", "MessageApi", "Order");
            WebApiConfigHelper.ApiRoute(config.Routes, "message/send/callback", "MessageApi", "Callback");
            
            WebApiConfigHelper.ApiRoute(config.Routes, "image/remove/{id}", "ImageApi", "RemoveImage");

            WebApiConfigHelper.ApiRoute(config.Routes, "album/create", "AlbumApi", "CreateAlbum");
            WebApiConfigHelper.ApiRoute(config.Routes, "album/{albumId}/delete", "AlbumApi", "DeleteAlbum");
            WebApiConfigHelper.ApiRoute(config.Routes, "album/{albumId}/sort/update", "AlbumApi", "UpdateSortOrder");
            WebApiConfigHelper.ApiRoute(config.Routes, "album/{albumId}/image/{imageId}/title/update", "AlbumApi", "UpdateImageTitle");
            WebApiConfigHelper.ApiRoute(config.Routes, "album/{albumId}/image/{imageId}/description/update", "AlbumApi", "UpdateImageDescription");
            WebApiConfigHelper.ApiRoute(config.Routes, "album/{albumId}/image/{imageId}/destinationUrl/update", "AlbumApi", "UpdateImageDestinationUrl");
            WebApiConfigHelper.ApiRoute(config.Routes, "album/{albumId}/{imageId}/add", "AlbumApi", "AddImageToAlbum");
            WebApiConfigHelper.ApiRoute(config.Routes, "album/{albumId}/images/remove/{id}", "AlbumApi", "RemoveImageFromAlbum");
            WebApiConfigHelper.ApiRoute(config.Routes, "album/update", "AlbumApi", "UpdateAlbum");

            WebApiConfigHelper.ApiRoute(config.Routes, "useralbum/getid", "UserAlbumApi", "GetUserAlbumId");
            WebApiConfigHelper.ApiRoute(config.Routes, "useralbum/{imageId}/add", "UserAlbumApi", "AddImageToUserAlbum");

            WebApiConfigHelper.ApiRoute(config.Routes, "video/album/create", "VideoAlbumApi", "CreateVideoAlbum");
            WebApiConfigHelper.ApiRoute(config.Routes, "video/album/{albumId}/{videoId}/add", "VideoAlbumApi", "AddVideoToAlbum");
            WebApiConfigHelper.ApiRoute(config.Routes, "video/album/{albumId}/delete", "VideoAlbumApi", "DeleteVideoAlbum");
            WebApiConfigHelper.ApiRoute(config.Routes, "video/album/{albumId}/sort/update", "VideoAlbumApi", "UpdateSortOrder");
            WebApiConfigHelper.ApiRoute(config.Routes, "video/{videoId}/title/update", "VideoAlbumApi", "UpdateVideoTitle");
            WebApiConfigHelper.ApiRoute(config.Routes, "video/{videoId}/description/update", "VideoAlbumApi", "UpdateVideoDescription");
            WebApiConfigHelper.ApiRoute(config.Routes, "video/{videoId}/delete", "VideoAlbumApi", "DeleteVideo");
            WebApiConfigHelper.ApiRoute(config.Routes, "video/album/{albumId}/title/{title}/update", "VideoAlbumApi", "UpdateAlbumTitle");

            WebApiConfigHelper.ApiRoute(config.Routes, "analytics/traffic/summary", "AnalyticsApi", "GetTrafficSummary");
            WebApiConfigHelper.ApiRoute(config.Routes, "analytics/traffic/summary/default", "AnalyticsApi", "GetDefaultTrafficSummary");
            WebApiConfigHelper.ApiRoute(config.Routes, "analytics/sources/summary", "AnalyticsApi", "GetSourcesSummary");
            WebApiConfigHelper.ApiRoute(config.Routes, "analytics/sources/summary/default", "AnalyticsApi", "GetDefaultSourcesSummary");
            
            WebApiConfigHelper.ApiRoute(config.Routes, "settings/yandex/auth", "SettingsApi", "RequestYandexOAuth");

            WebApiConfigHelper.ApiRoute(config.Routes, "temp/file/{id}/remove", "TempFileApi", "RemoveFile");

            WebApiConfigHelper.ApiRoute(config.Routes, "email/template/{name}/update", "EmailTemplateApi", "UpdateTemplate");

            WebApiConfigHelper.ApiRoute(config.Routes, "email/message/{mailId}/resend", "MailApi", "ReSendMail");
            WebApiConfigHelper.ApiRoute(config.Routes, "email/message/{mailId}/delete", "MailApi", "DeleteMail");

            WebApiConfigHelper.ApiRoute(config.Routes, "interface/strings/get", "InterfaceStringApi", "Get");
            WebApiConfigHelper.ApiRoute(config.Routes, "interface/strings/save", "InterfaceStringApi", "Save");
            WebApiConfigHelper.ApiRoute(config.Routes, "interface/strings/create", "InterfaceStringApi", "Create");

            WebApiConfigHelper.ApiRoute(config.Routes, "dev/db/check", "DeveloperApi", "CheckDB");
            WebApiConfigHelper.ApiRoute(config.Routes, "dev/db/update", "DeveloperApi", "UpdateDBConnections");
            WebApiConfigHelper.ApiRoute(config.Routes, "dev/widgets/get", "DeveloperApi", "GetWidgets");
            WebApiConfigHelper.ApiRoute(config.Routes, "dev/widgets/save", "DeveloperApi", "SaveWidgets");
            WebApiConfigHelper.ApiRoute(config.Routes, "dev/robots/save", "DeveloperApi", "SaveRobots");
            WebApiConfigHelper.ApiRoute(config.Routes, "dev/enter", "DeveloperApi", "EnterDevMode");

            WebApiConfigHelper.ApiRoute(config.Routes, "admin/redirect/get", "RedirectToPageRoutesApi", "GetRedirectToPageRouters");
            WebApiConfigHelper.ApiRoute(config.Routes, "admin/redirect/save", "RedirectToPageRoutesApi", "SaveRedirectToPageRouters");

            WebApiConfigHelper.ApiRoute(config.Routes, "orderform/{formId}/get", "OrderFormApi", "GetForm");
            WebApiConfigHelper.ApiRoute(config.Routes, "orderform/update", "OrderFormApi", "UpdateForm");
            WebApiConfigHelper.ApiRoute(config.Routes, "orderform/{formId}/delete", "OrderFormApi", "DeleteForm");
            WebApiConfigHelper.ApiRoute(config.Routes, "orderform/create", "OrderFormApi", "CreateForm");

        }
    }
}
