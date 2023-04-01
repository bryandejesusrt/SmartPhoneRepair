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
    public partial class BuscarTecnico : Form
    {
        public BuscarTecnico()
        {
            InitializeComponent();
        }
        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        public int IdSeleccionado { get; set; }
        public string NombreSeleccionado { get; set; }

        private void MostrarTecnico()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Tecnicos";
                DataTable dt = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dt);

                    adaptador.Dispose();
                    conexion.Close();

                }
                catch (Exception ex)
                {
                    throw new Exception("Error al mostrar los tecnico: " + ex.Message);
                }

                DataGridViewTecnico.DataSource = dt;

            }
        }

        private void BuscarTecnicos(string busqueda)
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
            }

        }
        private void DataGridViewClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewTecnico.SelectedRows.Count > 0)
            {
                // Obtiene el ID y el nombre de la celda seleccionada
                IdSeleccionado = Convert.ToInt32(DataGridViewTecnico.SelectedRows[0].Cells["Id"].Value);
                NombreSeleccionado = DataGridViewTecnico.SelectedRows[0].Cells["NombreTecnico"].Value.ToString();

                // Cierra el formulario hijo
                this.Close();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarTecnicos(txtBuscar.Text);
            if (DataGridViewTecnico.Rows.Count < 1)
            {
                panelSearch.Visible = true;
            }
            else
            {
                panelSearch.Visible = false;
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {  
            Tecnicos formularioModal = new Tecnicos();
            Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
            UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);
            formularioModal.ShowDialog(formularioDeFondo);

            formularioModal.Dispose();
            formularioDeFondo.Dispose();
            formularioModal.Close();

            MostrarTecnico();
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

        private void BuscarTecnico_Load(object sender, EventArgs e)
        {
            MostrarTecnico();
        }
    }
}
