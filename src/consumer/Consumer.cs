using prueba.src.model;

namespace prueba.src.consumer
{
    public class Consumer
    {
        private int Delay { get; set; }
        private string NombreConsumer { get; set; }
        private MonitorDatos Monitor = MonitorDatos.GetInstance();
        private List<Dato> DatosConsumidos = new List<Dato>();

        public Consumer(string name, int delay)
        {
            NombreConsumer = name;
            this.Delay = delay;
        }

        public void Consume(int? min, int? max)
        {
            Console.WriteLine($"CONSUMER - {NombreConsumer} starts consuming in thread {Environment.CurrentManagedThreadId}...");
            while (true)
            {
                Dato? dato = Monitor.Consume();
                if (dato != null)
                {
                    DatosConsumidos.Add(dato);
                    Console.WriteLine($"CONSUMER - {NombreConsumer} consumed {dato.Nombre}.\n" +
                        $"           Numero del dato: {dato.Numero}\n" +
                        $"           Mitad de ese numero: {dato.MitadDelNumero}");

                    var filteredList = DatosConsumidos;
                    if (min != null)
                    {
                        filteredList = (from d in filteredList
                                       where d.Numero >= min
                                       select d).ToList();
                    }
                    if (max != null)
                    {
                        filteredList = (from d in filteredList
                                        where d.Numero <= max
                                        select d).ToList();
                    }
                    if (min == null) { min = 0; }
                    if (max == null) { max = 0; }
                    Console.WriteLine($"CONSUMER - {NombreConsumer} has consumed a total of {filteredList.Count} elements " +
                        $"with a number between {min} and {max}.");
                }
                Thread.Sleep(Delay);
            }
        }
    }
}
