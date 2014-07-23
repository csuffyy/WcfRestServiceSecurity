using System.Runtime.Serialization;

namespace WcfRestServiceSecurity
{
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}