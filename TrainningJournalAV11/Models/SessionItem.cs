using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrainningJournalAV11.Models
{
    public class SessionItem
    {
        public string SessionName { get; set; }
        public string? Description { get; set; }
        public ObservableCollection<string>? Exercises { get; set; }
        public ObservableCollection<ExerciseItem> ExerciseList { get; set; }
        public string Date { get; set; }
        public SessionItem(string sessionName, string date)
        {
            SessionName = sessionName;
            Date = date;
            Exercises = XMLUtilities.GetExercisesNames(sessionName, date);
            ExerciseList = XMLUtilities.GetExerciseListFromASession(sessionName, date);
        }

        public SessionItem(string sessionName,string? description,string date)
        {
            SessionName = sessionName;
            Description = description;
            Date = date;
            Exercises = XMLUtilities.GetExercisesNames(sessionName, date);
            ExerciseList = XMLUtilities.GetExerciseListFromASession(sessionName, date);
        }
        public SessionItem(string sessionName)
        {
            SessionName = sessionName;
        }

    }
}
