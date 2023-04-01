using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace SmartPhone7
{
    public partial class Factura : Form
    {
        public Factura()
        {
            InitializeComponent();
        }
        int idCliente, idTecnico, idProducto, cantidadVenta = 0;

        Conexion conexion1 = new Conexion();
        bool editar = false;

        private void LimpiarControles()
        {
            txtCliente.Text = string.Empty;
            txtNumeroOrden.Text = string.Empty;
            dtFecha.Text = string.Empty;
            txtFalla.Text = string.Empty;
            txtDiasgnosticoFinal.Text = string.Empty;
            txtTecnico.Text = string.Empty;
            ComboEstado.Text = string.Empty;
            txtMetodoPago.Text = string.Empty;
            txtNota.Text = string.Empty;
            txtItebis.Text = string.Empty;
            txtDescuento.Text = string.Empty;
            lblSubTotal.Text = "0";
            lblTotal.Text = "0";
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                MessageBox.Show("El campo cliente es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ComboEstado.Text))
            {
                MessageBox.Show("El campo Estado de orden de usuario es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTecnico.Text))
            {
                MessageBox.Show("El campo contraseña es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTecnico.Text))
            {
                MessageBox.Show("El campo tecnico es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNota.Text))
            {
                MessageBox.Show("El campo nota es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

      private void ListarUsuariosEditar()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT c.NombreCliente AS NombreCliente, f.NumeroOrden, f.FechaFactura, t.NombreTecnico AS NombreTecnico, f.Falla, f.ServicioOfrecido \r\nFROM Factura AS f\r\nINNER JOIN Clientes c ON f.IdCliente = c.Id \r\nINNER JOIN Tecnicos t ON f.IdTecnico = t.Id;";
                DataTable dt = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@id", Id);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        editar = true;
                        txtCliente.Text = dt.Rows[0]["NombreCliente"].ToString();
                        txtNumeroOrden.Text = dt.Rows[0]["NumeroOrden"].ToString();
                        dtFecha.Text = dt.Rows[0]["FechaFactura"].ToString();
                        txtFalla.Text = dt.Rows[0]["Falla"].ToString();
                        txtDiasgnosticoFinal.Text = dt.Rows[0]["Diagnostico"].ToString();
                        txtTecnico.Text = dt.Rows[0]["Tecnico"].ToString();
                        ComboEstado.Text = dt.Rows[0]["EstadoReparacion"].ToString();
                        txtMetodoPago.Text = dt.Rows[0]["MetodoPago"].ToString();
                        txtNota.Text = dt.Rows[0]["Nota"].ToString();
                        txtItebis.Text = dt.Rows[0]["Itabis"].ToString();
                        txtDescuento.Text = dt.Rows[0]["Descuento"].ToString();
                        lblSubTotal.Text = dt.Rows[0]["Subtotal"].ToString();
                        lblTotal.Text = dt.Rows[0]["Total"].ToString();


                    }
                    adaptador.Dispose();
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la información del registro: " + ex.Message);
                }
            }
        }

        private void GuardarUsuario()
        {
            //if (!ValidarCampos()) return;

            //using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            //{
            //    string consulta = (editar) ?
            //        "UPDATE Usuario SET Nombre = @nombre, Correo = @correo, Rol = @rol, NombreUsuario = @nombreUsuario, Contrasena = @contrasena WHERE Id = @id" :
            //        "INSERT INTO Usuario (Nombre, Correo, Rol, NombreUsuario, Contrasena) VALUES (@nombre, @correo, @rol, @nombreUsuario, @contrasena)";

            //    try
            //    {
            //        conexion.Open();
            //        SqlCommand comando = new SqlCommand(consulta, conexion);
            //        comando.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
            //        comando.Parameters.AddWithValue("@correo", txtGmail.Text.Trim());
            //        comando.Parameters.AddWithValue("@rol", cbRol.Text.Trim());
            //        comando.Parameters.AddWithValue("@nombreUsuario", txtNombreUsuario.Text.Trim());
            //        comando.Parameters.AddWithValue("@contrasena", txtContraseña.Text.Trim());
            //        comando.Parameters.AddWithValue("@contrasena", txtConfirmarContraseña.Text.Trim());

            //        if (editar) comando.Parameters.AddWithValue("@id", IdUsuario);

            //        int resultado = comando.ExecuteNonQuery();
            //        if (resultado > 0)
            //        {
            //            MessageBox.Show("Usuario guardado exitosamente.");
            //            editar = false;
            //            IdUsuario = 0;
            //            LimpiarControles();
            //            Close();
            //        }
            //        else
            //        {
            //            MessageBox.Show("No se pudo guardar el usuario.");
            //        }

            //        conexion.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error al guardar el usuario: " + ex.Message);
            //    }
            //}
        }

      

        private void EfectoModal_Tick(object sender, EventArgs e)
        {
            if (Opacity >= 1)
            {
                EfectoModal.Stop();
            }
            else
            {
                Opacity += 0.4;
            }
        }

        private void Factura_Load(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Este campo solo acepta numeros debido a que esta informacion que se guarda aqui debe de ser en este formato.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Este campo solo acepta numeros debido a que esta informacion que se guarda aqui debe de ser en este formato.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Este campo solo acepta numeros debido a que esta informacion que se guarda aqui debe de ser en este formato.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarTecnico_Click(object sender, EventArgs e)
        {
            BuscarTecnico formularioHijo = new BuscarTecnico();
            formularioHijo.ShowDialog();

            idTecnico = formularioHijo.IdSeleccionado;
            txtTecnico.Text = formularioHijo.NombreSeleccionado;
        }

        private void btnAñadirProducto_Click(object sender, EventArgs e)
        {
            BuscarProducto formularioHijo = new BuscarProducto();
            formularioHijo.ShowDialog();

            if (formularioHijo.IdSeleccionado > 0)
            {

                int rowIndex = DataGridViewProducto.Rows.Add();

                DataGridViewProducto.Rows[rowIndex].Cells["Id"].Value = formularioHijo.IdSeleccionado;
                DataGridViewProducto.Rows[rowIndex].Cells["Producto"].Value = formularioHijo.NombreProducto;
                DataGridViewProducto.Rows[rowIndex].Cells["ModeloMarca"].Value = formularioHijo.ModeloMarca;
                DataGridViewProducto.Rows[rowIndex].Cells["Cantidad"].Value = formularioHijo.Cantidad;
                DataGridViewProducto.Rows[rowIndex].Cells["PrecioUnitario"].Value = formularioHijo.PrecioVenta;
                decimal subTotal = formularioHijo.Cantidad * formularioHijo.PrecioVenta;
                DataGridViewProducto.Rows[rowIndex].Cells["SubTotal"].Value = subTotal;
            }
        }

        private void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            BuscarCliente formularioHijo = new BuscarCliente();
            formularioHijo.ShowDialog();

            idCliente = formularioHijo.IdSeleccionado;
            txtCliente.Text = formularioHijo.NombreSeleccionado;
            txtFalla.Text = formularioHijo.Diagnostico;
            txtNumeroOrden.Text = "Orden#" + formularioHijo.IdSeleccionado;
            ComboEstado.SelectedIndex = 2;
        }

        //private void ProductoSeleccionadoEventHandler(object sender, EventArgs e)
        //{


        //    //// Restar la cantidad vendida del stock del producto
        //    //productoSeleccionado.Stock -= cantidadSeleccionada;


        //    //// Actualizar el stock en la base de datos
        //    //string sql = "UPDATE productos SET Stock = Stock - @cantidad WHERE ID = @productoId";
        //    //SqlCommand cmd = new SqlCommand(sql, conn);
        //    //cmd.Parameters.AddWithValue("@cantidad", cantidadVendida);
        //    //cmd.Parameters.AddWithValue("@productoId", productoId);
        //    //cmd.ExecuteNonQuery();

        //    //// Actualizar el stock en el control DataGridView
        //    //string sqlSelect = "SELECT * FROM productos";
        //    //SqlDataAdapter da = new SqlDataAdapter(sqlSelect, conn);
        //    //DataTable dt = new DataTable();
        //    //da.Fill(dt);
        //    //dataGridView1.DataSource = dt;

        //}

    }
}
