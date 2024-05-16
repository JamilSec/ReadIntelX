using System.IO;
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace ReadIntelX
{
    public partial class FormArchivosProcesados : Form
    {
        public TreeView TvResultados { get { return tvResultados; } }
        public FormArchivosProcesados()
        {
            InitializeComponent();
        }

        private void tvResultados_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;

            if (selectedNode.Tag is FileInfo)
            {
                FileInfo archivo = (FileInfo)selectedNode.Tag;
                Process.Start(new ProcessStartInfo
                {
                    FileName = archivo.FullName,
                    UseShellExecute = true
                });
            }
        }
    }
}
