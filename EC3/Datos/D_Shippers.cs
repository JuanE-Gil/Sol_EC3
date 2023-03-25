using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using EC3.Entidades;

namespace EC3.Datos
{
    public class D_Shippers
    {
        public static List<E_Shipper> ObtenerShippers() {
            List<E_Shipper> shippers = new List<E_Shipper>();

            using (SqlConnection conn = Conexion.getInstancia().CrearConexion()) {
                conn.Open();

                const string query = "SELECT ShipperID, CompanyName, Phone FROM Shippers ORDER BY CompanyName";
                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            E_Shipper shipper = new E_Shipper
                            {
                                ShipperId = Convert.ToInt32(reader["ShipperID"]),
                                CompanyName = reader["CompanyName"].ToString(),
                                Phone = reader["Phone"].ToString()
                            };

                            shippers.Add(shipper);
                        }
                    }
                }
            }

            return shippers;
        }
    }

}
