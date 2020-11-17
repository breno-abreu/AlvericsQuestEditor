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


        public Entidade(Sprite sprite)
        {
            ESprite = sprite;
            Excluir = false;
        }

        public void Desenhar(RenderWindow window)
        {
            window.Draw(ESprite);
        }
    }
}
