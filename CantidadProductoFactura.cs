using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartPhone7
{
    public partial class CantidadProductoFactura : Form
    {
        public CantidadProductoFactura()
        {
            InitializeComponent();
        }
        public int Cantidad { get; set; }

        private void txtCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Este campo solo acepta numeros debido a que esta informacion que se guarda aqui debe de ser en este formato.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.KeyChar == '0')
            {
                e.Handled = true;
                MessageBox.Show("Este campo solo acepta numeros debido a que esta informacion que se guarda aqui debe de ser en este formato.", "Solo Numeros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Cantidad = Convert.ToInt16(txtCantidad.Text);
            Close();
        }
    }
}
