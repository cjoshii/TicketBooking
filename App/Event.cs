public interface IEvent
{
    long Id { get; }
    string Name { get; }
    string Description { get; }
    DateTime Date { get; }
    string Location { get; }
}

public class Event : IEvent
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }

    public Event(long id, string name, string description, DateTime date, string location)
    {
        Id = id;
        Name = name;
        Description = description;
        Date = date;
        Location = location;
    }
}