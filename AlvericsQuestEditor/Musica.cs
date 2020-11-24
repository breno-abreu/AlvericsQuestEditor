using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;

namespace AlvericsQuestEditor
{
    class Musica
    {
        public string Titulo { get; set; }
        public Vector2f C1 { get; set; }
        public Vector2f C2 { get; set; }
        public bool C1Preenchido { get; set; }
        public bool C2Preenchido { get; set; }

        private RectangleShape rect;

        public Musica()
        {
            Titulo = "";
            C1 = new Vector2f();
            C2 = new Vector2f();
            C1Preenchido = false;
            C2Preenchido = false;
            rect = new RectangleShape();
        }

        public void Desenhar(RenderWindow window)
        {
            window.Draw(rect);
        }

        public void CriarRetangulo()
        {
            rect = new RectangleShape(new Vector2f(C2.X - C1.X, C2.Y - C1.Y));
            rect.Position = C1;
            rect.FillColor = new Color(0, 100, 200, 20);
        }
    }
}
