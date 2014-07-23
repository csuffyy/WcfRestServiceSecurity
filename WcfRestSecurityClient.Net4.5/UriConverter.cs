using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WcfRestSecurityClient
{
    [ValueConversion(typeof(string), typeof(string))]
    public class UriConverter : IValueConverter
    {
        private readonly string wcfRestAddress = System.Configuration.ConfigurationManager.AppSettings["WcfRestAddress"];

        public object Convert(object value, Type targetType,
                              object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            string sourceValue = value.ToString();

            var url = wcfRestAddress + "Image/" + sourceValue;
            return url;

            var img = Bitmap(sourceValue);
            return img;
        }

        private Image Bitmap(string sourceValue)
        {
            var url = wcfRestAddress + "Image/" + sourceValue;

            return Image(url).Result;
        }

        private static async Task<Image> Image(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("fangxing123");
            var msg = await client.GetAsync(url);
            var stream = await msg.Content.ReadAsStreamAsync();

            var bitmap = System.Drawing.Image.FromStream(stream);
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}