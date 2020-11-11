using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AlvericsQuestEditor
{
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
        private View viewMundo;
        private View viewMenu;
        private Menu menu;
        private const uint comprimentoInicialTela = 1600;
        private const uint alturaInicialTela = 900;
        public Editor()
        {
            // Inicializa a janela do programa
            window = new RenderWindow(new VideoMode(comprimentoInicialTela, alturaInicialTela), 
                                      "Alveric's Quest Editor", Styles.Default);

            // Cria uma view para o mundo
            viewMundo = new View();
            // Seleciona a fração da tela ocupada por essa view
            viewMundo.Viewport = new FloatRect(0f, 0f, 0.7f, 1f);
            viewMundo.Size = new Vector2f(window.Size.X * 0.7f, window.Size.Y);

            // Cria uma view para o menu
            viewMenu = new View();
            // Seleciona a fração da tela ocupada por essa view
            viewMenu.Viewport = new FloatRect(0.7f, 0f, 0.3f, 1f);
            viewMenu.Size = new Vector2f(window.Size.X * 0.3f, window.Size.Y);

            // Inicializa o menu
            menu = new Menu((Vector2f)window.Size, viewMenu);

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

            /* Inclui um método para os event handlers: */
            // Método chamado quando ó botão de finalizar o programa é pressionado
            window.Closed += Window_Close;

            // Método chamado quando a tela é redimensionada
            window.Resized += Window_Resized;
        }

        public void Executar()
        {
            while (window.IsOpen)
            {
                // Trata os eventos da janela
                window.DispatchEvents();

                // Muda para a viewMenu e desenha o menu na tela
                window.SetView(viewMenu);
                menu.Desenhar(window);

                // Executa a janela
                window.Display();

                // Limpa a janela
                window.Clear(Color.White);
            }
        }

        /* Método chamado quando a tela é redimensionada */
        private void Window_Resized(object sender, EventArgs e)
        {
            // Redimensionar menu
            viewMenu.Size = new Vector2f(window.Size.X * 0.3f, window.Size.Y);
            menu.RedimensionarMenu((Vector2f)window.Size, viewMenu);
        }

        /* Método chamado quando o botão 'fechar janela' é pressionado */
        private void Window_Close(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
    }
}
