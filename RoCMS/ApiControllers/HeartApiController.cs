using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    public class HeartApiController : ApiController
    {

        private readonly IHeartService _heartService;

        public HeartApiController(IHeartService heartService)
        {
            _heartService = heartService;
        }

        [System.Web.Http.HttpGet]
        public ICollection<Heart> GetHearts()
        {
            var hearts = _heartService.GetHearts();

            foreach (var heart in hearts)
            {
                var typeObj = GetTypeByName(heart.Type);

                var displayNameAttr = typeObj.GetCustomAttribute<DisplayNameAttribute>();

                var displayName = displayNameAttr != null ? displayNameAttr.DisplayName : heart.Type.Split('.').Last();
                heart.Type = displayName;
            }

            return hearts;

            //var types = hearts.Select(x => x.Type).Distinct();

            //ICollection<HeartGroup> groups = new List<HeartGroup>();

            //foreach (var type in types)
            //{
            //    var typeObj = GetTypeByName(type);

            //    var displayNameAttr = typeObj.GetCustomAttribute<DisplayNameAttribute>();

            //    var displayName = displayNameAttr != null ? displayNameAttr.DisplayName : type.Split('.').Last();

            //    var group = new HeartGroup() { Title = displayName };
            //    group.Hearts = hearts.Where(x => x.Type == type).ToList();

            //    groups.Add(group);

            //    //IsDefined(typeof(DisplayNameAttribute));
            //}

            //return groups;
        }

        [System.Web.Http.HttpGet]
        public ICollection<Heart> GetHeartsByType(string type)
        {
            var hearts = _heartService.GetHearts(type);
            foreach (var heart in hearts)
            {
                var typeObj = GetTypeByName(heart.Type);

                var displayNameAttr = typeObj.GetCustomAttribute<DisplayNameAttribute>();

                var displayName = displayNameAttr != null ? displayNameAttr.DisplayName : heart.Type.Split('.').Last();
                heart.Type = displayName;
            }

            return hearts;
            return hearts;
        }

        public class HeartGroup
        {
            public string Title { get; set; }
            public ICollection<Heart> Hearts { get; set; }
        }

        private Type GetTypeByName(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                var tt = assembly.GetType(name);
                if (tt != null)
                {
                    return tt;
                }
            }

            return null;
        }
    }
}
