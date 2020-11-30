using SFML.System;

namespace AlvericsQuestEditor
{
    class Dialogo
    {
        /* Classe que representa as linhas de diálogo pertencentes à um NPC */

        // Localização do NPC no mundo
        public Vector2f PosNPC { get; private set; }

        // Linhas de diálogo de um NPC
        public string Texto { get; set; }

        public Dialogo(Vector2f posNPC, string texto)
        {
            /* Inicializa um diálogo relacionando um NPC com suas linhas de diálogo */

            this.PosNPC = posNPC;
            this.Texto = texto;
        }
    }
}
