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
    public partial class Clientes : Form
    {
        public Clientes()
        {
            InitializeComponent();
        }

        Conexion conexion1 = new Conexion();
        bool editar = false;

        public static int IdCliente = 0;

        private void LimpiarControles()
        {
            txtNombreCLiente.Text = string.Empty;
            txtNumeroCliente.Text = string.Empty;
            txtModeloDispositivo.Text = string.Empty;
            txtSerie.Text = string.Empty;
            TxtDiagnostico.Text = string.Empty;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreCLiente.Text))
            {
                MessageBox.Show("El campo nombre es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNumeroCliente.Text))
            {
                MessageBox.Show("El campo numero es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtModeloDispositivo.Text))
            {
                MessageBox.Show("El campo modelo es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ListarClientesEditar()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Clientes WHERE Id = @id";
                DataTable dtUsuario = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@id", IdCliente);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dtUsuario);

                    if (dtUsuario.Rows.Count > 0)
                    {
                        editar = true;
                        txtNombreCLiente.Text = dtUsuario.Rows[0]["NombreCliente"].ToString();
                        txtNumeroCliente.Text = dtUsuario.Rows[0]["Telefono"].ToString();
                        txtModeloDispositivo.Text = dtUsuario.Rows[0]["MarcaModelo"].ToString();
                        txtSerie.Text = dtUsuario.Rows[0]["SerieIMEI"].ToString();
                        TxtDiagnostico.Text = dtUsuario.Rows[0]["Diagnostico"].ToString();
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

        private void GuardarCliente()
        {
            if (!ValidarCampos()) return;

            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = (editar) ?
                    "UPDATE Clientes SET NombreCliente = @NombreCliente, Telefono = @Telefono, MarcaModelo = @MarcaModelo, SerieIMEI = @SerieIMEI, Diagnostico = @Diagnostico WHERE Id = @id" :
                    "INSERT INTO Clientes (NombreCliente, Telefono, MarcaModelo, SerieIMEI, Diagnostico) VALUES (@NombreCliente, @Telefono, @MarcaModelo, @SerieIMEI, @Diagnostico)";

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@NombreCliente", txtNombreCLiente.Text.Trim());
                    comando.Parameters.AddWithValue("@Telefono", txtNumeroCliente.Text.Trim());
                    comando.Parameters.AddWithValue("@MarcaModelo", txtModeloDispositivo.Text.Trim());
                    comando.Parameters.AddWithValue("@SerieIMEI", txtSerie.Text.Trim());
                    comando.Parameters.AddWithValue("@Diagnostico", TxtDiagnostico.Text.Trim());

                    if (editar) comando.Parameters.AddWithValue("@id", IdCliente);

                    int resultado = comando.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        MessageBox.Show("Cliente guardado exitosamente.");
                        btnCancelar.Text = "Salir";
                        editar = false;
                        IdCliente = 0;
                        LimpiarControles();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el Cliente.");
                    }

                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el Cliente: " + ex.Message);
                }
            }
        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            LimpiarControles();
            ListarClientesEditar();
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            GuardarCliente();
            LimpiarControles();
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

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNumeroCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("El campo Numero de telefono solo acepta numeros.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
