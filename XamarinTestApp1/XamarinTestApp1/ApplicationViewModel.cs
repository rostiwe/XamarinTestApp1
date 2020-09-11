using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace XamarinTestApp1
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        bool initialized = false;
        Table selectedTable;
        private bool isBusy;
        public ObservableCollection<Table> Tables { get; set; }
        TableService tableService = new TableService();
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CreateTableCommand { protected set; get; }
        public ICommand DeleteTableCommand { protected set; get; }
        public ICommand SaveTableCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        public INavigation Navigation { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }
        public bool IsLoaded
        {
            get { return !isBusy; }
        }

        public ApplicationViewModel()
        {
            Tables = new ObservableCollection<Table>();
            IsBusy = false;
            CreateTableCommand = new Command(CreateTable);
            DeleteTableCommand = new Command(DeleteTable);
            SaveTableCommand = new Command(SaveTable);
            BackCommand = new Command(Back);
        }

        public Table SelectedTable
        {
            get { return selectedTable; }
            set
            {
                if (selectedTable != value)
                {
                    Table tempTable = new Table()
                    {
                        Id = value.Id,
                        Qrcode = value.Qrcode,
                        ThisMonth = value.ThisMonth,
                        AllTime = value.AllTime,
                        Примечания = value.Примечания
                    };
                    selectedTable = null;
                    OnPropertyChanged("SelectedFriend");
                    Navigation.PushAsync(new TablePage(tempTable, this));
                }
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async void CreateTable()
        {
            await Navigation.PushAsync(new TablePage(new Table(), this));
        }
        private void Back()
        {
            Navigation.PopAsync();
        }

        public async Task Gettables()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<Table> tables = await tableService.Get();

            // очищаем список
            while (Tables.Any())
                Tables.RemoveAt(Tables.Count - 1);

            // добавляем загруженные данные
            foreach (Table f in tables)
                Tables.Add(f);
            IsBusy = false;
            initialized = true;
        }
        private async void SaveTable(object tableObject)
        {
            Table table = tableObject as Table;
            if (table != null)
            {
                IsBusy = true;
                // редактирование
                if (table.Id > 0)
                {
                    Table updatedTable = await tableService.Update(table);
                    // заменяем объект в списке на новый
                    if (updatedTable != null)
                    {
                        int pos = Tables.IndexOf(updatedTable);
                        Tables.RemoveAt(pos);
                        Tables.Insert(pos, updatedTable);
                    }
                }
                // добавление
                else
                {
                    Table addedFriend = await tableService.Add(table);
                    if (addedFriend != null)
                        Tables.Add(addedFriend);
                }
                IsBusy = false;
            }
            Back();
        }
        private async void DeleteTable(object friendObject)
        {
            Table friend = friendObject as Table;
            if (friend != null)
            {
                IsBusy = true;
                Table deletedFriend = await tableService.Delete(friend.Id);
                if (deletedFriend != null)
                {
                    Tables.Remove(deletedFriend);
                }
                IsBusy = false;
            }
            Back();
        }
    }
}
   