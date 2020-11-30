using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Escada : Entidade
    {
        /* Classe para uma escada. Será conectada com outra escada permitindo que o jogador teleporte entre escadas */

        // Referência para a escada conectada à essa
        public Escada EscadaCon { get; set; }

        // Valor que indica se a escada já posicionada no mundo foi selecionada pelo jogador na opção de criar conexão
        // Se o valor for 'true' o retângulo 'rect' é desenhado na tela, indicando qual escada foi selecionada pelo jogador
        public bool Selecionado { get; set; }

        // Retângulo que será desenhado na tela quando a escada for selecionada pelo jogador ao usar a opção de criar uma conexão
        private RectangleShape rect;

        public Escada(Sprite sprite, TipoEntidade tipo, int posicaoSpriteX, int posicaoSpriteY) :
            base(sprite, tipo, posicaoSpriteX, posicaoSpriteY)
        {
            /* Inicializa EscadaCon com null, indicando que não há uma conexão e cria o retângulo 'rect' */

            EscadaCon = null;
            rect = new RectangleShape(new Vector2f(16, 16));
            rect.Position = ESprite.Position;
            rect.OutlineThickness = 1;
            rect.OutlineColor = Color.Green;
            rect.FillColor = Color.Transparent;
            rect.Origin = new Vector2f(8, 8);
        }
        public override void Desenhar(RenderWindow window)
        {
            /* Desenha a escada na tela */

            base.Desenhar(window);

            if (Selecionado)
                window.Draw(rect);
        }
    }
}
