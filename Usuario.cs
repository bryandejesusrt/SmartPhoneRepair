using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SmartPhone7
{
    public partial class Usuario : Form
    {
        Conexion conexion1 = new Conexion();
        bool editar = false;

        public static int IdUsuario = 0;
        public Usuario()
        {
            InitializeComponent();            
        }

        private void LimpiarControles()
        {
            txtNombre.Text = string.Empty;
            txtGmail.Text = string.Empty;
            cbRol.Text = string.Empty;
            txtNombreUsuario.Text = string.Empty;
            txtContraseña.Text = string.Empty;
            txtConfirmarContraseña.Text = string.Empty;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El campo nombre es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGmail.Text))
            {
                MessageBox.Show("El campo correo es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(txtGmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("El formato del correo no cumple con los criterios mínimos de un correo electrónico. Por favor, revisa el campo correo.", "Correo no válido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
            {
                MessageBox.Show("El campo nombre de usuario es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                MessageBox.Show("El campo contraseña es obligatorio.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(txtContraseña.Text, @"^(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres y contener al menos una letra mayúscula y un número. Ejemplo: Abc1234", "Contraseña no válida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtContraseña.Text != txtConfirmarContraseña.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor, intenta nuevamente.", "Contraseñas no coinciden.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ListarUsuariosEditar()
        {
            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = "SELECT * FROM Usuario WHERE Id = @id";
                DataTable dtUsuario = new DataTable();

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@id", IdUsuario);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(dtUsuario);

                    if (dtUsuario.Rows.Count > 0)
                    {
                        editar = true;
                        txtNombre.Text = dtUsuario.Rows[0]["Nombre"].ToString();
                        txtGmail.Text = dtUsuario.Rows[0]["Correo"].ToString();
                        cbRol.Text = dtUsuario.Rows[0]["Rol"].ToString();
                        txtNombreUsuario.Text = dtUsuario.Rows[0]["NombreUsuario"].ToString();
                        txtContraseña.Text = dtUsuario.Rows[0]["Contrasena"].ToString();
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

        private void GuardarUsuario()
        {
            if (!ValidarCampos()) return;

            using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
            {
                string consulta = (editar) ?
                    "UPDATE Usuario SET Nombre = @nombre, Correo = @correo, Rol = @rol, NombreUsuario = @nombreUsuario, Contrasena = @contrasena WHERE Id = @id" :
                    "INSERT INTO Usuario (Nombre, Correo, Rol, NombreUsuario, Contrasena) VALUES (@nombre, @correo, @rol, @nombreUsuario, @contrasena)";

                try
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    comando.Parameters.AddWithValue("@correo", txtGmail.Text.Trim());
                    comando.Parameters.AddWithValue("@rol", cbRol.Text.Trim());
                    comando.Parameters.AddWithValue("@nombreUsuario", txtNombreUsuario.Text.Trim());
                    comando.Parameters.AddWithValue("@contrasena", txtContraseña.Text.Trim());
                    comando.Parameters.AddWithValue("@contrasena", txtConfirmarContraseña.Text.Trim());

                    if (editar) comando.Parameters.AddWithValue("@id", IdUsuario);

                    int resultado = comando.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        MessageBox.Show("Usuario guardado exitosamente.");
                        btnCancelar.Text = "Salir";
                        editar = false;
                        IdUsuario = 0;
                        LimpiarControles();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el usuario.");
                    }

                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el usuario: " + ex.Message);
                }
            }
        }

        private void EfectoModal_Tick(object sender, EventArgs e)
        {
            if(Opacity >= 1)
            {
                EfectoModal.Stop();
            }
            else
            {
                Opacity += 0.4;
            }
        }

        private void NuevoUsuario_Load(object sender, EventArgs e)
        {
            LimpiarControles();
            ListarUsuariosEditar();
        }        

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarUsuario();
            IdUsuario = 0;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            this.Close();
        }

        private void txtConfirmarContraseña_TextChanged(object sender, EventArgs e)
        {
            if (txtContraseña.Text != txtConfirmarContraseña.Text)
                lblErrorContraseña.Visible = true;
            else lblErrorContraseña.Visible = false;            
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            ojoCerrado.Visible = true;
            ojoAbierto.Visible = false;
            txtContraseña.UseSystemPasswordChar = false;
            txtConfirmarContraseña.UseSystemPasswordChar = false;                        
        }

        private void ojoCerrado_Click(object sender, EventArgs e)
        {
            ojoAbierto.Visible = true;
            ojoCerrado.Visible = false;
            txtContraseña.UseSystemPasswordChar = true;
            txtConfirmarContraseña.UseSystemPasswordChar = true;
        }
    }
}
