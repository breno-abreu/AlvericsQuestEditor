using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Mecanismo : Entidade
    {
        public List<Armadilha> armadilhas { get; set; }
        public bool Selecionado { get; set; }

        private RectangleShape rect;

        public Mecanismo(Sprite sprite, TipoEntidade tipo, int posicaoSpriteX, int posicaoSpriteY) :
            base(sprite, tipo, posicaoSpriteX, posicaoSpriteY)
        {
            armadilhas = new List<Armadilha>();
            Selecionado = false;

            rect = new RectangleShape(new Vector2f(16, 16));
            rect.Position = ESprite.Position;
            rect.OutlineThickness = 1;
            rect.OutlineColor = Color.Red;
            rect.FillColor = Color.Transparent;
            rect.Origin = new Vector2f(8, 8);
        }

        public void IncluirArmadilha(Armadilha a)
        {
            bool aux = false;
            foreach (Armadilha armadilha in armadilhas)
            {
                if (armadilha.ESprite.Position.X == a.ESprite.Position.X &&
                   armadilha.ESprite.Position.Y == a.ESprite.Position.Y)
                    aux = true;
            }

            if(!aux) armadilhas.Add(a);
        }

        public void ExcluirArmadilha(Armadilha a)
        {
            armadilhas.Remove(a);
        }

        public void Imprimir()
        {
            Console.WriteLine(armadilhas.Count);
        }

        public override void Desenhar(RenderWindow window)
        {
            base.Desenhar(window);

            if (Selecionado)
                window.Draw(rect);
        }

        public void ApagarQuadradoArmadilhas()
        {
            foreach (Armadilha armadilha in armadilhas)
                armadilha.Selecionado = false;
        }

        public void DesenharQuadradoArmadilhas()
        {
            foreach (Armadilha armadilha in armadilhas)
                armadilha.Selecionado = true;
        }
    }
}
