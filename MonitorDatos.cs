using prueba.model;

namespace prueba
{
    public class MonitorDatos
    {
        private MonitorDatos() { }
        private static MonitorDatos _instance;

        private List<Dato> Datos {  get; set; }

        private static readonly object _lock = new();

        public static MonitorDatos GetInstance()
        {
            if (_instance == null )
            {
                lock (_lock)
                {
                    if (_instance == null )
                    {
                        _instance = new MonitorDatos();
                        _instance.Datos = new List<Dato>();
                    }
                }
            }
            return _instance;
        }

        public void Add(Dato dato)
        {
            lock (_lock)
            {
                Datos.Add(dato);
            }
        }

        public Dato? Consume()
        {
            lock ( _lock)
            {
                Dato? dato = Datos.FirstOrDefault();
                if ( dato != null )
                {
                    Console.WriteLine($"MONITOR  - Giving {dato.Nombre}");
                    Datos.RemoveAt(0);
                }
                return dato;
            }
        }
    }
}
