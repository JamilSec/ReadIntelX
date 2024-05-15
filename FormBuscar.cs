using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadIntelX
{
    public partial class FormBuscar : Form
    {
        public string PalabraBuscada { get; private set; }
        private string textoPredefinido = "Ingrese texto a buscar";
        public FormBuscar()
        {
            InitializeComponent();
            btnAceptar.DialogResult = DialogResult.OK;
            AcceptButton = btnAceptar;

            btnAceptar.Click += (sender, e) => {
                if (!string.IsNullOrWhiteSpace(txtBuscar.Text) && txtBuscar.Text != textoPredefinido)
                {
                    PalabraBuscada = txtBuscar.Text;
                }
            };
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            btnAceptar.Enabled = !string.IsNullOrWhiteSpace(txt.Text) && txt.Text != textoPredefinido;
        }
        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                txt.Text = textoPredefinido;
            }
        }
        private async void txtBuscar_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == "Ingrese texto a buscar")
            {
                await Task.Delay(1000);
                txt.Text = "";
            }
        }
    }
}
