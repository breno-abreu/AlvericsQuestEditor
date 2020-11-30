using System;
using System.Windows.Forms;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Threading;

namespace AlvericsQuestEditor
{
    public static class Informacoes
    {
        /* Classe contendo informações sobre o programa */

        // Indica a quantidade de linhas e colunas na matriz de entidades principal
        public static Vector2i qtdEntidades = new Vector2i(5, 37);

        // Indica o nome do arquivo contendo a imagem que servirá de base para a matriz de entidade principal
        public static string entidadesImgPath = @"all.png";

        // Proporção do menu em relação ao tamanho da tela
        public static float propViewMenu = .3f;

        // Proporção do mundo em relação ao tamanho da tela
        public static float propViewMundo = .7f;
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            /* Cria o editor e o executa */ 

            Editor editor = new Editor();
            editor.Executar();
        }
    }

    class Editor
    {
        /* Classe principal do editor */

        // Referência para a janela do programa
        private RenderWindow window;

        // Porção da tela separada para o mundo
        private SFML.Graphics.View viewMundo;

        // Porção da tela separada para o menu
        private SFML.Graphics.View viewMenu;

        // Menu principal
        private Menu menu;

        // Comprimento inicial da tela
        private const uint comprimentoInicialTela = 1600;

        // Algura inicial da tela
        private const uint alturaInicialTela = 900;

        // Tempo em segundos entre frames
        private float deltatime;

        // Relógio para calcular o tempo em segundos entre frames
        private Clock clock;

        // Velocidade da rolagem da roda do mouse
        private const float velViewMenu = 50000f;

        // Coordenadas da view do menu
        private Vector2f coordViewMenu;

        // Posição inicial em Y da view do menu
        private float yViewMenuInicial;

        // Mundo onde serão aplicados os objetos
        private Mundo mundo;

        // Fator de zoom da view do mundo
        private int zoom;

        // Acao indicada pelo jogador, mudará os eventos do programa
        private Acao acao;

        // Indica que outras ações não podem ser efetuadas se essa variável for verdadeira
        private bool bloqueandoAcoes;

        // Posição do mouse em relação ao mundo
        private Vector2f posMouseMundo;

        // Indica se um novo mundo deve ser criado
        private bool novoMundo;

        // Gerenciador de arquivos para salvar e carregar arquivos
        private GerenciadorArquivos gerenciadorArquivos;

        public Editor()
        {
            /* Inicializa o editor */

            // Inicializa a janela do programa
            window = new RenderWindow(new VideoMode(comprimentoInicialTela, alturaInicialTela), 
                                      "Alveric's Quest Editor", Styles.Default);

            // Cria uma view para o mundo
            viewMundo = new SFML.Graphics.View();

            // Seleciona a fração da tela ocupada por essa view
            viewMundo.Viewport = new FloatRect(0f, 0f, Informacoes.propViewMundo, 1f);
            viewMundo.Size = new Vector2f(window.Size.X * Informacoes.propViewMundo, window.Size.Y);
            viewMundo.Center = new Vector2f(0, 0);

            // Cria uma view para o menu
            viewMenu = new SFML.Graphics.View();

            // Seleciona a fração da tela ocupada por essa view
            viewMenu.Viewport = new FloatRect(Informacoes.propViewMundo, 0f, Informacoes.propViewMenu, 1f);
            viewMenu.Size = new Vector2f(window.Size.X * Informacoes.propViewMenu, window.Size.Y);

            // Tenta carregar o ícone da janela, caso não consigo o programa é finalizado
            try
            {
                Image icone = new Image(@"MenuIcons\window_icon.png");
                window.SetIcon(icone.Size.X, icone.Size.Y, icone.Pixels);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            // Inicializa o menu
            menu = new Menu(window, viewMenu);

            // Inizialica o mundo
            mundo = new Mundo(window, viewMundo);

            // Aplica o zoom na view do mundo
            zoom = 10;
            AplicarZoomMundo();

            deltatime = 0;
            clock = new Clock();

            coordViewMenu = viewMenu.Center;
            yViewMenuInicial = coordViewMenu.Y;
            bloqueandoAcoes = false;
            posMouseMundo = new Vector2f();
            novoMundo = false;
            gerenciadorArquivos = new GerenciadorArquivos();

            /* Inclui um método para os event handlers: */
            // Método chamado quando ó botão de finalizar o programa é pressionado
            window.Closed += Window_Close;

            // Método chamado quando a tela é redimensionada
            window.Resized += Window_Resized;

            // Método chamado quando um botão do mouse é pressionado
            window.MouseButtonPressed += Window_MouseButtonPressed;

            // Método chamado quando a roda do mouse é usada
            window.MouseWheelScrolled += Window_MouseWheelScrolled;
        }

        public void Executar()
        {
            while (window.IsOpen)
            {
                // Atualza o valor do deltatime entre dois frames
                deltatime = clock.Restart().AsSeconds();

                // Trata os eventos da janela
                window.DispatchEvents();
                
                // Muda para a view do mundo e desenha o mundo
                window.SetView(viewMundo);
                mundo.Desenhar();
                mundo.Mover(deltatime, -zoom);

                // Muda para a viewMenu e desenha o menu na tela
                window.SetView(viewMenu);
                menu.Desenhar();

                TratarEventos();

                // Executa a janela
                window.Display();

                // Limpa a janela
                window.Clear(Color.White);
            }
        }

        private void TratarEventos()
        {
            /* Trata certos eventos */

            // Retorna as coordenadas do mouse em relação à janela
            posMouseMundo = window.MapPixelToCoords(Mouse.GetPosition(window), viewMundo);

            // Adiciona um objeto caso seja permitido na posiçã em que o botão esquerdo do mouse foi pressionado
            if (acao == Acao.AdicionarObjeto &&
                Mouse.GetPosition(window).X <= window.Size.X * Informacoes.propViewMundo &&
                Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                mundo.GerenciadorEnt.InserirEntidade(posMouseMundo.X, posMouseMundo.Y);
            }

            // Exclui um objeto na posição em que o botão esquerdo do mouse foi pressionado
            else if (acao == Acao.ExcluirObjeto &&
                Mouse.GetPosition(window).X <= window.Size.X * Informacoes.propViewMundo &&
                Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                mundo.GerenciadorEnt.ExcluirEntidade(posMouseMundo.X, posMouseMundo.Y);
            }

            // Cria uma thread para criar uma nova janela mostrando um aviso antes de um novo mundo ser criado
            else if (acao == Acao.NovoMundo)
            {
                acao = Acao.Nenhum;
                bloqueandoAcoes = true;
                Thread t1 = new Thread(MostrarAvisoNovoMundo);
                t1.Start();
            }

            // Cria um novo mundo limpando todas as listas
            if (novoMundo)
            {
                novoMundo = false;
                mundo.NovoMundo();
                gerenciadorArquivos.LimparArquivos();
                viewMundo.Center = new Vector2f(0, 0);
            }
        }

        public void MostrarAvisoNovoMundo()
        {
            /* Mostra uma janela contendo um aviso antes de um novo mundo ser criado. Se a opção for sim, um munod novo é criado */

            DialogResult dialogResult = MessageBox.Show("Deseja criar um novo mundo?\nTodo o progresso não salvo será perdido!",
                                                            "Aviso", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                novoMundo = true;

            menu.ResetarCorBotoes();
            bloqueandoAcoes = false;
        }

        private void Window_Resized(object sender, EventArgs e)
        {
            /* Método chamado quando a tela é redimensionada */

            // Redimensionar menu
            viewMenu.Size = new Vector2f(window.Size.X * Informacoes.propViewMenu, window.Size.Y);
            menu.Redimensionar();

            // Redimensionar mundo
            viewMundo.Size = new Vector2f(window.Size.X * Informacoes.propViewMundo, window.Size.Y);
            AplicarZoomMundo();
        }

        private void Window_Close(object sender, EventArgs e)
        {
            /* Método chamado quando o botão 'fechar janela' é pressionado */

            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            /* Eventos quando os botões do mouse são pressionados */

            // Caso o botão esquerdo do mouse seja pressionado
            if (e.Button == Mouse.Button.Left && !bloqueandoAcoes)
            {
                // Caso seja pressionado no menu
                if (Mouse.GetPosition(window).X >= window.Size.X * Informacoes.propViewMundo)
                {
                    Acao botaoPressionado = menu.BotaoPressionado(Mouse.GetPosition(window));
                    acao = botaoPressionado;

                    if (botaoPressionado == Acao.IndicarEntidade)
                    {
                        mundo.GerenciadorEnt.PosicaoEntidade = menu.PosicaoEntidade;
                        acao = Acao.AdicionarObjeto;
                    }
                     
                    if (acao != Acao.GerenciarPropriedades)
                    {
                        menu.ArmadilhaAux = null;
                        menu.AtualizarValores(Acao.Nenhum);
                        mundo.GerenciadorEnt.ApagarQuadradoArmadilhas();
                    }

                    if (acao != Acao.GerenciarConexao)
                    {
                        mundo.ArmadilhaAux = null;
                        mundo.MecanismoAux = null;

                        if (acao != Acao.GerenciarPropriedades)
                        {
                            mundo.GerenciadorEnt.ApagarQuadradoArmadilhas();
                            mundo.GerenciadorEnt.ApagarQuadradoMecanismos();
                        }

                        else
                            mundo.GerenciadorEnt.ApagarQuadradoMecanismos();
                    }

                    if (acao == Acao.GerenciarEventos)
                         gerenciadorArquivos.CarregarEventos();

                    else if (acao == Acao.Salvar)
                         gerenciadorArquivos.SalvarMundo(mundo.GerenciadorEnt.EntidadesTangiveis,
                                                         mundo.GerenciadorEnt.EntidadesIntangiveis,
                                                         mundo.GerenciadorEnt.Armadilhas,
                                                         mundo.GerenciadorEnt.Mecanismos,
                                                         mundo.GerenciadorEnt.Escadas);
                    else if (acao == Acao.Carregar)
                    {
                        zoom = 5;
                        AplicarZoomMundo();
                        viewMundo.Center = new Vector2f(0, 0);
                        gerenciadorArquivos.CarregarMundo(mundo.GerenciadorEnt);
                    }
                        
                }

                // Caso o botão seja pressionado no mundo
                else
                {
                    switch (acao)
                    {
                        // Gerencia as propriedades de uma armadilha
                        case Acao.GerenciarPropriedades:
                            menu.ArmadilhaAux = mundo.GerenciadorEnt.SelecionarArmadilha(posMouseMundo.X, posMouseMundo.Y);
                            mundo.GerenciadorEnt.ApagarQuadradoArmadilhas();

                            if (menu.ArmadilhaAux != null)
                                menu.ArmadilhaAux.Selecionado = true;

                            menu.AtualizarValores(Acao.Nenhum);
                            break;

                        // Cria uma conexão entre duas entidades
                        case Acao.GerenciarConexao:
                            Armadilha aAux = mundo.GerenciadorEnt.SelecionarArmadilha(posMouseMundo.X, posMouseMundo.Y);
                            Mecanismo mAux = mundo.GerenciadorEnt.SelecionarMecanismo(posMouseMundo.X, posMouseMundo.Y);
                            Escada eAux = mundo.GerenciadorEnt.SelecionarEscada(posMouseMundo.X, posMouseMundo.Y);

                            if(eAux == null)
                            {
                                mundo.EscadaAux1 = null;
                                mundo.EscadaAux2 = null;
                                mundo.GerenciadorEnt.ApagarQuadradoEscadas();

                                if (aAux != null && mAux == null)
                                {
                                    mundo.ArmadilhaAux = aAux;
                                }

                                else if (aAux == null && mAux != null)
                                {
                                    mundo.GerenciadorEnt.ApagarQuadradoMecanismos();
                                    mundo.GerenciadorEnt.ApagarQuadradoArmadilhas();
                                    mundo.MecanismoAux = mAux;
                                    mundo.MecanismoAux.Selecionado = true;
                                    mundo.MecanismoAux.ApagarQuadradoArmadilhas();
                                    mundo.MecanismoAux.DesenharQuadradoArmadilhas();
                                }

                                else
                                {
                                    mundo.ArmadilhaAux = null;
                                    mundo.MecanismoAux = null;
                                    mundo.GerenciadorEnt.ApagarQuadradoArmadilhas();
                                    mundo.GerenciadorEnt.ApagarQuadradoMecanismos();
                                }

                                if (mundo.ArmadilhaAux != null && mundo.MecanismoAux != null)
                                {
                                    mundo.MecanismoAux.IncluirArmadilha(mundo.ArmadilhaAux);
                                    mundo.MecanismoAux.DesenharQuadradoArmadilhas();
                                }
                            }
                            else
                            {
                                if (mundo.EscadaAux1 == null)
                                {
                                    mundo.EscadaAux1 = eAux;

                                    mundo.EscadaAux1.Selecionado = true;

                                    if (mundo.EscadaAux1.EscadaCon != null)
                                    {
                                        mundo.EscadaAux2 = mundo.EscadaAux1.EscadaCon;
                                        mundo.EscadaAux2.Selecionado = true;
                                    }
                                   
                                }
                                else if(mundo.EscadaAux1 != null && mundo.EscadaAux2 == null)
                                {
                                    mundo.EscadaAux2 = eAux;
                                    mundo.EscadaAux1.EscadaCon = mundo.EscadaAux2;
                                    mundo.EscadaAux2.EscadaCon = mundo.EscadaAux1;
                                    mundo.EscadaAux2.Selecionado = true;
                                }
                            }
                            
                            break;

                        // Carrega um novo diálogo e o relaciona com um NPC
                        case Acao.GerenciarDialogos:
                            if (mundo.GerenciadorEnt.HaNPCAqui(posMouseMundo.X, posMouseMundo.Y))
                                gerenciadorArquivos.CarregarDialogo(posMouseMundo);
                            break;

                        case Acao.GerenciarMusicas:

                            // Cria a área ocupada por uma música ou carrega o título da músic aa partir de um arquivo
                            if (mundo.MusicaAux == null)
                            {
                                Musica auxM = mundo.HaMusicaAqui(new Vector2f(posMouseMundo.X, posMouseMundo.Y));
                                if (auxM == null)
                                {
                                    mundo.MusicaAux = new Musica()
                                    {
                                        C1 = new Vector2f(posMouseMundo.X, posMouseMundo.Y),
                                        C1Preenchido = true
                                    };
                                }
                                else
                                    gerenciadorArquivos.CarregarMusica(auxM);
                            }
                            else
                            {
                                if (!mundo.MusicaAux.C2Preenchido)
                                {
                                    mundo.MusicaAux.C2 = new Vector2f(posMouseMundo.X, posMouseMundo.Y);
                                    mundo.MusicaAux.C2Preenchido = true;
                                    mundo.MusicaAux.CriarRetangulo();
                                    mundo.InserirMusica();
                                    mundo.MusicaAux = null;
                                }
                            }
                            break;
                    }
                }
            }

            // Caso o botão direito do mouse seja pressioanado
            else if (e.Button == Mouse.Button.Right && !bloqueandoAcoes)
            {
                // Caso seja pressionado no mundo
                if (Mouse.GetPosition(window).X <= window.Size.X * Informacoes.propViewMundo)
                {
                    switch (acao)
                    {
                        // Exclui a coneão entre duas entidades
                        case Acao.GerenciarConexao:
                            if(mundo.MecanismoAux != null)
                            {
                                Armadilha armadilha = mundo.GerenciadorEnt.SelecionarArmadilha(posMouseMundo.X, posMouseMundo.Y);

                                if (armadilha != null)
                                {
                                    armadilha.Selecionado = false;
                                    mundo.MecanismoAux.ExcluirArmadilha(armadilha);
                                }
                            }
                            Escada escada = mundo.GerenciadorEnt.SelecionarEscada(posMouseMundo.X, posMouseMundo.Y);
                            if (escada != null)
                            {
                                escada.Selecionado = false;
                                escada.EscadaCon = null;
                                mundo.GerenciadorEnt.ExcluirConecaoEscada(escada);
                            }
                            break;

                        // Exclui uma música
                        case Acao.GerenciarMusicas:
                            Musica auxM = mundo.HaMusicaAqui(new Vector2f(posMouseMundo.X, posMouseMundo.Y));
                            if (auxM != null)
                                mundo.ExcluirMusica(auxM);

                            break;
                    }
                }
            }
        }

        private void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            /* Método chamado quando a roda do mouse é ativada */

            // Caso seja ativado no menu move o menu para baixo e para cima
            if(Mouse.GetPosition(window).X >= window.Size.X * Informacoes.propViewMundo)
            {
                if (e.Delta == 1 && coordViewMenu.Y >= yViewMenuInicial)
                {
                    coordViewMenu.Y -= velViewMenu * deltatime;
                    viewMenu.Center = coordViewMenu;
                    menu.AtualizarPosicaoBackground(-velViewMenu * deltatime);
                }
                else if (e.Delta == -1)
                {
                    coordViewMenu.Y += velViewMenu * deltatime;
                    viewMenu.Center = coordViewMenu;
                    menu.AtualizarPosicaoBackground(velViewMenu * deltatime);
                }
            }

            // Caso seja ativado no mundo aplica mais ou menos zoom no mundo
            else
            {
                if (e.Delta == 1)
                {
                    viewMundo.Zoom(0.9f);
                    zoom++;
                }
                    
                else if (e.Delta == -1)
                {
                    viewMundo.Zoom(1.1f);
                    zoom--;
                }
            }
        }

        private void AplicarZoomMundo()
        {
            /* Aplica mais ou menos zoom no mundo dependendo do fator 'zoom' */

            if (zoom > 0)
            {
                for (int i = 0; i < zoom; i++)
                    viewMundo.Zoom(0.9f);
            }
            else if (zoom < 0)
            {
                for (int i = 0; i < -zoom; i++)
                    viewMundo.Zoom(1.1f);
            }
        }
    }
}
