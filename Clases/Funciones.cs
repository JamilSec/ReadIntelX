using System.Drawing;
using System.Windows.Forms;

namespace ReadIntelX.Clases
{
    public class Funciones
    {
        public static string abrirArchivo()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Configura el filtro para los tipos de archivo .rar y .zip
                openFileDialog.Filter = "Archivos comprimidos|*.rar;*.zip";
                openFileDialog.Title = "Seleccionar un archivo comprimido";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName; // retorna nombre del archivo
                }
            }
            return null;
        }
        public static void actualizarEstadoUI(Control control, bool estado, string path = null)
        {
            if (control is Button btn)
            {
                btn.Enabled = estado;
            }
            else if (control is TextBox txt)
            {
                if (path != null)
                {
                    txt.Text = path;
                }
            }
        }
        public static bool manejarArchivoYBotones(TextBox pathTextBox, Button btnIniciar, Button btnDetener, Label lblEstado)
        {
            string filePath = abrirArchivo();
            if (string.IsNullOrEmpty(filePath) || !filePath.Contains("C:\\"))
            {
                actualizarEstadoUI(btnIniciar, false);
                actualizarEstadoUI(btnDetener, false);
                actualizarEstadoTexto(lblEstado, "Archivo no cargado", Color.Red);
                return false;
            }
            actualizarEstadoUI(btnIniciar, true);
            actualizarEstadoUI(pathTextBox, true, filePath);
            actualizarEstadoTexto(lblEstado, "Archivo cargado", Color.LightSeaGreen);
            actualizarEstadoUI(btnDetener, false);
            return true;
        }
        public static void actualizarEstadoTexto(Label txt, string mensaje, Color colorTexto)
        {
            txt.Text = mensaje;
            txt.ForeColor = colorTexto;
        }
        
    }
}
