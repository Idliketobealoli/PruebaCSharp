using prueba.src.model;

namespace prueba.src.producer
{
    public class Producer
    {
        private int maxProducciones { get; set; }
        private int delay { get; set; }
        private string nombreProducer { get; set; }
        private int producidas = 0;
        private MonitorDatos monitor = MonitorDatos.GetInstance();

        public Producer(int max, string nombre, int delay)
        {
            maxProducciones = max;
            nombreProducer = nombre;
            this.delay = delay;
        }

        public void producir()
        {
            Console.WriteLine($"PRODUCER - {nombreProducer} starts producing in thread {Environment.CurrentManagedThreadId}...");
            Random random = new();

            while (producidas < maxProducciones)
            {
                producidas++;
                Dato dato = new Dato(random.Next(1, 100), $"Dato {producidas} from {nombreProducer}.");
                monitor.Add(dato);
                Console.WriteLine($"PRODUCER - {nombreProducer} produced one data.");
                Thread.Sleep(delay);
            }
            Console.WriteLine($"PRODUCER - {nombreProducer} finished producing...");
        }
    }
}
