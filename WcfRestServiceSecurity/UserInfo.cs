﻿using System.Runtime.Serialization;

namespace WcfRestServiceSecurity1
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