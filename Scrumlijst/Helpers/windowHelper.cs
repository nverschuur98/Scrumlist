using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Scrumlijst.Helpers
{
    class windowHelper
    {

        public void Exit(Window window)
        {
            Models.settingsModel sM = Windows.MainWindow.Settings;

            sM.updateWindowSettings(window);
            sM.saveSettings();

            //Exit the program
            Application.Current.Shutdown();
        }
        
        public void Saved()
        {
            //Write the data to the source file
            sourceHelper source = new sourceHelper();
            source.writeAllDataSource();
        }

        public void About()
        {
            // Configure the message box to be displayed
            string messageBoxText = "This program is an alternative for the excel issue list.";
            messageBoxText += "\n\nPlease remember that this is still a pre-alpha release and not all functionality is implemented, correct, etc.";
            messageBoxText += "\nReport any issues to nick.verschuur@engie.com with the error and the databasefile.";
            string caption = "About";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
}
