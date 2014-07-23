using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace WcfRestServiceSecurity
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


        [WebGet(UriTemplate = "Image/{image}")]
        public Stream GetImage(string image)
        {
            var imageType = Path.GetExtension(image).TrimStart('.');
            WebOperationContext.Current.OutgoingResponse.ContentType = "image/" + imageType;
            var dir = System.Web.HttpContext.Current.Server.MapPath("~/Images");
            var file = Path.Combine(dir, image);
            return File.OpenRead(file);
        }

        [WebInvoke(UriTemplate = "Add/{image}", Method = "POST")]
        public void AddImage(Stream stream, string image)
        {
            var dir = System.Web.HttpContext.Current.Server.MapPath("~/Images");
            var file = Path.Combine(dir, image);
            var bitmap = Image.FromStream(stream);
            bitmap.Save(file);
        }

        [WebInvoke(UriTemplate = "Delete/{image}", Method = "DELETE")]
        public void DeleteImage(Stream stream, string image)
        {
            var dir = System.Web.HttpContext.Current.Server.MapPath("~/Images");
            var file = Path.Combine(dir, image);

            File.Delete(file);
        }

        [WebGet(UriTemplate = "AllImage", ResponseFormat = WebMessageFormat.Xml)]
        public string[] GetAllImageNames()
        {
            var dir = System.Web.HttpContext.Current.Server.MapPath("~/Images");
            var files = Directory.GetFiles(dir);
            var images = new List<string>();
            foreach (string file in files)
            {
                string ext = Path.GetExtension(file);
                if (ext == ".jpg" || ext == ".gif")
                {
                    images.Add(file);
                }
            }
            for (int i = 0; i < images.Count; i++)
            {
                images[i] = Path.GetFileName(images[i]);
            }
            return images.ToArray();
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