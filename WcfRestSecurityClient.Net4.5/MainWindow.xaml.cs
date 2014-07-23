using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;

namespace WcfRestSecurityClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly string wcfRestAddress = System.Configuration.ConfigurationManager.AppSettings["WcfRestAddress"];

        private readonly Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();

        private readonly Stopwatch watch = new Stopwatch();

        private const string Auth = "fangxing123";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            op.RestoreDirectory = true;
            op.Filter = "Jpeg Files(*.jpg)|*.jpg|Gif Files(*.gif)|*.gif";

            BindData();
        }

        private async void BindData()
        {
            var url = wcfRestAddress + "AllImage";
            //var url = "http://192.168.0.178:3577/ImageService/ListAll";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Auth);

            watch.Restart();

            var msg = await client.GetAsync(url);

            watch.Stop();
            Debug.WriteLine(watch.Elapsed.TotalMilliseconds);

            watch.Restart();
            var stream = await msg.Content.ReadAsStreamAsync();
            var dataSer = new DataContractSerializer(typeof(string[]));
            var obj = (string[])dataSer.ReadObject(stream);

            watch.Stop();
            Debug.WriteLine(watch.Elapsed.TotalMilliseconds);

            listBox1.DataContext = obj;
            //if (obj.Any())
            //{
            //    listBox1.SelectedIndex = 0;
            //}
        }

        /// <summary>
        /// 增加图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            var ret = op.ShowDialog();
            if (!ret.HasValue || !ret.Value || !op.CheckFileExists) return;
            var file = op.FileName;
            var name = System.IO.Path.GetFileName(file);

            var url = wcfRestAddress + "Add/" + name;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Auth);
            HttpContent content = new StreamContent(File.OpenRead(file));
            var resp = await client.PostAsync(url, content);
            resp.EnsureSuccessStatusCode();

            BindData();
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, RoutedEventArgs e)
        {
            string name = listBox1.SelectedValue.ToString();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var url = wcfRestAddress + "Delete/" + name;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Auth);
                var resp = await client.DeleteAsync(url);
                resp.EnsureSuccessStatusCode();

                BindData();
            }
            else
            {
                MessageBox.Show("请先选中要删除的图片！");
            }
        }

        private async Task DisplayResult(HttpResponseMessage response)
        {
            txtStatusCode.Text = response.StatusCode.ToString();
            this.textBox1.Text = await response.Content.ReadAsStringAsync();
        }

        private async void btnInvokeWithAuth_Click(object sender, RoutedEventArgs e)
        {
            var url = txtUrl.Text;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Auth);
            var resp = await client.GetAsync(url);
            await DisplayResult(resp);
        }

        private async void btnInvokeWithoutAuth_Click(object sender, RoutedEventArgs e)
        {
            var url = txtUrl.Text;
            var client = new HttpClient();
            var resp = await client.GetAsync(url);
            await DisplayResult(resp);
        }

        private async void ListBox1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox1.SelectedValue == null)
            {
                return;
            }

            var url = wcfRestAddress + "Image/" + listBox1.SelectedValue;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Auth);

            watch.Restart();

            var msg = await client.GetAsync(url);

            watch.Stop();
            Debug.WriteLine(watch.Elapsed.TotalMilliseconds);

            watch.Restart();

            var stream = await msg.Content.ReadAsStreamAsync();

            using (var bitmap = System.Drawing.Image.FromStream(stream))
            {
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);// 格式自处理,这里用 bitmap
                    var bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = new MemoryStream(ms.ToArray()); // 不要直接使用 ms
                    bi.EndInit();

                    watch.Stop();
                    Debug.WriteLine(watch.Elapsed.TotalMilliseconds);

                    image1.Source = bi;
                }
            }
        }
    }
}