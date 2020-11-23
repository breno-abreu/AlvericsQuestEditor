using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

namespace AlvericsQuestEditor
{
    class Dialogo
    {
        public Vector2f posNPC { get; private set; }
        public string texto { get; set; }

        public Dialogo(Vector2f posNPC, string texto)
        {
            this.posNPC = posNPC;
            this.texto = texto;
        }
    }
}
