using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Armadilha : Entidade
    {
        public float TempoInicial { get; set; }
        public float TempoEntreAtivacoes { get; set; }
        public float TempoAtivo { get; set; }
        public TipoArmadilha TipoA { get; private set; }
        public bool Selecionado { get; set; }

        private RectangleShape rect;

        public Armadilha(Sprite sprite, TipoEntidade tipo, TipoArmadilha tipoArmadilha, int posicaoSpriteX, int posicaoSpriteY) :
            base(sprite, tipo, posicaoSpriteX, posicaoSpriteY)
        {
            TempoInicial = 0;
            TempoEntreAtivacoes = 0;

            if (tipoArmadilha == TipoArmadilha.Espinhos)
                TempoAtivo = 0;
            else
                TempoAtivo = -1;

            TipoA = tipoArmadilha;

            Selecionado = false;

            rect = new RectangleShape(new Vector2f(16, 16));
            rect.Position = ESprite.Position;
            rect.OutlineThickness = 1;
            rect.OutlineColor = Color.Cyan;
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
