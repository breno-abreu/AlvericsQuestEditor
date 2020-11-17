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
        }

        public void InserirEntidade(float x, float y, int coluna, int linha)
        {
            // Dimensões de um bloco na textura principal
            int comprimento = (int)(spritePrincipal.Texture.Size.X / Informacoes.qtdEntidades.X);
            int altura = (int)(spritePrincipal.Texture.Size.Y / Informacoes.qtdEntidades.Y);

            // Encontra e recorta na textura principal a nova textura da entidade baseada na coluna e na linha recebidas
            IntRect rect = new IntRect(coluna * comprimento, linha * altura, comprimento, altura);
            spritePrincipal.TextureRect = rect;
            Sprite s = new Sprite(spritePrincipal);
            s.Position = new Vector2f(x, y);

            entidades.AddLast(new Entidade(s));
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
    }
}
