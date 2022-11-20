using System;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.Graphics {

    public abstract class ConsoleDrawable : Drawable {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public ConsoleColor Color { get; protected set; }

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public ConsoleDrawable(ConsoleDrawable toCopy) : base(toCopy) {
            this.Color = toCopy.Color;
        }

        //--------------------------------------------------------------------------------

        public ConsoleDrawable(ConsoleColor consoleColor, int posX, int posY)
            : base(posX, posY) {

            this.Color = consoleColor;
        }

        //--------------------------------------------------------------------------------
    }
}