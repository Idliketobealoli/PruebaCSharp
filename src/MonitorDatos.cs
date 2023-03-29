using prueba.src.model;
using System.Collections.Concurrent;

namespace prueba.src
{
    public class MonitorDatos
    {
        private MonitorDatos() { }
        private static MonitorDatos _instance;

        private ConcurrentQueue<Dato> Datos { get; set; }

        private static readonly object _lock = new();

        public static MonitorDatos GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MonitorDatos();
                        _instance.Datos = new ConcurrentQueue<Dato>();
                    }
                }
            }
            return _instance;
        }

        public void Add(Dato dato)
        {
             Datos.Enqueue(dato);
        }

        public Dato? Consume()
        {
            Datos.TryDequeue(out Dato? dato);
            if (dato != null)
            {
                Console.WriteLine($"MONITOR  - Giving {dato.Nombre}");
            }
            return dato;
        }
    }
}
