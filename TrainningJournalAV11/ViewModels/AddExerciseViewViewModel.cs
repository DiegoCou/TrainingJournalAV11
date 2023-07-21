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

        public ObservableCollection<string> exerciseList { get; set; }

        private ExerciseItem exercise { get; set; }

        private string? _ExerciseName;
        private int _ExerciseSelectedIndex;
        private int _Repetitions;
        private int _Sets;
        private int _RestBetweenSets;
        private int _ExtraWeight;
        public int ExerciseSelectedIndex { get => _ExerciseSelectedIndex; set { this.RaiseAndSetIfChanged(ref _ExerciseSelectedIndex, value); ExerciseName = exerciseList[_ExerciseSelectedIndex]; } }
        public int Repetitions { get => _Repetitions; set => this.RaiseAndSetIfChanged(ref _Repetitions, value); }
        public string? ExerciseName { get => _ExerciseName; set => this.RaiseAndSetIfChanged(ref _ExerciseName, value); }
        public int Sets { get => _Sets; set => this.RaiseAndSetIfChanged(ref _Sets, value); }
        public int RestBetweenSets { get => _RestBetweenSets; set => this.RaiseAndSetIfChanged(ref _RestBetweenSets, value); }
        public int ExtraWeight { get => _ExtraWeight; set => this.RaiseAndSetIfChanged(ref _ExtraWeight, value); }

        public ReactiveCommand<Window, Unit> CloseWindowCommand { get; }
        public ReactiveCommand<Unit, ExerciseItem> AddExerciseCommand { get; }
        

        public AddExerciseViewViewModel(string SessionName, DateTimeOffset date)
        {
            exerciseList = XMLUtilities.GetExercisesList();
            _ExerciseSelectedIndex = 0;
            _ExerciseName = exerciseList[_ExerciseSelectedIndex];
            _ExtraWeight = 0;
            _RestBetweenSets = 0;
            _Repetitions = 0;
            _Sets = 0;

            AddExerciseCommand = ReactiveCommand.Create(() =>
                {
                    ExerciseItem exercise = new ExerciseItem(_ExerciseName, null, null, null, _Repetitions,_Sets,_RestBetweenSets,_ExtraWeight);

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
