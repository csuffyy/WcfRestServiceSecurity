using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Net;

namespace WcfRestServiceSecurity1
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TaskService
    {
        /// <summary>
        /// 检查认证是否通过（可以省略了！）
        /// </summary>
        /// <returns></returns>
        private static bool CheckAuthorization()
        {
            var ctx = WebOperationContext.Current;
            var auth = ctx.IncomingRequest.Headers[HttpRequestHeader.Authorization];
            if (string.IsNullOrEmpty(auth) || auth != "fangxing123")
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
                return false;
            }
            return true;
        }

        [WebGet(UriTemplate = "All")]
        public List<Task> GetTask()
        {
            //可以省略了
            //if (!CheckAuthorization())
            //    return null;
            return GetData();
        }

        [WebGet(UriTemplate = "{taskId}")]
        public Task GetTaskById(string taskId)
        {
            //可以省略了
            //if (!CheckAuthorization())
            //    return null;
            return GetData().FirstOrDefault(t => t.Id == taskId);
        }

        private List<Task> GetData()
        {
            return new List<Task>
                   {
                       new Task {Id = "1", Name = "Task1"},
                       new Task {Id = "2", Name = "Task2"},
                       new Task {Id = "3", Name = "Task3"},
                   };
        }
    }
}