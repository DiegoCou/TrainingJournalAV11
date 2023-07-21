using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using TrainningJournalAV11.Models;
using TrainningJournalAV11.Views;

namespace TrainningJournalAV11.ViewModels
{
    public class AddSessionViewViewModel : ReactiveObject
    {
        public DateTimeOffset SelDate { get; set; }
        private string? _SessionName;
        private string? _Description;
        private SessionItem Session;
        public ReactiveCommand<Unit, SessionItem?> AddSessionComand { get; }
        public ReactiveCommand<Window, Unit> CloseWindowCommand { get; }
        public string? SessionName 
        {
            get 
            {
                return _SessionName;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _SessionName, value);
            }
        }
        public string? Description
        {
            get
            {
                return _Description;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _Description, value);
            }
        }

        public AddSessionViewViewModel(DateTimeOffset date)
        {
            SelDate = date;
            string day = date.Day + "/" + date.Month + "/" + date.Year;
            AddSessionComand = ReactiveCommand.Create(() =>
            {
                if (string.IsNullOrEmpty(SessionName) || SessionName=="")
                    return null;

                if (!XMLUtilities.AddSession(SessionName, Description, SelDate))
                    return null;
                Session = new SessionItem(SessionName, Description,day);
                return Session;

            });

            CloseWindowCommand = ReactiveCommand.Create<Window>(CloseWindow);
        }
        public void CloseWindow(Window window)
        {
            if(window != null) window.Close();
        }

    }
}
