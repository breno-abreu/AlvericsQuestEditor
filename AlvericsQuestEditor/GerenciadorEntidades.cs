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
        private List<Entidade> entidadesIntangiveis;

        // Lista contendo as entidades tangíveis
        private List<Entidade> entidadesTangiveis;

        // Lista de armadilhas
        private List<Armadilha> armadilhas;

        // Lista de mecanismos
        private List<Mecanismo> mecanismos;

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

        public void InserirEntidade(float x, float y)
        {
            int auxX = x >= 0 ? 8 : -8;
            int auxY = y >= 0 ? 8 : -8;

            auxX = (int)(x / 16) * 16 + auxX;
            auxY = (int)(y / 16) * 16 + auxY;

            TipoEntidade tipo;

            if (PosicaoEntidade.X == 1 && PosicaoEntidade.Y == 0)
                tipo = TipoEntidade.Intangivel;
            else
                tipo = TipoEntidade.Tangivel;

            if (!HaEntidadeAqui(auxX, auxY, tipo))
            {
                // Dimensões de um bloco na textura principal
                int comprimento = (int)(spritePrincipal.Texture.Size.X / Informacoes.qtdEntidades.X);
                int altura = (int)(spritePrincipal.Texture.Size.Y / Informacoes.qtdEntidades.Y);

                // Encontra e recorta na textura principal a nova textura da entidade baseada na coluna e na linha recebidas
                IntRect rect = new IntRect(PosicaoEntidade.X * comprimento, PosicaoEntidade.Y * altura, comprimento, altura);
                spritePrincipal.TextureRect = rect;
                Sprite s = new Sprite(spritePrincipal);
                s.Origin = new Vector2f(comprimento / 2, altura / 2);

                s.Position = new Vector2f(auxX, auxY);

                Entidade aux = new Entidade(s, tipo);
                //entidades.Add(aux);

                if (tipo == TipoEntidade.Intangivel)
                    entidadesIntangiveis.Add(aux);
                else
                    entidadesTangiveis.Add(aux);

            }
        }

        public void ExcluirEntidade(float x, float y)
        {
            int auxX = x >= 0 ? 8 : -8;
            int auxY = y >= 0 ? 8 : -8;

            auxX = (int)(x / 16) * 16 + auxX;
            auxY = (int)(y / 16) * 16 + auxY;

            for(int i = entidadesIntangiveis.Count - 1; i >= 0 && entidadesIntangiveis.Count > 0; i--)
            {
                if(entidadesIntangiveis[i].ESprite.Position.X == auxX && entidadesIntangiveis[i].ESprite.Position.Y == auxY)
                    entidadesIntangiveis.Remove(entidadesIntangiveis[i]);
            }

            for (int i = entidadesTangiveis.Count - 1; i >= 0 && entidadesTangiveis.Count > 0; i--)
            {
                if (entidadesTangiveis[i].ESprite.Position.X == auxX && entidadesTangiveis[i].ESprite.Position.Y == auxY)
                    entidadesTangiveis.Remove(entidadesTangiveis[i]);
            }
        }

        public void AtualizarEntidades()
        {
            foreach(Entidade entidade in entidadesIntangiveis)
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
        }
    }
}
