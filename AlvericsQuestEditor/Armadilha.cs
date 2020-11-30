using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Armadilha : Entidade
    {
        /* Classe para uma armadilha. Subclasse de Entidade 
         * Possui informações sobre seu funcionamento relacionando tempo e ativação 
         * Pode ser uma armadilha atiradora ou o chão com espinhos */

        // Valor que contém o tempo entre o início do jogo e a primeira ativação
        public float TempoInicial { get; set; }

        // Valor que contém o tempo entre duas ativações
        public float TempoEntreAtivacoes { get; set; }

        // Valor que contém o tempo em que o chão com espinhos ficará ativado
        public float TempoAtivo { get; set; }

        // Valor que define se uma armadilha é um atirador ou um chão com espinhos
        public TipoArmadilha TipoA { get; private set; }

        // Valor que indica se a armadilha já posicionada no mundo foi selecionada pelo jogador nas opções de criar conexão e gerenciar propriedades
        // Se o valor for 'true' o retângulo 'rect' é desenhado na tela, indicando qual armadilha foi selecionada pelo jogador
        public bool Selecionado { get; set; }

        // Retângulo que será desenhado na tela quando a armadilha for selecionada pelo jogador ao usar a opção de 
        // criar uma conexão ou gerenciar propriedades.
        private RectangleShape rect;

        public Armadilha(Sprite sprite, TipoEntidade tipo, TipoArmadilha tipoArmadilha, int posicaoSpriteX, int posicaoSpriteY) :
            base(sprite, tipo, posicaoSpriteX, posicaoSpriteY)
        {
            /* Inicializa suas propriedades e cria o retângulo 'rect' */

            // Inicializa suas propriedades
            TempoInicial = 0;
            TempoEntreAtivacoes = 0;
            
            // Caso a armadilha seja um atirador, seu TempoAtivo recebe -1 para indicar que essa propriedade não está disponível
            if (tipoArmadilha == TipoArmadilha.Espinhos)
                TempoAtivo = 0;
            else
                TempoAtivo = -1;

            TipoA = tipoArmadilha;
            Selecionado = false;

            // Cria o retângulo que envolve a armadilha
            rect = new RectangleShape(new Vector2f(16, 16));
            rect.Position = ESprite.Position;
            rect.OutlineThickness = 1;
            rect.OutlineColor = Color.Cyan;
            rect.FillColor = Color.Transparent;
            rect.Origin = new Vector2f(8, 8);
        }

        public override void Desenhar(RenderWindow window)
        {
            /* Desenha a armadilha na tela */

            base.Desenhar(window);

            if (Selecionado)
                window.Draw(rect);
        }
    }
}
