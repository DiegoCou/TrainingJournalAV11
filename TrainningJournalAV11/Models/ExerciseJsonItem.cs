using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainningJournalAV11.Models
{
    public class ExerciseJsonItem
    {
        public string name {  get; set; }
        public string froce { get; set; }
        public string level { get; set; }
        public string mechanic { get; set; }
        public string equipment { get; set; }
        public List<string> primaryMuscles { get; set; }
        public List<string> secondaryMuscles { get;set; }
        public List<string> instructions { get; set; }
        public string category { get; set; }
        public List<string> images { get; set; }
        public string id { get; set; }
    }
}