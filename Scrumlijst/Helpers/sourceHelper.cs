using System;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Linq;
using System.Xml.Linq;
using System.Windows;

namespace Scrumlijst.Helpers
{
    class sourceHelper
    {
        /// <summary>
        /// Check if the datasource saved in the settings file still exists. Returns false if not
        /// </summary>
        /// <returns></returns>
        public bool checkDataSource()
        {

            //upf == User Preference File
            if(!File.Exists(@"UserSettings.xml")){
                return false;
            }

            Windows.MainWindow.Settings.readSettings(@"UserSettings.xml");
            
            if(Windows.MainWindow.Settings.databaseLocation == null || Windows.MainWindow.Settings.databaseLocation == "" || Windows.MainWindow.Settings.databaseLocation.Length < 5)
            {
                return false;
            }

            if (File.Exists(Windows.MainWindow.Settings.databaseLocation))
            {
                return true;
            }else
            {
                return false;
            }
        }

        /// <summary>
        /// Open a dialog box to select the correct datasource
        /// </summary>
        public void selectDataSource()
        {
            //Open a dialog for selecting a file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            //Open the Folder Dialog
            openFileDialog.ShowDialog();
            Windows.MainWindow.Settings.updateDatabaseSettings(openFileDialog.FileName);

            //Write the new folder location to the settings file
            //File.WriteAllText("Settings.upf", Windows.MainWindow._settings.databaseLocation);
            Windows.MainWindow.conn.ConnectionString = $"Data Source={Windows.MainWindow.Settings.databaseLocation};Version=3;";

        }

        /// <summary>
        /// Read the tasks data source and put it in the taskList
        /// </summary>
        public void readDataSource()
        {
            #region old XML read out
            /*
            //Load the data from the location in settings[0]
            XDocument dataSource = XDocument.Load(MainWindow.settings[0]+"\\tasks.xml");
            //Select all tasks
            var dataSourceNodes = dataSource.Elements("Task");
            
            foreach(XElement node in dataSource.Descendants("Task"))
            {
                taskHelper toAdd = new taskHelper();

                //Covert all nodes to the right type and add them to toAdd
                if (node.Attribute("id") != null)
                {
                    toAdd.id = Convert.ToInt32(node.Attribute("id").Value);
                }

                if (node.Attribute("name") != null)
                {
                    toAdd.name = node.Attribute("name").Value;
                }

                if (node.Attribute("assignBy") != null)
                {
                    toAdd.assignBy = node.Attribute("assignBy").Value;
                }

                if (node.Attribute("assignTo") != null)
                {
                    toAdd.assignTo = node.Attribute("assignTo").Value;
                }

                if (node.Attribute("description") != null)
                {
                    toAdd.description = node.Attribute("description").Value;
                }

                if (node.Attribute("discipline") != null)
                {
                    //taskDiscipline discipline = (taskDiscipline)Enum.Parse(typeof(taskDiscipline), node.Attribute("discipline").Value);
                    toAdd.discipline = node.Attribute("discipline").Value;
                }

                if (node.Attribute("assignDate") != null)
                {
                    toAdd.assignDate = Convert.ToDateTime(node.Attribute("assignDate").Value);
                }

                if (node.Attribute("state") != null)
                {
                    //taskState state = (taskState)Enum.Parse(typeof(taskState), node.Attribute("state").Value);
                    toAdd.state = node.Attribute("state").Value;
                }

                if (node.Attribute("sprint") != null)
                {
                    toAdd.sprint = node.Attribute("sprint").Value;
                }

                MainWindow.tasklist.Add(toAdd);
            }*/
            #endregion

            Windows.MainWindow.conn.Open();

            string sql = "select * from tasks order by id asc";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            SQLiteDataReader reader = command.ExecuteReader();

            Windows.MainWindow.Tasklist.Clear();

            while (reader.Read())
            {
                Models.taskModel toAdd = new Models.taskModel();

                if (reader["id"] != null)
                {
                    toAdd.id = Convert.ToInt32(reader["id"]);
                }

                if (reader["name"] != null)
                {
                    toAdd.name = Convert.ToString(reader["name"]);
                }

                if (reader["assignBy"] != null)
                {
                    toAdd.assignBy = Convert.ToString(reader["assignBy"]);
                }

                if (reader["assignTo"] != null)
                {
                    toAdd.assignTo = Convert.ToString(reader["assignTo"]);
                }

                if (reader["description"] != null)
                {
                    toAdd.description = Convert.ToString(reader["description"]);
                }

                if (reader["discipline"] != null)
                {
                    toAdd.disciplineID = Convert.ToInt32(reader["discipline"]);
                }

                if (reader["assignDate"] != null)
                {
                    string test = reader["assignDate"].ToString();
                    toAdd.assignDate = Convert.ToDateTime(reader["assignDate"]);
                }

                if (reader["state"] != null)
                {
                    toAdd.state = Convert.ToString(reader["state"]);
                }

                if (reader["sprintId"] != null)
                {
                    toAdd.sprintID = Convert.ToInt32(reader["sprintId"]);
                }

                Windows.MainWindow.Tasklist.Add(toAdd);
            }

            Windows.MainWindow.conn.Close();
        }

        /// <summary>
        /// Read all the disciplines from the source
        /// </summary>
        public void readDisciplinesSource()
        {
            Windows.MainWindow.conn.Open();
            string sql = $"SELECT * FROM disciplines";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Windows.MainWindow.Disciplines.Add(new Models.disciplineModel {
                    id = Convert.ToInt32(reader["id"]),
                    name = reader["name"].ToString()
                });
                
            }

            Windows.MainWindow.conn.Close();
        }

        /// <summary>
        /// Read all the sprints and store them in a list for usage
        /// </summary>
        public void readSprintsSource()
        {
            Windows.MainWindow.Sprints.Clear();

            Windows.MainWindow.conn.Open();
            string sql = $"SELECT * FROM sprints";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Models.sprintModel toAdd = new Models.sprintModel();

                toAdd.id = Convert.ToInt32(reader["id"]);
                toAdd.name = Convert.ToString(reader["name"]);
                toAdd.description = Convert.ToString(reader["description"]);
                toAdd.startDate = Convert.ToDateTime(reader["startDate"]);
                toAdd.endDate = Convert.ToDateTime(reader["endDate"]);
                //toAdd.projects = Convert.ToInt32(reader["projects"]);

                Windows.MainWindow.Sprints.Add(toAdd);
            }

            Windows.MainWindow.conn.Close();
        }

        /// <summary>
        /// Write a new task to the database
        /// </summary>
        /// <param name="task"></param>
        public void writeNewTask(Models.taskModel task)
        {
            Windows.MainWindow.conn.Open();
            string sql = $"INSERT INTO tasks (id, name, assignBy, assignTo, description, discipline, assignDate, state, sprintId) VALUES({task.id}, \"{task.name}\", \"{task.assignBy}\", \"{task.assignTo}\", \"{task.description}\", \"{task.disciplineID}\", \"{task.assignDate.ToString()}\", \"{task.state}\", \"{task.sprintID}\")";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            command.ExecuteNonQuery();

            Windows.MainWindow.Tasklist.Add(task);

            Windows.MainWindow.conn.Close();
        }

        /// <summary>
        /// Write a new sprint to the database
        /// </summary>
        /// <param name="sprint"></param>
        public void writeNewSprint(Models.sprintModel sprint)
        {
            Windows.MainWindow.conn.Open();
            string sql = $"INSERT INTO sprints (id, name, description, startDate, endDate) VALUES({sprint.id}, \"{sprint.name}\", \"{sprint.description}\", \"{sprint.startDate.ToString()}\", \"{sprint.endDate.ToString()}\")";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            command.ExecuteNonQuery();
            Windows.MainWindow.conn.Close();
        }

        /// <summary>
        /// Write the tasklist to an XML file. The tasks datasource file
        /// </summary>
        /// <returns></returns>
        public void writeAllDataSource()
        {
            //Ask for confirmation
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("When saving, all task records will be removed and the database will be filled with new records. It is crucial not to interupt the program while saving. Are you sure you want to continue?", "Please confirm saving", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Windows.MainWindow.conn.Open();
                //Clear all tasks from the database
                string sql = "DELETE FROM tasks"; //WHERE sprint = selected sprint
                SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
                command.ExecuteNonQuery();

                //Write all tasks to the database
                foreach (Models.taskModel task in Windows.MainWindow.Tasklist)
                {
                    sql = $"INSERT INTO tasks (id, name, assignBy, assignTo, description, discipline, assignDate, state, sprintId) VALUES({task.id}, \"{task.name}\", \"{task.assignBy}\", \"{task.assignTo}\", \"{task.description}\", \"{task.disciplineID}\", \"{task.assignDate.ToString()}\", \"{task.state}\", \"{task.sprintID}\")";
                    command = new SQLiteCommand(sql, Windows.MainWindow.conn);
                    command.ExecuteNonQuery();
                }

                Windows.MainWindow.conn.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }

            

            #region old XML writer
            /*
            XDocument xdoc = XDocument.Load(MainWindow.settings[0] + "\\tasks.xml");
            //XDocumentType xType = new XDocumentType("Tasks", null, "Tasks.dtd", null);
            //xdoc.Add(xType);
            
            XElement xelmOld = xdoc.Element("Tasks");
            xelmOld.Remove();
            XElement xelm = new XElement("Tasks");

            foreach(taskHelper task in MainWindow.tasklist)
            {
                /*
                    XElement Element = new XElement("Task",
                        new XElement("id", task.id),
                        new XElement("name", task.name));
                        
                xelm.Add(Element);
                */
            /*

            XElement Element = new XElement("Task");
            Element.SetAttributeValue("id", task.id);
            Element.SetAttributeValue("name", task.name);
            Element.SetAttributeValue("assignBy", task.assignBy);
            Element.SetAttributeValue("assignTo", task.assignTo);
            Element.SetAttributeValue("description", task.description);
            Element.SetAttributeValue("discipline", task.discipline);
            Element.SetAttributeValue("assignDate", task.assignDate);
            Element.SetAttributeValue("state", task.state);
            Element.SetAttributeValue("sprint", task.sprint);

            xelm.Add(Element);
        }
        xdoc.Add(xelm);
        xdoc.Save(MainWindow.settings[0] + "\\tasks.xml");
        */
            #endregion

            //MANIER VERZINNEN OM DE WIJZIGINGEN BIJ TE HOUDEN

        }

        /// <summary>
        /// Updates the task in the database, based on the task.id
        /// </summary>
        /// <param name="task"></param>
        public void updateTask(Models.taskModel task)
        {
            Windows.MainWindow.conn.Open();

            try //Try to update the task
            {
                string sql = $"UPDATE tasks SET name=\"{task.name}\", assignBy=\"{task.assignBy}\", assignTo=\"{task.assignTo}\", description=\"{task.description}\", discipline=\"{task.disciplineID}\", assignDate=\"{task.assignDate.ToString()}\", state=\"{task.state}\", sprintId=\"{task.sprintID}\" WHERE id={task.id}";
                SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
                var result = command.ExecuteNonQuery();
                

                if(result == 0)
                {
                    //throw new Exception($"There was no data found with id: {task.id}");
                    Windows.MainWindow.conn.Close();
                    writeNewTask(task);

                }
            }
            catch(Exception e) //If for some reason the execution fails...
            {
                string messageBoxText = "There was an error while trying to update this task in the datebase. Please see below for a detailed error\n\n" + e.Message;
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
            }
            finally //...close the connection to the database.
            {
                Windows.MainWindow.conn.Close();
            }
        }

        /// <summary>
        /// Update the sprint in the database, based on the sprint.id
        /// </summary>
        /// <param name="sprint"></param>
        public void updateSprint(Models.sprintModel sprint)
        {
            Windows.MainWindow.conn.Open();
            string sql = $"UPDATE sprints SET name=\"{sprint.name}\", description=\"{sprint.description}\", startDate=\"{sprint.startDate.ToString()}\", endDate=\"{sprint.endDate.ToString()}\"  WHERE id={sprint.id}";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            command.ExecuteNonQuery();
            Windows.MainWindow.conn.Close();
        }

        /// <summary>
        /// Delete a task based on the task model
        /// </summary>
        /// <param name="task"></param>
        public void deleteTask(Models.taskModel task)
        {
            string messageBoxText = "Are you sure you want to delete the selected item?";
            string caption = "Delete Confirmation";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBoxResult.No;
            MessageBoxResult endResult = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, result);

            if (endResult == MessageBoxResult.No)
            {
                return;
            }

            Windows.MainWindow.conn.Open();
            
            try
            {
                string sql = $"DELETE FROM tasks WHERE id={task.id}";
                SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
                command.ExecuteNonQuery();

                Models.taskModel itemToRemove = Windows.MainWindow.Tasklist.Single(t => t.id == task.id);
                Windows.MainWindow.Tasklist.Remove(itemToRemove);
            }
            catch(Exception e)
            {
                messageBoxText = "There was no task selected, please select a task.\n\n" + e.Message;
                caption = "Error";
                button = MessageBoxButton.OK;
                icon = MessageBoxImage.Error;
                System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
            }
            finally
            {
                Windows.MainWindow.conn.Close();
            }
        }

        /// <summary>
        /// Delete a Sprint based on the sprintID
        /// </summary>
        /// <param name="Sprint"></param>
        public void deleteSprint(Models.sprintModel sprint)
        {
            string messageBoxText = "Are you sure you want to delete the selected item?";
            string caption = "Delete Confirmation";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            System.Windows.MessageBoxOptions option = System.Windows.MessageBoxOptions.RightAlign;
            MessageBoxResult result = MessageBoxResult.No;
            MessageBoxResult endResult = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, result, option);

            if (endResult == MessageBoxResult.No)
            {
                return;
            }

            Windows.MainWindow.conn.Open();
            string sql = $"DELETE FROM sprints WHERE id={sprint.id}";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            command.ExecuteNonQuery();

            try
            {
                Models.sprintModel itemToRemove = Windows.MainWindow.Sprints.Single(s => s == sprint);
                Windows.MainWindow.Sprints.Remove(itemToRemove);
            }
            catch (Exception e)
            {
                messageBoxText = "There was no sprint selected, please select a sprint.\n\n" + e.Message;
                caption = "Error";
                button = MessageBoxButton.OK;
                icon = MessageBoxImage.Error;
                System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
            }
            finally
            {
                Windows.MainWindow.conn.Close();
            }
        }
        
    }
}