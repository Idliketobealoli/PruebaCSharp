// control de errores con log4net (no me sale en NuGet - Mirar por que)

using prueba.src.consumer;
using prueba.src.producer;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace prueba.src
{
    public class Program
    {
        public static void Main()
        {
            Stopwatch sw = new();
            sw.Start();

            int NumConsumidores = 5;
            int NumProductores = 7;
            List<Producer> Productores = new();
            List<Task> ProdTasks = new();
            List<Task> ConsTasks = new();
            List<Consumer> Consumidores = new();
            Random random = new();
            CancellationTokenSource tokenSource = new();

            Console.WriteLine("MAIN     - Starting application.");

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
                    var task = Task.Run(() =>
                    {
                        var r = random.Next(1, 3);
                        if (r == 1) consumer.Consume(null, null);
                        else if (r == 2) consumer.Consume(random.Next(20, 30), random.Next(40, 50));
                        else consumer.Consume(random.Next(20, 50), random.Next(60, 80));
                    }, tokenSource.Token);
                    ConsTasks.Add(task);
                }
            );

            try
            {
                Task finished = Task.WhenAll(ProdTasks);
                finished.Wait();

                if (finished.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine($"MAIN     - Producers finished producing. Shutting down in 5 seconds...");
                    Thread.Sleep(5000);
                    Console.WriteLine($"MAIN     - Shutting down consumers...");
                    tokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                tokenSource.Cancel();
            }

            finally
            {
                tokenSource.Dispose();
                sw.Stop();
                Console.WriteLine($"\nMAIN     - Execution finished in {sw.ElapsedMilliseconds} ms.");
            }
        }
    }
}