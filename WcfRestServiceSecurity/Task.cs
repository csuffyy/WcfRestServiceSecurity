using System.Runtime.Serialization;

namespace WcfRestServiceSecurity
{
    [DataContract]
    public class Task
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}