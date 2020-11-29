using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Threading;

namespace AlvericsQuestEditor
{
    public static class Informacoes
    {
        public static Vector2i qtdEntidades = new Vector2i(5, 37);
        public static string entidadesImgPath = @"all.png";
        public static float propViewMenu = .3f;
        public static float propViewMundo = .7f;
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Editor editor = new Editor();
            editor.Executar();
        }
    }

    class Editor
    {
        private RenderWindow window;
        private SFML.Graphics.View viewMundo;
        private SFML.Graphics.View viewMenu;
        private Menu menu;
        private const uint comprimentoInicialTela = 1600;
        private const uint alturaInicialTela = 900;
        private float deltatime;
        private Clock clock;
        private const float velViewMenu = 50000f;
        private Vector2f coordViewMenu;
        private float yViewMenuInicial;
        private Mundo mundo;
        private int zoom;
        private Acao acao;
        private bool bloqueandoAcoes;
        private Vector2f posMouseMundo;
        private bool novoMundo;
        private GerenciadorArquivos gerenciadorArquivos;

        public Editor()
        {
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

            mundo = new Mundo(window, viewMundo);

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
                

                window.SetView(viewMundo);
                mundo.Desenhar();
                mundo.Mover(deltatime, -zoom);

                /*if(acao == Acao.GerenciarConexao)
                {
                    
                }*/

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
            posMouseMundo = window.MapPixelToCoords(Mouse.GetPosition(window), viewMundo);

            if (acao == Acao.AdicionarObjeto &&
                Mouse.GetPosition(window).X <= window.Size.X * Informacoes.propViewMundo &&
                Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                mundo.GerenciadorEnt.InserirEntidade(posMouseMundo.X, posMouseMundo.Y);
                //Console.WriteLine(mundo.GerenciadorEnt.QuantidadeTotalEntidades());
            }

            else if (acao == Acao.ExcluirObjeto &&
                Mouse.GetPosition(window).X <= window.Size.X * Informacoes.propViewMundo &&
                Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                mundo.GerenciadorEnt.ExcluirEntidade(posMouseMundo.X, posMouseMundo.Y);
            }

            else if (acao == Acao.NovoMundo)
            {
                acao = Acao.Nenhum;
                bloqueandoAcoes = true;
                Thread t1 = new Thread(MostrarAvisoNovoMundo);
                t1.Start();
            }

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
            DialogResult dialogResult = MessageBox.Show("Deseja criar um novo mundo?\nTodo o progresso não salvo será perdido!",
                                                            "Aviso", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                novoMundo = true;

            menu.ResetarCorBotoes();
            bloqueandoAcoes = false;
        }

        /* Método chamado quando a tela é redimensionada */
        private void Window_Resized(object sender, EventArgs e)
        {
            // Redimensionar menu
            viewMenu.Size = new Vector2f(window.Size.X * Informacoes.propViewMenu, window.Size.Y);
            menu.Redimensionar();

            viewMundo.Size = new Vector2f(window.Size.X * Informacoes.propViewMundo, window.Size.Y);
            AplicarZoomMundo();
        }

        /* Método chamado quando o botão 'fechar janela' é pressionado */
        private void Window_Close(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        /* Eventos quando os botões do mouse são pressionados */
        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left && !bloqueandoAcoes)
            {
                if (Mouse.GetPosition(window).X >= window.Size.X * Informacoes.propViewMundo)
                {
                    Acao botaoPressionado = menu.BotaoPressionado(Mouse.GetPosition(window));

                    switch (botaoPressionado)
                    {
                        case Acao.IndicarEntidade:
                            mundo.GerenciadorEnt.PosicaoEntidade = menu.PosicaoEntidade;
                            break;

                        default:
                            acao = botaoPressionado;
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
                            if(acao == Acao.GerenciarEventos)
                                gerenciadorArquivos.CarregarEventos();

                            if (acao == Acao.Salvar)
                                gerenciadorArquivos.SalvarMundo(mundo.GerenciadorEnt.entidadesTangiveis,
                                                                mundo.GerenciadorEnt.entidadesIntangiveis,
                                                                mundo.GerenciadorEnt.armadilhas,
                                                                mundo.GerenciadorEnt.mecanismos,
                                                                mundo.GerenciadorEnt.escadas);
                            if (acao == Acao.Carregar)
                                gerenciadorArquivos.CarregarMundo(mundo.GerenciadorEnt);
                                
                            break;
                    }
                }
                else
                {
                    switch (acao)
                    {
                        case Acao.GerenciarPropriedades:
                            menu.ArmadilhaAux = mundo.GerenciadorEnt.SelecionarArmadilha(posMouseMundo.X, posMouseMundo.Y);
                            mundo.GerenciadorEnt.ApagarQuadradoArmadilhas();

                            if (menu.ArmadilhaAux != null)
                                menu.ArmadilhaAux.Selecionado = true;

                            menu.AtualizarValores(Acao.Nenhum);
                            break;

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

                                    if (mundo.EscadaAux1.EscadaConn != null)
                                    {
                                        mundo.EscadaAux2 = mundo.EscadaAux1.EscadaConn;
                                        mundo.EscadaAux2.Selecionado = true;
                                    }
                                   
                                }
                                else if(mundo.EscadaAux1 != null && mundo.EscadaAux2 == null)
                                {
                                    mundo.EscadaAux2 = eAux;
                                    mundo.EscadaAux1.EscadaConn = mundo.EscadaAux2;
                                    mundo.EscadaAux2.EscadaConn = mundo.EscadaAux1;
                                    mundo.EscadaAux2.Selecionado = true;
                                }
                            }
                            
                            break;

                        case Acao.GerenciarDialogos:
                            if (mundo.GerenciadorEnt.HaNPCAqui(posMouseMundo.X, posMouseMundo.Y))
                                gerenciadorArquivos.CarregarDialogo(posMouseMundo);
                            break;

                        case Acao.GerenciarMusicas:
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
            else if (e.Button == Mouse.Button.Right && !bloqueandoAcoes)
            {
                if (Mouse.GetPosition(window).X <= window.Size.X * Informacoes.propViewMundo)
                {
                    switch (acao)
                    {
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
                                escada.EscadaConn = null;
                                mundo.GerenciadorEnt.ExcluirConecaoEscada(escada);
                            }
                            break;


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
            // Caso seja ativado no menu
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

            // Caso seja ativado no mundo
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
