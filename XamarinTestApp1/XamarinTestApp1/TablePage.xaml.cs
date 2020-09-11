using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTestApp1
{
    public partial class TablePage : ContentPage
    {
        public Table Model { get; private set; }
        public ApplicationViewModel ViewModel { get; private set; }
        public TablePage(Table model, ApplicationViewModel viewModel)
        {
            InitializeComponent();
            Model = model;
            ViewModel = viewModel;
            this.BindingContext = this;
        }
    }
}