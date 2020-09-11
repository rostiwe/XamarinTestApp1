using System;
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
        public TablesListPage()
        {
            InitializeComponent();
            viewModel = new ApplicationViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            await viewModel.Gettables();
            base.OnAppearing();
        }
        public void update()
        {
            viewModel = new ApplicationViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
        }
    }
}