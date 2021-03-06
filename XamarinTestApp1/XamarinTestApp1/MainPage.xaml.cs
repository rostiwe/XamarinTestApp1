﻿using System;
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
using Android.OS;

namespace XamarinTestApp1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        ApplicationViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            viewModel = new ApplicationViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
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
                          int R = Convert.ToInt32(trueResult);
                          Label1.Text = trueResult;
                          int id = FindTable(R);
                          if (id == -1)
                              await DisplayAlert("Обновление", "Было не найдено", "ОK");
                          else
                          {
                              viewModel.Tables[id].ThisMonth++;
                              viewModel.SaveTable(viewModel.Tables[id]);
                              await DisplayAlert("Обновление", "Прошло успешно", "ОK");
                          }
                      }
                      catch
                      {
                      }
                  });
              };
        }
        int FindTable(int id)
        {
           
            for (int i = 0; i< viewModel.Tables.Count; i++)
            {
                if (viewModel.Tables[i].Id == id)
                    return i;
            }
            return -1;
        }
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TablesListPage(viewModel));
            //TablesListPage(ApplicationViewModel viewModel)
        }
        protected override async void OnAppearing()
        {
            await viewModel.Gettables();
            base.OnAppearing();
        }
    }
}
