using System.Collections.Generic;

namespace EC3.Entidades
{
    public class Paises
    {
        public List<Pais> paises { get; set; }
    }

    public class Pais
    {
        public string Nombre { get; set; }
        public List<string> Ciudades { get; set; }
    }


}
