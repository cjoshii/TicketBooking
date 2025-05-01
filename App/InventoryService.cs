public interface IInventoryService
{
    Task<bool> ReserveTicketAsync(long eventId, List<long> tickets);
    Task<bool> ConfirmTicketAsync(long reservationId);
    Task<bool> ReleaseTicketAsync(long eventId, List<long> tickets);
}

public class InventoryService : IInventoryService
{
    private readonly Dictionary<long, Dictionary<long, ITicket>> _eventTickets = new();
    private readonly Dictionary<long, Dictionary<long, Lock>> _seatLocks = new();

    public InventoryService()
    {
        // Initialize with some sample data
        var event1 = new Event(1, "Concert", "Live concert", DateTime.Now.AddDays(30), "Stadium");
        var tickets = new List<ITicket>
        {
            new Ticket(100, 1, TicketStatus.Available,1),
            new Ticket(100, 2, TicketStatus.Available,2),
            new Ticket(100, 3, TicketStatus.Available,3),
        };
        _eventTickets[event1.Id] = tickets.ToDictionary(t => t.TicketId, t => t);
    }

    public Task<bool> ReserveTicketAsync(long eventId, List<long> tickets)
    {
        if (_eventTickets.TryGetValue(eventId, out var availableTickets))
        {
            foreach (var ticket in tickets.OrderBy(t => t))
            {
                // Lock the seat
                var seatLock = GetSeatLock(eventId, ticket);
                if (_eventTickets[eventId][ticket].Status != TicketStatus.Available)
                {
                    Console.WriteLine($"Ticket {ticket} is not available for reservation.");
                    return Task.FromResult(false);
                }

                lock (seatLock)
                {
                    _eventTickets[eventId][ticket].Status = TicketStatus.Reserved;
                }
            }
            return Task.FromResult(true);
        }
        else
        {
            Console.WriteLine($"Event with ID {eventId} not found.");
            return Task.FromResult(false);
        }
    }

    private Lock GetSeatLock(long eventId, long ticketId)
    {
        if (!_seatLocks.ContainsKey(eventId))
        {
            _seatLocks[eventId] = new Dictionary<long, Lock>();
        }

        if (!_seatLocks[eventId].ContainsKey(ticketId))
        {
            _seatLocks[eventId][ticketId] = new Lock();
        }

        return _seatLocks[eventId][ticketId];
    }

    public Task<bool> ConfirmTicketAsync(long reservationId)
    {
        foreach (var eventTickets in _eventTickets.Values)
        {
            foreach (var ticket in eventTickets.Values)
            {
                if (ticket.TicketId == reservationId && ticket.Status == TicketStatus.Reserved)
                {
                    ticket.Status = TicketStatus.Sold;
                    Console.WriteLine($"Ticket {reservationId} confirmed.");
                    return Task.FromResult(true);
                }
            }
        }
        Console.WriteLine($"Ticket {reservationId} not found or not reserved.");
        return Task.FromResult(false);
    }

    public Task<bool> ReleaseTicketAsync(long eventId, List<long> tickets)
    {
        throw new NotImplementedException();
    }
}