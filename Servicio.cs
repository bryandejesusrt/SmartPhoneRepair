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
    public partial class Servicio : Form
    {
        public Servicio()
        {
            InitializeComponent();
        }
        Conexion conexion1 = new Conexion();
        bool editar = false;

        public static int IdServicio = 0;


        private void LimpiarControles()
        {
            txtNombreServicio.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtPrecioServicio.Text = string.Empty;
            txtNota.Text = string.Empty;

        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreServicio.Text))
            {
                MessageBox.Show("El campo nombre es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrecioServicio.Text))
            {
                MessageBox.Show("El campo Precio del servicio es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ListarUsuariosServicio()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Servicios WHERE Id = @id";
                DataTable dtUsuario = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@id", IdServicio);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dtUsuario);

                    if (dtUsuario.Rows.Count > 0)
                    {
                        editar = true;
                        txtNombreServicio.Text = dtUsuario.Rows[0]["NombreServicio"].ToString();
                        txtCodigo.Text = dtUsuario.Rows[0]["Codigo"].ToString();
                        txtPrecioServicio.Text = dtUsuario.Rows[0]["Precio"].ToString();
                        txtNota.Text = dtUsuario.Rows[0]["Descripcion"].ToString();
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
                    "UPDATE Servicios SET NombreServicio = @NombreServicio, Codigo = @Codigo, Precio = @Precio, Descripcion = @Descripcion WHERE Id = @id" :
                    "INSERT INTO Servicios (NombreServicio, Codigo, Precio, Descripcion) VALUES (@NombreServicio, @Codigo, @Precio, @Descripcion)";

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@NombreServicio", txtNombreServicio.Text.Trim());
                    comando.Parameters.AddWithValue("@Codigo", txtCodigo.Text.Trim());
                    comando.Parameters.AddWithValue("@Precio", txtPrecioServicio.Text.Trim());
                    comando.Parameters.AddWithValue("@Descripcion", txtNota.Text.Trim());

                    if (editar) comando.Parameters.AddWithValue("@id", IdServicio);

                    int resultado = comando.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        MessageBox.Show("Servicio guardado exitosamente.");
                        btnCancelar.Text = "Salir";
                        editar = false;
                        IdServicio = 0;
                        LimpiarControles();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el Servicio.");
                    }

                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el servicio: " + ex.Message);
                }
            }
        }

        private void Servicio_Load(object sender, EventArgs e)
        {
            LimpiarControles();
            ListarUsuariosServicio();
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            GuardarServicio();
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Close();
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

        private void txtPrecioServicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("El campo Precio de producto solo acepta numeros.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
