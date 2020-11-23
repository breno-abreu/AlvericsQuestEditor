using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace AlvericsQuestEditor
{
    class GerenciadorArquivos
    {
        private string eventos;

        public GerenciadorArquivos()
        {
            eventos = "";
        }

        public void CarregarEventos()
        {
            string path = "";

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Carregar Arquivo de Eventos",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                RestoreDirectory = true,
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                path = openFileDialog.FileName;

            try
            {
                using (StreamReader reader = new StreamReader(@path))
                {
                    eventos = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
