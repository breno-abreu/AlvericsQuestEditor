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
            armadilhas.Add(a);
        }

        public void ExcluirArmadilha(Armadilha a)
        {
            armadilhas.Remove(a);
        }
    }
}
