using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ReadIntelX.Clases
{
    public class Funciones
    {
        public static string SeleccionarDirectorio()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    return fbd.SelectedPath;
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
            string filePath = SeleccionarDirectorio();
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
        public static async Task ProcesarArchivosAsync(string path, string textoBusqueda, Label lblEncontrados, ProgressBar pgbProceso, Form form)
        {
            try
            {
                await Task.Run(() =>
                {
                    var todosLosArchivos = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    var archivosDePasswords = todosLosArchivos.Where(f => Path.GetFileName(f).Equals("Passwords.txt", StringComparison.OrdinalIgnoreCase)).ToList();

                    int totalArchivos = archivosDePasswords.Count;
                    int archivoActual = 0;
                    int totalEncontrados = 0;
                    foreach (var archivo in archivosDePasswords)
                    {
                        int encontradosEnArchivoActual = ProcesarArchivo(archivo, textoBusqueda, textoBusqueda);
                        archivoActual++;
                        totalEncontrados += encontradosEnArchivoActual; // Acumular resultados

                        form.Invoke((MethodInvoker)delegate
                        {
                            lblEncontrados.Text = $"{totalEncontrados} Resultados";
                            pgbProceso.Value = (int)((archivoActual / (double)totalArchivos) * 100);
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando archivos: {ex.Message}");
            }
        }
        private static int ProcesarArchivo(string pathArchivo, string url, string textoBusqueda)
        {
            int cantidad = 0;
            string contenido = File.ReadAllText(pathArchivo);
            string[] lineas = contenido.Split('\n');
            for (int i = 0; i < lineas.Length - 2; i++)
            {
                if (Regex.IsMatch(lineas[i], @"URL:\s*(.+)"))
                {
                    Match matchUrl = Regex.Match(lineas[i], @"URL:\s*(.+)");
                    string urlActual = matchUrl.Groups[1].Value.Trim();
                    if (urlActual.Contains(url))
                    {
                        cantidad++;
                        string username = Regex.Match(lineas[i + 1], @"Username:\s*(.+)").Groups[1].Value.Trim();
                        string password = Regex.Match(lineas[i + 2], @"Password:\s*(.+)").Groups[1].Value.Trim();
                        GuardarContenido(urlActual, username, password, textoBusqueda);
                    }
                }
            }
            return cantidad;
        }
        private static void GuardarContenido(string url, string username, string password, string textoBusqueda)
        {
            string folderPath = Path.Combine(Environment.CurrentDirectory, "Resultados");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fechaActual = DateTime.Now.ToString("yyyy-MM-dd");
            string subfolderPath = Path.Combine(folderPath, fechaActual);

            if (!Directory.Exists(subfolderPath))
            {
                Directory.CreateDirectory(subfolderPath);
            }

            string fileName = $"resultado_{textoBusqueda}.txt";
            string filePath = Path.Combine(subfolderPath, fileName);

            string resultado = $"URL: {url}\nUsername: {username}\nPassword: {password}\n===============\n";
            File.AppendAllText(filePath, resultado);
        }
    }
}
