using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SFML.System;

namespace AlvericsQuestEditor
{
    class GerenciadorArquivos
    {
        /* Classe para carregar e salvar arquivos */

        // String que contém o texto que representa a linha de eventos do jogo
        private string eventos;

        // Lista contendo todos os diálogos relacionados à NPCs
        private List<Dialogo> dialogos;

        public GerenciadorArquivos()
        {
            /* Inicializa o texto de eventos e a lista de diálogos */

            eventos = "";
            dialogos = new List<Dialogo>();
        }

        public void CarregarEventos()
        {
            /* Carrega o atributo 'eventos' com o conteúdo de um arquivo de texto */

            string path = "";

            // Cria a tela de carregamento padrão do Windows
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Carregar Arquivo de Eventos",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                RestoreDirectory = true,

                // Define que será aceito apenas arquivos com extensão txt'
                Filter = "txt files (*.txt)|*.txt",
            };

            // Abre a janela de carregamento, se o botão 'OK' for pressionado 'path' recebe o caminho para o arquivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
            }

            
            if (path != "")
            {
                // Lê todo o conteúdo do arquivo de texto e salvar na variável 'eventos'
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

        public void CarregarDialogo(Vector2f posNPC)
        {
            /* Carrega o texto de uma rquivo de texto e o associa com um NPC, criando um novo diálogo */

            string path = "";

            // Ajusta a posição em que o mouse foi pressionado, para estar de acordo com as variáveis de posição corretas
            int auxX = posNPC.X >= 0 ? 8 : -8;
            int auxY = posNPC.Y >= 0 ? 8 : -8;

            auxX = (int)(posNPC.X / 16) * 16 + auxX;
            auxY = (int)(posNPC.Y / 16) * 16 + auxY;

            // Cria a tela de carregamento
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Carregar Arquivo de Diálogo",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                RestoreDirectory = true,

                // Define que será aceito apenas arquivos com extensão txt'
                Filter = "txt files (*.txt)|*.txt",
            };

            // Abre a janela de carregamento, se o botão 'OK' for pressionado 'path' recebe o caminho para o arquivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                path = openFileDialog.FileName;

            if (path != "")
            {
                // Lê um arquivo de texto e cria ou modifica um objeto Dialogo
                try
                {
                    using (StreamReader reader = new StreamReader(@path))
                    {
                        Dialogo auxD = null;

                        // Procura um NPC na posição recebida
                        foreach (Dialogo dialogo in dialogos)
                        {
                            if (dialogo.PosNPC.X == auxX && dialogo.PosNPC.Y == auxY)
                                auxD = dialogo;
                        }

                        // Caso não tenha achado NPC, cria um objeto de Dialogo
                        if (auxD == null)
                            dialogos.Add(new Dialogo(new Vector2f(auxX, auxY), reader.ReadToEnd()));

                        // Caso tenha um NPC naquela posição, modifica seu diálogo
                        else
                            auxD.Texto = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void CarregarMusica(Musica m)
        {
            /* Recebe o título de um arquivo MP3 e o associa a um objeto de Musica */

            string titulo = "";

            // Cria a janela de carregamento
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Carregar Arquivo de Musica",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                RestoreDirectory = true,

                // Define que será aceito apenas arquivos com extensão 'mp3'
                Filter = "audio files (*.mp3)|*.mp3",
            };

            // Abre a janela de carregamento, se o botão 'OK' for pressionado 'titulo' recebe o titulo de um arquivo MP3
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                titulo = openFileDialog.SafeFileName;

                // Associa o título recebido à um objeto de Musica
                m.Titulo = titulo;
            }
        }

        public void LimparArquivos()
        {
            /* Limpa a variável 'eventos' e a lista de diálogos */

            eventos = "";
            dialogos.Clear();
        }

        public void SalvarMundo(List<Entidade> et, List<Entidade> ei, List<Armadilha> arm, List<Mecanismo> mec, List<Escada> esc)
        {
            /* Salva as entidades criadas em um mundo.
             * Esse processo não salva conexões, propriedades, eventos, diálogos e músicas, apenas a localização e textura de uma entidade*/

            string path = "";

            // Cria a janela de salvamento
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Define que o arquivo salvo terá a extensão 'aql' (Alveric's Quest Level)
            saveFileDialog.Filter = "aql files (*.aql)|*.aql";
            saveFileDialog.RestoreDirectory = true;

            // Abre a janela de salvamento, se o botão 'OK' for pressionado 'path' recebe o caminho para o arquivo
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog.FileName; 
            }

            if (path != "")
            {
                try
                {
                    // Salva informações sobre o mundo em um arquivo binário

                    // Cria um arquivo
                    using FileStream output = File.Create(path);

                    // Cria um BinaryWriter que irá escrever o arquivo
                    using (BinaryWriter writer = new BinaryWriter(output))
                    {
                        // Salva a quantidade de elementos exsitentes em cada lista recebida
                        writer.Write(et.Count);
                        writer.Write(ei.Count);
                        writer.Write(mec.Count);
                        writer.Write(arm.Count);
                        writer.Write(esc.Count);

                        // Salva os elementos da lista de entidades tangíveis
                        foreach (Entidade entidade in et)
                        {
                            writer.Write(entidade.ESprite.Position.X);
                            writer.Write(entidade.ESprite.Position.Y);
                            writer.Write(entidade.posicaoSprite.X);
                            writer.Write(entidade.posicaoSprite.Y);
                        }

                        // Salva os elementos da lista de entidades intangíveis
                        foreach (Entidade entidade in ei)
                        {
                            writer.Write(entidade.ESprite.Position.X);
                            writer.Write(entidade.ESprite.Position.Y);
                            writer.Write(entidade.posicaoSprite.X);
                            writer.Write(entidade.posicaoSprite.Y);
                        }

                        // Salva os elementos da lista de armadilhas
                        foreach (Armadilha armadilha in arm)
                        {
                            writer.Write(armadilha.ESprite.Position.X);
                            writer.Write(armadilha.ESprite.Position.Y);
                            writer.Write(armadilha.TempoInicial);
                            writer.Write(armadilha.TempoEntreAtivacoes);
                            writer.Write(armadilha.TempoAtivo);
                        }

                        // Salva os elementos da lista de mecanismos
                        foreach (Mecanismo mecanismo in mec)
                        {
                            writer.Write(mecanismo.ESprite.Position.X);
                            writer.Write(mecanismo.ESprite.Position.Y);
                            writer.Write(mecanismo.armadilhas.Count);
                            foreach (Armadilha armadilha in mecanismo.armadilhas)
                            {
                                writer.Write(armadilha.ESprite.Position.X);
                                writer.Write(armadilha.ESprite.Position.Y);
                            }
                        }

                        // Salva os elementos da lista de escadas
                        foreach (Escada escada in esc)
                        {
                            writer.Write(escada.ESprite.Position.X);
                            writer.Write(escada.ESprite.Position.Y);

                            if (escada.EscadaCon != null)
                            {
                                writer.Write(escada.EscadaCon.ESprite.Position.X);
                                writer.Write(escada.EscadaCon.ESprite.Position.Y);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void CarregarMundo(GerenciadorEntidades ger)
        {
            /* Carrega um mundo a partir de um arquivo binário
             * Não carrega mecanismos, escadas, armadilhas, músicas, diálogos e eventos */

            string path = "";

            // Cria uma janela de carregamento
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Carregar Mundo",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "aql",
                RestoreDirectory = true,

                // Define que o arquivo carregado terá a extensão 'aql' (Alveric's Quest Level)
                Filter = "aql files (*.aql)|*.aql",
            };

            // Abre a janela de carregamento, se o botão 'OK' for pressionado 'path' recebe o caminho para o arquivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                path = openFileDialog.FileName;

            if(path != "")
            {
                try
                {
                    // Limpa o mundo atual
                    ger.LimparListas();

                    using FileStream input = File.OpenRead(path);

                    // Abre um arquivo binário e restaura um mundo com suas informações
                    using (BinaryReader reader = new BinaryReader(input))
                    {
                        // Recebe a quandiade de elementos extistentes em cada lista
                        int etCount = reader.ReadInt32();
                        int eiCount = reader.ReadInt32();
                        int mecCount = reader.ReadInt32();
                        int armCount = reader.ReadInt32();
                        int escCount = reader.ReadInt32();

                        // Recria as entidades tangíeis salvas
                        for (int i = 0; i < etCount; i++)
                            ger.InserirEntidade(reader.ReadSingle(), reader.ReadSingle(), reader.ReadInt32(), reader.ReadInt32());

                        // Recria as entidades intangíeis salvas
                        for (int i = 0; i < eiCount; i++)
                            ger.InserirEntidade(reader.ReadSingle(), reader.ReadSingle(), reader.ReadInt32(), reader.ReadInt32());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
