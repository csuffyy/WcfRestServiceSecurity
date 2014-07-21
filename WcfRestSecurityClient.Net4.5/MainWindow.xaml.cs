using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        private async Task DisplayResult(HttpResponseMessage response)
        {
            txtStatusCode.Text = response.StatusCode.ToString();
            this.textBox1.Text = await response.Content.ReadAsStringAsync();
        }

        private async void btnInvokeWithAuth_Click(object sender, RoutedEventArgs e)
        {
            var url = txtUrl.Text;
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", "fangxing123");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("fangxing123");
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
    }
}