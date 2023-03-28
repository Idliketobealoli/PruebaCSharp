// control de errores con log4net (no me sale en NuGet - Mirar por que)

using prueba.consumer;
using prueba.producer;

namespace prueba
{
    public class Program
    {
        public static void Main()
        {
            int NumConsumidores = 5;
            int NumProductores = 7;
            List<Producer> Productores = new();
            List<Task> ProdTasks = new();
            List<Task> ConsTasks = new();
            List<Consumer> Consumidores = new();
            Random random = new();
            CancellationTokenSource tokenSource = new();

            Console.WriteLine("MAIN - Starting application.");

            while (Productores.Count < NumProductores)
            {
                var productor = new Producer(
                        random.Next(10, 15),
                        $"Producer {Productores.Count + 1}",
                        random.Next(1000, 1500)
                    );
                Productores.Add(productor);
            }

            while (Consumidores.Count < NumConsumidores)
            {
                var consumer = new Consumer(
                        $"Consumer {Consumidores.Count + 1}",
                        random.Next(700, 1400)
                    );
                Consumidores.Add(consumer);
            }

            Productores.ForEach(
                productor =>
                {
                    var task = Task.Run(() => { productor.producir(); }, tokenSource.Token);
                    ProdTasks.Add(task);
                }
            );

            Consumidores.ForEach(
                consumer =>
                {
                    var task = Task.Run(() => { consumer.consume(); }, tokenSource.Token);
                    ConsTasks.Add(task);
                }
            );

            try
            {
                Task finished = Task.WhenAll(ProdTasks);
                finished.Wait();

                if (finished.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine($"MAIN - Producers finished producing. Shutting down in 5 seconds...");
                    Thread.Sleep(5000);
                    Console.WriteLine($"MAIN - Shutting down consumers...");
                    tokenSource.Cancel();
                    Environment.Exit(0);
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                tokenSource.Cancel();
            }

            finally
            {
                tokenSource.Dispose();
            }
        }
    }
}