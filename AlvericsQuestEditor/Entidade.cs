using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace AlvericsQuestEditor
{
    class Entidade
    {
        public Sprite ESprite { get; private set; }
        public bool Excluir { get; set; }
        public TipoEntidade Tipo { get; private set; }


        public Entidade(Sprite sprite, TipoEntidade tipo)
        {
            ESprite = sprite;
            Excluir = false;
            Tipo = tipo;
        }

        public void Desenhar(RenderWindow window)
        {
            window.Draw(ESprite);
        }
    }
}
