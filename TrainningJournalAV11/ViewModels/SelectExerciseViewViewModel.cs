using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrainningJournalAV11.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrainningJournalAV11.ViewModels
{
    public class SelectExerciseViewViewModel : ReactiveObject
    {
        public enum Muscle
        {
            all,
            arms,
            legs,
            back,
            chest,
            abdominals,
            neck,
            traps,
            middle_back,
            lats,
            lower_back,
            shoulders,
            triceps,
            biceps,
            forearms,
            glutes,
            hamstrings,
            quadriceps,
            abductors,
            adductors,
            calves
        }

        private bool _IsPaneOpen = true;
        private bool _BackButtonVisible = false;
        private string _ChangingText = string.Empty;
        private ObservableCollection<Muscle> _MuscleList;
        private ObservableCollection<string> _MuscleListSTR;

        private ObservableCollection<ExerciseJsonItem> _ExerciseList;
        private ObservableCollection<ExerciseJsonItem> _SelectedExerciseList;

        private ObservableCollection<ExerciseJsonItem> _SearchList;

        public ReactiveCommand<Unit, ExerciseItem> OpenSelectExerciseCommand { get; }
        public bool IsPaneOpen
        {
            get
            {
                return _IsPaneOpen;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _IsPaneOpen, value);
            }
        }
        public bool BackButtonVisible
        {
            get
            {
                return _BackButtonVisible;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _BackButtonVisible, value);
            }
        }
        public string ChangingText
        {
            get
            {
                return _ChangingText;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _ChangingText, value);

                SearchExercise(ChangingText);


            }
        }

        public ObservableCollection<Muscle> MuscleList
        {
            get
            {
                return _MuscleList;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _MuscleList, value);
            }
        }
        public ObservableCollection<string> MuscleListSTR
        {
            get
            {
                return _MuscleListSTR;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _MuscleListSTR, value);
            }
        }

        public ObservableCollection<ExerciseJsonItem> ExerciseList
        {
            get
            {
                return _ExerciseList;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _ExerciseList, value);
            }
        }
        public ObservableCollection<ExerciseJsonItem> SelectedExerciseList
        {
            get
            {
                return _SelectedExerciseList;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedExerciseList, value);
            }
        }

        public ObservableCollection<ExerciseJsonItem> SearchList
        {
            get
            {
                return _SearchList;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _SearchList, value);
            }
        }

        private void SearchExercise(string changingText)
        {
            SearchList.Clear();
            string pattern = changingText;

            foreach (var el in SelectedExerciseList)
            {
                    if (Regex.IsMatch(el.name, pattern, RegexOptions.IgnoreCase))
                        SearchList.Add(el);
            }
        }

        public SelectExerciseViewViewModel()
        {
            MuscleList = new ObservableCollection<Muscle>(Enum.GetValues(typeof(Muscle)).Cast<Muscle>().ToList());
            ExerciseList = XMLUtilities.GetExercisesListFromJson();
            SelectedExerciseList = XMLUtilities.GetExercisesListFromJson();
            SearchList = XMLUtilities.GetExercisesListFromJson();
            IsPaneOpen = true;
            PaneOpenCloseCommand = ReactiveCommand.Create<Muscle>(muscle => PaneOpenClose(muscle));
            OpenSelectExerciseCommand = ReactiveCommand.Create(() =>
            {
                return new ExerciseItem();
            }
            );
        }

        public ReactiveCommand<Muscle, Unit> PaneOpenCloseCommand { get; }
        private void PaneOpenClose(Muscle? muscle)
        {
            IsPaneOpen = !IsPaneOpen;
            if (!IsPaneOpen)
            {
                MuscleList.Clear();
                BackButtonVisible = true;
                ChangingText = string.Empty;
                SetSearchAndExerciseList(muscle);
            }

            else
            {
                BackButtonVisible = false;
                MuscleList = new ObservableCollection<Muscle>(Enum.GetValues(typeof(Muscle)).Cast<Muscle>().ToList());
            }
        }

        private void SetSearchAndExerciseList(Muscle? muscle)
        {
            if (muscle.ToString() == "all" || muscle == null)
            {
                SelectedExerciseList = XMLUtilities.GetExercisesListFromJson();
                SearchList = XMLUtilities.GetExercisesListFromJson();
                return;
            }

            SelectedExerciseList.Clear();
            foreach (var el in ExerciseList)
            {
                foreach (var el2 in el.primaryMuscles)
                {
                    if (Regex.IsMatch(el2, muscle.ToString(), RegexOptions.IgnoreCase))
                        SelectedExerciseList.Add(el);
                }
                foreach (var el2 in el.secondaryMuscles)
                {
                    if (Regex.IsMatch(el2, muscle.ToString(), RegexOptions.IgnoreCase))
                        SelectedExerciseList.Add(el);
                }
            }
            SearchList = new ObservableCollection<ExerciseJsonItem>(SelectedExerciseList);
        }

    }
}
