using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Escada : Entidade
    {
        public Escada EscadaConn { get; set; }
        public bool Selecionado { get; set; }
        private RectangleShape rect;

        public Escada(Sprite sprite, TipoEntidade tipo):
            base(sprite, tipo)
        {
            EscadaConn = null;

            rect = new RectangleShape(new Vector2f(16, 16));
            rect.Position = ESprite.Position;
            rect.OutlineThickness = 1;
            rect.OutlineColor = Color.Green;
            rect.FillColor = Color.Transparent;
            rect.Origin = new Vector2f(8, 8);
        }
        public override void Desenhar(RenderWindow window)
        {
            base.Desenhar(window);

            if (Selecionado)
                window.Draw(rect);
        }
    }
}
