using BankersAlgorithm.Models;
using BankersAlgorithm.Workers;

namespace BankersAlgorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Uso: dotnet run -- <r1> <r2> <r3> ...");
                Console.WriteLine("Exemplo: dotnet run -- 10 5 7");
                return;
            }

            int[] resources;

            try
            {
                resources = args.Select(int.Parse).ToArray();

                if (resources.Any(r => r < 0))
                {
                    Console.WriteLine("Erro: os recursos devem ser valores inteiros não negativos.");
                    return;
                }
            }
            catch
            {
                Console.WriteLine("Erro: todos os argumentos devem ser números inteiros válidos.");
                return;
            }

            const int numberOfCustomers = 5;

            var bank = new Bank(numberOfCustomers, resources);

            Console.WriteLine("Inicializando banco...");
            Console.WriteLine($"Clientes: {numberOfCustomers}");
            Console.WriteLine($"Recursos disponíveis iniciais: [{string.Join(", ", resources)}]");
            Console.WriteLine();

            bank.GenerateRandomMaximum();
            Console.WriteLine("Matriz Maximum gerada:");
            bank.PrintMaximum();
            bank.PrintState();

            List<Thread> threads = new();

            for (int i = 0; i < numberOfCustomers; i++)
            {
                int customerId = i;
                Thread thread = new(() =>
                {
                    var worker = new CustomerWorker(bank, customerId);
                    worker.Run();
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Execução encerrada.");
            bank.PrintState();
        }
    }
}
