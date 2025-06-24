namespace WeddingWonderAPI.BGService
{
    public class EmailBGService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailBGService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        Services.BGService bgService = scope.ServiceProvider.GetRequiredService<Services.BGService>();

                        await bgService.ChangeStatusOp1Op2();
                        await bgService.ChangeStatusConfirmFinish();
                        await bgService.ChangeStatusRejectAll();
                        await bgService.PaymentNotice();
                        await bgService.AutomationPayment();
                    }

                    Console.WriteLine($"Execution time: {DateTime.Now}");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }
    }
}
