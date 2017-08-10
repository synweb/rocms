using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace RoCMS.Helpers
{
    //TODO: копипаста. въехать
    public class UnityDependencyResolver : IDependencyResolver
    {
        const string HttpContextKey = "perRequestContainer";

        readonly IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        #region IDependencyResolver Members

        public object GetService(Type serviceType)
        {
            return IsRegistered(serviceType) ? GetContainer().Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IUnityContainer container = GetContainer();
            if (IsRegistered(serviceType))
            {
                yield return container.Resolve(serviceType);
            }
            foreach (object service in container.ResolveAll(serviceType))
            {
                yield return service;
            }
        }

        #endregion

        bool IsRegistered(Type type)
        {
            // Unity always can use Activator.CreateInstance
            if (!type.IsInterface && !type.IsAbstract)
            {
                return true;
            }

            IUnityContainer container = GetContainer();
            return container.IsRegistered(type)
                || (type.IsGenericType && container.IsRegistered(type.GetGenericTypeDefinition()));
        }

        IUnityContainer GetContainer()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                var childContainer = httpContext.Items[HttpContextKey] as IUnityContainer;
                if (childContainer == null)
                {
                    httpContext.Items[HttpContextKey] = childContainer = _container.CreateChildContainer();
                }
                return childContainer;
            }
            return _container;
        }

        public static void DisposeOfChildContainer()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                var childContainer = httpContext.Items[HttpContextKey] as IUnityContainer;
                if (childContainer != null)
                {
                    childContainer.Dispose();
                }
            }
        }
    }
}