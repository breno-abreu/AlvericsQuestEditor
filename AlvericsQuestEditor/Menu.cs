using System;
using System.Collections.Generic;
using System.Xml;
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

        // Lista contendo toda a parte textual do menu
        private List<Text> labels;

        // Proporção do distanciamento entre botões no eixo X
        private const float xProporcao = 7f;

        // Distancia entre botões no eixo Y
        private const float yDistancia = 70f;

        /* Enumeração de todas as ações do menu */
        public enum Acao
        {
            Nenhum,
            IndicarEntidade,
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
            public Acao BAcao { get; private set; }
            public int EntidadeID { get; private set; }

            public Botao(Vector2f posicao, string pathTextura, Acao acao, int entidadeID)
            {
                try
                {
                    textura = new Texture(pathTextura);
                    
                    BSprite = new Sprite(textura);
                    BSprite.Scale = new Vector2f(.3f, .3f);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }

                BSprite.Origin = new Vector2f(BSprite.Texture.Size.X / 2, BSprite.Texture.Size.Y / 2);
                BAcao = acao;
                BSprite.Position = posicao;
                EntidadeID = entidadeID;
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

        public Menu(Vector2f windowSize, View view)
        {
            // Inicializa o background
            background = new RectangleShape(view.Size);
            background.FillColor = new Color(50, 150, 220);
            background.Position = new Vector2f(view.Center.X - (windowSize.X * 0.3f) / 2f,
                                               view.Center.Y - windowSize.Y / 2f);

            // Inicializa os botões
            bNovoMundo = new Botao(GetPosicaoBotao(windowSize, view, -1, 1), @"MenuIcons\new.png", Acao.NovoMundo, -1);
            bSalvar = new Botao(GetPosicaoBotao(windowSize, view, 0, 1), @"MenuIcons\save.png", Acao.Salvar, -1);
            bCarregar = new Botao(GetPosicaoBotao(windowSize, view, 1, 1), @"MenuIcons\archive.png", Acao.Carregar, -1) ;
            bAdicionarObjeto = new Botao(GetPosicaoBotao(windowSize, view, -1, 2), @"MenuIcons\pencil.png", Acao.AdicionarObjeto, -1);
            bExcluirObjeto = new Botao(GetPosicaoBotao(windowSize, view, 0, 2), @"MenuIcons\eraser.png", Acao.ExcluirObjeto, -1);
            bGerenciarConexao = new Botao(GetPosicaoBotao(windowSize, view, 1, 2), @"MenuIcons\magnet.png", Acao.GerenciarConexao, -1);
            bInicioAtivacaoSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 3), @"MenuIcons\right.png", Acao.GerenciarConexao, -1);
            bInicioAtivacaoSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 3), @"MenuIcons\left.png", Acao.GerenciarConexao, -1);
            bTempoAtivadoSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 4), @"MenuIcons\right.png", Acao.GerenciarConexao, -1);
            bTempoAtivadoSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 4), @"MenuIcons\left.png", Acao.GerenciarConexao, -1);
            bTempoEntreAtivacoesSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 5), @"MenuIcons\right.png", Acao.GerenciarConexao, -1);
            bTempoEntreAtivacoesSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 5), @"MenuIcons\left.png", Acao.GerenciarConexao, -1);

            botoes = new List<Botao>() 
            { 
                bNovoMundo, 
                bSalvar, 
                bCarregar, 
                bAdicionarObjeto, 
                bExcluirObjeto, 
                bGerenciarConexao,
                bInicioAtivacaoSoma,
                bInicioAtivacaoSub,
                bTempoAtivadoSoma,
                bTempoAtivadoSub,
                bTempoEntreAtivacoesSoma,
                bTempoEntreAtivacoesSub,
            };
        }

        private Vector2f GetPosicaoBotao(Vector2f windowSize, View view, int coluna, int linha)
        {
            float posColuna = view.Center.X + coluna * (view.Size.X / xProporcao);
            float posLinha = view.Center.Y - windowSize.Y / 2 + yDistancia * linha;
            return new Vector2f(posColuna, posLinha);
        }

        public Acao BotaoPressionado(float x, float y)
        {
            // Encontra o botao que foi pressionado e o ativa
            foreach(Botao botao in botoes)
            {
                // Variáveis recebem os seguintes atributos de um botão: 
                // posição em x, posição em y, x mais o comprimento, y mais a altura

                /* ----------------------------------------------------------------*/
                float bx = botao.BSprite.Position.X;
                float by = botao.BSprite.Position.Y;
                float bcomprimento = bx + botao.BSprite.Texture.Size.X;
                float baltura = by + botao.BSprite.Texture.Size.Y;
                /* ----------------------------------------------------------------*/

                if (x >= bx && x <= bcomprimento &&
                    y >= by && y <= baltura)
                    return botao.BAcao;
            }

            // Caso nenhum botão tenha sido pressionado, retorna a acao Nenhum
            return Acao.Nenhum;
        }

        public void Desenhar(RenderWindow window)
        {
            window.Draw(background);

            foreach (Botao botao in botoes)
                window.Draw(botao.BSprite);

            /*foreach (Text label in labels)
                window.Draw(label);*/
        }

        public void RedimensionarMenu(Vector2f windowSize, View view)
        {
            // Redimensiona o background
            background.Size = view.Size;
            background.Position = new Vector2f(view.Center.X - (windowSize.X * 0.3f) / 2f,
                                               view.Center.Y - windowSize.Y / 2f);

            // Determina a nova posição dos botões
            bNovoMundo.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 1);
            bSalvar.BSprite.Position = GetPosicaoBotao(windowSize, view, 0, 1);
            bCarregar.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 1);
            bAdicionarObjeto.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 2);
            bExcluirObjeto.BSprite.Position = GetPosicaoBotao(windowSize, view, 0, 2);
            bGerenciarConexao.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 2);
            bInicioAtivacaoSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 3);
            bInicioAtivacaoSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 3);
            bTempoAtivadoSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 4);
            bTempoAtivadoSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 4);
            bTempoEntreAtivacoesSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 5);
            bTempoEntreAtivacoesSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 5);
        }
    }
}
