using Guna.UI2.WinForms;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SmartPhone7
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        public static int posicionX, PosicionY;
        public void container(object _form)
        {

            if (guna2Panel_container.Controls.Count > 0) guna2Panel_container.Controls.Clear();

            Form fm = _form as Form;
            fm.TopLevel = false;
            fm.FormBorderStyle = FormBorderStyle.None;
            fm.Dock = DockStyle.Fill;
            guna2Panel_container.Controls.Add(fm);
            guna2Panel_container.Tag = fm;
            fm.Show();

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            // Buscar botones en el panel
            foreach (var btn in panelButtons.Controls.OfType<Guna2Button>())
            {
                btn.Visible = btn.Text.ToLower().Contains(txtSearchButtom.Text.ToLower().Trim());
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            label_val.Text = "Lista De Usuarios";
            guna2PictureBox_val.Image = Properties.Resources.person__1_;
            container(new Inicio());
        }
        
        private void Principal_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            label_val.Text = "Descripción general del panel";
            guna2PictureBox_val.Image = Properties.Resources.dashboard__12_;
            container( new Inicio());

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            label_val.Text = "Facturacion";
            guna2PictureBox_val.Image = Properties.Resources.dashboard__12_;
            container(new ListadoFactura());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            label_val.Text = "Equipos";
            guna2PictureBox_val.Image = Properties.Resources.user__5_;
            container(new ListadoProducto());
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            label_val.Text = "Clientes";
            guna2PictureBox_val.Image = Properties.Resources.dashboard__12_;
            container(new ListadoClientes());
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            label_val.Text = "Tecnicos";
            guna2PictureBox_val.Image = Properties.Resources.user__5_;
            container(new ListadoTecnicos());
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            label_val.Text = "Facturacion";
            guna2PictureBox_val.Image = Properties.Resources.dashboard__12_;
            container(new ListadoFactura());
        }

        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            label_val.Text = "Servicios";
            guna2PictureBox_val.Image = Properties.Resources.dashboard__12_;
            container(new ListadoServicios());
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            label_val.Text = "Usuarios";
            guna2PictureBox_val.Image = Properties.Resources.user__5_;
            container(new ListadoUsuarios());
        }
    }
}
