using ReactiveUI;

namespace PoC.ReactiveCommandBubbling
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            People = new ReactiveList<PersonViewModel>();
            People.Add(new PersonViewModel("Jerry", "Seinfeld"));
            People.Add(new PersonViewModel("George", "Costanza"));
        }

        public ReactiveList<PersonViewModel> People { get; private set; }
    }

    public class PersonViewModel : ReactiveObject
    {
        public PersonViewModel(string first, string last)
        {
            FirstName = first;
            LastName = last;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
