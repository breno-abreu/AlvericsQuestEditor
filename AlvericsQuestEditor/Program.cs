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
        public static Vector2i qtdEntidades = new Vector2i(5, 1);
        public static string entidadesImgPath = @"all.png";
        public static float propViewMenu = .3f;
        public static float propViewMundo = .7f;
    }

    class Program
    {
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
        private Mundo mundo;
        private int zoom;
        private Acao acao;
        private bool bloqueandoAcoes;

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
            bloqueandoAcoes = false;

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
            Vector2f vec = window.MapPixelToCoords(Mouse.GetPosition(window), viewMundo);

            if (acao == Acao.AdicionarObjeto &&
                Mouse.GetPosition(window).X <= window.Size.X * Informacoes.propViewMundo &&
                Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                mundo.GerenciadorEnt.InserirEntidade(vec.X, vec.Y);
                //Console.WriteLine(mundo.GerenciadorEnt.QuantidadeTotalEntidades());
            }

            else if (acao == Acao.ExcluirObjeto &&
                Mouse.GetPosition(window).X <= window.Size.X * Informacoes.propViewMundo &&
                Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                mundo.GerenciadorEnt.ExcluirEntidade(vec.X, vec.Y);
            }


            else if (acao == Acao.NovoMundo)
            {
                acao = Acao.Nenhum;
                bloqueandoAcoes = true;
                Thread t1 = new Thread(MostrarAvisoNovoMundo);
                t1.Start();
            }
        }

        public void MostrarAvisoNovoMundo()
        {
            DialogResult dialogResult = MessageBox.Show("Deseja criar um novo mundo?\nTodo o progresso não salvo será perdido!",
                                                            "Aviso", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                mundo.NovoMundo();

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
                if(Mouse.GetPosition(window).X >= window.Size.X * Informacoes.propViewMundo)
                {
                    Acao botaoPressionado = menu.BotaoPressionado(Mouse.GetPosition(window));

                    switch (botaoPressionado)
                    {
                        case Acao.IndicarEntidade:
                            mundo.GerenciadorEnt.PosicaoEntidade = menu.PosicaoEntidade;
                            break;

                        default:
                            acao = botaoPressionado;
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
                if (e.Delta > 0)
                {
                    coordViewMenu.Y -= velViewMenu * deltatime;
                    viewMenu.Center = coordViewMenu;
                    menu.AtualizarPosicaoBackground(-velViewMenu * deltatime);
                }
                else if (e.Delta < 0)
                {
                    coordViewMenu.Y += velViewMenu * deltatime;
                    viewMenu.Center = coordViewMenu;
                    menu.AtualizarPosicaoBackground(velViewMenu * deltatime);
                }
            }

            // Caso seja ativado no mundo
            else
            {
                if (e.Delta > 0)
                {
                    viewMundo.Zoom(0.9f);
                    zoom++;
                }
                    
                else if (e.Delta < 0)
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
