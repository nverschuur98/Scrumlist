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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scrumlijst.Views
{
    /// <summary>
    /// Interaction logic for SprintView.xaml
    /// </summary>
    public partial class SprintView : UserControl
    {
        public SprintView()
        {
            InitializeComponent();
        }

        private void dgSprintList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            Models.sprintModel sprint = (Models.sprintModel)dgSprintList.CurrentItem;

            try
            {
                Windows.MainWindow.selectedSprint = sprint;
            }
            catch
            {
                try
                {
                    Windows.MainWindow.selectedSprint = Windows.MainWindow.Sprints[0];
                }
                catch
                {
                    Windows.MainWindow.selectedSprint = null;
                }
            }

        }
    }
}
