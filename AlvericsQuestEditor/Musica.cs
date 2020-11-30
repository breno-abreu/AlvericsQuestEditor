using SFML.System;
using SFML.Graphics;

namespace AlvericsQuestEditor
{
    class Musica
    {
        /* Classe de uma música. Contém o título da música e a região que será executada */

        // Título da música, referênte ao arquivo carregado
        public string Titulo { get; set; }

        // Primeira coordenada da região em que a música será executada
        public Vector2f C1 { get; set; }

        // Segunda coordenada da região em que a música será executada
        public Vector2f C2 { get; set; }

        // Indica se a primeira coordenada foi preenchida
        public bool C1Preenchido { get; set; }

        // Indica se a Segunda coordenada foi preenchida
        public bool C2Preenchido { get; set; }

        // Retângulo que representa a área relativa a uma música
        private RectangleShape rect;

        public Musica()
        {
            /* Inicializa variáveis */

            Titulo = "";
            C1 = new Vector2f();
            C2 = new Vector2f();
            C1Preenchido = false;
            C2Preenchido = false;
            rect = new RectangleShape();
        }

        public void Desenhar(RenderWindow window)
        {
            /* Desenha o retângulo 'rect' na tela */

            window.Draw(rect);
        }

        public void CriarRetangulo()
        {
            /* Cria um retângulo a partir da primeira e segunda coordenadas */

            rect = new RectangleShape(new Vector2f(C2.X - C1.X, C2.Y - C1.Y));
            rect.Position = C1;
            rect.FillColor = new Color(0, 100, 200, 20);
        }
    }
}
