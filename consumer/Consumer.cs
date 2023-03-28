using prueba.model;

namespace prueba.consumer
{
    public class Consumer
    {
        private int delay { get; set; }
        private string nombreConsumer { get; set; }
        private MonitorDatos monitor = MonitorDatos.GetInstance();

        public Consumer(string name, int delay)
        {
            nombreConsumer = name;
            this.delay = delay;
        }

        public void consume()
        {
            Console.WriteLine($"CONSUMER - {nombreConsumer} starts consuming in thread {Thread.CurrentThread.ManagedThreadId}...");
            while (true)
            {
                Dato? dato = monitor.Consume();
                if ( dato != null )
                {
                    Console.WriteLine($"CONSUMER - {nombreConsumer} consumed {dato.Nombre}.\n" +
                        $"           Numero del dato: {dato.Numero}\n" +
                        $"           Mitad de ese numero: {dato.MitadDelNumero}");
                }
                Thread.Sleep( delay );
            }
        }
    }
}
