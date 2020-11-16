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

        // Lista de botões quando a opção de propriedades é ativada
        private List<Botao> botoesPropriedades;

        // Lista contendo toda a parte textual do menu
        private List<Text> labels;

        // Fonte para os textos
        private Font fonte;

        // Textos do menu de propriedades
        private Text tempoInicial;
        private Text tempoEntreAtivacoes;
        private Text tempoAtivado;
        private Text tiValorT;
        private Text teaValorT;
        private Text taValorT;

        // Variáveis dos valores das propriedades
        private float tiValor;
        private float teaValor;
        private float taValor;
        private const string na = "N/A";

        // Proporção do distanciamento entre botões no eixo X
        private const float xProporcao = 7f;

        // Distancia entre botões no eixo Y
        private const float yDistancia = 70f;

        // Indica se o menu de propriedades deve ser mostrado
        private bool menuPropriedadesAtivo;

        // Indica se a cor de um botão no menu de propriedades pode ser apagado
        private bool botaoDesenhado;

        // Contador para permitir o botão ficar desenhado por um certo tempo até apagar
        private int contBotaoAux;

        // Botao para o menu de entidades
        private Botao menuEntidades;

        // Proporção dos botões do menu para redimensionamento
        private const float PROPORCAO_BOTOES = .35f;

        // Proporção do menu de entidades para redimensionamento
        private const float PROPORCAO_ENTIDADES = 4f;

        // Quantidade de linhas e colunas do menu de entidades
        private Vector2i quantidadeMenuEntidade;

        // Contém a posição de uma entidade no menu de entidades
        public Vector2i posicaoEntidade { get; private set; }

        public Menu(Vector2f windowSize, View view)
        {
            // Inicializa o background
            background = new RectangleShape(view.Size);
            background.FillColor = new Color(50, 150, 220);
            background.Position = new Vector2f(view.Center.X - (windowSize.X * 0.3f) / 2f,
                                               view.Center.Y - windowSize.Y / 2f);

            // Inicializa os botões
            bNovoMundo = new Botao(GetPosicaoBotao(windowSize, view, -1, 1), @"MenuIcons\new.png", Acao.NovoMundo, PROPORCAO_BOTOES);
            bSalvar = new Botao(GetPosicaoBotao(windowSize, view, 0, 1), @"MenuIcons\save.png", Acao.Salvar, PROPORCAO_BOTOES);
            bCarregar = new Botao(GetPosicaoBotao(windowSize, view, 1, 1), @"MenuIcons\archive.png", Acao.Carregar, PROPORCAO_BOTOES) ;
            bAdicionarObjeto = new Botao(GetPosicaoBotao(windowSize, view, -3, 2), @"MenuIcons\pencil.png", Acao.AdicionarObjeto, PROPORCAO_BOTOES);
            bExcluirObjeto = new Botao(GetPosicaoBotao(windowSize, view, -2, 2), @"MenuIcons\eraser.png", Acao.ExcluirObjeto, PROPORCAO_BOTOES);
            bConexao = new Botao(GetPosicaoBotao(windowSize, view, -1, 2), @"MenuIcons\magnet.png", Acao.GerenciarConexao, PROPORCAO_BOTOES);
            bPropriedades = new Botao(GetPosicaoBotao(windowSize, view, 0, 2), @"MenuIcons\proprieties.png", Acao.GerenciarPropriedades, PROPORCAO_BOTOES);
            bDialogos = new Botao(GetPosicaoBotao(windowSize, view, 1, 2), @"MenuIcons\dialog.png", Acao.GerenciarDialogos, PROPORCAO_BOTOES);
            bEventos = new Botao(GetPosicaoBotao(windowSize, view, 2, 2), @"MenuIcons\event.png", Acao.GerenciarEventos, PROPORCAO_BOTOES);
            bMusicas = new Botao(GetPosicaoBotao(windowSize, view, 3, 2), @"MenuIcons\music.png", Acao.GerenciarMusicas, PROPORCAO_BOTOES);

            // Inicializa os botões do menu de propriedades
            bInicioAtivacaoSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 4), @"MenuIcons\right.png", Acao.AumentarTempoInicio, PROPORCAO_BOTOES);
            bInicioAtivacaoSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 4), @"MenuIcons\left.png", Acao.ReduzirTempoInicio, PROPORCAO_BOTOES);
            bTempoAtivadoSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 5), @"MenuIcons\right.png", Acao.AumentarTempoEntreAtivacoes, PROPORCAO_BOTOES);
            bTempoAtivadoSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 5), @"MenuIcons\left.png", Acao.ReduzirTempoEntreAtivacoes, PROPORCAO_BOTOES);
            bTempoEntreAtivacoesSoma = new Botao(GetPosicaoBotao(windowSize, view, 1, 6), @"MenuIcons\right.png", Acao.AumentarTempoAtivo, PROPORCAO_BOTOES);
            bTempoEntreAtivacoesSub = new Botao(GetPosicaoBotao(windowSize, view, -1, 6), @"MenuIcons\left.png", Acao.ReduzirTempoAtivo, PROPORCAO_BOTOES);

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

            // Preenche a lista de botões de propriedades
            botoesPropriedades = new List<Botao>()
            {
                bInicioAtivacaoSoma,
                bInicioAtivacaoSub,
                bTempoEntreAtivacoesSoma,
                bTempoEntreAtivacoesSub,
                bTempoAtivadoSoma,
                bTempoAtivadoSub,
            };

            menuPropriedadesAtivo = false;

            try
            {
                fonte = new Font(@"arial.ttf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            tempoInicial = new Text("Inicio", fonte);
            tempoEntreAtivacoes = new Text("Entre Ativações", fonte);
            tempoAtivado = new Text("Ativado", fonte);
            tiValorT = new Text(na, fonte);
            teaValorT = new Text(na, fonte);
            taValorT = new Text(na, fonte);

            labels = new List<Text>()
            {
                tempoInicial,
                tiValorT,
                tempoEntreAtivacoes,
                teaValorT,
                tempoAtivado,
                taValorT,
            };

            float auxY = view.Center.Y - windowSize.Y / 2 + 3.5f * yDistancia;
            float auxX = view.Center.X;

            foreach (Text label in labels)
            {
                label.CharacterSize = 15;
                label.FillColor = Color.White;
                label.Position = new Vector2f(auxX, auxY);
                auxY += yDistancia / 2;
            }

            AtualizarOriginLabels();

            tempoInicial.Style = Text.Styles.Italic;
            tempoEntreAtivacoes.Style = Text.Styles.Italic;
            tempoAtivado.Style = Text.Styles.Italic;

            botaoDesenhado = false;
            contBotaoAux = 0;

            taValor = 0;
            teaValor = 0;
            tiValor = 0;

            quantidadeMenuEntidade = new Vector2i(5, 3);
            posicaoEntidade = new Vector2i();

            menuEntidades = new Botao(GetPosicaoBotao(windowSize, view, 0, 5), @"teste.png", Acao.IndicarEntidade, PROPORCAO_ENTIDADES);
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

                    if (botao.BAcao == Acao.GerenciarPropriedades)
                        menuPropriedadesAtivo = true;
                    else
                        menuPropriedadesAtivo = false;

                    return botao.BAcao;
                } 
            }

            if (menuPropriedadesAtivo)
            {
                foreach(Botao botao in botoesPropriedades)
                {
                    bCoord = window.MapCoordsToPixel(botao.BSprite.Position);
                    bCoord.X -= (int)(botao.Dimensoes.X / 2);
                    bCoord.Y -= (int)(botao.Dimensoes.Y / 2);
                    bComprimento = bCoord.X + botao.Dimensoes.X;
                    bAltura = bCoord.Y + botao.Dimensoes.Y;

                    if (mousePos.X >= bCoord.X && mousePos.X <= bComprimento &&
                        mousePos.Y >= bCoord.Y && mousePos.Y <= bAltura)
                    {
                        ResetarCorBotoesPropriedades();
                        botao.BSprite.Color = new Color(50, 70, 90);
                        botaoDesenhado = true;
                        AtualizarValores(botao.BAcao);
                        return botao.BAcao;
                    }
                }
            }
            else
            {
                // Localiza em qual linha e qual coluna do menu de entidades o mouse foi pressionado
                bCoord = window.MapCoordsToPixel(menuEntidades.BSprite.Position);
                bCoord.X -= (int)(menuEntidades.Dimensoes.X / 2);
                bCoord.Y -= (int)(menuEntidades.Dimensoes.Y / 2);
                bComprimento = bCoord.X + menuEntidades.Dimensoes.X;
                bAltura = bCoord.Y + menuEntidades.Dimensoes.Y;

                if (mousePos.X >= bCoord.X && mousePos.X <= bComprimento &&
                        mousePos.Y >= bCoord.Y && mousePos.Y <= bAltura)
                {
                    int x = (int)((mousePos.X - bCoord.X) / (menuEntidades.Dimensoes.X / quantidadeMenuEntidade.X));
                    int y = (int)((mousePos.Y - bCoord.Y) / (menuEntidades.Dimensoes.Y / quantidadeMenuEntidade.Y));
                    posicaoEntidade = new Vector2i(x, y);
                    return menuEntidades.BAcao;
                }
            }

            // Caso nenhum botão tenha sido pressionado, retorna a acao Nenhum
            return Acao.Nenhum;
        }

        private void AtualizarValores(Acao a)
        {
            if(a == Acao.AumentarTempoInicio)
            {
                tiValor += 0.5f;
                tiValorT.DisplayedString = tiValor.ToString(); 
            }
            else if (a == Acao.ReduzirTempoInicio && tiValor > 0)
            {
                tiValor -= 0.5f;
                tiValorT.DisplayedString = tiValor.ToString();
            }
            else if (a == Acao.AumentarTempoEntreAtivacoes)
            {
                teaValor += 0.5f;
                teaValorT.DisplayedString = teaValor.ToString();
            }
            else if (a == Acao.ReduzirTempoEntreAtivacoes && teaValor > 0)
            {
                teaValor -= 0.5f;
                teaValorT.DisplayedString = teaValor.ToString();
            }
            else if (a == Acao.AumentarTempoAtivo)
            {
                taValor += 0.5f;
                taValorT.DisplayedString = taValor.ToString();
            }
            else if (a == Acao.ReduzirTempoAtivo && taValor > 0)
            {
                taValor -= 0.5f;
                taValorT.DisplayedString = taValor.ToString();
            }

            AtualizarOriginLabels();
        }

        /* Reseta a cor de todos os botões, originalmente brancos */
        public void ResetarCorBotoes()
        {
            foreach (Botao botao in botoes)
                botao.BSprite.Color = Color.White;
        }

        public void ResetarCorBotoesPropriedades()
        {
            foreach (Botao botao in botoesPropriedades)
                botao.BSprite.Color = Color.White;
        }

        public void AtualizarPosicaoBackground(float y)
        {
            Vector2f aux = new Vector2f(background.Position.X, background.Position.Y + y);
            background.Position = aux;
        }

        /* Desenha todos os elementos do menu na tela */
        public void Desenhar(RenderWindow window)
        {
            window.Draw(background);

            foreach (Botao botao in botoes)
                window.Draw(botao.BSprite);

            if (menuPropriedadesAtivo)
            {
                foreach (Botao botao in botoesPropriedades)
                    window.Draw(botao.BSprite);

                foreach (Text label in labels)
                    window.Draw(label);

                if (botaoDesenhado)
                {
                    contBotaoAux++;

                    if (contBotaoAux >= 200)
                    {
                        ResetarCorBotoesPropriedades();
                        botaoDesenhado = false;
                        contBotaoAux = 0;
                    } 
                }
            }
            else
            {
                window.Draw(menuEntidades.BSprite);
            }
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

            // Determina a nova posição dos botões do menu de propriedades
            bInicioAtivacaoSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 4);
            bInicioAtivacaoSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 4);
            bTempoAtivadoSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 5);
            bTempoAtivadoSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 5);
            bTempoEntreAtivacoesSoma.BSprite.Position = GetPosicaoBotao(windowSize, view, 1, 6);
            bTempoEntreAtivacoesSub.BSprite.Position = GetPosicaoBotao(windowSize, view, -1, 6);

            // Determinar a nova posição dos botões do menu de entidades
            menuEntidades.BSprite.Position = GetPosicaoBotao(windowSize, view, 0, 5);

            // Determina a nova posição dos labels do menu de propriedades
            float auxY = view.Center.Y - windowSize.Y / 2 + 3.5f * yDistancia;
            float auxX = view.Center.X;

            foreach (Text label in labels)
            {
                label.FillColor = Color.White;
                label.Position = new Vector2f(auxX, auxY);
                auxY += yDistancia / 2;
                FloatRect auxVec = label.GetLocalBounds();
                label.Origin = new Vector2f(auxVec.Left + auxVec.Width / 2, auxVec.Top + auxVec.Height / 2);
            }
        }
        private void AtualizarOriginLabels()
        {
            foreach (Text label in labels)
            {
                FloatRect auxVec = label.GetLocalBounds();
                label.Origin = new Vector2f(auxVec.Left + auxVec.Width / 2, auxVec.Top + auxVec.Height / 2);
            }
        }

        /* Classe aninhada para um botão */
        class Botao
        {
            private Texture textura;
            public Sprite BSprite { get; private set; }
            public Acao BAcao { get; private set; }
            public int Entidade { get; private set; }
            public Vector2f Dimensoes { get; private set; }

            public Botao(Vector2f posicao, string pathTextura, Acao acao, float proporcao)
            {
                Dimensoes = new Vector2f();

                try
                {
                    textura = new Texture(pathTextura);

                    if(acao != Acao.IndicarEntidade)
                        textura.Smooth = true;

                    BSprite = new Sprite(textura);
                    BSprite.Scale = new Vector2f(proporcao, proporcao);
                    Vector2f dim = new Vector2f(textura.Size.X * proporcao, textura.Size.Y * proporcao);
                    Dimensoes = dim;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }

                BSprite.Origin = new Vector2f(BSprite.Texture.Size.X / 2, BSprite.Texture.Size.Y / 2);
                BAcao = acao;
                BSprite.Position = posicao;
                //Entidade = entidade;
            }
        }
    }
}
