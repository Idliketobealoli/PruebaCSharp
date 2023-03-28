namespace prueba.model
{
    public class Dato
    {
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public double MitadDelNumero { get; set; }

        public Dato(int num, string name)
        {
            Numero = num;
            Nombre = name;
            MitadDelNumero = Numero / 2;
        }
    }
}
