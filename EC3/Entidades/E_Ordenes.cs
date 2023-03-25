using System;

namespace EC3.Entidades
{
    public class E_Ordenes
    {
        public int OrdenId { get; set; }
        public string ClienteId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime FechaOrden { get; set; }
        public DateTime FechaRequerida { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int EnvioPedido { get; set; }
        public decimal Flete { get; set; }
        public string NombreEnvio { get; set; }
        public string DireccionEnvio { get; set; }
        public string CiudadEnvio { get; set; }
        public string RegionEnvio { get; set; }
        public string CodigoPostalEnvio { get; set; }
        public string PaisEnvio { get; set; }

        public E_Ordenes() {
        }

        public E_Ordenes(int ordenId, string clienteId, int empleadoId, DateTime fechaOrden, DateTime fechaRequerida, DateTime fechaEnvio, int envioPedido, decimal flete, string nombreEnvio, string direccionEnvio, string ciudadEnvio, string regionEnvio, string codigoPostalEnvio, string paisEnvio) {
            OrdenId = ordenId;
            ClienteId = clienteId;
            EmpleadoId = empleadoId;
            FechaOrden = fechaOrden;
            FechaRequerida = fechaRequerida;
            FechaEnvio = fechaEnvio;
            EnvioPedido = envioPedido;
            Flete = flete;
            NombreEnvio = nombreEnvio;
            DireccionEnvio = direccionEnvio;
            CiudadEnvio = ciudadEnvio;
            RegionEnvio = regionEnvio;
            CodigoPostalEnvio = codigoPostalEnvio;
            PaisEnvio = paisEnvio;
        }

        public E_Ordenes(string clienteId, int empleadoId, DateTime fechaOrden, DateTime fechaRequerida, DateTime fechaEnvio, int envioPedido, decimal flete, string nombreEnvio, string direccionEnvio, string ciudadEnvio, string regionEnvio, string codigoPostalEnvio, string paisEnvio) {
            ClienteId = clienteId;
            EmpleadoId = empleadoId;
            FechaOrden = fechaOrden;
            FechaRequerida = fechaRequerida;
            FechaEnvio = fechaEnvio;
            EnvioPedido = envioPedido;
            Flete = flete;
            NombreEnvio = nombreEnvio;
            DireccionEnvio = direccionEnvio;
            CiudadEnvio = ciudadEnvio;
            RegionEnvio = regionEnvio;
            CodigoPostalEnvio = codigoPostalEnvio;
            PaisEnvio = paisEnvio;
        }
    }
}
