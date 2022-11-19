using System;
using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.Graphics {

    public class ConsoleMeshDrawable : ConsoleDrawable {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public override bool ForceOverride => false;

        public MeshTransform MeshTransform { get; private set; }

        //--------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------

        public ConsoleMeshDrawable(ConsoleMeshDrawable toCopy) : base(toCopy) {
            this.MeshTransform = new MeshTransform(toCopy.MeshTransform);
        }

        //--------------------------------------------------------------------------------

        public ConsoleMeshDrawable(ConsoleColor consoleColor, Mesh mesh, int posX = 0, int posY = 0, int scaleX = 1, int scaleY = 1)
             : base(consoleColor, posX, posY) {

            this.MeshTransform = new MeshTransform(mesh, posX, posY, scaleX, scaleY);
        }

        //--------------------------------------------------------------------------------

        public ConsoleMeshDrawable(ConsoleColor consoleColor, Mesh mesh, Quaternion rotation, int posX = 0, int posY = 0, int scaleX = 1, int scaleY = 1)
             : base(consoleColor, posX, posY) {

            this.MeshTransform = new MeshTransform(mesh, rotation, posX, posY, scaleX, scaleY);
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public override string[] GetContent(ref Vector3 viewDirection) {

            char[][] pixels = this.MeshTransform.Mesh.MeshToPixels(
                ref viewDirection,
                this.MeshTransform.BoundsSize,
                this.MeshTransform.ScaleX,
                this.MeshTransform.ScaleY);

            string[] content = new string[pixels.Length];

            for (int i = 0; i < pixels.Length; i++) {
                content[i] = new string(pixels[i]);
            }

            return content;
        }

        //--------------------------------------------------------------------------------
    }
}
