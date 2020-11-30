using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Mecanismo : Entidade
    {
        /* Classe para um mecanismo, que contém uma lista de armadilhas que ele ativará */

        // Lista de armadilhas que serão ativadas por esse mecanismo
        public List<Armadilha> armadilhas { get; set; }

        // Valor que indica se a escada já posicionada no mundo foi selecionada pelo jogador na opção de criar conexão
        // Se o valor for 'true' o retângulo 'rect' é desenhado na tela, indicando qual escada foi selecionada pelo jogador
        public bool Selecionado { get; set; }

        // Retângulo que será desenhado na tela quando a escada for selecionada pelo jogador ao usar a opção de criar uma conexão
        private RectangleShape rect;

        public Mecanismo(Sprite sprite, TipoEntidade tipo, int posicaoSpriteX, int posicaoSpriteY) :
            base(sprite, tipo, posicaoSpriteX, posicaoSpriteY)
        {
            /* Inicializa variáveis e cria o retângulo que irá ser desenhado quando o mecanismo for selecionado */

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
            /* Inclui uma referência a uma armadilha na lista de armadilhas */

            bool aux = false;

            // Percorre a lista de armadilhas e indica se já existe uma armadilha naquelas coordenadas na lista
            foreach (Armadilha armadilha in armadilhas)
            {
                if (armadilha.ESprite.Position.X == a.ESprite.Position.X &&
                   armadilha.ESprite.Position.Y == a.ESprite.Position.Y)
                    aux = true;
            }

            // Se não houver uma armadilha nas coordenadas da armadilha recebida já incluída na lista, adiciona a rmadilha na lista
            if(!aux) armadilhas.Add(a);
        }

        public void ExcluirArmadilha(Armadilha a)
        {
            /* Remove uma armadilha da lista */

            armadilhas.Remove(a);
        }

        public override void Desenhar(RenderWindow window)
        {
            /* Desenha a armadilha e seu retângulo 'rect' */

            base.Desenhar(window);

            if (Selecionado)
                window.Draw(rect);
        }

        public void ApagarQuadradoArmadilhas()
        {
            /* Apaga os qudrados que envolvem as armadilhas da lista de armadilhas */

            foreach (Armadilha armadilha in armadilhas)
                armadilha.Selecionado = false;
        }

        public void DesenharQuadradoArmadilhas()
        {
            /* Desenha os qudrados que envolvem as armadilhas da lista de armadilhas */

            foreach (Armadilha armadilha in armadilhas)
                armadilha.Selecionado = true;
        }
    }
}
