namespace BankersAlgorithm.Models
{
    public class Bank
    {
        private readonly object _lockObj = new();
        private readonly Random _random = new();

        public int NumberOfCustomers { get; }
        public int NumberOfResources { get; }

        public int[] Available { get; }
        public int[,] Maximum { get; }
        public int[,] Allocation { get; }
        public int[,] Need { get; }

        public Bank(int numberOfCustomers, int[] resources)
        {
            NumberOfCustomers = numberOfCustomers;
            NumberOfResources = resources.Length;

            Available = new int[NumberOfResources];
            Array.Copy(resources, Available, NumberOfResources);

            Maximum = new int[NumberOfCustomers, NumberOfResources];
            Allocation = new int[NumberOfCustomers, NumberOfResources];
            Need = new int[NumberOfCustomers, NumberOfResources];
        }

        public void GenerateRandomMaximum()
        {
            lock (_lockObj)
            {
                for (int i = 0; i < NumberOfCustomers; i++)
                {
                    for (int j = 0; j < NumberOfResources; j++)
                    {
                        Maximum[i, j] = _random.Next(0, Available[j] + 1);
                        Need[i, j] = Maximum[i, j];
                        Allocation[i, j] = 0;
                    }
                }
            }
        }

        public int RequestResources(int customerNum, int[] request)
        {
            lock (_lockObj)
            {
                Console.WriteLine();
                Console.WriteLine($"Cliente {customerNum} solicitando: [{string.Join(", ", request)}]");

                for (int i = 0; i < NumberOfResources; i++)
                {
                    if (request[i] > Need[customerNum, i])
                    {
                        Console.WriteLine("Pedido negado: excede a necessidade do cliente.");
                        return -1;
                    }
                }

                for (int i = 0; i < NumberOfResources; i++)
                {
                    if (request[i] > Available[i])
                    {
                        Console.WriteLine("Pedido negado: recursos insuficientes no momento.");
                        return -1;
                    }
                }

                for (int i = 0; i < NumberOfResources; i++)
                {
                    Available[i] -= request[i];
                    Allocation[customerNum, i] += request[i];
                    Need[customerNum, i] -= request[i];
                }

                if (!IsSafe())
                {
                    for (int i = 0; i < NumberOfResources; i++)
                    {
                        Available[i] += request[i];
                        Allocation[customerNum, i] -= request[i];
                        Need[customerNum, i] += request[i];
                    }

                    Console.WriteLine("Pedido negado: deixaria o sistema em estado inseguro.");
                    PrintState();
                    return -1;
                }

                Console.WriteLine($"Pedido aprovado para cliente {customerNum}.");
                PrintState();
                return 0;
            }
        }

        public int ReleaseResources(int customerNum, int[] release)
        {
            lock (_lockObj)
            {
                Console.WriteLine();
                Console.WriteLine($"Cliente {customerNum} liberando: [{string.Join(", ", release)}]");

                for (int i = 0; i < NumberOfResources; i++)
                {
                    if (release[i] > Allocation[customerNum, i])
                    {
                        Console.WriteLine("Liberação inválida: cliente tentando liberar mais do que possui.");
                        return -1;
                    }
                }

                for (int i = 0; i < NumberOfResources; i++)
                {
                    Available[i] += release[i];
                    Allocation[customerNum, i] -= release[i];
                    Need[customerNum, i] += release[i];
                }

                Console.WriteLine($"Recursos liberados pelo cliente {customerNum}.");
                PrintState();
                return 0;
            }
        }

        public int[] GenerateRandomRequest(int customerNum)
        {
            int[] request = new int[NumberOfResources];

            lock (_lockObj)
            {
                for (int i = 0; i < NumberOfResources; i++)
                {
                    request[i] = Need[customerNum, i] > 0
                        ? _random.Next(0, Need[customerNum, i] + 1)
                        : 0;
                }
            }

            return request;
        }

        public int[] GenerateRandomRelease(int customerNum)
        {
            int[] release = new int[NumberOfResources];

            lock (_lockObj)
            {
                for (int i = 0; i < NumberOfResources; i++)
                {
                    release[i] = Allocation[customerNum, i] > 0
                        ? _random.Next(0, Allocation[customerNum, i] + 1)
                        : 0;
                }
            }

            return release;
        }

        public bool HasRemainingNeed(int customerNum)
        {
            lock (_lockObj)
            {
                for (int i = 0; i < NumberOfResources; i++)
                {
                    if (Need[customerNum, i] > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool HasAllocatedResources(int customerNum)
        {
            lock (_lockObj)
            {
                for (int i = 0; i < NumberOfResources; i++)
                {
                    if (Allocation[customerNum, i] > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool IsSafe()
        {
            int[] work = new int[NumberOfResources];
            bool[] finish = new bool[NumberOfCustomers];

            Array.Copy(Available, work, NumberOfResources);

            bool found;

            do
            {
                found = false;

                for (int i = 0; i < NumberOfCustomers; i++)
                {
                    if (finish[i])
                    {
                        continue;
                    }

                    bool canFinish = true;

                    for (int j = 0; j < NumberOfResources; j++)
                    {
                        if (Need[i, j] > work[j])
                        {
                            canFinish = false;
                            break;
                        }
                    }

                    if (canFinish)
                    {
                        for (int j = 0; j < NumberOfResources; j++)
                        {
                            work[j] += Allocation[i, j];
                        }

                        finish[i] = true;
                        found = true;
                    }
                }
            }
            while (found);

            return finish.All(f => f);
        }

        public void PrintMaximum()
        {
            for (int i = 0; i < NumberOfCustomers; i++)
            {
                Console.Write($"Cliente {i}: [");

                for (int j = 0; j < NumberOfResources; j++)
                {
                    Console.Write(Maximum[i, j]);
                    if (j < NumberOfResources - 1)
                    {
                        Console.Write(", ");
                    }
                }

                Console.WriteLine("]");
            }

            Console.WriteLine();
        }

        public void PrintState()
        {
            Console.WriteLine();
            Console.WriteLine("===== ESTADO ATUAL DO SISTEMA =====");
            Console.WriteLine($"Available: [{string.Join(", ", Available)}]");
            Console.WriteLine();

            Console.WriteLine("Maximum:");
            PrintMatrix(Maximum);

            Console.WriteLine();
            Console.WriteLine("Allocation:");
            PrintMatrix(Allocation);

            Console.WriteLine();
            Console.WriteLine("Need:");
            PrintMatrix(Need);

            Console.WriteLine("===================================");
            Console.WriteLine();
        }

        private void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < NumberOfCustomers; i++)
            {
                Console.Write($"Cliente {i}: [");

                for (int j = 0; j < NumberOfResources; j++)
                {
                    Console.Write(matrix[i, j]);
                    if (j < NumberOfResources - 1)
                    {
                        Console.Write(", ");
                    }
                }

                Console.WriteLine("]");
            }
        }
    }
}
