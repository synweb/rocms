using System.Configuration;

namespace RoCMS.Base.ForWeb
{
    public class PageRenderHelperConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("embeddableViews")]
        [ConfigurationCollection(typeof(EmbeddableViews), AddItemName = "add")]
        public EmbeddableViews EmbeddableViews
        {
            get
            {
                return base["embeddableViews"] as EmbeddableViews;
            }
        }
    }

    public class EmbeddableViews : ConfigurationElementCollection
    {

        public EmbeddableViews()
        {
            //EmbeddableView element = (EmbeddableView)CreateNewElement();
            //BaseAdd(element); // doesn't work here does if i remove it
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EmbeddableView();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmbeddableView)element).Pattern;
        }

        public void Add(EmbeddableView element)
        {
            BaseAdd(element);
        }
        public void Clear()
        {
            BaseClear();
        }

        public EmbeddableView this[int index]
        {
            get
            {
                return (EmbeddableView)BaseGet(index);
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

    public class EmbeddableView : ConfigurationElement
    {
        public EmbeddableView() { }

        [ConfigurationProperty("pattern", IsKey = true, IsRequired = true)]
        public string Pattern
        {
            get
            {
                return (string)base["pattern"];
            }
            set
            {
                base["pattern"] = value;
            }
        }

        [ConfigurationProperty("path", IsKey = false, IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)base["path"];
            }
            set
            {
                base["path"] = value;
            }
        }
    }
}
