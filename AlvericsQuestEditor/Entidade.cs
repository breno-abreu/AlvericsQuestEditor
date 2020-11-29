using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Entidade
    {
        public Sprite ESprite { get; private set; }
        public bool Excluir { get; set; }
        public TipoEntidade Tipo { get; private set; }
        public Vector2i posicaoSprite { get; private set; }

        public Entidade(Sprite sprite, TipoEntidade tipo, int posicaoSpriteX, int posicaoSpriteY)
        {
            ESprite = sprite;
            Excluir = false;
            Tipo = tipo;
            posicaoSprite = new Vector2i(posicaoSpriteX, posicaoSpriteY);
        }

        public virtual void Desenhar(RenderWindow window)
        {
            window.Draw(ESprite);
        }
    }
}
