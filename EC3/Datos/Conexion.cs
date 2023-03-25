using System;
using System.Data.SqlClient;

namespace EC3.Datos
{
    internal class Conexion
    {
        private readonly string Base;
        private readonly string Servidor;
        private readonly string Usuario;
        private readonly string Clave;
        private static Conexion Con = null;

        public Conexion() {
            Base = "Northwind";
            Servidor = "DESKTOP-U3NP75V";
            Usuario = "JuanE_Gil";
            Clave = "JuanGil2002";
        }

        public SqlConnection CrearConexion() {
            SqlConnection Cadena = new SqlConnection();
            try {
                Cadena.ConnectionString = "Server=" + this.Servidor +
                                        "; Database=" + this.Base +
                                        "; User id=" + this.Usuario +
                                        "; Password=" + this.Clave;
            }
            catch (Exception ex) {
                Cadena = null;
                throw ex;
            }
            return Cadena;
        }

        public static Conexion getInstancia() {
            if (Con == null) {
                Con = new Conexion();
            }
            return Con;
        }
    }
}
