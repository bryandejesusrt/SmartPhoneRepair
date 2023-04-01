using Guna.UI2.WinForms.Suite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace SmartPhone7
{
    public partial class Tecnicos : Form
    {

        public Tecnicos()
        {
            InitializeComponent();
        }

        Conexion conexion1 = new Conexion();
        bool editar = false;

        public static int IdTecnico = 0;
 
        private void LimpiarControles()
        {
            txtNombre.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtCedula.Text = string.Empty;
            dtFechaEntrada.Value = DateTime.Now;
            txtDireccion.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtPersonContacto.Text = string.Empty;
            txtTelefonoPersonContacto.Text = string.Empty;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El campo nombre es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(txtCorreo.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("El formato del correo no cumple con los criterios mínimos de un correo electrónico. Por favor, revisa el campo correo.", "Correo no válido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCedula.Text))
            {
                MessageBox.Show("El campo cedula de usuario es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPersonContacto.Text))
            {
                MessageBox.Show("El campo de una persona de contacto es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTelefonoPersonContacto.Text))
            {
                MessageBox.Show("El campo Telefono de una persona de contacto es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ListarTecnicosEditar()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Tecnicos WHERE Id = @id";
                DataTable dt = new DataTable();
                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@id", IdTecnico);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        editar = true;
                        txtNombre.Text = dt.Rows[0]["NombreTecnico"].ToString();
                        txtTelefono.Text = dt.Rows[0]["Telefono"].ToString();
                        txtCedula.Text = dt.Rows[0]["Cedula"].ToString();
                        DateTime fecha = Convert.ToDateTime(dt.Rows[0]["FechaEntrada"]);
                        dtFechaEntrada.Value = fecha;
                        txtDireccion.Text = dt.Rows[0]["Direccion"].ToString();
                        txtCorreo.Text = dt.Rows[0]["CorreoElectronico"].ToString();
                        txtPersonContacto.Text = dt.Rows[0]["PersonaContacto"].ToString();
                        txtTelefonoPersonContacto.Text = dt.Rows[0]["TelefonoPersonaContacto"].ToString();
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

        private void GuardarTecnico()
        {
            if (!ValidarCampos()) return;

            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = (editar) ?
                    "UPDATE Tecnicos SET NombreTecnico = @NombreTecnico, Telefono = @Telefono, Cedula = @Cedula, FechaEntrada = @FechaEntrada, Direccion = @Direccion, CorreoElectronico = @CorreoElectronico, PersonaContacto = @PersonaContacto, TelefonoPersonaContacto = @TelefonoPersonaContacto  WHERE Id = @id" :
                    "INSERT INTO Tecnicos (NombreTecnico, Telefono, Cedula, FechaEntrada, Direccion, CorreoElectronico, PersonaContacto, TelefonoPersonaContacto) VALUES (@NombreTecnico, @Telefono, @Cedula, @FechaEntrada, @Direccion, @CorreoElectronico , @PersonaContacto, @TelefonoPersonaContacto)";

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@NombreTecnico", txtNombre.Text.Trim());
                    comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                    comando.Parameters.AddWithValue("@Cedula", txtCedula.Text.Trim());

                    DateTime fechaSeleccionada = dtFechaEntrada.Value;
                    comando.Parameters.AddWithValue("@FechaEntrada", fechaSeleccionada);
                     
                    comando.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());
                    comando.Parameters.AddWithValue("@CorreoElectronico", txtCorreo.Text.Trim());
                    comando.Parameters.AddWithValue("@PersonaContacto", txtPersonContacto.Text.Trim());
                    comando.Parameters.AddWithValue("@TelefonoPersonaContacto", txtTelefonoPersonContacto.Text.Trim());



                    if (editar) comando.Parameters.AddWithValue("@id", IdTecnico);

                    int resultado = comando.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        MessageBox.Show("Tecnico guardado exitosamente.");
                        btnCancelar.Text = "Salir";
                        editar = false;
                        IdTecnico = 0;
                        LimpiarControles();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el tecnicos.");
                    }

                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el tecnicos: " + ex.Message);
                }
            }
        }

        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("El campo numero de telefono solo acepta numeros.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2TextBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("El campo numero de telefono solo acepta numeros.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void Tecnicos_Load(object sender, EventArgs e)
        {
            LimpiarControles();
            ListarTecnicosEditar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarTecnico();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTelefonoPersonContacto_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
