using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace PoC.ReactiveCommandBubbling
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            People = new ReactiveList<PersonViewModel>();
            People.ActOnEveryObject(x => x.Delete.Subscribe(p => People.Remove((PersonViewModel) p)), _ => { });
            People.AddRange(new[]
            {
                new PersonViewModel("Jerry", "Seinfeld"),
                new PersonViewModel("George", "Costanza"),
                new PersonViewModel("Elaine", "Something"),
                new PersonViewModel("Newman", "Something"),
            });
        }

        public ReactiveList<PersonViewModel> People { get; private set; }
    }

    public class PersonViewModel : ReactiveObject
    {
        public PersonViewModel(string first, string last)
        {
            FirstName = first;
            LastName = last;

            Delete = ReactiveCommand.Create(Observable.Return(true));
        }

        public ReactiveCommand<object> Delete { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
