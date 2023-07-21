using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using TrainningJournalAV11.Models;
using System.Reactive.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;
using System.Reactive;

namespace TrainningJournalAV11.ViewModels;

public class MainViewModel : ReactiveObject
{
    

    public Interaction<AddSessionViewViewModel, SessionItem?> ShowDialog { get; }
    public Interaction<AddExerciseViewViewModel, ExerciseItem?> ShowDialogAddExercise { get; }
    public ICommand AddSessionCommand { get; }
    public ICommand AddExerciseCommand { get; }

    private DateTimeOffset _SelDate = new DateTimeOffset(DateTime.Today);

    private ObservableCollection<SessionItem> _Sessions = new ObservableCollection<SessionItem>() { };

    public ObservableCollection<SessionItem> Sessions
    {
        get
        {
            return _Sessions;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _Sessions, value);
        }
    }

    public DateTimeOffset SelDate
    {
        get
        {
            return _SelDate;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _SelDate, value);
            Sessions.Clear();
            Sessions = XMLUtilities.GetSessionNames(SelDate);
        }
    }

    public MainViewModel()
    {
        Sessions = XMLUtilities.GetSessionNames(_SelDate);
        ShowDialog = new Interaction<AddSessionViewViewModel, SessionItem?>();
        ShowDialogAddExercise = new Interaction<AddExerciseViewViewModel, ExerciseItem?>();

        AddSessionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addSessionView = new AddSessionViewViewModel(SelDate);

            var result = await ShowDialog.Handle(addSessionView);
            if (result != null) Sessions = XMLUtilities.GetSessionNames(_SelDate);
        });

        AddExerciseCommand = ReactiveCommand.CreateFromTask<string>(AddExerciseCommandTask);

    }


    public async Task AddExerciseCommandTask(string name)
    {
        var addExerciseView = new AddExerciseViewViewModel(name, _SelDate);

        var result = await ShowDialogAddExercise.Handle(addExerciseView);
        if (result != null) Sessions = XMLUtilities.GetSessionNames(_SelDate);
    }

    /// <summary>
    /// Given a SessioName as binding, and using the current date selected deletes a session with that name in that date.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task DeleteSession(string name)
    {
        
        List<SessionItem> list = new List<SessionItem>(Sessions);
        list.RemoveAll(x => x.SessionName == name);

        //Create a Message box to make sure the user wants to delete the session
        var box = MessageBoxManager
                  .GetMessageBoxStandard("Warning", "Are you sure you would like to delete \""+ name +"\"?",
                      ButtonEnum.YesNo);
        var result = await box.ShowAsync();

        //If the user selects Yes, we proceed to delete the Session and renew the Sessions for the day to update the interface
        if (result == ButtonResult.Yes)
        {
            XMLUtilities.DeleteSessionFromFile(name, _SelDate);
            Sessions = XMLUtilities.GetSessionNames(_SelDate);
        }
        
    }


    /// <summary>
    /// Increases by one day the current selected date.
    /// </summary>
    public void SelectNextDay()
    {
        SelDate = SelDate.AddDays(1);
    }

    /// <summary>
    /// Decreases by one day the current selected date.
    /// </summary>
    public void SelectPreviousDay()
    {
        SelDate = SelDate.Subtract(TimeSpan.FromDays(1));
    }
}
