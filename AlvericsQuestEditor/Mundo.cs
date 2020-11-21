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
        public GerenciadorEntidades GerenciadorEnt { get; private set; }

        // Velocidade de movimentacao do mundo
        private const float velocidade = 50f;

        // Vetor auxiliar para operações de movimentação
        private Vector2f auxVec;

        // Referência para um mecanismo,usada para determinar uma conexão com uma armadilha
        public Mecanismo MecanismoAux { get; set; }

        // Referencia para uma armadilha, usada para determinar uma conexão com um mecanismo
        public Armadilha ArmadilhaAux { get; set; }

        public Mundo(RenderWindow window, View view)
        {
            this.window = window;
            this.view = view;
            GerenciadorEnt = new GerenciadorEntidades(window, view);

            auxVec = new Vector2f();
        }

        public void Desenhar()
        {
            GerenciadorEnt.AtualizarEntidades();
        }

        public void Mover(float deltatime, int zoom)
        {
            int z = zoom > 10 ? 2 * zoom : 10;
            float vel = velocidade * z * deltatime;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                auxVec.Y = view.Center.Y - vel;

            else if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                auxVec.X = view.Center.X - vel;

            else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                auxVec.Y = view.Center.Y + vel;

            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                auxVec.X = view.Center.X + vel;

            view.Center = auxVec;
        }

        public void NovoMundo()
        {
            GerenciadorEnt.LimparListas();
        }

    }
}
