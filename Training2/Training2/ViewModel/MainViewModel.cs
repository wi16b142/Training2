using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using Training2.Models;

namespace Training2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        Server server;
        const string ip = "127.0.0.1";
        const int port = 10100;
        bool isConnected = false;
        private ObservableCollection<Entry> history;
        private ObservableCollection<string> users;
        private ObservableCollection<Entry> selectedHistory;
        private string selectedUser; 

        public RelayCommand ReceiveBtnClick { get; set; }

        public ObservableCollection<Entry> SelectedHistory
        {
            get { return selectedHistory; }
            set { selectedHistory = value; RaisePropertyChanged(); }
        }

        public string SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                RaisePropertyChanged("SelectedUser");
                SelectedHistory.Clear();
                if(History.Count > 0)
                {
                    foreach(var entry in History)
                    {
                        if (SelectedUser.Equals(entry.Username)) SelectedHistory.Add(entry);
                    }
                    RaisePropertyChanged("SelectedHistory");
                }
            }
        }

        public ObservableCollection<string> Users
        {
            get { return users; }
            set { users = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<Entry> History
        {
            get { return history; }
            set { history = value; }
        }

        public MainViewModel()
        {
            ReceiveBtnClick = new RelayCommand(()=>
            {
                server = new Server(ip, port, GuiUpdate);
                isConnected = true;
            }, ()=> { return !isConnected; });

            History = new ObservableCollection<Entry>();
            Users = new ObservableCollection<string>();
            SelectedHistory = new ObservableCollection<Entry>();
        }

        private void GuiUpdate(Entry newEntry)
        {
            App.Current.Dispatcher.Invoke(()=> 
            {
                if (!Users.Contains(newEntry.Username))
                {
                    Users.Add(newEntry.Username); RaisePropertyChanged("Users");
                }
                History.Add(newEntry);
                RaisePropertyChanged("History");
            });   
        }
    }
}