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

            string comboFileName = "combo.txt";
            string comboFilePath = Path.Combine(subfolderPath, comboFileName);
            string comboResultado = $"{username}:{password}\n";
            File.AppendAllText(comboFilePath, comboResultado);
        }
        public static void CargarRegistros(TreeView treeView, ImageList imageList)
        {
            string folderPath = Path.Combine(Environment.CurrentDirectory, "Resultados");

            if (Directory.Exists(folderPath))
            {
                treeView.Nodes.Clear();
                treeView.ImageList = imageList;

                DirectoryInfo rootDir = new DirectoryInfo(folderPath);
                TreeNode rootNode = new TreeNode(rootDir.Name)
                {
                    Tag = rootDir,
                    ImageIndex = 0,
                    SelectedImageIndex = 0
                };
                treeView.Nodes.Add(rootNode);
                CargarDirectorios(rootDir, rootNode);
            }
        }

        private static void CargarDirectorios(DirectoryInfo dir, TreeNode node)
        {
            try
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                {
                    TreeNode directoryNode = new TreeNode(directory.Name)
                    {
                        Tag = directory,
                        ImageIndex = 0,
                        SelectedImageIndex = 0
                    };
                    node.Nodes.Add(directoryNode);
                    CargarDirectorios(directory, directoryNode);
                }

                foreach (FileInfo file in dir.GetFiles("*.txt"))
                {
                    TreeNode fileNode = new TreeNode(file.Name)
                    {
                        Tag = file,
                        ImageIndex = 1,
                        SelectedImageIndex = 1
                    };
                    node.Nodes.Add(fileNode);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void AcercaDe() 
        {
            string message = @"IntelX Reader

¿Qué es IntelX Reader?
IntelX Reader es una aplicación de escritorio diseñada para ayudar a los usuarios a buscar y extraer información específica de archivos de texto contenidos dentro de directorios. Esta herramienta es especialmente útil para analizar grandes volúmenes de datos y encontrar información sensible de manera rápida y eficiente.

Características Principales:
- Selección de Directorio: Permite al usuario seleccionar un directorio que contiene varios archivos comprimidos y subdirectorios.
- Búsqueda de Archivos Específicos: Busca archivos de texto específicos, como 'Passwords.txt', dentro de todos los subdirectorios del directorio seleccionado.
- Extracción de Información: Extrae información específica de los archivos encontrados, como URLs, nombres de usuario y contraseñas.
- Guardado de Resultados: Guarda los resultados en archivos de texto organizados por fecha en una carpeta denominada 'Resultados'.
- Interfaz Intuitiva: Proporciona una interfaz gráfica de usuario (GUI) sencilla y fácil de usar.
- Barra de Progreso: Muestra el progreso de la búsqueda y extracción de información.
- Historial de Archivos Procesados: Permite visualizar un historial de los archivos procesados organizados por fecha y abrirlos directamente desde la aplicación.

¿Quién puede beneficiarse?
- Investigadores de Seguridad: Para analizar grandes volúmenes de datos en busca de información sensible.
- Analistas de Datos: Para extraer información relevante de conjuntos de datos grandes y desorganizados.
- Usuarios Avanzados: Para mantener registros organizados y acceder rápidamente a información específica dentro de archivos de texto.

Cómo Usar IntelX Reader:
1. Abrir la Aplicación: Inicie IntelX Reader desde su computadora.
2. Seleccionar un Directorio: Utilice el menú 'Archivo' para seleccionar un directorio que desea analizar.
3. Buscar Información: Ingrese el texto de interés en el cuadro correspondiente y haga clic en 'Iniciar'.
4. Ver Progreso: Observe la barra de progreso y el contador de resultados mientras la aplicación procesa los archivos.
5. Ver Resultados: Una vez completado, puede ver y abrir los resultados en la sección 'Archivos Procesados' del menú 'Ver'.
6. Abrir Resultados: Haga clic en cualquier archivo en el historial para abrirlo y ver el contenido extraído.

IntelX Reader facilita la tarea de encontrar y organizar información sensible en archivos de texto, proporcionando una solución eficiente y fácil de usar para profesionales y usuarios avanzados.";

            MessageBox.Show(message, "Acerca de IntelX Reader", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
