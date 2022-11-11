using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class Program {

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        private static List<Drawable> CreateDrawables(ConsoleRenderer renderer) {

            List<Drawable> drawables = new List<Drawable>();

            Vector3[] vertices = MeshHelpers.Torus(12, 8, 2f, 1f, out int[] triangles);
            Mesh torusMesh = new Mesh(vertices, triangles);

            int x = renderer.ConsoleWidth / 8;

            drawables.Add(new Drawable(torusMesh, x * 2, renderer.ConsoleHeight / 2, 8, 4));
            drawables.Add(new Drawable(torusMesh, x * 6, renderer.ConsoleHeight / 2, 8, 4));

            return drawables;
        }

        //--------------------------------------------------------------------------------

        private static void TransformDrawables(List<Drawable> drawables) {

            for (int i = 0; i < drawables.Count; i++) {

                Vector3 rotationAxis = new Vector3(1f, 0.5f, 0f);
                float angle = 0.01f;

                Drawable drawable = drawables[i];
                drawable.scaleX = 12;
                drawable.scaleY = 6;

                drawable.Rotate(ref rotationAxis, angle);
            }
        }

        //--------------------------------------------------------------------------------

        private static void Main(string[] args) {

            ConsoleRenderer renderer = new ConsoleRenderer();

            List<Drawable> drawables = Program.CreateDrawables(renderer);

            while (true) {

                TransformDrawables(drawables);

                renderer.Draw(drawables);

                int threadSleepDuration = renderer.ThreadSleepDuration;

                if (threadSleepDuration != 0) {
                    Thread.Sleep(threadSleepDuration);
                }
            }
        }

        //--------------------------------------------------------------------------------
    }
}