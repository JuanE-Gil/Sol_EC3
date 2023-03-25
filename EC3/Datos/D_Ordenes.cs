using System;
using System.Data;
using System.Data.SqlClient;

using EC3.Entidades;

namespace EC3.Datos
{
    public class D_Ordenes
    {
        public DataTable ListadoOrdenes() {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection sqlCon = new SqlConnection();
            try {
                sqlCon = Conexion.getInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("SELECT Orders.OrderID,Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate," +
                                                    "Shippers.CompanyName AS Shipper, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity," +
                                                    "Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry " +
                                                    "FROM Orders " +
                                                    "INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID " +
                                                    "INNER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID", sqlCon);
                sqlCon.Open();
                resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;
            }
            catch (Exception ex) {
                throw ex;
            } finally {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
        }

        public void guardarOrden(E_Ordenes orden) {
            using (SqlConnection SqlCon = Conexion.getInstancia().CrearConexion()) {
                SqlCon.Open();

                SqlCommand sqlCmd = SqlCon.CreateCommand();
                sqlCmd.CommandText = "INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)" +
                                      "VALUES (@CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry)";

                sqlCmd.Parameters.AddWithValue("@CustomerId", orden.ClienteId);
                sqlCmd.Parameters.AddWithValue("@EmployeeId", orden.EmpleadoId);
                sqlCmd.Parameters.AddWithValue("@OrderDate", orden.FechaOrden);
                sqlCmd.Parameters.AddWithValue("@RequiredDate", orden.FechaRequerida);
                sqlCmd.Parameters.AddWithValue("@ShippedDate", orden.FechaEnvio);
                sqlCmd.Parameters.AddWithValue("@ShipVia", orden.EnvioPedido);
                sqlCmd.Parameters.AddWithValue("@Freight", orden.Flete);
                sqlCmd.Parameters.AddWithValue("@ShipName", orden.NombreEnvio);
                sqlCmd.Parameters.AddWithValue("@ShipAddress", orden.DireccionEnvio);
                sqlCmd.Parameters.AddWithValue("@ShipCity", orden.CiudadEnvio);
                sqlCmd.Parameters.AddWithValue("@ShipRegion", orden.RegionEnvio);
                sqlCmd.Parameters.AddWithValue("@ShipPostalCode", orden.CodigoPostalEnvio);
                sqlCmd.Parameters.AddWithValue("@ShipCountry", orden.PaisEnvio);

                int rows = sqlCmd.ExecuteNonQuery();

                if (rows != 1) {
                    Exception exception = new Exception("Error al insertar la orden");
                    throw exception;
                }
                SqlCon.Close();
            }
        }

        public E_Ordenes BuscarPorId(int ordenId) {
            E_Ordenes orden = null;
            using (SqlConnection SqlCon = Conexion.getInstancia().CrearConexion()) {
                SqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Orders WHERE OrderId = @ordenId", SqlCon);
                sqlCmd.Parameters.AddWithValue("@ordenId", ordenId);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                if (reader.Read()) {
                    orden = new E_Ordenes
                    {
                        OrdenId = Convert.ToInt32(reader["OrderId"]),
                        ClienteId = reader["CustomerId"].ToString(),
                        EmpleadoId = Convert.ToInt32(reader["EmployeeId"]),
                        FechaOrden = Convert.ToDateTime(reader["OrderDate"]),
                        FechaRequerida = Convert.ToDateTime(reader["RequiredDate"]),
                        FechaEnvio = Convert.ToDateTime(reader["ShippedDate"]),
                        EnvioPedido = Convert.ToInt32(reader["ShipVia"]),
                        Flete = Convert.ToDecimal(reader["Freight"]),
                        NombreEnvio = reader["ShipName"].ToString(),
                        DireccionEnvio = reader["ShipAddress"].ToString(),
                        CiudadEnvio = reader["ShipCity"].ToString(),
                        RegionEnvio = reader["ShipRegion"].ToString(),
                        CodigoPostalEnvio = reader["ShipPostalCode"].ToString(),
                        PaisEnvio = reader["ShipCountry"].ToString()
                    };
                }
                reader.Close();
                SqlCon.Close();
            }
            return orden;
        }

        public bool actualizarOrden(E_Ordenes orden) {
            bool actualizado = false;
            using (SqlConnection SqlCon = Conexion.getInstancia().CrearConexion()) {
                SqlCon.Open();
                string query = "SELECT COUNT(*) FROM Orders WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, SqlCon)) {
                    command.Parameters.AddWithValue("@OrderID", orden.OrdenId);

                    int count = (int)command.ExecuteScalar();

                    if (count == 0) {
                        return false;
                    }
                }

                query = "UPDATE Orders " +
                        "SET CustomerID = @CustomerID, " +
                        "EmployeeID = @EmployeeID, " +
                        "OrderDate = @OrderDate, " +
                        "RequiredDate = @RequiredDate, " +
                        "ShippedDate = @ShippedDate, " +
                        "ShipVia = @ShipVia, " +
                        "Freight = @Freight, " +
                        "ShipName = @ShipName, " +
                        "ShipAddress = @ShipAddress, " +
                        "ShipCity = @ShipCity, " +
                        "ShipRegion = @ShipRegion, " +
                        "ShipPostalCode = @ShipPostalCode, " +
                        "ShipCountry = @ShipCountry " +
                        "WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, SqlCon)) {
                    command.Parameters.AddWithValue("@CustomerID", orden.ClienteId);
                    command.Parameters.AddWithValue("@EmployeeID", orden.EmpleadoId);
                    command.Parameters.AddWithValue("@OrderDate", orden.FechaOrden);
                    command.Parameters.AddWithValue("@RequiredDate", orden.FechaRequerida);
                    command.Parameters.AddWithValue("@ShippedDate", orden.FechaEnvio);
                    command.Parameters.AddWithValue("@ShipVia", orden.EnvioPedido);
                    command.Parameters.AddWithValue("@Freight", orden.Flete);
                    command.Parameters.AddWithValue("@ShipName", orden.NombreEnvio);
                    command.Parameters.AddWithValue("@ShipAddress", orden.DireccionEnvio);
                    command.Parameters.AddWithValue("@ShipCity", orden.CiudadEnvio);
                    command.Parameters.AddWithValue("@ShipRegion", orden.RegionEnvio);
                    command.Parameters.AddWithValue("@ShipPostalCode", orden.CodigoPostalEnvio);
                    command.Parameters.AddWithValue("@ShipCountry", orden.PaisEnvio);
                    command.Parameters.AddWithValue("@OrderID", orden.OrdenId);

                    int rows = command.ExecuteNonQuery();

                    if (rows == 1) {
                        actualizado = true;
                    }
                }
            }
            return actualizado;
        }

        public void EliminarOrden(int orderID) {
            using (SqlConnection SqlCon = Conexion.getInstancia().CrearConexion()) {
                SqlCon.Open();

                string query = "DELETE FROM Orders WHERE OrderID = @OrderID";

                using (SqlCommand cmd = new SqlCommand(query, SqlCon)) {
                    cmd.Parameters.AddWithValue("@OrderID", orderID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0) {
                        throw new Exception("No se encontró el registro de la orden.");
                    }
                }
            }
        }


    }
}
