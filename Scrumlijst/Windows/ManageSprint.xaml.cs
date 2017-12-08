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
using Fluent;

namespace Scrumlijst.Windows
{
    /// <summary>
    /// Interaction logic for ManageSprint.xaml
    /// </summary>
    public partial class ManageSprint : RibbonWindow
    {
        private bool isNewSprint { get; set; }
        private Models.sprintModel oldSprint { get; set; }
        private static Helpers.dataHelper dH = new Helpers.dataHelper();

        public ManageSprint(Models.sprintModel sprint)
        {
            oldSprint = sprint;
            InitializeComponent();
            
            //cbx_Project = MainWindow.Projects;

            if(string.IsNullOrEmpty(sprint.id.ToString()) || sprint.id.ToString() == "" || sprint.id == 0)
            {
                isNewSprint = true;

                //Add the latest id +1 to the ID box
                tbx_ID.Text = (dH.returnLastSprintId() + 1).ToString();
                tbx_StartDate.Text = (DateTime.Now).ToString();
            }
            else
            {
                isNewSprint = false;
                btn_Adjust.Content = "Change";

                tbx_ID.Text = Convert.ToString(sprint.id);
                tbx_Item.Text = sprint.name;
                tbx_Description.Text = sprint.description;
                tbx_StartDate.Text = Convert.ToString(sprint.startDate);
                tbx_EndDate.Text = Convert.ToString(sprint.endDate);
            }
        }

        private void btn_CancelClick(object sender, RoutedEventArgs e)
        {
            //Close this window
            this.Close();
        }

        private void btn_AdjustClick(object sender, RoutedEventArgs e)
        {
            Models.sprintModel toAdd = new Models.sprintModel();
            Helpers.sourceHelper sH = new Helpers.sourceHelper();

            toAdd.id = Convert.ToInt32(tbx_ID.Text);
            toAdd.name = Convert.ToString(tbx_Item.Text);
            toAdd.description = Convert.ToString(tbx_Description.Text);
            try
            {
                toAdd.startDate = Convert.ToDateTime(tbx_StartDate.Text);
            }
            catch
            {
                toAdd.startDate = new DateTime();
            }
            try
            {
                toAdd.endDate = Convert.ToDateTime(tbx_EndDate.Text);
            }
            catch
            {
                toAdd.endDate = new DateTime();
            }

            if (isNewSprint)
            {
                //Write to tasklist
                MainWindow.Sprints.Add(toAdd);

                //Write to database
                sH.writeNewSprint(toAdd);
            }
            else
            {
                MainWindow.Sprints.Remove(oldSprint);
                MainWindow.Sprints.Add(toAdd);

                sH.updateSprint(toAdd);
            }

            //Close this window
            this.Close();
        }

    }
}
