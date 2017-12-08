using System;

namespace Scrumlijst.Models
{
    public class taskModel
    {
        private static Helpers.dataHelper dH = new Helpers.dataHelper();

        public int id { get; set; }
        public string name { get; set; }
        public string assignBy { get; set; }
        public string assignTo { get; set; }
        public string description { get; set; }

        public Models.disciplineModel discipline { get { return dH.returnDisciplineFromId(disciplineID); } set { disciplineID = value.id; } }
        public int disciplineID { get; set; } //Is used as a reference
        public string disciplineName { get { return discipline.name; } }

        public DateTime assignDate { get; set; }
        public string state { get;
            set; }

        public Models.sprintModel sprint { get { return dH.returnSprintFromId(sprintID); } set { sprintID = value.id; } }
        public int sprintID { get; set; } //Is used as a reference
        public string sprintName { get { return sprint.name; } }
    }
}
