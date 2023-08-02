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
using Avalonia.Controls;
using TrainningJournalAV11.Views.Controls;

namespace TrainningJournalAV11.ViewModels;

public class MainViewModel : ReactiveObject
{
    

    public Interaction<AddSessionViewViewModel, SessionItem?> ShowDialog { get; }
    public Interaction<AddExerciseViewViewModel, ExerciseItem?> ShowDialogAddExercise { get; }

    public Interaction<EditSessionViewViewModel, string?> ShowDialogEditSession { get; }
    public ICommand AddSessionCommand { get; }
    public ICommand AddExerciseCommand { get; }
    public ICommand EditSessionCommand { get; }

    private DateTimeOffset _SelDate = new DateTimeOffset(DateTime.Today);

    private ObservableCollection<SessionItem> _Sessions = new ObservableCollection<SessionItem>() { };
    private ExerciseItem _SelectedExercise = new ExerciseItem("", null, null, null, null, null, null, null);

    public ExerciseItem SelectedExercise
    {
        get
        {
            return _SelectedExercise;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _SelectedExercise, value);
        }
    }
    public int SelectedIndex { get; set; }

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
        ShowDialogEditSession = new Interaction<EditSessionViewViewModel, string?>();

        AddSessionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addSessionView = new AddSessionViewViewModel(SelDate);

            var result = await ShowDialog.Handle(addSessionView);
            if (result != null) Sessions = XMLUtilities.GetSessionNames(_SelDate);
        });

        AddExerciseCommand = ReactiveCommand.CreateFromTask<string>(AddExerciseCommandTask);

        EditSessionCommand = ReactiveCommand.CreateFromTask<string>(EditSessionCommandTask);
    }

    public async Task EditSessionCommandTask(string name)
    {
        var editSessionView = new EditSessionViewViewModel(name, _SelDate);

        var result = await ShowDialogEditSession.Handle(editSessionView);
        if (result != null) Sessions = XMLUtilities.GetSessionNames(_SelDate);
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

    public async Task DeleteExercise(string sessionName)
    {
         var box = MessageBoxManager
                  .GetMessageBoxStandard("Warning", $"Are you sure you would like to delete \"{SelectedExercise.Name}\" from \"{sessionName}\"?",
                      ButtonEnum.YesNo);
        var result = await box.ShowAsync();

        if(result  == ButtonResult.Yes && SelectedIndex!=null)
        {
            
            XMLUtilities.DeleteExerciseIndexFromJournal(SelectedIndex, sessionName, SelDate);
            Sessions = XMLUtilities.GetSessionNames(SelDate);
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
