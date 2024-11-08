﻿using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.Graphics {

    public abstract class Drawable {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public abstract bool ForceOverride { get; }

        public int PosX => this.posX;
        public int PosY => this.posY;

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        protected int posX;
        protected int posY;

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public Drawable(Drawable toCopy) {

            this.posX = toCopy.posX;
            this.posY = toCopy.posY;
        }

        //--------------------------------------------------------------------------------

        public Drawable(int posX, int posY) {

            this.posX = posX;
            this.posY = posY;
        }

        //--------------------------------------------------------------------------------
        // Abstract methods
        //--------------------------------------------------------------------------------

        public abstract string[] GetContent(ref Vector3 viewDirection);
    }
}