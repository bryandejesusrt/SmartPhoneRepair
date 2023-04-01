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
            //txtNombre.Text = string.Empty;
            //txtGmail.Text = string.Empty;
            //cbRol.Text = string.Empty;
            //txtNombreUsuario.Text = string.Empty;
            //txtContraseña.Text = string.Empty;
            //txtConfirmarContraseña.Text = string.Empty;
        }

        //private bool ValidarCampos()
        //{
        //    if (string.IsNullOrWhiteSpace(txtNombre.Text))
        //    {
        //        MessageBox.Show("El campo nombre es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(txtGmail.Text))
        //    {
        //        MessageBox.Show("El campo correo es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    if (!Regex.IsMatch(txtGmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        //    {
        //        MessageBox.Show("El formato del correo no cumple con los criterios mínimos de un correo electrónico. Por favor, revisa el campo correo.", "Correo no válido", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
        //    {
        //        MessageBox.Show("El campo nombre de usuario es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(txtContraseña.Text))
        //    {
        //        MessageBox.Show("El campo contraseña es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    if (!Regex.IsMatch(txtContraseña.Text, @"^(?=.*[A-Z])(?=.*\d).{8,}$"))
        //    {
        //        MessageBox.Show("La contraseña debe tener al menos 8 caracteres y contener al menos una letra mayúscula y un número. Ejemplo: Abc1234", "Contraseña no válida", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    if (txtContraseña.Text != txtConfirmarContraseña.Text)
        //    {
        //        MessageBox.Show("Las contraseñas no coinciden. Por favor, intenta nuevamente.", "Contraseñas no coinciden.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    return true;
        //}

        //private void ListarUsuariosEditar()
        //{
        //    using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
        //    {
        //        string consulta = "SELECT * FROM Usuario WHERE Id = @id";
        //        DataTable dtUsuario = new DataTable();

        //        try
        //        {
        //            conexion.Open();
        //            SqlCommand comando = new SqlCommand(consulta, conexion);
        //            comando.Parameters.AddWithValue("@id", IdUsuario);
        //            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
        //            adaptador.Fill(dtUsuario);

        //            if (dtUsuario.Rows.Count > 0)
        //            {
        //                editar = true;
        //                txtNombre.Text = dtUsuario.Rows[0]["Nombre"].ToString();
        //                txtGmail.Text = dtUsuario.Rows[0]["Correo"].ToString();
        //                cbRol.Text = dtUsuario.Rows[0]["Rol"].ToString();
        //                txtNombreUsuario.Text = dtUsuario.Rows[0]["NombreUsuario"].ToString();
        //                txtContraseña.Text = dtUsuario.Rows[0]["Contrasena"].ToString();
        //            }
        //            adaptador.Dispose();
        //            conexion.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error al cargar la información del registro: " + ex.Message);
        //        }
        //    }
        //}

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
