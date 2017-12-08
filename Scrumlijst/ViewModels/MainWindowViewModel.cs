using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Scrumlijst.Helpers;

namespace Scrumlijst.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        Helpers.sourceHelper sH = new Helpers.sourceHelper();

        public ICommand TaskCommand { get; set; }
        public ICommand SprintCommand { get; set; }
        public ICommand UpdateDatabaseCommand { get; set; }
        public ICommand SelectDatabaseCommand { get; set; }

        private object selectedViewModel;

        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }

        public MainWindowViewModel()
        {
            TaskCommand = new BaseCommand(OpenTaskView);
            SprintCommand = new BaseCommand(OpenSprintView);
            UpdateDatabaseCommand = new BaseCommand(UpdateDatabase);
            SelectDatabaseCommand = new BaseCommand(SelectDatabase);
            
            SelectedViewModel = new ViewModels.StartupViewModel();
        }

        private void OpenTaskView(object obj)
        {
            sH.readDataSource();
            SelectedViewModel = new ViewModels.TaskViewModel();
        }

        private void OpenSprintView(object obj)
        {
            sH.readSprintsSource();
            SelectedViewModel = new ViewModels.SprintViewModel();
        }

        private void UpdateDatabase(object obj)
        {
            //Hey we're here!
        }

        private void SelectDatabase(object obj)
        {
            Helpers.sourceHelper sH = new Helpers.sourceHelper();
            sH.selectDataSource();
            //Load the data source 
            sH.readDataSource();
            sH.readDisciplinesSource();
            sH.readSprintsSource();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
