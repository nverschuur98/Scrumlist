using Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Scrumlijst.Windows
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class ManageTask : RibbonWindow
    {
        private bool isNewTask { get; set; }
        private Models.taskModel oldTask { get; set; }
        private static Helpers.dataHelper dH = new Helpers.dataHelper();

        public ManageTask(Models.taskModel task)
        {
            oldTask = task;
            
            //Initialize the screen
            InitializeComponent();

            cbx_Discipline.ItemsSource = MainWindow.Disciplines;
            cbx_State.ItemsSource = MainWindow.states;
            cbx_Sprint.ItemsSource = MainWindow.Sprints;

            if (string.IsNullOrEmpty(task.id.ToString()) || task.id.ToString() == "" || task.id == 0)
            {
                isNewTask = true;

                //Add the latest +1 id to the ID box
                tbx_ID.Text = (dH.returnLastTaskId() + 1).ToString();

                tbx_AssignDate.Text = (DateTime.Now).ToString();
            }else
            {
                isNewTask = false;
                btn_Adjust.Content = "Change";

                tbx_ID.Text = Convert.ToString(task.id);
                tbx_Item.Text = task.name;
                tbx_AssignBy.Text = task.assignBy;
                tbx_AssignTo.Text = task.assignTo;
                tbx_Description.Text = task.description;
                tbx_AssignDate.Text = Convert.ToString(task.assignDate);
                cbx_Discipline.SelectedItem = task.discipline;
                cbx_State.SelectedItem = Convert.ToString(task.state);
                cbx_Sprint.SelectedItem = task.sprint;
            }
        }

        private void btn_CancelClick(object sender, RoutedEventArgs e)
        {
            //Close this window
            this.Close();
        }

        private void btn_AdjustClick(object sender, RoutedEventArgs e)
        {
            Models.taskModel toAdd = new Models.taskModel();
            Helpers.sourceHelper sH = new Helpers.sourceHelper();

            toAdd.id = Convert.ToInt32(tbx_ID.Text);
            toAdd.name = Convert.ToString(tbx_Item.Text);
            toAdd.assignBy = Convert.ToString(tbx_AssignBy.Text);
            toAdd.assignTo = Convert.ToString(tbx_AssignTo.Text);
            toAdd.description = Convert.ToString(tbx_Description.Text);
            toAdd.disciplineID = (cbx_Discipline.SelectedItem as Models.disciplineModel).id;
            toAdd.assignDate = Convert.ToDateTime(tbx_AssignDate.Text);
            toAdd.state = Convert.ToString(cbx_State.Text);
            toAdd.sprintID = (cbx_Sprint.SelectedItem as Models.sprintModel).id;

            if (isNewTask)
            {
                //Write to tasklist
                //MainWindow.Tasklist.Add(toAdd);

                //Write to database
                sH.writeNewTask(toAdd);
            }else
            {
                MainWindow.Tasklist.Remove(oldTask);
                MainWindow.Tasklist.Add(toAdd);
                
                sH.updateTask(toAdd);
            }
            
            //Close this window
            this.Close();
        }
    }
}
