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

        // Lista contendo todas as entidades presentes no mundo
        private List<Entidade> entidades;

        // Lista contendo as entidades intangíveis
        public List<Entidade> entidadesIntangiveis { get; set; }

        // Lista contendo as entidades tangíveis
        public List<Entidade> entidadesTangiveis { get; set; }

        // Lista de armadilhas
        public List<Armadilha> armadilhas { get; set; }

        // Lista de mecanismos
        public List<Mecanismo> mecanismos { get; set; }

        // Lista de escadas
        public List<Escada> escadas { get; set; }

        // Lista de NPCs
        private List<Entidade> npcs;

        // Textura contendo todas as texturas das entidades
        private Texture texturaPrincipal;

        // Sprite que recebe a textura principal
        private Sprite spritePrincipal;

        // Variável auxiliar contendo o tipo de entidade que será criada
        public Vector2i PosicaoEntidade { get; set; }

        /*// Indicador do atributo identificador de cada entidade, será incrementada a cada nova entidade criada,
        // Fazendo assim com que cada entidade tenha um identificador único
        private int idAux;*/

        public GerenciadorEntidades(RenderWindow window, View view)
        {
            this.window = window;
            this.view = view;

            entidades = new List<Entidade>();
            entidadesIntangiveis = new List<Entidade>();
            entidadesTangiveis = new List<Entidade>();
            armadilhas = new List<Armadilha>();
            mecanismos = new List<Mecanismo>();
            npcs = new List<Entidade>();
            escadas = new List<Escada>();

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
            Vector2f vec = GetPosicaoAjustada(x, y);

            TipoEntidade tipo;

            if (posicaoEntidadeX == -1) posicaoEntidadeX = PosicaoEntidade.X;
            if (posicaoEntidadeY == -1) posicaoEntidadeY = PosicaoEntidade.Y;

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

                //entidade aux = new Entidade(s, tipo);
                //entidades.Add(aux);

                if((posicaoEntidadeX >= 1 && posicaoEntidadeX <= 4) && posicaoEntidadeY == 4)
                {
                    Armadilha a = new Armadilha(s, tipo, TipoArmadilha.Atirador, posicaoEntidadeX, posicaoEntidadeY);
                    entidadesTangiveis.Add(a);
                    armadilhas.Add(a);
                }
                else if(posicaoEntidadeX == 4 && posicaoEntidadeY == 3)
                {
                    Armadilha a = new Armadilha(s, tipo, TipoArmadilha.Espinhos, posicaoEntidadeX, posicaoEntidadeY);
                    entidadesTangiveis.Add(a);
                    armadilhas.Add(a);
                }
                else if (posicaoEntidadeX == 0 && (posicaoEntidadeY == 4 || posicaoEntidadeY == 5))
                {
                    Mecanismo m = new Mecanismo(s, tipo, posicaoEntidadeX, posicaoEntidadeY);
                    entidadesTangiveis.Add(m);
                    mecanismos.Add(m);
                }
                else if (posicaoEntidadeX == 0 && posicaoEntidadeY == 1)
                {
                    Entidade e = new Entidade(s, tipo, posicaoEntidadeX, posicaoEntidadeY);
                    entidadesTangiveis.Add(e);
                    npcs.Add(e);
                }
                else if(posicaoEntidadeY == 6 && (posicaoEntidadeX == 2 ||
                                                  posicaoEntidadeX == 3 ||
                                                  posicaoEntidadeX == 4))
                {
                    Escada e = new Escada(s, tipo, posicaoEntidadeX, posicaoEntidadeY);
                    entidadesTangiveis.Add(e);
                    escadas.Add(e);
                }
                else
                {
                    Entidade aux = new Entidade(s, tipo, posicaoEntidadeX, posicaoEntidadeY);

                    if (tipo == TipoEntidade.Intangivel)
                        entidadesIntangiveis.Add(aux);
                    else
                        entidadesTangiveis.Add(aux);
                }
            }
        }

        public void ExcluirEntidade(float x, float y)
        {
            Vector2f vec = GetPosicaoAjustada(x, y);

            for (int i = entidadesIntangiveis.Count - 1; i >= 0 && entidadesIntangiveis.Count > 0; i--)
            {
                if(entidadesIntangiveis[i].ESprite.Position.X == vec.X && entidadesIntangiveis[i].ESprite.Position.Y == vec.Y)
                    entidadesIntangiveis.Remove(entidadesIntangiveis[i]);
            }

            for (int i = entidadesTangiveis.Count - 1; i >= 0 && entidadesTangiveis.Count > 0; i--)
            {
                if (entidadesTangiveis[i].ESprite.Position.X == vec.X && entidadesTangiveis[i].ESprite.Position.Y == vec.Y)
                    entidadesTangiveis.Remove(entidadesTangiveis[i]);
            }

            for (int i = mecanismos.Count - 1; i >= 0 && mecanismos.Count > 0; i--)
            {
                if (mecanismos[i].ESprite.Position.X == vec.X && mecanismos[i].ESprite.Position.Y == vec.Y)
                    mecanismos.Remove(mecanismos[i]);
            }

            for (int i = armadilhas.Count - 1; i >= 0 && armadilhas.Count > 0; i--)
            {
                if (armadilhas[i].ESprite.Position.X == vec.X && armadilhas[i].ESprite.Position.Y == vec.Y)
                    armadilhas.Remove(armadilhas[i]);
            }

            for (int i = npcs.Count - 1; i >= 0 && npcs.Count > 0; i--)
            {
                if (npcs[i].ESprite.Position.X == vec.X && npcs[i].ESprite.Position.Y == vec.Y)
                    npcs.Remove(npcs[i]);
            }

            for (int i = escadas.Count - 1; i >= 0 && escadas.Count > 0; i--)
            {
                if (escadas[i].ESprite.Position.X == vec.X && escadas[i].ESprite.Position.Y == vec.Y)
                    escadas.Remove(escadas[i]);
            }

        }

        public void AtualizarEntidades()
        {
            foreach (Entidade entidade in entidadesIntangiveis)
                entidade.Desenhar(window);
            foreach (Entidade entidade in entidadesTangiveis)
                entidade.Desenhar(window);
        }

        public int QuantidadeTotalEntidades()
        {
            return entidades.Count;
        }

        private bool HaEntidadeAqui(float x, float y, TipoEntidade tipo)
        {
            if(tipo == TipoEntidade.Intangivel)
            {
                foreach (Entidade entidade in entidadesIntangiveis)
                {
                    if (entidade.ESprite.Position.X == x && entidade.ESprite.Position.Y == y)
                            return true;
                }
            }
            else
            {
                foreach (Entidade entidade in entidadesTangiveis)
                {
                    if (entidade.ESprite.Position.X == x && entidade.ESprite.Position.Y == y)
                        return true;
                }
            }
            
            return false;
        }

        public void LimparListas()
        {
            //entidades.Clear();
            entidadesIntangiveis.Clear();
            entidadesTangiveis.Clear();
            armadilhas.Clear();
            mecanismos.Clear();
            npcs.Clear();
            escadas.Clear();
        }

        public Armadilha SelecionarArmadilha(float x, float y)
        {
            Vector2f vec = GetPosicaoAjustada(x, y);

            foreach (Armadilha armadilha in armadilhas)
            {
                if (armadilha.ESprite.Position.X == vec.X && armadilha.ESprite.Position.Y == vec.Y)
                    return armadilha;
            }
            return null;
        }

        public Mecanismo SelecionarMecanismo(float x, float y)
        {
            Vector2f vec = GetPosicaoAjustada(x, y);

            foreach (Mecanismo mecanismo in mecanismos)
            {
                if (mecanismo.ESprite.Position.X == vec.X && mecanismo.ESprite.Position.Y == vec.Y)
                    return mecanismo;
            }
            return null;
        }

        public Escada SelecionarEscada(float x, float y)
        {
            Vector2f vec = GetPosicaoAjustada(x, y);

            foreach (Escada escada in escadas)
            {
                if (escada.ESprite.Position.X == vec.X && escada.ESprite.Position.Y == vec.Y)
                    return escada;
            }
            return null;
        }

        public bool HaNPCAqui(float x, float y)
        {
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
            int auxX = x >= 0 ? 8 : -8;
            int auxY = y >= 0 ? 8 : -8;

            auxX = (int)(x / 16) * 16 + auxX;
            auxY = (int)(y / 16) * 16 + auxY;

            return new Vector2f(auxX, auxY);
        }

        public void ApagarQuadradoArmadilhas()
        {
            foreach (Armadilha armadilha in armadilhas)
                armadilha.Selecionado = false;
        }

        public void ApagarQuadradoMecanismos()
        {
            foreach (Mecanismo mecanismo in mecanismos)
                mecanismo.Selecionado = false;
        }
        public void ApagarQuadradoEscadas()
        {
            foreach (Escada escada in escadas)
                escada.Selecionado = false;
        }

        public void ExcluirConecaoEscada(Escada e)
        {
            foreach(Escada escada in escadas)
            {
                if (escada.EscadaConn == e)
                    escada.EscadaConn = null;
            }
        }
    }
}
