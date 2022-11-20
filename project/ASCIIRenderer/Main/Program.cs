using ASCIIRenderer.Definitions;
using ASCIIRenderer.FrameManagement;
using ASCIIRenderer.Graphics;
using ASCIIRenderer.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class Program {

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private static List<Vector3> directions = new List<Vector3>();
        private static List<double> rotations = new List<double>();

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        private static List<ConsoleMeshDrawable> CreateDrawables(FrameRenderer<ConsoleRow, ConsoleDrawable> renderer) {

            List<ConsoleMeshDrawable> drawables = new List<ConsoleMeshDrawable>();

            Vector3[] vertices = MeshHelpers.Torus(16, 14, 2f, 1f, out int[] triangles);
            Mesh torusMesh = new Mesh(vertices, triangles);

            int x = renderer.FrameWidth  / 8;
            int y = renderer.FrameHeight / 6;

            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Red,       x * 1, y * 1, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Yellow,    x * 1, y * 6, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Green,     x * 1, y * 10, 8, 4));
                                                  
            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Green,     x * 4, y * 1, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Yellow,    x * 4, y * 6, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Cyan,      x * 4, y * 10, 8, 4));
                                                
            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Magenta,   x * 7, y * 1, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Cyan,      x * 7, y * 6, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, ConsoleColor.Gray,      x * 7, y * 10, 8, 4));

            return drawables;
        }

        //--------------------------------------------------------------------------------

        private static ConsoleMeshDrawable CreateDrawable(Mesh mesh, ConsoleColor color, int posX, int posY, int scaleX, int scaleY) {

            Random rand = new Random(Guid.NewGuid().GetHashCode());

            double x =   rand.NextDouble() * (rand.NextDouble() >= 0.5 ? -1.0 : 1.0);
            double y =   rand.NextDouble() * (rand.NextDouble() >= 0.5 ? -1.0 : 1.0);
            double z =   rand.NextDouble() * (rand.NextDouble() >= 0.5 ? -1.0 : 1.0);
            double rot = rand.NextDouble() * (rand.NextDouble() >= 0.5 ? -1.0 : 1.0);

            directions.Add(new Vector3((float)x, (float)y, (float)z));
            rotations.Add(rot /= 10.0);

            return new ConsoleMeshDrawable(color, new Mesh(mesh), posX, posY, scaleX, scaleY);
        }

        //--------------------------------------------------------------------------------

        private static void TransformDrawables(List<ConsoleMeshDrawable> drawables) {

            for (int i = 0; i < drawables.Count; i++) {

                Vector3 rotationAxis = directions[i];
                float angle = (float) rotations[i];

                ConsoleMeshDrawable drawable = drawables[i];

                drawable.MeshTransform.Rotate(ref rotationAxis, angle);
            }
        }

        //--------------------------------------------------------------------------------

        private static void Main(string[] args) {

            Logger logger = new Logger(40, 8);
            FrameRenderer<ConsoleRow, ConsoleDrawable> renderer = new FrameRenderer<ConsoleRow, ConsoleDrawable>(new ConsoleFrameDefinition());

            List<ConsoleMeshDrawable> meshDrawables = Program.CreateDrawables(renderer);

            List<ConsoleDrawable> consoleDrawables = new List<ConsoleDrawable>(meshDrawables);
            consoleDrawables.Add(new ConsoleLoggerDrawable(ConsoleColor.White, logger));

            Stopwatch stopWatch = new Stopwatch();

            while (true) {

                DateTime before = DateTime.Now;

                stopWatch.Restart();

                TransformDrawables(meshDrawables);
                renderer.Draw(consoleDrawables);

                stopWatch.Stop();

                DateTime after = DateTime.Now;
                int ms = (after - before).Milliseconds;
                int fps = 1000 / ms;
                logger.Log("Frame: " + ms + " ms   | FPS: " + fps);

                int threadSleepDuration = renderer.ThreadSleepMS;
                
                if (threadSleepDuration != 0) {
                    Thread.Sleep(threadSleepDuration);
                }
            }
        }

        //--------------------------------------------------------------------------------
    }
}