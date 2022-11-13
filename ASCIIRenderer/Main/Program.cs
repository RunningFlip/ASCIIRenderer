﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class Program {


        private static List<Vector3> directions = new List<Vector3>();
        private static List<double> rotations = new List<double>();

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        private static List<Drawable> CreateDrawables(FrameRenderer renderer) {

            List<Drawable> drawables = new List<Drawable>();

            Vector3[] vertices = MeshHelpers.Torus(16, 14, 2f, 1f, out int[] triangles);
            Mesh torusMesh = new Mesh(vertices, triangles);

            int x = renderer.FrameWidth / 8;
            int y = renderer.FrameHeight / 6;

            drawables.Add(CreateDrawable(torusMesh, x * 1, y * 1, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, x * 1, y * 6, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, x * 1, y * 10, 8, 4));
            
            drawables.Add(CreateDrawable(torusMesh, x * 4, y * 1, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, x * 4, y * 6, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, x * 4, y * 10, 8, 4));
            
            drawables.Add(CreateDrawable(torusMesh, x * 7, y * 1, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, x * 7, y * 6, 8, 4));
            drawables.Add(CreateDrawable(torusMesh, x * 7, y * 10, 8, 4));

            return drawables;
        }

        //--------------------------------------------------------------------------------

        private static Drawable CreateDrawable(Mesh mesh, int posX, int posY, int scaleX, int scaleY) {

            Random rand = new Random(Guid.NewGuid().GetHashCode());

            double x =   rand.NextDouble() * (rand.NextDouble() >= 0.5 ? -1.0 : 1.0);
            double y =   rand.NextDouble() * (rand.NextDouble() >= 0.5 ? -1.0 : 1.0);
            double z =   rand.NextDouble() * (rand.NextDouble() >= 0.5 ? -1.0 : 1.0);
            double rot = rand.NextDouble() * (rand.NextDouble() >= 0.5 ? -1.0 : 1.0);

            directions.Add(new Vector3((float)x, (float)y, (float)z));
            rotations.Add(rot /= 10.0);

            return new Drawable(new Mesh(mesh), posX, posY, scaleX, scaleY);
        }

        //--------------------------------------------------------------------------------

        private static void TransformDrawables(List<Drawable> drawables) {

            for (int i = 0; i < drawables.Count; i++) {

                Vector3 rotationAxis = directions[i];
                float angle = (float) rotations[i];

                Drawable drawable = drawables[i];

                drawable.Rotate(ref rotationAxis, angle);
            }
        }

        //--------------------------------------------------------------------------------

        private static void Main(string[] args) {

            FrameRenderer renderer = new FrameRenderer(new ConsoleFrameDefinition());

            List<Drawable> drawables = Program.CreateDrawables(renderer);

            while (true) {

                TransformDrawables(drawables);
                renderer.Draw(drawables);

                int threadSleepDuration = renderer.ThreadSleepMS;

                if (threadSleepDuration != 0) {
                    Thread.Sleep(threadSleepDuration);
                }
            }
        }

        //--------------------------------------------------------------------------------
    }
}