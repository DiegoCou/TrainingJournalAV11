using Avalonia.Controls;
using System;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;
using TrainningJournalAV11.Models;
using TrainningJournalAV11.ViewModels;

namespace TrainningJournalAV11.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.ShowDialogAddExercise.RegisterHandler(DoShowDialogAsync)));
    }
    private async Task DoShowDialogAsync(InteractionContext<AddSessionViewViewModel, SessionItem?> interaction)
    {
        var dialog = new AddSessionView();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<SessionItem?>(this);
        interaction.SetOutput(result);
    }
    private async Task DoShowDialogAsync(InteractionContext<AddExerciseViewViewModel, ExerciseItem?> interaction)
    {
        var dialog = new AddExerciseView();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<ExerciseItem?>(this);
        interaction.SetOutput(result);
    }

    
}
