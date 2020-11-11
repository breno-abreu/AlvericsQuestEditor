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
        private Botao bConexao;

        // Botão para gerenciar as propriedades das armadilhas
        private Botao bPropriedades;

        // Botão para gerenciar diálogos de um NPC
        private Botao bDialogos;

        // Botão para gerenciar eventos do jogo
        private Botao bEventos;

        // Botão para gerenciar as músicas do jogo
        private Botao bMusicas;


        /*// Botão para aumentar o valor do tempo de início de ativação de uma armadilha
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
        private Botao bTempoEntreAtivacoesSub;*/

        // Lista contendo todos os botoes
        private List<Botao> botoes;

        // Lista contendo toda a parte textual do menu
        private List<Text> labels;

        // Proporção do distanciamento entre botões no eixo X
        private const float xProporcao = 7f;

        // Distancia entre botões no eixo Y
        private const float yDistancia = 60f;

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
            GerenciarPropriedades,
            GerenciarDialogos,
            GerenciarEventos,
            GerenciarMusicas,

            /*AumentarTempoInicio,
            ReduzirTempoInicio,
            AumentarTempoAtivo,
            ReduzirTempoAtivo,
            AumentarTempoEntreAtivacoes,
            ReduzirTempoEntreAtivacoes,*/
        }

        /* Classe aninhada para um botão */
        class Botao
        {
            private Texture textura;
            public Sprite BSprite { get; private set; }
            public Acao BAcao { get; private set; }
            public int EntidadeID { get; private set; }
            public Vector2f Dimensoes { get; private set; }

            private const float PROPORCAO = .35f;

            public Botao(Vector2f posicao, string pathTextura, Acao acao, int entidadeID)
            {
                Dimensoes = new Vector2f();

                try
                {
                    textura = new Texture(pathTextura);
                    textura.Smooth = true;
                    BSprite = new Sprite(textura);
                    BSprite.Scale = new Vector2f(PROPORCAO, PROPORCAO);
                    Vector2f dim = new Vector2f(textura.Size.X * PROPORCAO, textura.Size.Y * PROPORCAO);
                    Dimensoes = dim;
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
            bAdicionarObjeto = new Botao(GetPosicaoBotao(windowSize, view, -3, 2), @"MenuIcons\pencil.png", Acao.AdicionarObjeto, -1);
            bExcluirObjeto = new Botao(GetPosicaoBotao(windowSize, view, -2, 2), @"MenuIcons\eraser.png", Acao.ExcluirObjeto, -1);
            bConexao = new Botao(GetPosicaoBotao(windowSize, view, -1, 2), @"MenuIcons\magnet.png", Acao.GerenciarConexao, -1);
            bPropriedades = new Botao(GetPosicaoBotao(windowSize, view, 0, 2), @"MenuIcons\proprieties.png", Acao.GerenciarPropriedades, -1);
            bDialogos = new Botao(GetPosicaoBotao(windowSize, view, 1, 2), @"MenuIcons\dialog.png", Acao.GerenciarDialogos, -1);
            bEventos = new Botao(GetPosicaoBotao(windowSize, view, 2, 2), @"MenuIcons\event.png", Acao.GerenciarEventos, -1);
            bMusicas = new Botao(GetPosicaoBotao(windowSize, view, 3, 2), @"MenuIcons\music.png", Acao.GerenciarMusicas, -1);



            /*bInicioAtivacaoSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 3), @"MenuIcons\right.png", Acao.GerenciarConexao, -1);
            bInicioAtivacaoSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 3), @"MenuIcons\left.png", Acao.GerenciarConexao, -1);
            bTempoAtivadoSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 4), @"MenuIcons\right.png", Acao.GerenciarConexao, -1);
            bTempoAtivadoSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 4), @"MenuIcons\left.png", Acao.GerenciarConexao, -1);
            bTempoEntreAtivacoesSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 5), @"MenuIcons\right.png", Acao.GerenciarConexao, -1);
            bTempoEntreAtivacoesSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 5), @"MenuIcons\left.png", Acao.GerenciarConexao, -1);*/

            // Preenche a lista de botões
            botoes = new List<Botao>() 
            { 
                bNovoMundo, 
                bSalvar, 
                bCarregar, 
                bAdicionarObjeto, 
                bExcluirObjeto, 
                bConexao,
                bPropriedades,
                bDialogos,
                bEventos,
                bMusicas,
            };
        }

        /* Dado uma linha e uma coluna, retorna as coordenadas de um botão */
        private Vector2f GetPosicaoBotao(Vector2f windowSize, View view, int coluna, int linha)
        {
            float posColuna = view.Center.X + coluna * (view.Size.X / xProporcao);
            float posLinha = view.Center.Y - windowSize.Y / 2 + yDistancia * linha;
            return new Vector2f(posColuna, posLinha);
        }

        /* Quando o botão esquerdo do mouse for pressionado percorre a lista de botões e encontra qual ação deverá ser efetuada */
        public Acao BotaoPressionado(RenderWindow window, Vector2i mousePos)
        {
            Vector2i bCoord;
            float bComprimento;
            float bAltura;

            // Encontra o botao que foi pressionado e o ativa
            foreach(Botao botao in botoes)
            {
                // Variáveis recebem os seguintes atributos de um botão: 
                // posição em x, posição em y, x mais o comprimento, y mais a altura

                bCoord = window.MapCoordsToPixel(botao.BSprite.Position);
                bCoord.X -= (int)(botao.Dimensoes.X / 2);
                bCoord.Y -= (int)(botao.Dimensoes.Y / 2);
                bComprimento = bCoord.X + botao.Dimensoes.X;
                bAltura = bCoord.Y + botao.Dimensoes.Y;

                if (mousePos.X >= bCoord.X && mousePos.X <= bComprimento &&
                    mousePos.Y >= bCoord.Y && mousePos.Y <= bAltura)
                {
                    ResetarCorBotoes();
                    botao.BSprite.Color = new Color(50, 70, 90);
                    return botao.BAcao;
                } 
            }

            // Caso nenhum botão tenha sido pressionado, retorna a acao Nenhum
            return Acao.Nenhum;
        }

        /* Reseta a cor de todos os botões, originalmente brancos */
        public void ResetarCorBotoes()
        {
            foreach (Botao botao in botoes)
            {
                botao.BSprite.Color = Color.White;
            }
        }

        /* Desenha todos os elementos do menu na tela */
        public void Desenhar(RenderWindow window)
        {
            window.Draw(background);

            foreach (Botao botao in botoes)
                window.Draw(botao.BSprite);

            /*foreach (Text label in labels)
                window.Draw(label);*/
        }

        /* Quando a janela for redimensionada, todos os elementos são atualizados */
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
            bAdicionarObjeto.BSprite.Position = GetPosicaoBotao(windowSize, view, -3, 2);
            bExcluirObjeto.BSprite.Position = GetPosicaoBotao(windowSize, view, -2, 2);
            bConexao.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 2);
            bPropriedades.BSprite.Position = GetPosicaoBotao(windowSize, view, 0, 2);
            bDialogos.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 2);
            bEventos.BSprite.Position = GetPosicaoBotao(windowSize, view, 2, 2);
            bMusicas.BSprite.Position = GetPosicaoBotao(windowSize, view, 3, 2);


            /*bInicioAtivacaoSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 3);
            bInicioAtivacaoSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 3);
            bTempoAtivadoSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 4);
            bTempoAtivadoSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 4);
            bTempoEntreAtivacoesSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 5);
            bTempoEntreAtivacoesSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 5);*/
        }
    }
}
