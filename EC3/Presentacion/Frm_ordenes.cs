﻿using System;
using System.Windows.Forms;

using EC3.Datos;
using EC3.Entidades;

namespace EC3.Presentacion
{
    public partial class Frm_ordenes : Form
    {
        private D_Ordenes Ordenes = new D_Ordenes();
        private int inputID = 0;
        public Frm_ordenes() {
            InitializeComponent();
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
            dgvListado.DataSource = Ordenes.ListadoOrdenes();
            this.FormatoLista();
        }

        private void Frm_ordenes_Load(object sender, System.EventArgs e) {
            this.Listado_Ordenes();
        }

        private void FormatoLista() {
            dgvListado.Columns[0].Width = 100;
            dgvListado.Columns[0].HeaderText = "OrdenId";
            dgvListado.Columns[1].Width = 100;
            dgvListado.Columns[1].HeaderText = "ClienteId";
            dgvListado.Columns[2].Width = 100;
            dgvListado.Columns[2].HeaderText = "EmpleadoId";
            dgvListado.Columns[3].Width = 100;
            dgvListado.Columns[3].HeaderText = "FechaOrden";
            dgvListado.Columns[4].Width = 100;
            dgvListado.Columns[4].HeaderText = "FechaRequerida";
            dgvListado.Columns[5].Width = 100;
            dgvListado.Columns[5].HeaderText = "FechaEnvio";
            dgvListado.Columns[6].Width = 100;
            dgvListado.Columns[6].HeaderText = "EnvioPedido";
            dgvListado.Columns[7].Width = 100;
            dgvListado.Columns[7].HeaderText = "Flete";
            dgvListado.Columns[8].Width = 100;
            dgvListado.Columns[8].HeaderText = "NombreEnvio";
            dgvListado.Columns[9].Width = 100;
            dgvListado.Columns[9].HeaderText = "DireccionEnvio";
            dgvListado.Columns[10].Width = 100;
            dgvListado.Columns[10].HeaderText = "CiudadEnvio";
            dgvListado.Columns[11].Width = 100;
            dgvListado.Columns[11].HeaderText = "RegionEnvio";
            dgvListado.Columns[12].Width = 100;
            dgvListado.Columns[12].HeaderText = "CodigoPostalEnvio";
            dgvListado.Columns[13].Width = 100;
            dgvListado.Columns[13].HeaderText = "PaisEnvio";
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
            int EnvioPedido = Convert.ToInt32(txtEnvioPedido.Text);
            Decimal Flete = Convert.ToDecimal(txtFlete.Text);
            string NombreEnvio = txtNombreEnvio.Text;
            string DireccionEnvio = txtDireccion.Text;
            string CiudadEnvio = txtCuidad.Text;
            string RegionEnvio = txtRegion.Text;
            string CodigoPostalEnvio = txtCodigoPostal.Text;
            string PaisEnvio = txtPais.Text;

            E_Ordenes orden = new E_Ordenes(ClienteId, EmpleadoId, FechaOrden, FechaRequerida, FechaEnvio, EnvioPedido, Flete, NombreEnvio, DireccionEnvio, CiudadEnvio, RegionEnvio, CodigoPostalEnvio, PaisEnvio);

            Ordenes.guardarOrden(orden);
            this.Listado_Ordenes();
            MessageBox.Show("!Nueva Orden registrada exitosamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.EstadoBotones(false);
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
            txtEnvioPedido.Text = "";
            txtFlete.Text = "";
            txtNombreEnvio.Text = "";
            txtDireccion.Text = "";
            txtCuidad.Text = "";
            txtRegion.Text = "";
            txtCodigoPostal.Text = "";
            txtPais.Text = "";
        }

        private void EstadoTexto(bool lEstado) {
            txtClienteId.Enabled = lEstado;
            txtEmpleadoId.Enabled = lEstado;
            txtEnvioPedido.Enabled = lEstado;
            txtFlete.Enabled = lEstado;
            txtNombreEnvio.Enabled = lEstado;
            txtDireccion.Enabled = lEstado;
            txtCuidad.Enabled = lEstado;
            txtRegion.Enabled = lEstado;
            txtCodigoPostal.Enabled = lEstado;
            txtPais.Enabled = lEstado;
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
                    txtEnvioPedido.Text = orden.EnvioPedido.ToString();
                    txtFlete.Text = orden.Flete.ToString();
                    txtNombreEnvio.Text = orden.NombreEnvio;
                    txtDireccion.Text = orden.DireccionEnvio;
                    txtCuidad.Text = orden.CiudadEnvio;
                    txtDireccion.Text = orden.DireccionEnvio;
                    txtRegion.Text = orden.RegionEnvio;
                    txtCodigoPostal.Text = orden.CodigoPostalEnvio;
                    txtPais.Text = orden.PaisEnvio;

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
                    EnvioPedido = Convert.ToInt32(txtEnvioPedido.Text),
                    Flete = Convert.ToDecimal(txtFlete.Text),
                    NombreEnvio = txtNombreEnvio.Text,
                    DireccionEnvio = txtDireccion.Text,
                    CiudadEnvio = txtCuidad.Text,
                    RegionEnvio = txtRegion.Text,
                    CodigoPostalEnvio = txtCodigoPostal.Text,
                    PaisEnvio = txtPais.Text
                };

                Ordenes.actualizarOrden(orden);
                MessageBox.Show("Registro actualizado con éxito.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.EstadoTexto(false);
            }
            catch (FormatException ex) {
                MessageBox.Show("Error: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    }
}
