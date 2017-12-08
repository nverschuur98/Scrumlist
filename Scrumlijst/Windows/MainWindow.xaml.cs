using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Data.SQLite;
using Fluent;
using System.Linq;
using System.Diagnostics;

namespace Scrumlijst.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        #region creations
        private Helpers.windowHelper window = new Helpers.windowHelper();
        private static Helpers.sourceHelper source = new Helpers.sourceHelper();
        public static Models.settingsModel Settings = new Models.settingsModel();
        public string databaselocation = Settings.databaseLocation;
        public static ObservableCollection<Models.taskModel> Tasklist = new ObservableCollection<Models.taskModel>();
        public static ObservableCollection<Models.sprintModel> Sprints = new ObservableCollection<Models.sprintModel>();
        public static List<string> settings = new List<string>();
        public static ObservableCollection<string> states = new ObservableCollection<string>() { "Not Started", "Started", "Finished", "Canceled" };
        public static ObservableCollection<Models.disciplineModel> Disciplines = new ObservableCollection<Models.disciplineModel>();
        public static SQLiteConnection conn = new SQLiteConnection();
        public static Models.taskModel selectedTask { get; set; }
        public static Models.sprintModel selectedSprint { get; set; }
        #endregion

        public MainWindow()
        {
            //Clear the log
            System.IO.File.Create(@"log.txt");

            //On exception log the error
            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                List<string> ErrorMessages = new List<string>();
                ErrorMessages.Add("--------------------------------------\n");
                ErrorMessages.Add("Source:\t\t" + eventArgs.Exception.Source + "\n");
                ErrorMessages.Add("InnerException:\t" + eventArgs.Exception.InnerException + "\n");
                ErrorMessages.Add("TargetSite:\t" + eventArgs.Exception.TargetSite + "\n");
                ErrorMessages.Add("Data.Values:\t" + eventArgs.Exception.Data.Values.ToString() + "\n");
                ErrorMessages.Add("Helplink:\t" + eventArgs.Exception.HelpLink + "\n");
                ErrorMessages.Add("Message:\t" + eventArgs.Exception.Message + "\n");
                
                System.IO.File.AppendAllLines(@"log.txt", ErrorMessages);
            };

            
            //First check if the data source is found
            //If not give a dialog window to select the source
            if (!source.checkDataSource())
            {
                source.selectDataSource();
            }

            Settings.applyWindowSettings(this);

            //Set up the connection
            conn.ConnectionString = $"Data Source={Settings.databaseLocation};Version=3;";
            
            //Load the data source 
            source.readDataSource();
            source.readDisciplinesSource();
            source.readSprintsSource();
            
            //Initialize the screen
            InitializeComponent();
            this.DataContext = new ViewModels.MainWindowViewModel();
        }

        #region window actions

        private void Save_button(object sender, RoutedEventArgs e)
        {
            window.Saved();
        }

        private void menuExit_click(object sender, RoutedEventArgs e)
        {
            window.Exit(this);
            conn.Close();
        }

        private void menuAbout_click(object sender, RoutedEventArgs e)
        {
            window.About();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            conn.Close();
            window.Exit(this);
        }

        private void btnTasksNew_Click(object sender, RoutedEventArgs e)
        {
            Windows.ManageTask screen = new Windows.ManageTask(new Models.taskModel());
            screen.ShowDialog();
        }

        private void btnTasksChange_Click(object sender, RoutedEventArgs e)
        {
            Models.taskModel task = MainWindow.Tasklist.Single(t => t == selectedTask);
            Windows.ManageTask screen = new Windows.ManageTask(task);
            screen.ShowDialog();
        }

        private void btnTasksDelete_Click(object sender, RoutedEventArgs e)
        {
            if (window.DeleteConfirmation())
            {
                source.deleteTask(selectedTask);
            }
        }

        private void btnSprintsNew_Click(object sender, RoutedEventArgs e)
        {
            Models.sprintModel sprint = new Models.sprintModel();

            Windows.ManageSprint screen = new Windows.ManageSprint(sprint);
            screen.ShowDialog();
        }

        private void btnSprintsChange_Click(object sender, RoutedEventArgs e)
        {
            Models.sprintModel sprint = new Models.sprintModel();

            try
            {
                sprint = MainWindow.Sprints.Single(s => s == selectedSprint);
            }
            catch
            {
                return;
            }
            
            Windows.ManageSprint screen = new Windows.ManageSprint(sprint);
            screen.ShowDialog();
        }

        private void btnSprintsDelete_Click(object sender, RoutedEventArgs e)
        {
            if (window.DeleteConfirmation())
            {
                source.deleteSprint(selectedSprint);
            }
            
        }

        #endregion

    }
}
