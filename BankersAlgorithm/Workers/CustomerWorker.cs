using BankersAlgorithm.Models;

namespace BankersAlgorithm.Workers
{
    public class CustomerWorker
    {
        private readonly Bank _bank;
        private readonly int _customerId;
        private readonly Random _random;

        public CustomerWorker(Bank bank, int customerId)
        {
            _bank = bank;
            _customerId = customerId;
            _random = new Random(Guid.NewGuid().GetHashCode());
        }

        public void Run()
        {
            for (int step = 0; step < 10; step++)
            {
                Thread.Sleep(_random.Next(300, 900));

                if (_bank.HasRemainingNeed(_customerId))
                {
                    int[] request = _bank.GenerateRandomRequest(_customerId);

                    if (request.Any(r => r > 0))
                    {
                        _bank.RequestResources(_customerId, request);
                    }
                }

                Thread.Sleep(_random.Next(300, 900));

                if (_bank.HasAllocatedResources(_customerId))
                {
                    int[] release = _bank.GenerateRandomRelease(_customerId);

                    if (release.Any(r => r > 0))
                    {
                        _bank.ReleaseResources(_customerId, release);
                    }
                }
            }

            while (_bank.HasAllocatedResources(_customerId))
            {
                int[] release = _bank.GenerateRandomRelease(_customerId);

                if (release.Any(r => r > 0))
                {
                    _bank.ReleaseResources(_customerId, release);
                }
            }

            Console.WriteLine($"Cliente {_customerId} finalizou.");
        }
    }
}
