using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TrainningJournalAV11.ViewModels;
using System;

namespace TrainningJournalAV11.Views
{
    public partial class EditSessionView : ReactiveWindow<EditSessionViewViewModel>
    {
        public EditSessionView()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated(disposable => disposable(ViewModel!.EditSessionCommand.Subscribe(Close)));
        }
    }
}
