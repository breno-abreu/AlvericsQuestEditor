using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AlvericsQuestEditor
{
    /* Classe do menu do editor. Contém os botões que irão engatilhar eventos. */
    class Menu
    {
        // Retângulo para o background do menu
        private RectangleShape background;

        // Cor do background
        private Color backgroundColor;

        // Botão para criar novo mundo
        private Botao bNovoMundo;

        // Botão para salvar mundo
        private Botao bSalvar;

        // Botão para carregar mundo
        private Botao bCarregar;

        // Botão para adicionar objeto
        private Botao bAdicionarObjeto;

        // Botão para excluir objeto
        private Botao bExcluirObjeto;

        // Botão para gerenciar a conexão entre objetos
        private Botao bGerenciarConexao;

        // Botão para aumentar o valor do tempo de início de ativação de uma armadilha
        private Botao bInicioAtivacaoSoma;

        // Botão para reduzir o valor do tempo de início de ativação de uma armadilha
        private Botao bInicioAtivacaoSub;

        // Botão para aumentar o valor do tempo em que a armadilha ficará ativada
        private Botao bTempoAtivadoSoma;

        // Botão para reduzir o valor do tempo em que a armadilha ficará ativada
        private Botao bTempoAtivadoSub;

        // Botão para aumentar o valor do tempo entre ativações de uma armadilha
        private Botao bTempoEntreAtivacoesSoma;

        // Botão para reduzir o valor do tempo entre ativações de uma armadilha
        private Botao bTempoEntreAtivacoesSub;

        // Lista contendo todos os botoes
        private List<Botao> botoes;

        /* Enumeração de todas as ações do menu */
        public enum Acao
        {
            Nenhum,
            NovoMundo,
            Salvar,
            Carregar,
            AdicionarObjeto,
            ExcluirObjeto,
            GerenciarConexao,
            AumentarTempoInicio,
            ReduzirTempoInicio,
            AumentarTempoAtivo,
            ReduzirTempoAtivo,
            AumentarTempoEntreAtivacoes,
            ReduzirTempoEntreAtivacoes,
        }

        /* Classe aninhada para um botão */
        class Botao
        {
            private Texture textura;
            public Sprite BSprite { get; private set; }
            public Acao BAcao { get; set; }

            public Botao(Vector2f posicao, string pathTextura, Acao acao)
            {
                try
                {
                    textura = new Texture(pathTextura);
                    BSprite = new Sprite(textura);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }

                BAcao = acao;
                BSprite.Color = Color.White;
                BSprite.Position = posicao;
            }

            public void AtualizarPosicao(float x, float y)
            {
                /* Atualiza as coordenadas do botão na tela */
                Vector2f pos = new Vector2f();
                pos.X = BSprite.Position.X + x;
                pos.Y = BSprite.Position.Y + y;
                BSprite.Position = pos;
            }
        }

        public Menu()
        {

        }

        public Acao BotaoPressionado(float x, float y)
        {
            // Encontra o botao que foi pressionado e o ativa
            foreach(Botao botao in botoes)
            {
                if (x >= botao.BSprite.Position.X && x <= botao.BSprite.Texture.Size.X &&
                    y >= botao.BSprite.Position.Y && y <= botao.BSprite.Texture.Size.Y)
                    return botao.BAcao;
            }

            // Caso nenhum botão tenha sido pressionado, retorna false
            return Acao.Nenhum;
        }
    }
}
