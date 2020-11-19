using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace AlvericsQuestEditor
{
    class Armadilha : Entidade
    {
        public float TempoInicial { get; set; }
        public float TempoEntreAtivacoes { get; set; }
        public float TempoAtivo { get; set; }
        public Armadilha(Sprite sprite, TipoEntidade tipo, TipoArmadilha tipoArmadilha):
            base(sprite, tipo)
        {
            TempoInicial = 0;
            TempoEntreAtivacoes = 0;

            if (tipoArmadilha == TipoArmadilha.Espinhos)
                TempoAtivo = 0;
            else
                TempoAtivo = -1;
        }
    }
}
