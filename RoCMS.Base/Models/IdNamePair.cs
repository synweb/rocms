using System.Runtime.Serialization;

namespace RoCMS.Base.Models
{
    //Атрибут нужен для правильной сериализации в api
    [DataContract]
    public class IdNamePair<T>
    {
        [DataMember]
        public T ID { get; set; }
        [DataMember]
        public string Name { get; set; }

        public IdNamePair(T id, string name)
        {
            ID = id;
            Name = name;
        }

        public IdNamePair()
        {
            
        }
    }
}
