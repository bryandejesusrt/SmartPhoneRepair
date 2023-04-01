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
    public partial class ListadoTecnicos : Form
    {
        public ListadoTecnicos()
        {
            InitializeComponent();
        }

        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        // Objeto de conexión a la base de datos
        public int id = 0;

        private void MostrarTecnico()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Tecnicos";
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
                    throw new Exception("Error al mostrar los tecnico: " + ex.Message);
                }

                DataGridViewTecnico.DataSource = dtUsuarios;
                lblTotal.Text = DataGridViewTecnico.RowCount.ToString();

            }
        }

        private void BuscarTecnico(string busqueda)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Tecnicos WHERE NombreTecnico LIKE @busqueda OR Cedula LIKE @busqueda OR PersonaContacto LIKE @busqueda OR CorreoElectronico LIKE @busqueda";
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
                    throw new Exception("Error al buscar los tecnicos: " + ex.Message);
                }

                DataGridViewTecnico.DataSource = dt;
                lblTotal.Text = DataGridViewTecnico.RowCount.ToString();
            }

        }

        private void EliminarTecnico(int id)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "DELETE FROM Tecnicos WHERE Id = @id";

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

        private void ListadoTecnicos_Load(object sender, EventArgs e)
        {
            MostrarTecnico();
            DataGridViewTecnico.ClearSelection();
        }

        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            BuscarTecnico(txtBuscar.Text);
            if (DataGridViewTecnico.Rows.Count < 1)
            {
                panelSearch.Visible = true;
            }
            else
            {
                panelSearch.Visible = false;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            id = 0;
            Tecnicos formularioModal = new Tecnicos();
            Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
            UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);
            formularioModal.ShowDialog(formularioDeFondo);
            formularioModal.Dispose();
            formularioDeFondo.Dispose();

            MostrarTecnico();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                Tecnicos.IdTecnico = id;
                Tecnicos formularioModal = new Tecnicos();
                Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
                UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);
                formularioModal.ShowDialog(formularioDeFondo);
                formularioModal.Dispose();
                formularioDeFondo.Dispose();

                MostrarTecnico();
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (DataGridViewTecnico.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Estas Seguro que quieres eliminar este tecnico, esto significara que no esta informacion no volvera a estar disponible", "Eliminar tecnico" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    EliminarTecnico(id);
                    MostrarTecnico();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            UtilidadesFormularios.ExportToPDF(DataGridViewTecnico, "Listado de todo los tecnicos");
        }

        private void DataGridViewTecnico_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridViewTecnico.Rows[e.RowIndex];
                id = Convert.ToInt16(row.Cells[0].Value);
            }
        }
    }
}
