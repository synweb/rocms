using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace RoCMS.Base.UnityExtensions
{
public class ResolveInstancesExtension : UnityContainerExtension
    {
        #region Fields

        readonly Type _attributeType;

        readonly HashSet<Type> _typesToResolve = new HashSet<Type>();

        #endregion

        #region Constructors

        public ResolveInstancesExtension(Type attributeType)
        {
            _attributeType = attributeType;
        }

        #endregion

        #region Methods

        public void ResolveInstances()
        {
            foreach (Type type in _typesToResolve)
            {
                Context.Container.Resolve(type);
            }
        }

		protected override void Initialize()
		{
			Context.Registering += new EventHandler<RegisterEventArgs>(Context_Registering);
		}

		void Context_Registering(object sender, RegisterEventArgs e)
		{
			if (e.TypeTo.GetCustomAttributes(_attributeType, true).Length > 0)
			{
				_typesToResolve.Add(e.TypeFrom);
			}
		}

        #endregion
    }
}

