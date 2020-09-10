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

namespace XamarinTestApp1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        SqlConnection sqlConnection;
        string ConnectionString = "workstation id=SNTZaria.mssql.somee.com;packet size=4096;user id=Rostiwe_SQLLogin_1;pwd=r3646bycrg;data source=SNTZaria.mssql.somee.com;persist security info=False;initial catalog=SNTZaria";
        public MainPage()
        {
            InitializeComponent();
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                sqlConnection.Open();

            }
            catch (Exception ex)
            {
                Bad_Sql_conection();
                DisplayAlert("Попытка подключения", "Не получилось подключиться к SQl,"+ex.Message, "ОK");
            }
            finally
            {
                sqlConnection.StateChange += new StateChangeEventHandler(sqlConnection_StateChange);
            }
        }
        protected override void OnDisappearing()
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
            base.OnDisappearing();
        }
        void sqlConnection_StateChange(object sender, StateChangeEventArgs e)
        {
            if (sqlConnection == null || sqlConnection.State == ConnectionState.Closed)
            {
                try
                {
                    sqlConnection.Open();
                }
                catch
                {
                    DisplayAlert("Ошибка", "Соединение с сервером потеряно", "ОK");
                    Bad_Sql_conection();
                    DisplayAlert("SQl подключение", "Разорвано содединение с SQl,", "ОK");
                }
            
            }
        }
        private void Bad_Sql_conection ()
        {
            Button1.Clicked -= Button_Clicked;
            Button1.Clicked += Button_Clicked_2;
            Label2.Text = "Не подключен к серверу";
            Button1.Text = "Подключиться к серверу";
        }
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            try 
            {
                sqlConnection.Open();
                Button1.Clicked += Button_Clicked;
                Label2.Text = "";
                Button1.Text = "Сканировать";
                DisplayAlert("Попытка подключения", "Подключились", "ОK");
                Button1.Clicked -= Button_Clicked_2;
            }
            catch
            {
                DisplayAlert("Попытка подключения", "Не получилось подключиться", "ОK");
            }
            
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
                          string trueResult = "" + result.Text;
                          while (trueResult.Length < 10)
                          {
                              trueResult += " ";
                          }
                          string sqlExpression = "SELECT COUNT (id) FROM [dbo].[Trash] WHERE QrCode = N'"+ trueResult + "'";
                          SqlCommand sqlCommand = new SqlCommand(sqlExpression, sqlConnection);
                          int b = Convert.ToInt32(sqlCommand.ExecuteScalar());
                          if (b == 0)
                          {
                              
                              Label1.Text = "Не найден - "+result.Text;
                          }
                          if (b == 1)
                          {
                              Label1.Text = "Успешно";
                              sqlExpression = "UPDATE [dbo].[Trash] SET ThisMonth = ThisMonth + 1 WHERE QrCode = N'" + trueResult + "'";
                              sqlCommand.CommandText = sqlExpression;
                              sqlCommand.ExecuteNonQuery();
                          }
                          if (b > 1)
                          {
                              await DisplayAlert("Ошибка!", "Такой код найден более одного раза. Обратитесь к администрации!", "ОK");
                              Label1.Text = "Нашёл больше одного. В базе - беда";
                          }
                      }
                      catch 
                      {
                          sqlConnection.Close();
                      }
                  });
              };
        }
    }
}
