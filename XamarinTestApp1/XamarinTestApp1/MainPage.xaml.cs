using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace XamarinTestApp1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
              {
                  Device.BeginInvokeOnMainThread(async () =>
                  {
                      await Navigation.PopAsync();
                      try
                      {
                          string trueResult = "", str = "" + result.Text;
                          for (int i = 0; i < str.Length; i++)
                              trueResult += Convert.ToString(Convert.ToInt32(str[i]));
                          Label1.Text = trueResult;
                      }
                      catch
                      {
                      }
                  });
              };
        }
    }
}
