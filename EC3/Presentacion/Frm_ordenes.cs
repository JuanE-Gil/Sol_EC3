using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using EC3.Datos;
using EC3.Entidades;

using Newtonsoft.Json;

namespace EC3.Presentacion
{
    public partial class Frm_ordenes : Form
    {
        // Atributos
        private readonly D_Ordenes Ordenes = new D_Ordenes();
        private int inputID;

        // Declarar un diccionario para almacenar los países y sus ciudades
        readonly Dictionary<string, List<string>> paises = new Dictionary<string, List<string>>();

        public Frm_ordenes() {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos() {
            CargarPaises();
            CargarListaOrdenes();
            CargarListaShippers();
            DefinirFormatoLista();
            DefinirEstadoBotones(true);
            DefinirEstadoCamposTexto(false);
        }

        private void CargarPaises() {
            try {
                // Leer el archivo JSON
                var Json = File.ReadAllText(@"Datos\paises.json");

                dynamic data = JsonConvert.DeserializeObject(Json);

                foreach (var pais in data.paises) {
                    string nombrePais = pais.nombre;
                    List<string> ciudades = pais.ciudades.ToObject<List<string>>();
                    paises.Add(nombrePais, ciudades);
                }

                // Agregar las llaves del diccionario al ComboBox de países
                cmbPaises.Items.AddRange(paises.Keys.ToArray());
            }
            catch (JsonException ex) {
                MessageBox.Show("Error al cargar los países: " + ex.Message);
            }
        }

        private void DefinirEstadoBotones(bool isHabilitado) {
            btnCancelar.Visible = !isHabilitado;
            btnGuadar.Visible = !isHabilitado;

            btnNuevo.Enabled = isHabilitado;
            btnActualizar.Enabled = isHabilitado;
            btnEliminar.Enabled = isHabilitado;
            btnSalir.Enabled = isHabilitado;

            btnBuscar.Enabled = isHabilitado;
            txtBuscar.Enabled = isHabilitado;
            dgvListado.Enabled = isHabilitado;
        }

        private void CargarListaOrdenes() {
            dgvListado.DataSource = Ordenes.ListadoOrdenes();
        }

        // Cargar la lista de shippers desde la base de datos y agregarla al ComboBox de shippers
        private void CargarListaShippers() {
            cmbPedido.DataSource = D_Shippers.ObtenerShippers();
            cmbPedido.DisplayMember = "CompanyName";
            cmbPedido.ValueMember = "ShipperID";
        }

        private void DefinirFormatoLista() {
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

        private void Frm_ordenes_Load(object sender, EventArgs e) {
            CargarListaShippers();
        }

        private void BtnNuevo_Click(object sender, EventArgs e) {
            LimpiaTexto();
            DefinirEstadoCamposTexto(true);
            DefinirEstadoBotones(false);
        }

        private void BtnGuadar_Click(object sender, EventArgs e) {
            if (cmbCiudades.SelectedItem != null && cmbPaises.SelectedItem != null) {
                string CiudadEnvio = cmbCiudades.SelectedItem.ToString();
                string PaisEnvio = cmbPaises.SelectedItem.ToString();
                string ClienteId = txtClienteId.Text;
                int EmpleadoId = Convert.ToInt32(txtEmpleadoId.Text);
                DateTime FechaOrden = Convert.ToDateTime(dtpFechaOrden.Value);
                DateTime FechaRequerida = Convert.ToDateTime(dtpFechaRequerida.Value);
                DateTime FechaEnvio = Convert.ToDateTime(dtpFechaEnvio.Value);
                int ShipperId = Convert.ToInt32(cmbPedido.SelectedValue);
                Decimal Flete = Convert.ToDecimal(txtFlete.Text);
                string NombreEnvio = txtNombreEnvio.Text;
                string DireccionEnvio = txtDireccion.Text;
                string RegionEnvio = txtRegion.Text;
                string CodigoPostalEnvio = txtCodigoPostal.Text;

                E_Ordenes orden = new E_Ordenes(ClienteId, EmpleadoId, FechaOrden, FechaRequerida, FechaEnvio, ShipperId, Flete, NombreEnvio, DireccionEnvio, CiudadEnvio, RegionEnvio, CodigoPostalEnvio, PaisEnvio);

                Ordenes.guardarOrden(orden);
                CargarDatos();
                MessageBox.Show("!Nueva Orden registrada exitosamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiaTexto();

            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e) {
            LimpiaTexto();
            DefinirEstadoCamposTexto(false);
            DefinirEstadoBotones(true);
        }

        private void BtnActualizar_Click(object sender, EventArgs e) {
            if (inputID != 0 && cmbCiudades.SelectedItem != null && cmbPaises.SelectedItem != null) {
                string CiudadEnvio = cmbCiudades.SelectedItem.ToString();
                string PaisEnvio = cmbPaises.SelectedItem.ToString();
                string ClienteId = txtClienteId.Text;
                int EmpleadoId = Convert.ToInt32(txtEmpleadoId.Text);
                DateTime FechaOrden = Convert.ToDateTime(dtpFechaOrden.Value);
                DateTime FechaRequerida = Convert.ToDateTime(dtpFechaRequerida.Value);
                DateTime FechaEnvio = Convert.ToDateTime(dtpFechaEnvio.Value);
                int ShipperId = Convert.ToInt32(cmbPedido.SelectedValue);
                decimal Flete = Convert.ToDecimal(txtFlete.Text);
                string NombreEnvio = txtNombreEnvio.Text;
                string DireccionEnvio = txtDireccion.Text;
                string RegionEnvio = txtRegion.Text;
                string CodigoPostalEnvio = txtCodigoPostal.Text;

                E_Ordenes orden = new E_Ordenes(inputID, ClienteId, EmpleadoId, FechaOrden, FechaRequerida, FechaEnvio, ShipperId, Flete, NombreEnvio, DireccionEnvio, CiudadEnvio, RegionEnvio, CodigoPostalEnvio, PaisEnvio);

                if (Ordenes.actualizarOrden(orden)) {
                    MessageBox.Show("Orden actualizada correctamente.");
                    CargarDatos();
                    LimpiaTexto();

                    inputID = 0;
                } else {
                    MessageBox.Show("Error al actualizar la orden.");
                }
            } else {
                MessageBox.Show("Debe seleccionar un país y una ciudad de envío.");
            }

        }

        private void BtnEliminar_Click(object sender, EventArgs e) {
            try {
                int orderID = Convert.ToInt32(inputID);

                DialogResult result = MessageBox.Show("¿Está seguro que desea eliminar la orden con ID " + orderID + "?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes) {
                    Ordenes.EliminarOrden(orderID);
                    MessageBox.Show("Orden eliminada exitosamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                    LimpiaTexto();

                }
            }
            catch (FormatException) {
                MessageBox.Show("Por favor ingrese un número de orden válido.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e) {
            Close();
        }

        private void BtnBuscar_Click(object sender, EventArgs e) {
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

                    DefinirEstadoCamposTexto(true);
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

        private void LimpiaTexto() {
            txtClienteId.Text = "";
            txtEmpleadoId.Text = "";
            txtFlete.Text = "";
            txtNombreEnvio.Text = "";
            txtDireccion.Text = "";
            txtRegion.Text = "";
            txtCodigoPostal.Text = "";
        }

        private void DefinirEstadoCamposTexto(bool isDeshabilitado) {
            txtClienteId.Enabled = isDeshabilitado;
            txtEmpleadoId.Enabled = isDeshabilitado;
            cmbPedido.Enabled = isDeshabilitado;
            txtFlete.Enabled = isDeshabilitado;
            txtNombreEnvio.Enabled = isDeshabilitado;
            txtDireccion.Enabled = isDeshabilitado;
            cmbCiudades.Enabled = isDeshabilitado;
            txtRegion.Enabled = isDeshabilitado;
            txtCodigoPostal.Enabled = isDeshabilitado;
            cmbPaises.Enabled = isDeshabilitado;
            dtpFechaEnvio.Enabled = isDeshabilitado;
            dtpFechaRequerida.Enabled = isDeshabilitado;
            dtpFechaOrden.Enabled = isDeshabilitado;
        }

        private void DgvListado_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            int index = e.RowIndex;

            DataGridViewRow selectedRow = dgvListado.Rows[index];

            inputID = Convert.ToInt32(selectedRow.Cells[0].Value);
            txtBuscar.Text = inputID.ToString();
        }

        // Manejar el evento SelectedIndexChanged del ComboBox de países
        private void CmbPaises_SelectedIndexChanged(object sender, EventArgs e) {
            // Obtener la llave seleccionada (nombre del país)
            string pais = cmbPaises.SelectedItem.ToString();

            // Buscar la lista de ciudades correspondiente en el diccionario
            if (paises.TryGetValue(pais, out List<string> ciudades)) {
                // Limpiar el ComboBox de ciudades y agregar las ciudades encontradas
                cmbCiudades.Items.Clear();
                cmbCiudades.Items.AddRange(ciudades.ToArray());
            }
        }
    }
}
