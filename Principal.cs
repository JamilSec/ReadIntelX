using System;
using System.Drawing;
using System.Windows.Forms;
using static ReadIntelX.Clases.Funciones;

namespace ReadIntelX
{
    public partial class Principal : Form
    {
        FormBuscar dialogoBuscar = new FormBuscar();
        public Principal()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Comandos de menú Archivo
            switch (keyData)
            {
                case Keys.Control | Keys.A:
                    abrirArchivo();
                    return true;
                case Keys.Control | Keys.B:
                    Console.WriteLine("Comando B, Busca Archivos");
                    return true;
                case Keys.Control | Keys.D:
                    Console.WriteLine("Comando D, Abre Documentacion");
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirArchivo();
        }
        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            pgbProceso.Value = 0; // Reiniciar el ProgressBar
            pgbProceso.Maximum = 100; // Establecer el máximo en 100 para representar porcentajes

            actualizarEstadoUI(btnIniciar, false);
            actualizarEstadoUI(btnDetener, true);
            actualizarEstadoTexto(lblEstado, "Procesando...", Color.LimeGreen);
            await ProcesarArchivosAsync(txtPath.Text, lblPalabraBuscar.Text, lblEncontrados, pgbProceso, this);
            actualizarEstadoUI(btnDetener, false);
            actualizarEstadoTexto(lblEstado, "Proceso completado", Color.DarkGreen);
        }
        private void btnDetener_Click(object sender, EventArgs e)
        {
            actualizarEstadoUI(btnDetener, false);
            actualizarEstadoTexto(lblEstado, "Proceso detenido", Color.IndianRed);
        }
        private void abrirArchivo()
        {
            bool archivoCargadoCorrectamente = manejarArchivoYBotones(txtPath, btnIniciar, btnDetener, lblEstado);
            if (archivoCargadoCorrectamente)
            {
                if (dialogoBuscar.ShowDialog() == DialogResult.OK)
                {
                    lblPalabraBuscar.Text = dialogoBuscar.PalabraBuscada;
                }
            }
        }
    }
}
