using System;
using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class Mesh {

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private readonly char[] shadingSymbols = { '.', ',', '-', '~', ':', ';', '=', '!', '*', '#', '$', '@' };
  
        public readonly Vector3[] vertices;
        public readonly int[] triangles;

        //--------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------

        public Mesh(Vector3[] vertices, int[] triangles) {

            if (triangles.Length % 3 != 0) {
                throw new Exception("Triangles array not dividable by three.");
            }

            this.vertices = vertices;
            this.triangles = triangles;
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public char[][] MeshToPixels(Vector3 viewDirection, Vector3 boundsSize, int scaleX, int scaleY) {

            char[][] pixels = new char[scaleY * (int)(boundsSize.Y + 0.75f)][];

            int sizeY = pixels.Length;

            for (int j = 0; j < sizeY; j++) {
                pixels[j] = new char[scaleX * (int)(boundsSize.X + 0.75f)];
            }

            int sizeX = pixels[0].Length;
            
            for (int i = 0; i < this.triangles.Length - 2; i += 3) {

                Vector3 vec1 = this.vertices[this.triangles[i]];
                Vector3 vec2 = this.vertices[this.triangles[i + 1]];
                Vector3 vec3 = this.vertices[this.triangles[i + 2]];

                if (this.IsNormalVisible(ref viewDirection, ref vec1, ref vec2, ref vec3)) {
                    
                    for (int y = 0; y < sizeY; y++) {

                        Vector3 v1 = new Vector3(vec1.X * scaleX, vec1.Y * scaleY, 0f);
                        Vector3 v2 = new Vector3(vec2.X * scaleX, vec2.Y * scaleY, 0f);
                        Vector3 v3 = new Vector3(vec3.X * scaleX, vec3.Y * scaleY, 0f);

                        for (int x = 0; x < sizeX; x++) {

                            if(this.PointInTriangle(new Vector3(x - sizeX / 2f, y - sizeY / 2f, 0f), ref v1, ref v2, ref v3)) {

                                Vector3 normal = this.GetNormal(ref vec1, ref vec2, ref vec3);
                                float shading = Vector3.Dot(normal, viewDirection);
                                char c = this.shadingSymbols[(int)((this.shadingSymbols.Length - 1) * shading)];

                                pixels[y][x] = c;
                            }
                        }
                    }
                }
            }

            return pixels;
        }

        //--------------------------------------------------------------------------------

        private Vector3 GetNormal(ref Vector3 vec1, ref Vector3 vec2, ref Vector3 vec3) {
            return Vector3.Normalize(Vector3.Cross(vec2 - vec1, vec3 - vec1));
        }

        //--------------------------------------------------------------------------------

        private bool IsNormalVisible(ref Vector3 viewDirection, ref Vector3 vec1, ref Vector3 vec2, ref Vector3 vec3) {

            Vector3 normal = this.GetNormal(ref vec1, ref vec2, ref vec3);
            return Vector3.Dot(normal, viewDirection) > 0f;
        }

        //--------------------------------------------------------------------------------

        private bool PointInTriangle(Vector3 point, ref Vector3 v1, ref Vector3 v2, ref Vector3 v3) {

            float d1 = this.Sign(ref point, ref v1, ref v2);
            float d2 = this.Sign(ref point, ref v2, ref v3);
            float d3 = this.Sign(ref point, ref v3, ref v1);

            bool hasNeg = (d1 < 0f) || (d2 < 0f) || (d3 < 0f);
            bool hasPos = (d1 > 0f) || (d2 > 0f) || (d3 > 0f);                     

            return !(hasNeg && hasPos);
        }

        //--------------------------------------------------------------------------------

        private float Sign(ref Vector3 p1, ref Vector3 p2, ref Vector3 p3) {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        //--------------------------------------------------------------------------------
    }
}