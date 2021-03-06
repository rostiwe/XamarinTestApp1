﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTestApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TablesListPage : ContentPage
    {
        ApplicationViewModel viewModel;
        public TablesListPage(ApplicationViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = viewModel;
        }
        protected override async void OnDisappearing()
        {
            await viewModel.Gettables();
            base.OnDisappearing();
        }
        protected override async void OnAppearing()
        {
            await viewModel.Gettables();
            base.OnAppearing();
        }



    }
}