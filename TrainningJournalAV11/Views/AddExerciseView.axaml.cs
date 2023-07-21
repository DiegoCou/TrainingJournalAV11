using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TrainningJournalAV11.ViewModels;
using System;

namespace TrainningJournalAV11.Views
{
    public partial class AddExerciseView : ReactiveWindow<AddExerciseViewViewModel>
    {
        public AddExerciseView()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated(disposable => disposable(ViewModel!.AddExerciseCommand.Subscribe(Close)));
        }


    }
}
