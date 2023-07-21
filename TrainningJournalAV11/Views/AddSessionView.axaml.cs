using Avalonia.Controls;
using System;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TrainningJournalAV11.ViewModels;

namespace TrainningJournalAV11.Views
{
    public partial class AddSessionView : ReactiveWindow<AddSessionViewViewModel>
    {
        public AddSessionView()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated(disposable => disposable(ViewModel!.AddSessionComand.Subscribe(Close)));
        }
    }
}
