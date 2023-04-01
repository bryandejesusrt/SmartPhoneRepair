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
    public partial class BuscarCliente : Form
    {
        public BuscarCliente()
        {
            InitializeComponent();
        }
        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        public int IdSeleccionado { get; set; }
        public string NombreSeleccionado { get; set; }
        public string Diagnostico { get; set; }
        public string MarcaModelo { get; set; }
        


        private void MostrarClientes()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Clientes";
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
                    throw new Exception("Error al mostrar los Clientes: " + ex.Message);
                }

                DataGridViewClientes.DataSource = dt;

            }
        }

        private void BuscarClientes(string busqueda)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Clientes WHERE NombreCliente LIKE @busqueda OR Telefono  LIKE @busqueda OR MarcaModelo LIKE @busqueda OR Diagnostico  LIKE @busqueda";
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
                    throw new Exception("Error al buscar los clientes: " + ex.Message);
                }

                DataGridViewClientes.DataSource = dtUsuarios;
            }

        }

        private void DataGridViewClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewClientes.SelectedRows.Count > 0)
            {
                // Obtiene el ID y el nombre de la celda seleccionada
                IdSeleccionado = Convert.ToInt32(DataGridViewClientes.SelectedRows[0].Cells["Id"].Value);
                NombreSeleccionado = DataGridViewClientes.SelectedRows[0].Cells["NombreCliente"].Value.ToString();
                MarcaModelo = DataGridViewClientes.SelectedRows[0].Cells["MarcaModelo"].Value.ToString();
                Diagnostico = DataGridViewClientes.SelectedRows[0].Cells["Diagnostico"].Value.ToString();
                // Cierra el formulario hijo
                this.Close();
            }

        }
        private void BuscarCliente_Load(object sender, EventArgs e)
        {
            MostrarClientes();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
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

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Clientes formularioModal = new Clientes();

            Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
            UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);

            formularioModal.ShowDialog(formularioDeFondo);
            formularioModal.Dispose();
            formularioDeFondo.Dispose();
            formularioModal.Close();
            MostrarClientes();
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
    }
}
