using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartPhone7
{
    public partial class ListadoServicios : Form
    {
        public ListadoServicios()
        {
            InitializeComponent();
        }
        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        // Objeto de conexión a la base de datos
        public int id = 0;


        private void MostrarServicio()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Servicios";
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
                    throw new Exception("Error al mostrar los Servicios: " + ex.Message);
                }

                DataGridViewServicio.DataSource = dtUsuarios;
                lblTotal.Text = DataGridViewServicio.RowCount.ToString();

            }
        }

        private void BuscarServicio(string busqueda)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Servicios WHERE NombreServicio LIKE @busqueda OR Descripcion LIKE @busqueda";
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
                    throw new Exception("Error al buscar los Servicios: " + ex.Message);
                }

                DataGridViewServicio.DataSource = dtUsuarios;
                lblTotal.Text = DataGridViewServicio.RowCount.ToString();
            }

        }

        private void EliminarServicio(int id)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "DELETE FROM Servicios WHERE Id = @id";

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

        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            BuscarServicio(txtBuscar.Text);
            if (DataGridViewServicio.Rows.Count < 1)
            {
                panelSearch.Visible = true;
            }
            else
            {
                panelSearch.Visible = false;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                Servicio.IdServicio = id;
                Servicio formularioModal = new Servicio();
                Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
                UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);
                formularioModal.ShowDialog(formularioDeFondo);
                formularioModal.Dispose();
                formularioDeFondo.Dispose();

                MostrarServicio();
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            id = 0;
            // Crear el formulario modal
            Servicio formularioModal = new Servicio();

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
            MostrarServicio();
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            UtilidadesFormularios.ExportToPDF(DataGridViewServicio, "Listado de todo los servicios");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (DataGridViewServicio.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Estas Seguro que quieres eliminar este Servicio, esto significara que no esta informacion no volvera a estar disponible", "Eliminar Servicio" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    EliminarServicio(id);
                    MostrarServicio();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewServicio_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { 
                DataGridViewRow row = DataGridViewServicio.Rows[e.RowIndex];
                id = Convert.ToInt16(row.Cells[0].Value); 
            }
        }

        private void ListadoServicios_Load(object sender, EventArgs e)
        {
            MostrarServicio();
            DataGridViewServicio.ClearSelection();
        }
    }
}
