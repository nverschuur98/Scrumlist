using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scrumlijst.Models
{
    public class settingsModel
    {
        //DataSource settings
        public string databaseLocation { get; set; }

        //Window Settings
        public double windowWidth { get; set; }
        public double windowHeight { get; set; }
        public double windowLeft { get; set; }
        public double windowTop { get; set; }
        public WindowStartupLocation windowLocation { get; set; }
        public WindowState windowState { get; set; }

        public settingsModel()
        {
            databaseLocation = "test";

            windowHeight = 400;
            windowWidth = 600;

            windowLocation = WindowStartupLocation.Manual;

            windowState = WindowState.Normal;
        }

        public void applyWindowSettings(Window window)
        {
            window.Height = windowHeight;
            window.Width = windowWidth;

            window.Left = windowLeft;
            window.Top = windowTop;

            //window.WindowStartupLocation = windowLocation;

            window.WindowState = windowState;
        }
        
        public void updateWindowSettings(Window window)
        {
            windowHeight = window.ActualHeight;
            windowWidth = window.ActualWidth;

            windowLeft = window.Left;
            windowTop = window.Top;

            windowLocation = window.WindowStartupLocation;

            windowState = window.WindowState;
        }

        public void applyDatabaseSettings()
        {

        }

        public void updateDatabaseSettings(String location)
        {
            databaseLocation = location;
        }

        public void readSettings(string location)
        {
            XDocument xdoc = XDocument.Load(location);

            XElement Settings = xdoc.Element("Settings");

            XElement DataBase = Settings.Element("DataBase");
            databaseLocation = DataBase.Attribute("Location").Value;
            
            XElement Window = Settings.Element("WindowSettings");
            windowWidth = Convert.ToDouble(Window.Attribute("Width").Value);
            windowHeight = Convert.ToDouble(Window.Attribute("Height").Value);
            windowLeft = Convert.ToDouble(Window.Attribute("Left").Value);
            windowTop = Convert.ToDouble(Window.Attribute("Top").Value);
            windowState = (WindowState)Enum.Parse(typeof(WindowState), Window.Attribute("State").Value);
        }

        public void saveSettings()
        {
            XDocument xdoc = new XDocument();

            XElement Settings = new XElement("Settings");
            
            XElement DataBase = new XElement("DataBase");
            DataBase.SetAttributeValue("Location", databaseLocation);
            Settings.Add(DataBase);

            XElement Window = new XElement("WindowSettings");
            Window.SetAttributeValue("Width", windowWidth);
            Window.SetAttributeValue("Height", windowHeight);
            Window.SetAttributeValue("Left", windowLeft);
            Window.SetAttributeValue("Top", windowTop);
            Window.SetAttributeValue("Location", windowLocation);
            Window.SetAttributeValue("State", windowState);

            Settings.Add(Window);
            xdoc.Add(Settings);
            xdoc.Save("UserSettings.xml");
            }
        }
    }
