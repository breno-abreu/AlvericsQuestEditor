using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Mundo
    {
        // Referência para a jenela do programa
        private RenderWindow window;

        // Referência para a view que contém o menu
        private View view;

        // Referência para um gerenciador de entidades
        private GerenciadorEntidades gerenciadorEntidades;

        // Velocidade de movimentacao do mundo
        private const float velocidade = 50f;

        public Mundo(RenderWindow window, View view)
        {
            this.window = window;
            this.view = view;
            gerenciadorEntidades = new GerenciadorEntidades(window, view);

            gerenciadorEntidades.InserirEntidade(10, 10, 0, 1);
            gerenciadorEntidades.InserirEntidade(100, 10, 0, 1);
            gerenciadorEntidades.InserirEntidade(50, 50, 0, 1);
            gerenciadorEntidades.InserirEntidade(-50, -50, 0, 1);
        }

        public void Desenhar()
        {
            gerenciadorEntidades.AtualizarEntidades();
        }

        public void Mover(float deltatime, int zoom)
        {
            int z = zoom > 10 ? 2 * zoom : 10;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                Vector2f aux = new Vector2f(view.Center.X, view.Center.Y - velocidade * z * deltatime);
                view.Center = aux;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                Vector2f aux = new Vector2f(view.Center.X - velocidade *  z * deltatime, view.Center.Y);
                view.Center = aux;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                Vector2f aux = new Vector2f(view.Center.X, view.Center.Y + velocidade * z * deltatime);
                view.Center = aux;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                Vector2f aux = new Vector2f(view.Center.X + velocidade * z * deltatime, view.Center.Y);
                view.Center = aux;
            }
        }

    }
}
