using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace AlvericsQuestEditor
{
    class GerenciadorEntidades
    {
        // Armazena uma refêrência para a janela do programa
        private RenderWindow window;

        // Armazena uma referência para a view do mundo
        private View view;

        // Lista contendo as entidades intangíveis
        public List<Entidade> EntidadesIntangiveis { get; set; }

        // Lista contendo as entidades tangíveis
        public List<Entidade> EntidadesTangiveis { get; set; }

        // Lista de armadilhas
        public List<Armadilha> Armadilhas { get; set; }

        // Lista de mecanismos
        public List<Mecanismo> Mecanismos { get; set; }

        // Lista de escadas
        public List<Escada> Escadas { get; set; }

        // Lista de NPCs
        private List<Entidade> npcs;

        // Textura contendo todas as texturas das entidades
        private Texture texturaPrincipal;

        // Sprite que recebe a textura principal
        private Sprite spritePrincipal;

        // Variável auxiliar contendo o tipo de entidade que será criada
        public Vector2i PosicaoEntidade { get; set; }

        public GerenciadorEntidades(RenderWindow window, View view)
        {
            /* Inicializa variáveis e cria as listas  */

            this.window = window;
            this.view = view;

            EntidadesIntangiveis = new List<Entidade>();
            EntidadesTangiveis = new List<Entidade>();
            Armadilhas = new List<Armadilha>();
            Mecanismos = new List<Mecanismo>();
            npcs = new List<Entidade>();
            Escadas = new List<Escada>();

            try
            {
                texturaPrincipal = new Texture(Informacoes.entidadesImgPath);
                spritePrincipal = new Sprite(texturaPrincipal);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            PosicaoEntidade = new Vector2i();
        }

        public void InserirEntidade(float x, float y, int posicaoEntidadeX=-1, int posicaoEntidadeY=-1)
        {
            /* Cria uma nova entidade dado sua posição e sua localização dentro do sprite principal */

            // Retorna as coordenadas ajustadas de uma entidade. Transforma as coordenadas recebidas do mouse para as coordenadas ajustadas no mundo
            Vector2f vec = GetPosicaoAjustada(x, y);

            TipoEntidade tipo;

            if (posicaoEntidadeX == -1) posicaoEntidadeX = PosicaoEntidade.X;
            if (posicaoEntidadeY == -1) posicaoEntidadeY = PosicaoEntidade.Y;

            // Define se a entidade é tangível ou intágivel
            if ((posicaoEntidadeX == 1 && posicaoEntidadeY == 9) ||
                (posicaoEntidadeX == 3 && posicaoEntidadeY == 9) ||
                (posicaoEntidadeX == 0 && posicaoEntidadeY == 32) ||
                (posicaoEntidadeX == 2 && posicaoEntidadeY == 0) ||
                (posicaoEntidadeX == 3 && posicaoEntidadeY == 0))
                tipo = TipoEntidade.Intangivel;
            else
                tipo = TipoEntidade.Tangivel;

            if (!HaEntidadeAqui(vec.X, vec.Y, tipo))
            {
                // Dimensões de um bloco na textura principal
                int comprimento = (int)(spritePrincipal.Texture.Size.X / Informacoes.qtdEntidades.X);
                int altura = (int)(spritePrincipal.Texture.Size.Y / Informacoes.qtdEntidades.Y);

                // Encontra e recorta na textura principal a nova textura da entidade baseada na coluna e na linha recebidas
                IntRect rect = new IntRect(posicaoEntidadeX * comprimento, posicaoEntidadeY * altura, comprimento, altura);
                spritePrincipal.TextureRect = rect;
                Sprite s = new Sprite(spritePrincipal);
                s.Origin = new Vector2f(comprimento / 2, altura / 2);
                s.Position = new Vector2f(vec.X, vec.Y);

                // Uma nova entidade será criada dependendo de qual objeto o jogador selecionou no menu
                if((posicaoEntidadeX >= 1 && posicaoEntidadeX <= 4) && posicaoEntidadeY == 4)
                {
                    Armadilha a = new Armadilha(s, tipo, TipoArmadilha.Atirador, posicaoEntidadeX, posicaoEntidadeY);
                    EntidadesTangiveis.Add(a);
                    Armadilhas.Add(a);
                }
                else if(posicaoEntidadeX == 4 && posicaoEntidadeY == 3)
                {
                    Armadilha a = new Armadilha(s, tipo, TipoArmadilha.Espinhos, posicaoEntidadeX, posicaoEntidadeY);
                    EntidadesTangiveis.Add(a);
                    Armadilhas.Add(a);
                }
                else if (posicaoEntidadeX == 0 && (posicaoEntidadeY == 4 || posicaoEntidadeY == 5))
                {
                    Mecanismo m = new Mecanismo(s, tipo, posicaoEntidadeX, posicaoEntidadeY);
                    EntidadesTangiveis.Add(m);
                    Mecanismos.Add(m);
                }
                else if (posicaoEntidadeX == 0 && posicaoEntidadeY == 1)
                {
                    Entidade e = new Entidade(s, tipo, posicaoEntidadeX, posicaoEntidadeY);
                    EntidadesTangiveis.Add(e);
                    npcs.Add(e);
                }
                else if(posicaoEntidadeY == 6 && (posicaoEntidadeX == 2 ||
                                                  posicaoEntidadeX == 3 ||
                                                  posicaoEntidadeX == 4))
                {
                    Escada e = new Escada(s, tipo, posicaoEntidadeX, posicaoEntidadeY);
                    EntidadesTangiveis.Add(e);
                    Escadas.Add(e);
                }
                else
                {
                    Entidade aux = new Entidade(s, tipo, posicaoEntidadeX, posicaoEntidadeY);

                    if (tipo == TipoEntidade.Intangivel)
                        EntidadesIntangiveis.Add(aux);
                    else
                        EntidadesTangiveis.Add(aux);
                }
            }
        }

        public void ExcluirEntidade(float x, float y)
        {
            /* Percorre todas as listas e exclui as entidades que estão na posição recebida */

            Vector2f vec = GetPosicaoAjustada(x, y);

            for (int i = EntidadesIntangiveis.Count - 1; i >= 0 && EntidadesIntangiveis.Count > 0; i--)
            {
                if(EntidadesIntangiveis[i].ESprite.Position.X == vec.X && EntidadesIntangiveis[i].ESprite.Position.Y == vec.Y)
                    EntidadesIntangiveis.Remove(EntidadesIntangiveis[i]);
            }

            for (int i = EntidadesTangiveis.Count - 1; i >= 0 && EntidadesTangiveis.Count > 0; i--)
            {
                if (EntidadesTangiveis[i].ESprite.Position.X == vec.X && EntidadesTangiveis[i].ESprite.Position.Y == vec.Y)
                    EntidadesTangiveis.Remove(EntidadesTangiveis[i]);
            }

            for (int i = Mecanismos.Count - 1; i >= 0 && Mecanismos.Count > 0; i--)
            {
                if (Mecanismos[i].ESprite.Position.X == vec.X && Mecanismos[i].ESprite.Position.Y == vec.Y)
                    Mecanismos.Remove(Mecanismos[i]);
            }

            for (int i = Armadilhas.Count - 1; i >= 0 && Armadilhas.Count > 0; i--)
            {
                if (Armadilhas[i].ESprite.Position.X == vec.X && Armadilhas[i].ESprite.Position.Y == vec.Y)
                    Armadilhas.Remove(Armadilhas[i]);
            }

            for (int i = npcs.Count - 1; i >= 0 && npcs.Count > 0; i--)
            {
                if (npcs[i].ESprite.Position.X == vec.X && npcs[i].ESprite.Position.Y == vec.Y)
                    npcs.Remove(npcs[i]);
            }

            for (int i = Escadas.Count - 1; i >= 0 && Escadas.Count > 0; i--)
            {
                if (Escadas[i].ESprite.Position.X == vec.X && Escadas[i].ESprite.Position.Y == vec.Y)
                    Escadas.Remove(Escadas[i]);
            }

        }

        public void AtualizarEntidades()
        {
            /* Desenha as entidades na tela */

            foreach (Entidade entidade in EntidadesIntangiveis)
                entidade.Desenhar(window);
            foreach (Entidade entidade in EntidadesTangiveis)
                entidade.Desenhar(window);
        }

        private bool HaEntidadeAqui(float x, float y, TipoEntidade tipo)
        {
            /* Indica se há alguma entidade naquela posição */

            // Dependendo do tipo de entidade, pode retornar que não há entidades ali
            // Entidades do mesmo tipo não podem ser sobrepostas, mas de tipos diferentes podem
            if(tipo == TipoEntidade.Intangivel)
            {
                foreach (Entidade entidade in EntidadesIntangiveis)
                {
                    if (entidade.ESprite.Position.X == x && entidade.ESprite.Position.Y == y)
                            return true;
                }
            }
            else
            {
                foreach (Entidade entidade in EntidadesTangiveis)
                {
                    if (entidade.ESprite.Position.X == x && entidade.ESprite.Position.Y == y)
                        return true;
                }
            }
            
            return false;
        }

        public void LimparListas()
        {
            /* Limpa todas as listas */

            EntidadesIntangiveis.Clear();
            EntidadesTangiveis.Clear();
            Armadilhas.Clear();
            Mecanismos.Clear();
            npcs.Clear();
            Escadas.Clear();
        }

        public Armadilha SelecionarArmadilha(float x, float y)
        {
            /* Retorna uma armadilha que se encontra em determinada posição */

            Vector2f vec = GetPosicaoAjustada(x, y);

            foreach (Armadilha armadilha in Armadilhas)
            {
                if (armadilha.ESprite.Position.X == vec.X && armadilha.ESprite.Position.Y == vec.Y)
                    return armadilha;
            }
            return null;
        }

        public Mecanismo SelecionarMecanismo(float x, float y)
        {
            /* Retorna um mecanismo que se encontra em determinada posição */

            Vector2f vec = GetPosicaoAjustada(x, y);

            foreach (Mecanismo mecanismo in Mecanismos)
            {
                if (mecanismo.ESprite.Position.X == vec.X && mecanismo.ESprite.Position.Y == vec.Y)
                    return mecanismo;
            }
            return null;
        }

        public Escada SelecionarEscada(float x, float y)
        {
            /* Retorna uma escada que se encontra em determinada posição */

            Vector2f vec = GetPosicaoAjustada(x, y);

            foreach (Escada escada in Escadas)
            {
                if (escada.ESprite.Position.X == vec.X && escada.ESprite.Position.Y == vec.Y)
                    return escada;
            }
            return null;
        }

        public bool HaNPCAqui(float x, float y)
        {
            /* Retorna 'true' se há um NPC na posição recebida */

            Vector2f vec = GetPosicaoAjustada(x, y);

            foreach(Entidade npc in npcs)
            {
                if (npc.ESprite.Position.X == vec.X && npc.ESprite.Position.Y == vec.Y)
                    return true;
            }

            return false;
        }

        public Vector2f GetPosicaoAjustada(float x, float y)
        {
            /* Ajusta uma coordenada para que uma entidade seja criada apenas em áreas que estão à 16x16 pixels de distância  */

            int auxX = x >= 0 ? 8 : -8;
            int auxY = y >= 0 ? 8 : -8;

            auxX = (int)(x / 16) * 16 + auxX;
            auxY = (int)(y / 16) * 16 + auxY;

            return new Vector2f(auxX, auxY);
        }

        public void ApagarQuadradoArmadilhas()
        {
            /* Apaga o quadrado que envolve uma armadilha */

            foreach (Armadilha armadilha in Armadilhas)
                armadilha.Selecionado = false;
        }

        public void ApagarQuadradoMecanismos()
        {
            /* Apaga o quadrado que envolve um mecanismo */

            foreach (Mecanismo mecanismo in Mecanismos)
                mecanismo.Selecionado = false;
        }
        public void ApagarQuadradoEscadas()
        {
            /* Apaga o quadrado que envolve uma escada */

            foreach (Escada escada in Escadas)
                escada.Selecionado = false;
        }

        public void ExcluirConecaoEscada(Escada e)
        {
            /* Exclui a conexão entre escadas */

            foreach(Escada escada in Escadas)
            {
                if (escada.EscadaCon == e)
                    escada.EscadaCon = null;
            }
        }
    }
}
