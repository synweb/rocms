using System.Configuration;

namespace RoCMS.Base.ForWeb
{
    public class RedirectToPageRoutesConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("pageRoutes")]
        [ConfigurationCollection(typeof(RedirectPageRoutes), AddItemName = "add")]
        public RedirectPageRoutes RedirectPageRoutes
        {
            get
            {
                return base["pageRoutes"] as RedirectPageRoutes;
            }
        }
    }

    public class RedirectPageRoutes : ConfigurationElementCollection
    {

        public RedirectPageRoutes()
        {
            //EmbeddableView element = (EmbeddableView)CreateNewElement();
            //BaseAdd(element); // doesn't work here does if i remove it
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RedirectPageRoute();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RedirectPageRoute)element).Key;
        }

        public void Add(RedirectPageRoute element)
        {
            BaseAdd(element);
        }
        public void Clear()
        {
            BaseClear();
        }

        public RedirectPageRoute this[int index]
        {
            get
            {
                return (RedirectPageRoute)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

    }

    public class RedirectPageRoute : ConfigurationElement
    {
        public RedirectPageRoute() { }

        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get
            {
                return (string)base["key"];
            }
            set
            {
                base["key"] = value;
            }
        }

        [ConfigurationProperty("value", IsKey = false, IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)base["value"];
            }
            set
            {
                base["value"] = value;
            }
        }
    }
}
