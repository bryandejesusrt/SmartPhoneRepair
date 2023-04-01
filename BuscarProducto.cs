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
    public partial class BuscarProducto : Form
    {
        public BuscarProducto()
        {
            InitializeComponent();
        }
        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        public int IdSeleccionado { get; set; }
        public string NombreProducto { get; set; }
        public string ModeloMarca { get; set; }
        public int Stock { get; set; }
        public int PrecioVenta { get; set; }
        public int Cantidad { get; set; }

        private void MostrarProducto()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Producto";
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
                    throw new Exception("Error al mostrar los Producto: " + ex.Message);
                }

                DataGridViewProducto.DataSource = dt;

            }
        }

        private void BuscarProductos(string busqueda)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Producto WHERE NombreProducto LIKE @busqueda OR ModeloMarca LIKE @busqueda OR Codigo LIKE @busqueda OR DescripcionProducto  LIKE @busqueda";
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
                    throw new Exception("Error al buscar los Productos: " + ex.Message);
                }

                DataGridViewProducto.DataSource = dt;
            }

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarProductos(txtBuscar.Text);
            if (DataGridViewProducto.Rows.Count < 1)
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
            Producto formularioModal = new Producto();

            Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
            UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);

            formularioModal.ShowDialog(formularioDeFondo);
            formularioModal.Dispose();
            formularioModal.Close();
            MostrarProducto();
        }

        private void BuscarProducto_Load(object sender, EventArgs e)
        {
            MostrarProducto();
        }

        private void DataGridViewProducto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            CantidadProductoFactura formularioModal = new CantidadProductoFactura();
            formularioModal.ShowDialog();

            if (formularioModal.Cantidad > 0)
            {
                if (DataGridViewProducto.SelectedRows.Count > 0)
                 {
                    IdSeleccionado = Convert.ToInt32(DataGridViewProducto.SelectedRows[0].Cells["Id"].Value);
                    NombreProducto = DataGridViewProducto.SelectedRows[0].Cells["NombreProducto"].Value.ToString();
                    ModeloMarca = DataGridViewProducto.SelectedRows[0].Cells["ModeloMarca"].Value.ToString();
                    Stock = Convert.ToInt32(DataGridViewProducto.SelectedRows[0].Cells["Stock"].Value);
                    PrecioVenta = Convert.ToInt32(DataGridViewProducto.SelectedRows[0].Cells["PrecioVenta"].Value);
                    Cantidad = formularioModal.Cantidad;
                    this.Close();
                }       
            }

            
        }

        private void DataGridViewProducto_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
