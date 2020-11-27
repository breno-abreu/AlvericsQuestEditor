using SFML.System;

namespace AlvericsQuestEditor
{
    class Dialogo
    {
        public Vector2f PosNPC { get; private set; }
        public string Texto { get; set; }

        public Dialogo(Vector2f posNPC, string texto)
        {
            this.PosNPC = posNPC;
            this.Texto = texto;
        }
    }
}
