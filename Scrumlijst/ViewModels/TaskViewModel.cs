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
    class TaskViewModel : INotifyPropertyChanged
    {
        //Resources
        sourceHelper sH = new sourceHelper();
        dataHelper dH = new dataHelper();

        //Commands
        public ICommand doubleClickCommand { get; set; }
        public ICommand saveTaskCommand { get; set; }
        public ICommand newTaskCommand { get; set; }
        public ICommand deleteTaskCommand { get; set; }

        //Binding Variables
        private Models.taskModel selectedTaskModel;
        public Models.taskModel SelectedTaskModel
        {
            get { return selectedTaskModel; }
            set { selectedTaskModel = value; OnPropertyChanged("SelectedTaskModel"); Windows.MainWindow.selectedTask = value; }
        }

        private Models.taskModel selectedTaskModelEdit;
        public Models.taskModel SelectedTaskModelEdit
        {
            get { return selectedTaskModelEdit; }
            set { selectedTaskModelEdit = value; OnPropertyChanged("SelectedTaskModelEdit"); SelectedTaskIdExists = dH.returnIfTaskExists(value); }
        }

        public bool SelectedTaskIdExists { get; set; }

        //Lists
        public static ObservableCollection<Models.taskModel> Tasks
        {
            get { return Windows.MainWindow.Tasklist; }
        }

        public static ObservableCollection<Models.disciplineModel> Disciplines
        {
            get { return Windows.MainWindow.Disciplines; }
        }

        public static ObservableCollection<string> States
        {
            get { return Windows.MainWindow.states; }
        }

        public static ObservableCollection<Models.sprintModel> Sprints
        {
            get { return Windows.MainWindow.Sprints; }
        }

        //Constructor
        public TaskViewModel()
        {
            doubleClickCommand = new BaseCommand(ShowTask);
            saveTaskCommand = new BaseCommand(SaveTask);
            newTaskCommand = new BaseCommand(NewTask);
            deleteTaskCommand = new BaseCommand(DeleteTask);

            SelectedTaskModel = new Models.taskModel();
        }

        //Show the selected task on double click in the edit section
        private void ShowTask(object obj)
        {
            SelectedTaskModelEdit = SelectedTaskModel;
        }

        //Save the selected task to the database
        private void SaveTask(object obj)
        {
            try
            {
                sH.updateTask(SelectedTaskModelEdit);
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

        //Delete the selected task from the database
        private void DeleteTask(object obj)
        {
            try
            {
                sH.deleteTask(SelectedTaskModelEdit);
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

        //Create a new task
        private void NewTask(object obj)
        {
            if(SelectedTaskModelEdit == null || SelectedTaskModelEdit.id == 0)
            {
                Models.taskModel newTask = new Models.taskModel();
                newTask.id = dH.returnLastTaskId() + 1;
                newTask.assignDate = DateTime.Now;

                SelectedTaskModelEdit = newTask;
            }
            /*else
            {
                try
                {

                    sH.writeNewTask(SelectedTaskModelEdit);
                }
                catch (Exception e)
                {
                    string messageBoxText = "There was an error while trying to create a new task.\n\n" + e.Message;
                    string caption = "Error";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Error;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                }

                OnPropertyChanged("SelectedTaskModelEdit");
            }*/
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
    }
}
