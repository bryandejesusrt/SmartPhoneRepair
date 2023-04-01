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
    public partial class Producto : Form
    {
        public Producto()
        {
            InitializeComponent();
        }
        Conexion conexion1 = new Conexion();
        bool editar = false;

        public static int IdProducto = 0;


        private void LimpiarControles()
        {
            txtNombreProducto.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtDecripcion.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtStock.Text = string.Empty;
            txtPrecioCompra.Text = string.Empty;
            txtPrecioVenta.Text = string.Empty;
            txtNota.Text = string.Empty;

        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreProducto.Text))
            {
                MessageBox.Show("El campo nombre es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMarca.Text))
            {
                MessageBox.Show("El campo Marca es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("El campo Stock es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPrecioVenta.Text))
            {
                MessageBox.Show("El campo Precio de Venta es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPrecioCompra.Text))
            {
                MessageBox.Show("El campo Precio de Compra es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ListarUsuariosServicio()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Producto WHERE Id = @id";
                DataTable dt = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@id", IdProducto);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        editar = true;
                        txtNombreProducto.Text = dt.Rows[0]["NombreProducto"].ToString();
                        txtMarca.Text = dt.Rows[0]["ModeloMarca"].ToString();
                        txtDecripcion.Text = dt.Rows[0]["DescripcionProducto"].ToString();
                        txtCodigo.Text = dt.Rows[0]["Codigo"].ToString();
                        txtStock.Text = dt.Rows[0]["Stock"].ToString();
                        txtPrecioCompra.Text = dt.Rows[0]["PrecioCompra"].ToString();
                        txtPrecioVenta.Text = dt.Rows[0]["PrecioVenta"].ToString();
                        txtNota.Text = dt.Rows[0]["Nota"].ToString();

                    }
                    adaptador.Dispose();
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la información del registro: " + ex.Message);
                }
            }
        }

        private void GuardarServicio()
        {
            if (!ValidarCampos()) return;

            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = (editar) ?
                    "UPDATE Producto SET NombreProducto = @NombreProducto, ModeloMarca = @ModeloMarca, DescripcionProducto = @DescripcionProducto, Codigo = @Codigo, Stock = @Stock, PrecioCompra = @PrecioCompra, PrecioVenta = @PrecioVenta, Nota = @Nota WHERE Id = @id" :
                    "INSERT INTO Producto (NombreProducto, ModeloMarca, DescripcionProducto, Codigo, Stock, PrecioCompra, PrecioVenta, Nota) VALUES (@NombreProducto, @ModeloMarca, @DescripcionProducto, @Codigo, @Stock, @PrecioCompra, @PrecioVenta, @Nota)";

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@NombreProducto", txtNombreProducto.Text.Trim());
                    comando.Parameters.AddWithValue("@ModeloMarca", txtMarca.Text.Trim());
                    comando.Parameters.AddWithValue("@DescripcionProducto", txtDecripcion.Text.Trim());
                    comando.Parameters.AddWithValue("@Codigo", txtCodigo.Text.Trim());
                    comando.Parameters.AddWithValue("@Stock", txtStock.Text.Trim());
                    comando.Parameters.AddWithValue("@PrecioCompra", txtPrecioCompra.Text.Trim());
                    comando.Parameters.AddWithValue("@PrecioVenta", txtPrecioVenta.Text.Trim());
                    comando.Parameters.AddWithValue("@Nota", txtNota.Text.Trim());


                    if (editar) comando.Parameters.AddWithValue("@id", IdProducto);

                    int resultado = comando.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        MessageBox.Show("Producto guardado exitosamente.");
                        btnCancelar.Text = "Salir";
                        editar = false;
                        IdProducto = 0;
                        LimpiarControles();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el Producto.");
                    }

                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el Producto: " + ex.Message);
                }
            }
        }

        private void Producto_Load(object sender, EventArgs e)
        {
            LimpiarControles();
            ListarUsuariosServicio();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarServicio();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EfectoModal_Tick_1(object sender, EventArgs e)
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

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("El campo Stock de producto solo acepta numeros.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("El campo Precio de compra del producto solo acepta numeros.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("El campo Precio de venta del producto solo acepta numeros.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
