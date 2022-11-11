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

            Vector3[] vertices = MeshHelpers.Torus(16, 14, 2f, 1f, out int[] triangles);
            Mesh torusMesh = new Mesh(vertices, triangles);

            int x = renderer.ConsoleWidth / 8;
            int y = renderer.ConsoleHeight / 6;

            drawables.Add(new Drawable(torusMesh, x * 2, y * 2, 8, 4));
            drawables.Add(new Drawable(torusMesh, x * 6, y * 2, 8, 4));
            drawables.Add(new Drawable(torusMesh, x * 2, y * 5, 8, 4));
            drawables.Add(new Drawable(torusMesh, x * 6, y * 5, 8, 4));

            return drawables;
        }

        //--------------------------------------------------------------------------------

        private static void TransformDrawables(List<Drawable> drawables) {

            for (int i = 0; i < drawables.Count; i++) {

                Vector3 rotationAxis = new Vector3(1f, 0.5f, 0f);
                float angle = 0.01f;

                Drawable drawable = drawables[i];

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