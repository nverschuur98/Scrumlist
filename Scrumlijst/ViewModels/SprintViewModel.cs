using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Scrumlijst.Helpers;
using System.Windows;

namespace Scrumlijst.ViewModels
{
    class SprintViewModel : INotifyPropertyChanged
    {
        //Resources
        sourceHelper sH = new sourceHelper();

        //Commands
        public ICommand doubleClickCommand { get; set; }
        public ICommand saveTaskCommand { get; set; }
        public ICommand deleteTaskCommand { get; set; }

        //Binding Variables
        private Models.sprintModel selectedSprintModel;
        public Models.sprintModel SelectedSprintModel
        {
            get { return selectedSprintModel; }
            set { selectedSprintModel = value; OnPropertyChanged("SelectedSprintModel"); Windows.MainWindow.selectedSprint = value; }
        }

        private Models.sprintModel selectedSprintModelEdit;
        public Models.sprintModel SelectedSprintModelEdit
        {
            get { return selectedSprintModelEdit; }
            set { selectedSprintModelEdit = value; OnPropertyChanged("SelectedSprintModelEdit"); }
        }

        //Lists
        public static ObservableCollection<Models.sprintModel> Sprints
        {
            get { return Windows.MainWindow.Sprints; }
        }

        //Constructor
        public SprintViewModel()
        {
            doubleClickCommand = new BaseCommand(ShowSprint);
            saveTaskCommand = new BaseCommand(SaveSprint);
            deleteTaskCommand = new BaseCommand(DeleteSprint);

            SelectedSprintModel = new Models.sprintModel();
        }

        //Show the selected sprint on double click in the edit section
        private void ShowSprint(object obj)
        {
            SelectedSprintModelEdit = SelectedSprintModel;
        }

        private void SaveSprint(object obj)
        {
            try
            {
                sH.updateSprint(SelectedSprintModelEdit);
            }
            catch (Exception e)
            {
                string messageBoxText = "There was an error while trying to save this task.\n\n" + e.Message;
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        private void DeleteSprint(object obj)
        {
            try
            {
                sH.deleteSprint(SelectedSprintModelEdit);
            }
            catch (Exception e)
            {
                string messageBoxText = "There was an error while trying to delete this task.\n\n" + e.Message;
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        //Event handling
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        private void dgSprintList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {/*
            DataGrid send = sender as DataGrid;
            SelectedSprintModel = (Models.sprintModel)send.CurrentItem;
            */
        }

        
    }

}
