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

        // Referencia para a música que terá seus atributos modificados
        public Musica MusicaAux { get; set; }

        // Referência para uma escada que servirá de auxílio a se fazer uma conexão entre escadas
        public Escada EscadaAux1 { get; set; }

        // Referência para uma escada que servirá de auxílio a se fazer uma conexão entre escadas
        public Escada EscadaAux2 { get; set; }

        // Lista de músicas
        private List<Musica> musicas;

        public Mundo(RenderWindow window, View view)
        {
            /* Inicializa um Mundo */

            this.window = window;
            this.view = view;
            GerenciadorEnt = new GerenciadorEntidades(window, view);
            auxVec = new Vector2f();
            MusicaAux = null;
            musicas = new List<Musica>();
            EscadaAux1 = null;
            EscadaAux2 = null;
        }

        public void Desenhar()
        {
            /* Desenha na tela todas as entidades pertencentes à um mundo */

            GerenciadorEnt.AtualizarEntidades();
            
            foreach(Musica musica in musicas)
                musica.Desenhar(window);
        }

        public void InserirMusica()
        {
            /* Insere uma nova múisica à lista de músicas */

            musicas.Add(MusicaAux);
        }

        public void Mover(float deltatime, int zoom)
        {
            /* Movimenta a View do mundo nos sentidos: norte, sul, leste e oeste */

            // Variável auxiliar que aumenta a velocidade de movimentação da View quando menor for o zoom aplicado
            int z = zoom > 10 ? 2 * zoom : 10;
            float vel = velocidade * z * deltatime;

            // Movimenta a View de acordo com o botão pressionado
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
            /* Cria um novo mundo limpando todas as listas e resetando as variáveis auxiliares */

            GerenciadorEnt.LimparListas();
            musicas.Clear();
            MecanismoAux = null;
            ArmadilhaAux = null;
            MusicaAux = null;
        }

        public Musica HaMusicaAqui(Vector2f vec)
        {
            /* Verifica se há uma múisca nas coordenadas recebidas */

            // Percorre a lista de músicas e retorna a música que está na posição recebida.
            foreach(Musica musica in musicas)
            {
                if ((vec.X >= musica.C1.X && vec.X <= musica.C2.X &&
                     vec.Y >= musica.C1.Y && vec.Y <= musica.C2.Y) ||
                    (vec.X <= musica.C1.X && vec.X >= musica.C2.X &&
                     vec.Y <= musica.C1.Y && vec.Y >= musica.C2.Y))
                    return musica;
            }

            // Retorna 'null' caso não haja nenhuma música naquela posição
            return null;
        }

        public void ExcluirMusica(Musica m)
        {
            /* Retira uma música da lista de músicas */

            musicas.Remove(m);
        }

    }
}
