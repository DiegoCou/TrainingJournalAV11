using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TrainningJournalAV11.ViewModels;

namespace TrainningJournalAV11.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
}
