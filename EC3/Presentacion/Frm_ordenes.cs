using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

using EC3.Datos;
using EC3.Entidades;

using Newtonsoft.Json;

namespace EC3.Presentacion
{
    public partial class Frm_ordenes : Form
    {
        private D_Ordenes Ordenes = new D_Ordenes();
        private int inputID = 0;

        private D_Shippers Shippers = new D_Shippers(); // Instancia de la clase D_Shippers
        private int ShipperId; // Variable para guardar el ShipperID seleccionado

        private List<Pais> paises;

        public Frm_ordenes() {
            InitializeComponent();
            CargarPaises();

        }

        private void CargarPaises() {
            try {
                // Leer el archivo JSON
                var json = File.ReadAllText(@"Datos\paises.json");
                var paises = JsonConvert.DeserializeObject<Paises>(json);

                foreach (var pais in paises.paises) {
                    cmbPaises.Items.Add(pais.Nombre);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error al cargar los países: " + ex.Message);
            }
        }

        private void EstadoBotones(bool LEstado) {
            btnCancelar.Visible = !LEstado;
            btnGuadar.Visible = !LEstado;

            btnNuevo.Enabled = LEstado;
            btnActualizar.Enabled = LEstado;
            btnEliminar.Enabled = LEstado;
            btnSalir.Enabled = LEstado;

            btnBuscar.Enabled = LEstado;
            txtBuscar.Enabled = LEstado;
            dgvListado.Enabled = LEstado;
        }

        private void Listado_Ordenes() {

            // Llenar el ComboBox con los nombres de los shippers
            List<E_Shipper> shippers = D_Shippers.ObtenerShippers();
            cmbPedido.DataSource = shippers;
            cmbPedido.DisplayMember = "CompanyName";
            cmbPedido.ValueMember = "ShipperID";

            dgvListado.DataSource = Ordenes.ListadoOrdenes();
            this.FormatoLista();
        }

        private void Frm_ordenes_Load(object sender, System.EventArgs e) {


        }

        private void FormatoLista() {
            if (dgvListado.Columns.Count >= 14) {
                dgvListado.Columns[0].HeaderText = "Orden Id";
                dgvListado.Columns[0].Width = 75;
                dgvListado.Columns[1].HeaderText = "Cliente Id";
                dgvListado.Columns[1].Width = 100;
                dgvListado.Columns[2].HeaderText = "Empleado Id";
                dgvListado.Columns[2].Width = 100;
                dgvListado.Columns[3].HeaderText = "Fecha Orden";
                dgvListado.Columns[3].Width = 120;
                dgvListado.Columns[4].HeaderText = "Fecha Requerida";
                dgvListado.Columns[4].Width = 120;
                dgvListado.Columns[5].HeaderText = "Fecha Envio";
                dgvListado.Columns[5].Width = 120;
                dgvListado.Columns[6].HeaderText = "Envio Pedido";
                dgvListado.Columns[6].Width = 100;
                dgvListado.Columns[7].HeaderText = "Flete";
                dgvListado.Columns[7].Width = 75;
                dgvListado.Columns[8].HeaderText = "Nombre Envio";
                dgvListado.Columns[8].Width = 100;
                dgvListado.Columns[9].HeaderText = "Direccion Envio";
                dgvListado.Columns[9].Width = 110;
                dgvListado.Columns[10].HeaderText = "Ciudad Envio";
                dgvListado.Columns[10].Width = 100;
                dgvListado.Columns[11].HeaderText = "Region Envio";
                dgvListado.Columns[11].Width = 100;
                dgvListado.Columns[12].HeaderText = "Codigo Postal";
                dgvListado.Columns[12].Width = 100;
                dgvListado.Columns[13].HeaderText = "Pais Envio";
                dgvListado.Columns[13].Width = 100;
            }
        }


        private void btnNuevo_Click(object sender, System.EventArgs e) {
            this.LimpiaTexto();
            this.EstadoTexto(true);
            this.EstadoBotones(false);
        }

        private void btnGuadar_Click(object sender, EventArgs e) {
            string ClienteId = txtClienteId.Text;
            int EmpleadoId = Convert.ToInt32(txtEmpleadoId.Text);
            DateTime FechaOrden = Convert.ToDateTime(dtpFechaOrden.Value);
            DateTime FechaRequerida = Convert.ToDateTime(dtpFechaRequerida.Value);
            DateTime FechaEnvio = Convert.ToDateTime(dtpFechaEnvio.Value);
            int ShipperId = Convert.ToInt32(cmbPedido.SelectedValue);
            Decimal Flete = Convert.ToDecimal(txtFlete.Text);
            string NombreEnvio = txtNombreEnvio.Text;
            string DireccionEnvio = txtDireccion.Text;
            string CiudadEnvio = cmbCiudades.SelectedValue.ToString();
            string RegionEnvio = txtRegion.Text;
            string CodigoPostalEnvio = txtCodigoPostal.Text;
            string PaisEnvio = cmbPaises.SelectedValue.ToString();

            E_Ordenes orden = new E_Ordenes(ClienteId, EmpleadoId, FechaOrden, FechaRequerida, FechaEnvio, ShipperId, Flete, NombreEnvio, DireccionEnvio, CiudadEnvio, RegionEnvio, CodigoPostalEnvio, PaisEnvio);

            Ordenes.guardarOrden(orden);
            this.Listado_Ordenes();
            MessageBox.Show("!Nueva Orden registrada exitosamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiaTexto();
            this.EstadoTexto(false);
            this.EstadoBotones(true);
        }

        private void btnCancelar_Click(object sender, EventArgs e) {
            this.LimpiaTexto();
            this.EstadoTexto(false);
            this.EstadoBotones(true);
        }

        private void LimpiaTexto() {
            txtClienteId.Text = "";
            txtEmpleadoId.Text = "";
            txtFlete.Text = "";
            txtNombreEnvio.Text = "";
            txtDireccion.Text = "";
            txtRegion.Text = "";
            txtCodigoPostal.Text = "";
        }

        private void EstadoTexto(bool lEstado) {
            txtClienteId.Enabled = lEstado;
            txtEmpleadoId.Enabled = lEstado;
            cmbPedido.Enabled = lEstado;
            txtFlete.Enabled = lEstado;
            txtNombreEnvio.Enabled = lEstado;
            txtDireccion.Enabled = lEstado;
            cmbCiudades.Enabled = lEstado;
            txtRegion.Enabled = lEstado;
            txtCodigoPostal.Enabled = lEstado;
            cmbPaises.Enabled = lEstado;
            dtpFechaEnvio.Enabled = lEstado;
            dtpFechaRequerida.Enabled = lEstado;
            dtpFechaOrden.Enabled = lEstado;
        }

        private void btnBuscar_Click(object sender, EventArgs e) {
            try {
                inputID = Convert.ToInt32(txtBuscar.Text);


                E_Ordenes orden = Ordenes.BuscarPorId(inputID);

                if (orden == null) {
                    MessageBox.Show("Error ID no existe", "Mensaje",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    txtClienteId.Text = orden.ClienteId;
                    txtEmpleadoId.Text = orden.EmpleadoId.ToString();
                    dtpFechaEnvio.Value = orden.FechaEnvio;
                    dtpFechaOrden.Value = orden.FechaOrden;
                    dtpFechaRequerida.Value = orden.FechaRequerida;
                    cmbPedido.SelectedValue = orden.EnvioPedido;
                    txtFlete.Text = orden.Flete.ToString();
                    txtNombreEnvio.Text = orden.NombreEnvio;
                    txtDireccion.Text = orden.DireccionEnvio;
                    cmbCiudades.Text = orden.CiudadEnvio;
                    txtDireccion.Text = orden.DireccionEnvio;
                    txtRegion.Text = orden.RegionEnvio;
                    txtCodigoPostal.Text = orden.CodigoPostalEnvio;
                    cmbPaises.Text = orden.PaisEnvio;

                    this.EstadoTexto(true);
                }
            }
            catch (FormatException ex) {
                MessageBox.Show(ex.Message, "Mensaje",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) {
                MessageBox.Show("Error... Fallo de Conexión!", "Mensaje",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnActualizar_Click(object sender, EventArgs e) {
            try {
                E_Ordenes orden = new E_Ordenes
                {
                    ClienteId = txtClienteId.Text,
                    EmpleadoId = Convert.ToInt32(txtEmpleadoId.Text),
                    FechaEnvio = dtpFechaEnvio.Value,
                    FechaOrden = dtpFechaOrden.Value,
                    FechaRequerida = dtpFechaRequerida.Value,
                    EnvioPedido = Convert.ToInt32(cmbPedido.SelectedValue),
                    Flete = Convert.ToDecimal(txtFlete.Text),
                    NombreEnvio = txtNombreEnvio.Text,
                    DireccionEnvio = txtDireccion.Text,
                    CiudadEnvio = cmbCiudades.SelectedValue.ToString(),
                    RegionEnvio = txtRegion.Text,
                    CodigoPostalEnvio = txtCodigoPostal.Text,
                    PaisEnvio = cmbPaises.SelectedValue.ToString(),
                    OrdenId = Convert.ToInt32(txtBuscar.Text)
                };

                Ordenes.actualizarOrden(orden);
                MessageBox.Show("Registro actualizado con éxito.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.EstadoTexto(false);
                this.LimpiaTexto();
                this.EstadoBotones(true);
                this.Listado_Ordenes();

            }
            catch (FormatException ex) {
                MessageBox.Show("Error: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex) {
                if (ex.Number == 547) {
                    MessageBox.Show("Error: No se encontró el registro de la orden.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    MessageBox.Show("Error al actualizar el registro: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error al actualizar el registro: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e) {
            try {
                int orderID = Convert.ToInt32(inputID);

                DialogResult result = MessageBox.Show("¿Está seguro que desea eliminar la orden con ID " + orderID + "?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes) {
                    Ordenes.EliminarOrden(orderID);
                    MessageBox.Show("Orden eliminada exitosamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.EstadoTexto(false);
                    this.LimpiaTexto();
                    this.EstadoBotones(true);
                    this.Listado_Ordenes();
                }
            }
            catch (FormatException) {
                MessageBox.Show("Por favor ingrese un número de orden válido.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbPaises_SelectedIndexChanged(object sender, EventArgs e) {
            // Obtener la llave seleccionada (nombre del país)
            string pais = cmbPaises.SelectedItem.ToString();

            // Buscar la lista de ciudades correspondiente en el diccionario
            List<string> ciudades;
            if (paises.TryGetValue(pais, out ciudades)) {
                // Limpiar el ComboBox de ciudades y agregar las ciudades encontradas
                cmbCiudades.Items.Clear();
                cmbCiudades.Items.AddRange(ciudades.ToArray());
            }
        }
    }
}
