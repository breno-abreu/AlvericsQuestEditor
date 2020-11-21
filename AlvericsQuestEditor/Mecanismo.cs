using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace AlvericsQuestEditor
{
    class Mecanismo : Entidade
    {
        private List<Armadilha> armadilhas;
        public Mecanismo(Sprite sprite, TipoEntidade tipo) :
            base(sprite, tipo)
        {
            armadilhas = new List<Armadilha>();
        }

        public void IncluirArmadilha(Armadilha a)
        {
            bool aux = false;
            foreach (Armadilha armadilha in armadilhas)
            {
                if (armadilha.ESprite.Position.X == a.ESprite.Position.X &&
                   armadilha.ESprite.Position.Y == a.ESprite.Position.Y)
                    aux = true;
            }

            if(!aux) armadilhas.Add(a);
        }

        public void ExcluirArmadilha(Armadilha a)
        {
            armadilhas.Remove(a);
        }

        public void Imprimir()
        {
            Console.WriteLine(armadilhas.Count);
        }
    }
}
