using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SmartPhone7
{
    public partial class ListadoUsuarios : Form
    {

        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        // Objeto de conexión a la base de datos
        public int id = 0;

        public ListadoUsuarios()
        {
            InitializeComponent();

        }

        private void MostrarUsuario()
        {           
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Usuario";
                DataTable dtUsuarios = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dtUsuarios);

                    adaptador.Dispose();
                    conexion.Close();

                }
                catch (Exception ex)
                {
                    throw new Exception("Error al mostrar los usuarios: " + ex.Message);
                }

                DataGridViewUsuario.DataSource = dtUsuarios;
                lblTotal.Text = DataGridViewUsuario.RowCount.ToString();

            }
        }

        private void BuscarUsuario(string busqueda)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Usuario WHERE Nombre LIKE @busqueda OR Correo LIKE @busqueda OR Rol LIKE @busqueda OR NombreUsuario LIKE @busqueda";
                DataTable dtUsuarios = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dtUsuarios);

                    adaptador.Dispose();
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar los usuarios: " + ex.Message);
                }

                DataGridViewUsuario.DataSource = dtUsuarios;
                lblTotal.Text = DataGridViewUsuario.RowCount.ToString();
            }

        }

        private void EliminarUsuario(int id)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "DELETE FROM Usuario WHERE Id = @id";

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@id", id);
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Registro eliminado correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún registro con el Id especificado.");
                    }

                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el registro: " + ex.Message);
                }
            }
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            MostrarUsuario();
            DataGridViewUsuario.ClearSelection();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
             BuscarUsuario(txtBuscar.Text);
            if (DataGridViewUsuario.Rows.Count < 1)
            {
                panelSearch.Visible = true;
            }
            else
            {
                panelSearch.Visible = false;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            id = 0;
            // Crear el formulario modal
            Usuario formularioModal = new Usuario();

            // Crear el formulario de fondo
            Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);

            // Mostrar el formulario modal centrado en el formulario principal
            UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);

            // Mostrar el formulario modal como cuadro de diálogo
            formularioModal.ShowDialog(formularioDeFondo);

            // Liberar recursos
            formularioModal.Dispose();
            formularioDeFondo.Dispose();

            //Actualizar datagrid
            MostrarUsuario();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                Usuario.IdUsuario = id;
                Usuario formularioModal = new Usuario();
                Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
                UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);
                formularioModal.ShowDialog(formularioDeFondo);
                formularioModal.Dispose();
                formularioDeFondo.Dispose();

                MostrarUsuario();
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewUsuario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Verifica que el usuario haya hecho clic en una fila, no en los encabezados de las columnas
            {
                DataGridViewRow row = DataGridViewUsuario.Rows[e.RowIndex];
                id = Convert.ToInt16(row.Cells[0].Value); // Obtiene el valor de la celda en la columna 0 de la fila seleccionada
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (DataGridViewUsuario.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Estas Seguro que quieres eliminar este usuario, esto significara que no esta informacion no volvera a estar disponible", "Eliminar usuario" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    EliminarUsuario(id);
                    MostrarUsuario();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            UtilidadesFormularios.ExportToPDF(DataGridViewUsuario, "Listado de todo los usuarios");
        }

        private void panelSearch_Paint(object sender, PaintEventArgs e)
        {

        }

    }

}
