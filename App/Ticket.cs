public interface ITicket
{
    long TicketId { get; }
    decimal Price { get; }
    int SeatNumber { get; }
    TicketStatus Status { get; set; }
}

public class Ticket : ITicket
{
    public long TicketId { get; set; }
    public decimal Price { get; set; }
    public int SeatNumber { get; set; }
    public TicketStatus Status { get; set; }

    public Ticket(decimal price, int seatNumber, TicketStatus status, long ticketId)
    {
        TicketId = ticketId;
        Price = price;
        SeatNumber = seatNumber;
        Status = status;
    }
}

public enum TicketStatus
{
    Available,
    Sold,
    Reserved
}
