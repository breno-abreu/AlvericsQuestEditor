using System;
using System.Collections.Generic;
using System.Text;
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
        private LinkedList<Entidade> entidades;

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

            entidades = new LinkedList<Entidade>();

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

            if (!HaEntidadeAqui(auxX, auxY))
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

                entidades.AddLast(new Entidade(s));
            }
        }

        public void AtualizarEntidades()
        {
            foreach(Entidade entidade in entidades)
            {
                entidade.Desenhar(window);

                /*if (entidade.Excluir)
                    entidades.Remove(entidade);*/
            }
        }

        public int QuantidadeTotalEntidades()
        {
            return entidades.Count;
        }

        public bool HaEntidadeAqui(float x, float y)
        {
            foreach(Entidade entidade in entidades)
                if (entidade.ESprite.Position.X == x && entidade.ESprite.Position.Y == y) return true;
                
            return false;
        }
    }
}
