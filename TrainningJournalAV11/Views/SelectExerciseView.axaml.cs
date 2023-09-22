using Avalonia.Controls;
using System;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TrainningJournalAV11.ViewModels;

namespace TrainningJournalAV11.Views
{
    public partial class SelectExerciseView : ReactiveWindow<SelectExerciseViewViewModel>
    {
        public SelectExerciseView()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated(disposable => disposable(ViewModel!.OpenSelectExerciseCommand.Subscribe(Close)));

        }
    }
}
