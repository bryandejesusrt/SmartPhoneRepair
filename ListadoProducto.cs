using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartPhone7
{
    public partial class ListadoProducto : Form
    {
        public ListadoProducto()
        {
            InitializeComponent();
        }
        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        // Objeto de conexión a la base de datos
        public int id = 0;

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

                DataGridViewProductos.DataSource = dt;
                lblTotal.Text = DataGridViewProductos.RowCount.ToString();

            }
        }

        private void BuscarProducto(string busqueda)
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

                DataGridViewProductos.DataSource = dt;
                lblTotal.Text = DataGridViewProductos.RowCount.ToString();
            }

        }

        private void EliminarProducto(int id)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "DELETE FROM Producto WHERE Id = @id";

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

        private void ListadoProducto_Load(object sender, EventArgs e)
        {
            MostrarProducto();
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            UtilidadesFormularios.ExportToPDF(DataGridViewProductos, "Listado de todo los productos");
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarProducto(txtBuscar.Text);
            if (DataGridViewProductos.Rows.Count < 1)
            {
                panelSearch.Visible = true;
            }
            else
            {
                panelSearch.Visible = false;
            }
        }

        private void DataGridViewProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridViewProductos.Rows[e.RowIndex];
                id = Convert.ToInt16(row.Cells[0].Value);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (DataGridViewProductos.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Estas Seguro que quieres eliminar este Producto, esto significara que no esta informacion no volvera a estar disponible", "Eliminar Producto" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    EliminarProducto(id);
                    MostrarProducto();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                Producto.IdProducto = id;
                Producto formularioModal = new Producto();
                Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
                UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);
                formularioModal.ShowDialog(formularioDeFondo);
                formularioModal.Dispose();
                formularioDeFondo.Dispose();

                MostrarProducto();
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            id = 0;
            Producto formularioModal = new Producto();

            Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
            UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);

            formularioModal.ShowDialog(formularioDeFondo);
            formularioModal.Dispose();
            formularioDeFondo.Dispose();

            MostrarProducto();
        }
    }
}
