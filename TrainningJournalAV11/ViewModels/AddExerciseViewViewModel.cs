using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using TrainningJournalAV11.Models;

namespace TrainningJournalAV11.ViewModels
{
    public class AddExerciseViewViewModel : ReactiveObject
    {

        public ObservableCollection<ExerciseJsonItem> exerciseList { get; set; }
        private ExerciseJsonItem _Exercise;
        private string? _ExerciseName;
        private int _ExerciseSelectedIndex;
        private int _Repetitions;
        private int _Sets;
        private int _RestBetweenSets;
        private int _ExtraWeight;
        private string _Instructions;

        public string Instructions { get => _Instructions; set=> this.RaiseAndSetIfChanged(ref _Instructions, value); }
        public int ExerciseSelectedIndex 
        { 
            get => _ExerciseSelectedIndex; 
            
            set { 
                this.RaiseAndSetIfChanged(ref _ExerciseSelectedIndex, value); 
                ExerciseName = exerciseList[_ExerciseSelectedIndex].name; 
                Exercise = exerciseList[ExerciseSelectedIndex];
                Instructions = string.Join("\n\n", _Exercise.instructions);
            } 
        }
        public int Repetitions { get => _Repetitions; set => this.RaiseAndSetIfChanged(ref _Repetitions, value); }
        public string? ExerciseName { get => _ExerciseName; set => this.RaiseAndSetIfChanged(ref _ExerciseName, value); }
        public int Sets { get => _Sets; set => this.RaiseAndSetIfChanged(ref _Sets, value); }
        public int RestBetweenSets { get => _RestBetweenSets; set => this.RaiseAndSetIfChanged(ref _RestBetweenSets, value); }
        public int ExtraWeight { get => _ExtraWeight; set => this.RaiseAndSetIfChanged(ref _ExtraWeight, value); }
        private ExerciseJsonItem Exercise { get => _Exercise; set { this.RaiseAndSetIfChanged(ref _Exercise, value); } }
        public ReactiveCommand<Window, Unit> CloseWindowCommand { get; }
        public ReactiveCommand<Unit, ExerciseItem> AddExerciseCommand { get; }
        

        public AddExerciseViewViewModel(string SessionName, DateTimeOffset date)
        {
            exerciseList = XMLUtilities.GetExercisesListFromJson();
            _ExerciseSelectedIndex = 0;
            _ExerciseName = exerciseList[_ExerciseSelectedIndex].name;
            _Exercise = exerciseList[_ExerciseSelectedIndex];
            _Instructions = string.Join("\n\n", _Exercise.instructions);
            _ExtraWeight = 0;
            _RestBetweenSets = 0;
            _Repetitions = 0;
            _Sets = 0;

            AddExerciseCommand = ReactiveCommand.Create(() =>
                {
                    ExerciseItem exercise = new ExerciseItem(_Exercise.name, null, null, null, _Repetitions,_Sets,_RestBetweenSets,_ExtraWeight);

                    if(XMLUtilities.AddExercise(SessionName, exercise, date))
                        return exercise;

                    return null;
                }
            );

            CloseWindowCommand = ReactiveCommand.Create<Window>(CloseWindow);
        }

        public void CloseWindow(Window window)
        {
            if (window != null) window.Close();
        }
    }
}
