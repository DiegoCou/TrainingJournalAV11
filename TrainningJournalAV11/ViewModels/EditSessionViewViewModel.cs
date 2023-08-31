using System;
using System.Collections.Generic;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using TrainningJournalAV11.Models;

namespace TrainningJournalAV11.ViewModels
{
	public class EditSessionViewViewModel : ReactiveObject
	{
		private string _SessionName;
		private string _Description;
        private string oldName;

		public string SessionName { get => _SessionName; set => this.RaiseAndSetIfChanged(ref _SessionName, value); }
		public string Description { get => _Description; set => this.RaiseAndSetIfChanged(ref _Description, value); }
        public ReactiveCommand<Unit, string> EditSessionCommand { get; }
        public EditSessionViewViewModel(string sessionName, DateTimeOffset date)
		{
            oldName = sessionName;
			SessionName = sessionName;
			Description = XMLUtilities.GetSession(sessionName, date).Description;

            EditSessionCommand = ReactiveCommand.Create(() =>
            {
                return EditSession(date);
            });

		}

        private string? EditSession(DateTimeOffset date)
        {
            if (string.IsNullOrEmpty(SessionName) || SessionName == "")
                return "";

            bool result = XMLUtilities.EditSession(oldName, SessionName, Description, date);
            if (result == false)
                return null;
            return "";
        }
    }
}