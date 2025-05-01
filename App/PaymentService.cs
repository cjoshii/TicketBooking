public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(decimal amount);
}
public class PaymentService : IPaymentService
{
    public Task<bool> ProcessPaymentAsync(decimal amount)
    {
        // Simulate payment processing logic
        Console.WriteLine($"Processing payment of {amount:C}");
        // Simulate a delay
        Task.Delay(2000).Wait();
        // Simulate a successful payment
        Console.WriteLine("Payment processed successfully.");
        return Task.FromResult(new Random().Next(0, 2) == 1);
    }
}