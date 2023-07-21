using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainningJournalAV11.Models
{
    public class ExerciseItem
    {

        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Equipment { get; set; }
        public string? BodyPart { get; set; }
        public int? Repetitions { get; set; }
        public int? Sets { get; set; }
        public int? RestBetweenSets { get; set; }
        public int? ExtraWeight { get; set; }


        public ExerciseItem(string name, string? description,string? equipment, string? bodyPart, int? repetitions, int? sets, int? restBetweenSets, int? extraWeight) 
        { 
            Name = name;
            Description = description;
            Repetitions = repetitions;
            Sets = sets;
            RestBetweenSets = restBetweenSets;
            ExtraWeight = extraWeight;
            Equipment = equipment;
            BodyPart = bodyPart;


        }

    }
}
