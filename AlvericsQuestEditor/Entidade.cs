using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Entidade
    {
        /* Classe para uma entidade. Todos os objetos pertencentes à um mundo e que são desenhados na tela pertencem à essa classe */

        // Sprite de uma entidade. Contém sua forma geométrica, localização e textura
        public Sprite ESprite { get; private set; }

        // Indica se uma entidade pode ser excluída ou não
        public bool Excluir { get; set; }

        // Indica se a entidade é tangível ou intangível
        public TipoEntidade Tipo { get; private set; }

        // Indica a posição de um subsprite que representa a entidade dentro do sprite que contém todos as imagens das entidades
        public Vector2i posicaoSprite { get; private set; }

        public Entidade(Sprite sprite, TipoEntidade tipo, int posicaoSpriteX, int posicaoSpriteY)
        {
            /* Inicializa seus atributos */

            ESprite = sprite;
            Excluir = false;
            Tipo = tipo;
            posicaoSprite = new Vector2i(posicaoSpriteX, posicaoSpriteY);
        }

        public virtual void Desenhar(RenderWindow window)
        {
            /* Desenha a entidade na tela */

            window.Draw(ESprite);
        }
    }
}
