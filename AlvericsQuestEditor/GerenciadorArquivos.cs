using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SFML.System;

namespace AlvericsQuestEditor
{
    class GerenciadorArquivos
    {
        private string eventos;
        private List<Dialogo> dialogos;

        public GerenciadorArquivos()
        {
            eventos = "";
            dialogos = new List<Dialogo>();
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

        public void CarregarDialogo(Vector2f posNPC)
        {
            string path = "";

            int auxX = posNPC.X >= 0 ? 8 : -8;
            int auxY = posNPC.Y >= 0 ? 8 : -8;

            auxX = (int)(posNPC.X / 16) * 16 + auxX;
            auxY = (int)(posNPC.Y / 16) * 16 + auxY;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Carregar Arquivo de Diálogo",

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
                    Dialogo auxD = null;

                    foreach(Dialogo dialogo in dialogos)
                    {
                        if (dialogo.PosNPC.X == auxX && dialogo.PosNPC.Y == auxY)
                            auxD = dialogo;
                    }

                    if (auxD == null)
                        dialogos.Add(new Dialogo(new Vector2f(auxX, auxY), reader.ReadToEnd()));

                    else
                        auxD.Texto = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CarregarMusica(Musica m)
        {
            string titulo = "";

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Carregar Arquivo de Musica",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "audio files (*.mp3)|*.mp3",
                RestoreDirectory = true,
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                titulo = openFileDialog.SafeFileName;
                m.Titulo = titulo;
                //Console.WriteLine(m.Titulo);
            }
        }

        public void LimparArquivos()
        {
            eventos = "";
            dialogos.Clear();
        }

        public void SalvarMundo(List<Entidade> et, List<Entidade> ei, List<Armadilha> arm, List<Mecanismo> mec, List<Escada> esc)
        {
            string path = "";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "aql files (*.aql)|*.aql";
            saveFileDialog.RestoreDirectory = true;

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog.FileName; 
            }

            try
            {
                using FileStream output = File.Create(path);
                using (BinaryWriter writer = new BinaryWriter(output))
                {
                    writer.Write(et.Count);
                    writer.Write(ei.Count);
                    writer.Write(mec.Count);
                    writer.Write(arm.Count);
                    writer.Write(esc.Count);

                    foreach (Entidade entidade in et)
                    {
                        writer.Write(entidade.ESprite.Position.X);
                        writer.Write(entidade.ESprite.Position.Y);
                        writer.Write(entidade.posicaoSprite.X);
                        writer.Write(entidade.posicaoSprite.Y);
                    }

                    foreach (Entidade entidade in ei)
                    {
                        writer.Write(entidade.ESprite.Position.X);
                        writer.Write(entidade.ESprite.Position.Y);
                        writer.Write(entidade.posicaoSprite.X);
                        writer.Write(entidade.posicaoSprite.Y);
                    }

                    foreach (Armadilha armadilha in arm)
                    {
                        writer.Write(armadilha.ESprite.Position.X);
                        writer.Write(armadilha.ESprite.Position.Y);
                        writer.Write(armadilha.TempoInicial);
                        writer.Write(armadilha.TempoEntreAtivacoes);
                        writer.Write(armadilha.TempoAtivo);
                    }

                    foreach (Mecanismo mecanismo in mec)
                    {
                        writer.Write(mecanismo.ESprite.Position.X);
                        writer.Write(mecanismo.ESprite.Position.Y);
                        writer.Write(mecanismo.armadilhas.Count);
                        foreach(Armadilha armadilha in mecanismo.armadilhas)
                        {
                            writer.Write(armadilha.ESprite.Position.X);
                            writer.Write(armadilha.ESprite.Position.Y);
                        }
                    }

                    foreach (Escada escada in esc)
                    {
                        writer.Write(escada.ESprite.Position.X);
                        writer.Write(escada.ESprite.Position.Y);
                        writer.Write(escada.EscadaConn.ESprite.Position.X);
                        writer.Write(escada.EscadaConn.ESprite.Position.Y);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CarregarMundo(GerenciadorEntidades ger)
        {

        }
    }
}
