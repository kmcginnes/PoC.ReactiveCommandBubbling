Bubble Commands with ReactiveUI
===============================

So often in MVVM do we run across the situation where, in a list of elements we must perform an operation involving both the single element and the list itself.

The cannonical example is the delete button on each element in the list. The actionable item exists on the singular element, but the action must manipulate the list as a whole.

There are many ways to solve this problem: Caliburn Actions, RelativeSource Ancestor, event aggregation, etc.

Using ReactiveUI's ReactiveCommand coupled with ReactiveList we can handle this scenario with grace.

Solution
--------

We can declare the command on the child view model.

```c#
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
```

This allows easy binding of the command in the view.

```XML
<ItemsControl ItemsSource="{Binding People}">
    <ItemsControl.ItemTemplate>
        <DataTemplate DataType="{x:Type local:PersonViewModel}">
            <st:AutoGrid Columns="Auto,*">
                <TextBlock FontSize="18">
                    <Run Text="{Binding FirstName}"/>
                    <Run Text="{Binding LastName}"/>
                </TextBlock>
                <Button Content="Delete" 
                        Command="{Binding Delete}" 
                        CommandParameter="{Binding}" />
            </st:AutoGrid>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
</ItemsControl>
```
__Note:__ Snipped some styling

The parent view model can subscribe to the commands execution. The execution still is originating from the command on the item, but we can simply listen for its execution from anywhere the instance is available. Since the item is being passed as the parameter, we have access to all of its public values.

```c#
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
```

We use the convenient extension method `ActOnEveryObject` off of ReactiveList to ensure that as new items are added, the delete handler will be wired up.


Conclusion
----------

This technique is a great way to simplify what should be an easy task.
