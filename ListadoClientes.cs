using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace SmartPhone7
{
    public partial class ListadoClientes : Form
    {
        public ListadoClientes()
        {
            InitializeComponent();
        }
        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        // Objeto de conexión a la base de datos
        public int id =0;

        private void MostrarClientes()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Clientes";
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
                    throw new Exception("Error al mostrar los clientes: " + ex.Message);
                }

                DataGridViewClientes.DataSource = dtUsuarios;
                lblTotal.Text = DataGridViewClientes.RowCount.ToString();

            }
        }

        private void BuscarClientes(string busqueda)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Clientes WHERE NombreCliente LIKE @busqueda OR Telefono  LIKE @busqueda OR MarcaModelo LIKE @busqueda OR Diagnostico  LIKE @busqueda";
                DataTable dt = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dt);

                    adaptador.Dispose();
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar los Clientes: " + ex.Message);
                }

                DataGridViewClientes.DataSource = dt;
                lblTotal.Text = DataGridViewClientes.RowCount.ToString();
            }

        }

        private void EliminarClientes(int id)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "DELETE FROM Clientes WHERE Id = @id";

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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            id = 0;
            Clientes formularioModal = new Clientes();

            Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
            UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);

            formularioModal.ShowDialog(formularioDeFondo);
            formularioModal.Dispose();
            formularioDeFondo.Dispose();

            MostrarClientes();
        }

        private void ListadoClientes_Load(object sender, EventArgs e)
        {
            MostrarClientes();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (DataGridViewClientes.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Estas Seguro que quieres eliminar este cliente, esto significara que no esta informacion no volvera a estar disponible", "Eliminar cliente" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    EliminarClientes(id);
                    MostrarClientes();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                DataGridViewRow row = DataGridViewClientes.Rows[e.RowIndex];
                id = Convert.ToInt16(row.Cells[0].Value);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                Clientes.IdCliente = id;
                Clientes formularioModal = new Clientes();
                Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
                UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);
                formularioModal.ShowDialog(formularioDeFondo);
                formularioModal.Dispose();
                formularioDeFondo.Dispose();

                MostrarClientes();
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            BuscarClientes(txtBuscar.Text);
            if (DataGridViewClientes.Rows.Count < 1)
            {
                panelSearch.Visible = true;
            }
            else
            {
                panelSearch.Visible = false;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
          UtilidadesFormularios.ExportToPDF(DataGridViewClientes, "Listado de todo los clientes");
        }
    }
}
