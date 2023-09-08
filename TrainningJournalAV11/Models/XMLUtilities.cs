using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TrainningJournalAV11.ViewModels;

namespace TrainningJournalAV11.Models
{
    public static class XMLUtilities
    {
        public static string currentDirectory = Directory.GetCurrentDirectory();

        public static string filename = "Assets\\Journal.xml";
        public static string journalFilePath = Path.Combine(currentDirectory, filename);
       // public static XElement journalElement = XElement.Load("C:\\Users\\diego\\source\\repos\\TrainningJournal\\TrainningJournal\\Assets\\Journal.xml");
        public static XDocument journalDoc = XDocument.Load(journalFilePath);

        public static string exFileName = "Assets\\Exercises.xml";
        public static string exercisesFilePath = Path.Combine(currentDirectory, exFileName);
        public static XDocument exercisesDoc = XDocument.Load(exercisesFilePath);
        public static ObservableCollection<SessionItem> GetSessionNames(DateTimeOffset date)
        {
            ObservableCollection<SessionItem> sessionList = new ObservableCollection<SessionItem>();

            try
            {
                sessionList.Clear();
                string day = date.Day + "/" + date.Month + "/" + date.Year;

                var sessions = from item in journalDoc.Descendants("Session")
                         where (string)item.Parent.Attribute("day") == day
                         select item;

                foreach (var session in sessions)
                {
                    sessionList.Add(new SessionItem(session.Elements("Name").ElementAt(0).Value, session.Elements("Description").ElementAt(0).Value,day));
                }
                return sessionList;
            }
            catch (Exception ex)
            {
                return sessionList;
            }
        }

        public static string DateToString(DateTimeOffset date)
        {
            return date.Day + "/" + date.Month + "/" + date.Year;
        }
        public static SessionItem? GetSession(string sessionName, DateTimeOffset date)
        {
            SessionItem session;

            try
            {

                string day = DateToString(date);

                var sessions = from item in journalDoc.Descendants("Session")
                               where (string)item.Parent.Attribute("day") == day
                               where (string)item.Element("Name") == sessionName
                               select item;

                var ses = sessions.FirstOrDefault();


                session = new SessionItem(ses.Element("Name").Value, ses.Element("Description").Value, day);
                
                return session;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ObservableCollection<string> GetExercisesNames(string sessionName, string date)
        {
            ObservableCollection<string> exercises = new ObservableCollection<string>();
            try
            {
                exercises.Clear();

                // var exercises = journalElement.Descendants("Exercise").Where(item => (string)item.Parent.Parent.Element("Name") == SessionName);

                var exer = from item in journalDoc.Descendants("Exercise")
                                where (string)item.Parent.Parent.Parent.Attribute("day") == date
                                where (string)item.Parent.Parent.Element("Name") == sessionName
                                select item;

                foreach (var exc in exer)
                {
                    exercises.Add(exc.Elements("Name").ElementAt(0).Value);
                }

                return exercises;

                

            }
            catch (Exception ex)
            {
                return exercises;
            }
        }

        public static ObservableCollection<ExerciseItem> GetExerciseListFromASession(string sessionName, string date)
        {
            ObservableCollection<ExerciseItem> exercises = new ObservableCollection<ExerciseItem>();
            try
            {
                exercises.Clear();

                // var exercises = journalElement.Descendants("Exercise").Where(item => (string)item.Parent.Parent.Element("Name") == SessionName);

                var exer = from item in journalDoc.Descendants("Exercise")
                           where (string)item.Parent.Parent.Parent.Attribute("day") == date
                           where (string)item.Parent.Parent.Element("Name") == sessionName
                           select item;

                foreach (var exc in exer)
                {
                    exercises.Add(new ExerciseItem(
                            exc.Element("Name").Value, 
                            exc.Element("Description").Value, 
                            exc.Element("Equipment").Value,
                            exc.Element("BodyPart").Value,
                            Int32.Parse(exc.Element("Repetitions").Value), 
                            Int32.Parse(exc.Element("Sets").Value), 
                            Int32.Parse(exc.Element("RestBetweenSets").Value),
                            Int32.Parse(exc.Element("ExtraWeight").Value)
                            )
                        );
                }

                return exercises;
            }
            catch (Exception ex)
            {
                return exercises;
            }
        }

        public static bool AddSession(string? name, string? description, DateTimeOffset date)
        {
            try
            {

                ObservableCollection<SessionItem> sessionList = GetSessionNames(date);

                foreach (var session in sessionList)
                {
                    if (name.ToLower() == session.SessionName.ToLower())
                    {
                        throw new Exception("There is another session with that name on this date");
                    }
                }

                string today = date.Day + "/" + date.Month + "/" + date.Year;

                var it = from item in journalDoc.Descendants("Day")
                         where (string)item.Attribute("day") == today
                         select item;

                XElement day = it.FirstOrDefault();

                //If there is no day element we create it and give its value to day variable to add the session to it
                if (day == null) 
                {
                    journalDoc.Root.Add(new XElement("Day", new XAttribute("day", today)));
                    day = (from item in journalDoc.Descendants("Day") where (string)item.Attribute("day") == today select item).FirstOrDefault();
                }

                //Adds a Session element with its elements to the current day element
                day.Add
                (
                    new XElement
                    (
                        "Session", 
                            new XAttribute("ID", 3),
                            new XElement("Name", name), 
                            new XElement("Description", description),
                            new XElement("ExerciseList")
                    )
                        
                );

                journalDoc.Save(journalFilePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool AddExercise(string sessionName, ExerciseItem exercise, DateTimeOffset date)
        {
            try
            {

                string today = date.Day + "/" + date.Month + "/" + date.Year;

                var it = from item in journalDoc.Descendants("ExerciseList")
                                        where (string)item.Parent.Parent.Attribute("day") == today
                                        && (string)item.Parent.Element("Name") == sessionName
                                        select item;

                XElement exerciseList = it.FirstOrDefault();


                exerciseList.Add
                (
                    new XElement
                    (
                        "Exercise",
                            new XElement("Name", exercise.Name),
                            new XElement("Description", exercise.Description),
                            new XElement("Equipment", exercise.Equipment),
                            new XElement("BodyPart", exercise.BodyPart),
                            new XElement("Repetitions", exercise.Repetitions),
                            new XElement("Sets", exercise.Sets),
                            new XElement("RestBetweenSets", exercise.RestBetweenSets),
                            new XElement("ExtraWeight", exercise.ExtraWeight)

                    )

                );

                journalDoc.Save(journalFilePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void DeleteSessionFromFile(string name, DateTimeOffset date)
        {
            string day = date.Day + "/" + date.Month + "/" + date.Year;

            try 
            {
                var del = from item in journalDoc.Descendants("Session")
                          where (string)item.Parent.Attribute("day") == day
                          where (string)item.Element("Name") == name
                          select item;

                del.Remove();

                journalDoc.Save(journalFilePath);
            }
            catch (Exception ex) 
            { 

            }
        }

        /// <summary>
        /// Given an Index (from a datagrid with the exercises), a Session Name and the date of the session, removes said exercise from the journal.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="date"></param>
        public static void DeleteExerciseIndexFromJournal(int? index, string name, DateTimeOffset date)
        {
            string day = date.Day + "/" + date.Month + "/" + date.Year;

            try
            {
                var del = from item in journalDoc.Descendants("Exercise")
                          where (string)item.Parent.Parent.Parent.Attribute("day") == day
                          where (string)item.Parent.Parent.Element("Name") == name
                          select item;

                del.ElementAt((int)index).Remove();

                journalDoc.Save(journalFilePath);
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// Edits sessiong name or description given a the original name, a new name, a new description and the date.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newName"></param>
        /// <param name="description"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool EditSession(string name, string newName, string description, DateTimeOffset date)
        {
            string day = DateToString(date);

            try
            {
                var sessions = from item in journalDoc.Descendants("Session")
                          where (string)item.Parent.Attribute("day") == day
                          select item;

                var ses = (from item in sessions
                          where (string)item.Element("Name") == name
                          select item).FirstOrDefault();

                foreach (var s in sessions)
                {
                    if (s.Element("Name").Value.ToLower() == newName.Trim().ToLower())
                        throw new Exception();
                }

                ses.Element("Name").Value = newName;
                ses.Element("Description").Value = description;

                journalDoc.Save(journalFilePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static ObservableCollection<string> GetExercisesList()
        {
            ObservableCollection<string> exerciseList = new ObservableCollection<string>();

            try
            {

                var el = from item in exercisesDoc.Descendants("Name")
                         select item.Value;

                foreach(string e in el)
                {
                    exerciseList.Add(e);
                }

                exerciseList = new ObservableCollection<string>(exerciseList.Order());

            }catch (Exception ex)
            {

            }

            return exerciseList;
        }
        public static ObservableCollection<ExerciseJsonItem> GetExercisesListFromJson()
        {
            ObservableCollection<ExerciseJsonItem> exerciseList = new ObservableCollection<ExerciseJsonItem>();
            

            try {

                string json = System.IO.File.ReadAllText("Assets\\exercises.json");
                var exercises = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExerciseJsonItem>>(json);

                exerciseList = new ObservableCollection<ExerciseJsonItem>(exercises);

            }
            catch (Exception ex)
            {

            }

            return exerciseList;
        }
    }
}
