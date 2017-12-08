using System;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace Scrumlijst.Helpers
{
    class dataHelper
    {
        public int returnLastTaskId()
        {
            Windows.MainWindow.conn.Open();
            string sql = "SELECT MAX(id) AS id FROM tasks";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            SQLiteDataReader reader = command.ExecuteReader();

            int x;

            try
            {
                x = Convert.ToInt32(reader["id"]);
            }
            catch
            {
                x = 0;
            }

            Windows.MainWindow.conn.Close();
            return x;
        }

        public int returnLastSprintId()
        {
            Windows.MainWindow.conn.Open();
            string sql = "SELECT MAX(id) AS id FROM sprints";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            SQLiteDataReader reader = command.ExecuteReader();

            int x;

            try
            {
                x = Convert.ToInt32(reader["id"]);
            }
            catch
            {
                x = 0;
            }

            Windows.MainWindow.conn.Close();
            return x;
        }

        public Models.sprintModel returnSprintFromId(int ID)
        {
            Windows.MainWindow.conn.Open();
            string sql = $"SELECT name FROM sprints WHERE id=\"{ID}\"";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            SQLiteDataReader reader = command.ExecuteReader();

            Models.sprintModel x;

            try
            {
                x = Windows.MainWindow.Sprints.Where(t => t.id == ID).FirstOrDefault();
            }
            catch
            {
                x = new Models.sprintModel();
            }

            Windows.MainWindow.conn.Close();
            return x;
        }

        public Models.disciplineModel returnDisciplineFromId(int ID)
        {
            Windows.MainWindow.conn.Open();
            string sql = $"SELECT name FROM sprints WHERE id=\"{ID}\"";
            SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
            SQLiteDataReader reader = command.ExecuteReader();

            Models.disciplineModel x;

            try
            {
                x = Windows.MainWindow.Disciplines.Where(t => t.id == ID).FirstOrDefault();
            }
            catch
            {
                x = new Models.disciplineModel();
            }

            Windows.MainWindow.conn.Close();
            return x;
        }

        public bool returnIfTaskExists(Models.taskModel task)
        {

            Windows.MainWindow.conn.Open();

            try
            {
                string sql = $"SELECT 1 FROM tasks WHERE id=\"{task.id}\"";
                SQLiteCommand command = new SQLiteCommand(sql, Windows.MainWindow.conn);
                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                string messageBoxText = "There was an error while checking if this ID already exists.\n\n" + e.Message;
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
                return false;
            }
            finally
            {

                Windows.MainWindow.conn.Close();
            }

        }
    }
}
