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
    public partial class ListadoFactura : Form
    {
        public ListadoFactura()
        {
            InitializeComponent();
        }

        // Cadena de conexión a la base de datos
        Conexion conexion1 = new Conexion();

        // Objeto de conexión a la base de datos
        public int id = 0;

        private void MostrarFactura()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT c.NombreCliente AS NombreCliente, f.NumeroOrden, f.FechaFactura, t.NombreTecnico AS NombreTecnico, f.Falla, f.ServicioOfrecido \r\nFROM Factura AS f\r\nINNER JOIN Clientes c ON f.IdCliente = c.Id \r\nINNER JOIN Tecnicos t ON f.IdTecnico = t.Id;";
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
                    throw new Exception("Error al mostrar los usuarios: " + ex.Message);
                }

                DataGridViewFactura.DataSource = dt;
                lblTotal.Text = DataGridViewFactura.RowCount.ToString();

            }
        }

        private void BuscarFactura(string busqueda)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT c.NombreCliente AS NombreCliente, f.NumeroOrden, f.FechaFactura, t.NombreTecnico AS NombreTecnico, f.Falla, f.ServicioOfrecido \r\nFROM Factura AS f\r\nINNER JOIN Clientes c ON f.IdCliente = c.Id \r\nINNER JOIN Tecnicos t ON f.IdTecnico = t.Id\r\nWhere c.NombreCliente LIKE @busqueda OR f.NumeroOrden LIKE @busqueda OR  t.NombreTecnico LIKE @busqueda OR  f.Falla LIKE @busqueda OR  f.ServicioOfrecido LIKE @busqueda  ";
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
                    throw new Exception("Error al buscar: " + ex.Message);
                }

                DataGridViewFactura.DataSource = dtUsuarios;
                lblTotal.Text = DataGridViewFactura.RowCount.ToString();
            }

        }

        private void EliminarFactura(int id)
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "DELETE FROM Factura WHERE Id = @id";

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

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            id = 0;

            Factura formularioModal = new Factura();
            Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
            UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);

            formularioModal.ShowDialog(formularioDeFondo);
            formularioModal.Dispose();
            formularioDeFondo.Dispose();
        }

        private void ListadoFactura_Load(object sender, EventArgs e)
        {
            MostrarFactura();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarFactura(txtBuscar.Text);
            if (DataGridViewFactura.Rows.Count < 1)
            {
                panelSearch.Visible = true;
            }
            else
            {
                panelSearch.Visible = false;
            }
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            UtilidadesFormularios.ExportToPDF(DataGridViewFactura, "Listado de Facturas");
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                //Factura.IdFactura = id;
                //Factura.IdCliente = id;
                //Factura.IdIdTecnico = id;
                Clientes formularioModal = new Clientes();
                Form formularioDeFondo = UtilidadesFormularios.CrearFormularioDeFondo(Principal.ActiveForm);
                UtilidadesFormularios.CentrarFormularioModal(formularioModal, Principal.ActiveForm);
                formularioModal.ShowDialog(formularioDeFondo);
                formularioModal.Dispose();
                formularioDeFondo.Dispose();

                MostrarFactura();
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewFactura_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridViewFactura.Rows[e.RowIndex];
                id = Convert.ToInt16(row.Cells[0].Value);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (DataGridViewFactura.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Estas Seguro que quieres eliminar esta Factura, esto significara que no esta informacion no volvera a estar disponible", "Eliminar Factura" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    EliminarFactura(id);
                    MostrarFactura();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Primero tienes que seleccionar la fila que deaseas eliminar para poder realizar la accion", "Error" + id.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
    }
}
