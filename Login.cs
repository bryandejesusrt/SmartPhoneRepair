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
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
        }
Conexion conexion1 = new Conexion();
        public class UsuarioService
        {

            public bool ValidarUsuario(string usuario, string contrasena)
            {
               
               using (SqlConnection conexion = new SqlConnection(conexion1.cadenaConexion))
               {
                  var query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario AND Contrasena = @Contrasena";
                  var command = new SqlCommand(query, conexion);
                  command.Parameters.AddWithValue("@Usuario", usuario);
                  command.Parameters.AddWithValue("@Contrasena", contrasena);
                  conexion.Open();
                  var count = (int)command.ExecuteScalar();
                  return count > 0;
               }
            }
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //var usuario = txtUsuario.Text;
            //var contrasena = txtContrasena.Text;
            //var usuarioService = new UsuarioService(conexion);
            //if (usuarioService.ValidarUsuario(usuario, contrasena))
            //{
            //    Loading _load = new Loading();
            //    _load.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Nombre de usuario o contraseña incorrectos");
            //}
            
        }
    }
}
