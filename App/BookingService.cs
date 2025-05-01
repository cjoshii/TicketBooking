public interface IBookingService
{
    Task<bool> Reserve(long eventId, List<long> tickets);
    Task<bool> ConfirmTicket(long reservationId);
    Task<bool> ReleaseTicket(int eventId, List<long> tickets);
}

public class BookingService : IBookingService
{
    private readonly IInventoryService _inventoryService;
    private readonly IPaymentService _paymentService;

    public BookingService(IInventoryService inventoryService, IPaymentService paymentService)
    {
        _inventoryService = inventoryService;
        _paymentService = paymentService;
    }

    public async Task<bool> Reserve(long eventId, List<long> tickets)
    {
        var reservationSuccess = await _inventoryService.ReserveTicketAsync(eventId, tickets);
        if (!reservationSuccess)
        {
            Console.WriteLine("Failed to reserve tickets.");
            return false;
        }

        var totalAmount = tickets.Sum(ticket => ticket);
        var paymentSuccess = await _paymentService.ProcessPaymentAsync(totalAmount);
        if (!paymentSuccess)
        {
            Console.WriteLine("Payment failed. Releasing reserved tickets.");
            await _inventoryService.ReleaseTicketAsync(eventId, tickets);
            return false;
        }

        Console.WriteLine("Tickets reserved and payment processed successfully.");
        return true;
    }
    public async Task<bool> ConfirmTicket(long reservationId)
    {
        var confirmationSuccess = await _inventoryService.ConfirmTicketAsync(reservationId);
        if (!confirmationSuccess)
        {
            Console.WriteLine("Failed to confirm ticket.");
            return false;
        }

        Console.WriteLine("Ticket confirmed successfully.");
        return true;
    }
    public async Task<bool> ReleaseTicket(int eventId, List<long> tickets)
    {
        var releaseSuccess = await _inventoryService.ReleaseTicketAsync(eventId, tickets);
        if (!releaseSuccess)
        {
            Console.WriteLine("Failed to release tickets.");
            return false;
        }

        Console.WriteLine("Tickets released successfully.");
        return true;
    }
}