using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    /* Classe do menu do editor. Contém os botões que irão engatilhar eventos. */
    class Menu
    {
        // Retângulo para o background do menu
        private RectangleShape background;

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

        // Lista contendo todos os botoes principais
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

        // Contém a posição de uma entidade no menu de entidades
        public Vector2i posicaoEntidade { get; private set; }

        // Referência para a jenela do programa
        private RenderWindow window;

        // Referência para a view que contém o menu
        private View view;


        public Menu(RenderWindow window, View view)
        {
            this.window = window;
            this.view = view;

            /* Inicializa os componentes do menu, carregando-os em suas respectivas estruturas de dados */
            InicializarBackground();
            InicializarBotoes();
            InicializarLabels();
        }

        private void InicializarBackground()
        {
            /* Inicializa o background definindo seu tamanho, cor e posição na view recebida */
            background = new RectangleShape(view.Size);
            background.FillColor = new Color(50, 150, 220);
            background.Position = new Vector2f(view.Center.X - (window.Size.X * 0.3f) / 2f,
                                               view.Center.Y - window.Size.Y / 2f);
        }

        private void InicializarBotoes()
        {
            /* Inicializa todos os botões, que possuem uma posição, ícone, uma ação e a proporção para o redimensionamento da imagem do ícone */

            // Inicializa os botões principais
            bNovoMundo = new Botao(GetPosicaoBotao(-1, 1), @"MenuIcons\new.png", Acao.NovoMundo, PROPORCAO_BOTOES);
            bSalvar = new Botao(GetPosicaoBotao( 0, 1), @"MenuIcons\save.png", Acao.Salvar, PROPORCAO_BOTOES);
            bCarregar = new Botao(GetPosicaoBotao(1, 1), @"MenuIcons\archive.png", Acao.Carregar, PROPORCAO_BOTOES);
            bAdicionarObjeto = new Botao(GetPosicaoBotao(-3, 2), @"MenuIcons\pencil.png", Acao.AdicionarObjeto, PROPORCAO_BOTOES);
            bExcluirObjeto = new Botao(GetPosicaoBotao(-2, 2), @"MenuIcons\eraser.png", Acao.ExcluirObjeto, PROPORCAO_BOTOES);
            bConexao = new Botao(GetPosicaoBotao(-1, 2), @"MenuIcons\magnet.png", Acao.GerenciarConexao, PROPORCAO_BOTOES);
            bPropriedades = new Botao(GetPosicaoBotao(0, 2), @"MenuIcons\proprieties.png", Acao.GerenciarPropriedades, PROPORCAO_BOTOES);
            bDialogos = new Botao(GetPosicaoBotao(1, 2), @"MenuIcons\dialog.png", Acao.GerenciarDialogos, PROPORCAO_BOTOES);
            bEventos = new Botao(GetPosicaoBotao(2, 2), @"MenuIcons\event.png", Acao.GerenciarEventos, PROPORCAO_BOTOES);
            bMusicas = new Botao(GetPosicaoBotao(3, 2), @"MenuIcons\music.png", Acao.GerenciarMusicas, PROPORCAO_BOTOES);

            // Inicializa os botões do menu de propriedades
            bInicioAtivacaoSoma = new Botao(GetPosicaoBotao(1, 4), @"MenuIcons\right.png", Acao.AumentarTempoInicio, PROPORCAO_BOTOES);
            bInicioAtivacaoSub = new Botao(GetPosicaoBotao(-1, 4), @"MenuIcons\left.png", Acao.ReduzirTempoInicio, PROPORCAO_BOTOES);
            bTempoAtivadoSoma = new Botao(GetPosicaoBotao(1, 5), @"MenuIcons\right.png", Acao.AumentarTempoEntreAtivacoes, PROPORCAO_BOTOES);
            bTempoAtivadoSub = new Botao(GetPosicaoBotao(-1, 5), @"MenuIcons\left.png", Acao.ReduzirTempoEntreAtivacoes, PROPORCAO_BOTOES);
            bTempoEntreAtivacoesSoma = new Botao(GetPosicaoBotao(1, 6), @"MenuIcons\right.png", Acao.AumentarTempoAtivo, PROPORCAO_BOTOES);
            bTempoEntreAtivacoesSub = new Botao(GetPosicaoBotao(-1, 6), @"MenuIcons\left.png", Acao.ReduzirTempoAtivo, PROPORCAO_BOTOES);

            // Preenche a lista de botões principais
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
            botaoDesenhado = false;
            contBotaoAux = 0;

            // Cria um botão para o menu entidade. A entidade escolhida será determinada através da posição do mouse dentro da imagem, 
            // por isso existe apenas um botão para todas as entidades do mundo no menu
            menuEntidades = new Botao(GetPosicaoBotao(0, 5), Informacoes.entidadesImgPath, Acao.IndicarEntidade, PROPORCAO_ENTIDADES);

            posicaoEntidade = new Vector2i();
        }

        private void InicializarLabels()
        {
            /* Inicializa a parte textual do menu */

            // Tenta carregar uma fonte, caso não consiga o programa é desligado enviando uma mensagem de erro
            try
            {
                fonte = new Font(@"arial.ttf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            // Inicializa cada objeto de Text com sua string e a fonte carregada anteriormente
            tempoInicial = new Text("Inicio", fonte);
            tempoEntreAtivacoes = new Text("Entre Ativações", fonte);
            tempoAtivado = new Text("Ativado", fonte);
            tiValorT = new Text(na, fonte);
            teaValorT = new Text(na, fonte);
            taValorT = new Text(na, fonte);

            // Carrega a lista de textos
            labels = new List<Text>()
            {
                tempoInicial,
                tiValorT,
                tempoEntreAtivacoes,
                teaValorT,
                tempoAtivado,
                taValorT,
            };

            // Processo de posicionamento dos textos na tela. Variáveis auxiliares para a posição inicial do primeiro texto,
            // Será incrementado no laço a seguir
            float auxY = view.Center.Y - window.Size.Y / 2 + 3.5f * yDistancia;
            float auxX = view.Center.X;

            foreach (Text label in labels)
            {
                // Define o tamanho e a cor do texto
                label.CharacterSize = 15;
                label.FillColor = Color.White;

                // Define a localização do texto, mantendo a posição em x e incrementando a posição em y a cada iteração
                label.Position = new Vector2f(auxX, auxY);
                auxY += yDistancia / 2;
            }

            // Define que os títulos de cada operação sejam em itálico
            tempoInicial.Style = Text.Styles.Italic;
            tempoEntreAtivacoes.Style = Text.Styles.Italic;
            tempoAtivado.Style = Text.Styles.Italic;

            // Define a origem dos textos em seus centros
            AtualizarOriginLabels();

            // Inicializa o valor de cada uma das operações
            taValor = 0;
            teaValor = 0;
            tiValor = 0;
        }

        
        private Vector2f GetPosicaoBotao(int coluna, int linha)
        {
            /* Dado uma linha e uma coluna, retorna as coordenadas de um botão */
            float posColuna = view.Center.X + coluna * (view.Size.X / xProporcao);
            float posLinha = view.Center.Y - window.Size.Y / 2 + yDistancia * linha;
            return new Vector2f(posColuna, posLinha);
        }

        
        public Acao BotaoPressionado(Vector2i mousePos)
        {
            /* Quando o botão esquerdo do mouse for pressionado percorre a lista de botões e encontra qual ação deverá ser retornado */
            Vector2i bCoord;
            float bComprimento;
            float bAltura;

            // Encontra o botao que foi pressionado e o ativa caso faça aprte dos botões principais
            foreach(Botao botao in botoes)
            {
                // Variáveis recebem os seguintes atributos de um botão: 
                // posição em x, posição em y, x mais o comprimento, y mais a altura do botão
                // window.MapCoordsToPixel retorna as coordenadas em relação à janela daquele sprite
                bCoord = window.MapCoordsToPixel(botao.BSprite.Position);
                // Permite descobrir as coordenadas reais do sprite, sem sua mudança no valor da origem
                bCoord.X -= (int)(botao.Dimensoes.X / 2);
                bCoord.Y -= (int)(botao.Dimensoes.Y / 2);
                // Permite descobrir as coordenadas finais de um botão
                bComprimento = bCoord.X + botao.Dimensoes.X;
                bAltura = bCoord.Y + botao.Dimensoes.Y;

                // Percorre a lista de botões e retorna se um dos botões foi pressionado
                if (mousePos.X >= bCoord.X && mousePos.X <= bComprimento &&
                    mousePos.Y >= bCoord.Y && mousePos.Y <= bAltura)
                {
                    // Reseta a cor de todos os botões para o branco
                    ResetarCorBotoes();

                    // Pinta o botão com uma cor diferente dos demais
                    botao.BSprite.Color = new Color(50, 70, 90);

                    // Caso o botão pressionado for o para ativar as propriedades de uma entidade, 
                    // ativa a variável para que seja possível trocar o menu de entidades para o de propriedades
                    if (botao.BAcao == Acao.GerenciarPropriedades)
                        menuPropriedadesAtivo = true;
                    else
                        menuPropriedadesAtivo = false;

                    return botao.BAcao;
                } 
            }

            // Caso o menu de propriedades esteja ativo e o botão pressionado não seja um dos principais,
            // Percorre os botões de propriedades e retorna o que foi pressionado
            if (menuPropriedadesAtivo)
            {
                // Percorre a lista de botões de propriedades e retorna se um deles foi pressionado
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
                        // Reseta a cor de todos os botões para o branco
                        ResetarCorBotoes();

                        // Pinta o botão com uma cor diferente dos demais
                        botao.BSprite.Color = new Color(50, 70, 90);

                        // Variável que permite que o botão esteja pintado por um tempo até voltar a sua cor original
                        botaoDesenhado = true;

                        // Atualiza os valores das propriedades
                        AtualizarValores(botao.BAcao);

                        return botao.BAcao;
                    }
                }
            }

            // Caso o botão pressionado não esteja nem no menu principal, nem no menu de propriedades,
            // Percorre a lista de entidades e retorna sua ação
            else
            {
                bCoord = window.MapCoordsToPixel(menuEntidades.BSprite.Position);
                bCoord.X -= (int)(menuEntidades.Dimensoes.X / 2);
                bCoord.Y -= (int)(menuEntidades.Dimensoes.Y / 2);
                bComprimento = bCoord.X + menuEntidades.Dimensoes.X;
                bAltura = bCoord.Y + menuEntidades.Dimensoes.Y;

                // Localiza em qual linha e qual coluna do menu de entidades o mouse foi pressionado
                if (mousePos.X >= bCoord.X && mousePos.X <= bComprimento &&
                        mousePos.Y >= bCoord.Y && mousePos.Y <= bAltura)
                {
                    int x = (int)((mousePos.X - bCoord.X) / (menuEntidades.Dimensoes.X / Informacoes.qtdEntidades.X));
                    int y = (int)((mousePos.Y - bCoord.Y) / (menuEntidades.Dimensoes.Y / Informacoes.qtdEntidades.Y));
                    posicaoEntidade = new Vector2i(x, y);
                    return menuEntidades.BAcao;
                }
            }

            // Caso nenhum botão tenha sido pressionado, retorna a acao Nenhum
            return Acao.Nenhum;
        }

        private void AtualizarValores(Acao a)
        {
            /* Atualiza os avlores das operações do menu de propriedades dependendo da operação usada */
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

            // Atualiza a origem dos textos, pois com a mudança no texto a origem deve ser alterada para mantê-la na posição correta
            AtualizarOriginLabels();
        }

        public void ResetarCorBotoes()
        {
            /* Retorna todos os botões do menu principal para a cor branca */
            foreach (Botao botao in botoes)
                botao.BSprite.Color = Color.White;
        }

        public void ResetarCorBotoesPropriedades()
        {
            /* Retorna todos os botões do menu de propriedades para a cor branca */
            foreach (Botao botao in botoesPropriedades)
                botao.BSprite.Color = Color.White;
        }

        public void AtualizarPosicaoBackground(float y)
        {
            /* Atualiza a posição do background para mantê-lo no centro caso a roda do mouse seja usada */
            Vector2f aux = new Vector2f(background.Position.X, background.Position.Y + y);
            background.Position = aux;
        }

        
        public void Desenhar()
        {
            /* Desenha todos os elementos do menu na tela */
            // Desenha o background
            window.Draw(background);

            // Desenha os botões do menu principal
            foreach (Botao botao in botoes)
                window.Draw(botao.BSprite);

            // Caso o menu de propriedades for ativado, desenha os botões do menu de propriedades e os textos
            if (menuPropriedadesAtivo)
            {
                foreach (Botao botao in botoesPropriedades)
                    window.Draw(botao.BSprite);

                foreach (Text label in labels)
                    window.Draw(label);

                // Processo que permite que um botão seja desenhado por certo período de tempo até ser apagado
                // Para mantê-lo desenhado por mais tempo que um único frame
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

            // Caso o menu de propriedades não esteja ativado, desenha o menu de entidades
            else
            {
                window.Draw(menuEntidades.BSprite);
            }
        }

        public void Redimensionar()
        {
            /* Quando a janela for redimensionada, todos os elementos são atualizados */
            RedimensionarBackground();
            AtualizarPosicaoBotoes();
            AtualizarPosicaoLabels();
        }

        private void AtualizarPosicaoBotoes()
        {
            /* Determina a nova posição dos botões */
            // Determina a nova posição dos botões do menu principal
            bNovoMundo.BSprite.Position = GetPosicaoBotao(-1, 1);
            bSalvar.BSprite.Position = GetPosicaoBotao(0, 1);
            bCarregar.BSprite.Position = GetPosicaoBotao(1, 1);
            bAdicionarObjeto.BSprite.Position = GetPosicaoBotao(-3, 2);
            bExcluirObjeto.BSprite.Position = GetPosicaoBotao(-2, 2);
            bConexao.BSprite.Position = GetPosicaoBotao(-1, 2);
            bPropriedades.BSprite.Position = GetPosicaoBotao(0, 2);
            bDialogos.BSprite.Position = GetPosicaoBotao(1, 2);
            bEventos.BSprite.Position = GetPosicaoBotao(2, 2);
            bMusicas.BSprite.Position = GetPosicaoBotao(3, 2);

            // Determina a nova posição dos botões do menu de propriedades
            bInicioAtivacaoSoma.BSprite.Position = GetPosicaoBotao(1, 4);
            bInicioAtivacaoSub.BSprite.Position = GetPosicaoBotao(-1, 4);
            bTempoAtivadoSoma.BSprite.Position = GetPosicaoBotao(1, 5);
            bTempoAtivadoSub.BSprite.Position = GetPosicaoBotao(-1, 5);
            bTempoEntreAtivacoesSoma.BSprite.Position = GetPosicaoBotao(1, 6);
            bTempoEntreAtivacoesSub.BSprite.Position = GetPosicaoBotao(-1, 6);

            // Determinar a nova posição dos botões do menu de entidades
            menuEntidades.BSprite.Position = GetPosicaoBotao(0, 5);
        }

        private void RedimensionarBackground()
        {
            /* Redimensiona o background */
            background.Size = view.Size;
            background.Position = new Vector2f(view.Center.X - (window.Size.X * 0.3f) / 2f,
                                               view.Center.Y - window.Size.Y / 2f);
        }

        private void AtualizarPosicaoLabels()
        {
            /* Determina a nova posição dos labels do menu de propriedades */
            float auxY = view.Center.Y - window.Size.Y / 2 + 3.5f * yDistancia;
            float auxX = view.Center.X;

            foreach (Text label in labels)
            {
                label.FillColor = Color.White;
                label.Position = new Vector2f(auxX, auxY);
                auxY += yDistancia / 2;
            }

            AtualizarOriginLabels();
        }

        private void AtualizarOriginLabels()
        {
            /* Atualiza o ponto de origem das labels */
            foreach (Text label in labels)
            {
                FloatRect auxVec = label.GetLocalBounds();
                label.Origin = new Vector2f(auxVec.Left + auxVec.Width / 2, auxVec.Top + auxVec.Height / 2);
            }
        }

        /* Classe aninhada para um botão */
        class Botao
        {
            // Textura de um botão
            private Texture textura;

            // Sprite de um botão
            public Sprite BSprite { get; private set; }

            // Determina a ação que um botão representa
            public Acao BAcao { get; private set; }

            // Determina as dimensões de um botão
            public Vector2f Dimensoes { get; private set; }

            public Botao(Vector2f posicao, string pathTextura, Acao acao, float proporcao)
            {
                Dimensoes = new Vector2f();

                // Tenta carregar a textura e aplicá-la em um sprite, caso não dê certo, o programa é finalizado com uma mensagem de erro
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

                // Determina a origem de um botão em seu centro
                BSprite.Origin = new Vector2f(BSprite.Texture.Size.X / 2, BSprite.Texture.Size.Y / 2);

                BAcao = acao;
                BSprite.Position = posicao;
            }
        }
    }
}
