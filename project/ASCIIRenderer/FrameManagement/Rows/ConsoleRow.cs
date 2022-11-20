using ASCIIRenderer.Graphics;
using System;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.FrameManagement {

    public class ConsoleRow : Row<ConsoleDrawable> {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public ConsoleColor[] ContentColors => this.contentColors;

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private ConsoleColor[] contentColors;

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public ConsoleRow() : base() { }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        protected override void OnClear() { }

        //--------------------------------------------------------------------------------

        protected override void OnResize(int length) {
            Array.Resize(ref this.contentColors, length);
        }

        //--------------------------------------------------------------------------------

        protected override void OnCharacterChanged(int position, ConsoleDrawable drawable) {
            this.contentColors[position] = drawable.Color;
        }

        //--------------------------------------------------------------------------------
    }
}