using System;
using System.Collections.Generic;
using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public static class MeshHelpers {

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public static Vector3[] Torus(int segments, int tubes, float segmentRadius, float tubeRadius, out int[] triangles) {

            // Init vertexList and indexList
            List<Vector3> verticesList = new List<Vector3>();
            List<int> indicesList = new List<int>();

            // Save these locally as floats
            float numSegments = segments;
            float numTubes = tubes;

            // Calculate size of segment and tube
            float segmentSize = 2f * (float)Math.PI / numSegments;
            float tubeSize = 2f * (float)Math.PI / numTubes;

            // Create floats for our xyz coordinates
            float x;
            float y;
            float z;

            // Init temp lists with tubes and segments
            List<List<Vector3>> segmentList = new List<List<Vector3>>();

            // Loop through number of tubes
            for (int i = 0; i < numSegments; i++) {
                List<Vector3> tubeList = new List<Vector3>();

                for (int j = 0; j < numTubes; j++) {

                    // Calculate X, Y, Z coordinates.
                    x = (segmentRadius + tubeRadius * (float)Math.Cos(j * tubeSize)) * (float)Math.Cos(i * segmentSize);
                    y = (segmentRadius + tubeRadius * (float)Math.Cos(j * tubeSize)) * (float)Math.Sin(i * segmentSize);
                    z = tubeRadius * (float)Math.Sin(j * tubeSize);

                    // Add the vertex to the tubeList
                    tubeList.Add(new Vector3(x, z, y));

                    // Add the vertex to global vertex list
                    verticesList.Add(new Vector3(x, z, y));
                }

                // Add the filled tubeList to the segmentList
                segmentList.Add(tubeList);
            }

            // Loop through the segments
            for (int i = 0; i < segmentList.Count; i++) {
                // Find next (or first) segment offset
                int n = (i + 1) % segmentList.Count;

                // Find current and next segments
                List<Vector3> currentTube = segmentList[i];
                List<Vector3> nextTube = segmentList[n];

                // Loop through the vertices in the tube
                for (int j = 0; j < currentTube.Count; j++) {
                    // Find next (or first) vertex offset
                    int m = (j + 1) % currentTube.Count;

                    // Find the 4 vertices that make up a quad
                    Vector3 v1 = currentTube[j];
                    Vector3 v2 = currentTube[m];
                    Vector3 v3 = nextTube[m];
                    Vector3 v4 = nextTube[j];

                    // Draw the first triangle
                    indicesList.Add(verticesList.IndexOf(v1));
                    indicesList.Add(verticesList.IndexOf(v2));
                    indicesList.Add(verticesList.IndexOf(v3));

                    // Finish the qu
                    indicesList.Add(verticesList.IndexOf(v3));
                    indicesList.Add(verticesList.IndexOf(v4));
                    indicesList.Add(verticesList.IndexOf(v1));
                }
            }

            triangles = indicesList.ToArray();
            return verticesList.ToArray();
        }

        //--------------------------------------------------------------------------------

        public static Vector3[] GenerateSphere(float radius, int latitudes, int longitudes) {

            List<Vector3> vertices = new List<Vector3>();

            float latitude_increment = 360.0f / latitudes;
            float longitude_increment = 180.0f / longitudes;

            for (float u = 0; u < 360.0f; u += latitude_increment) {

                for (float t = 0; t < 180.0f; t += longitude_increment) {

                    float rad = radius;

                    double radT = MeshHelpers.ToRadians(t);
                    double radU = MeshHelpers.ToRadians(u);

                    float x = (float)(rad * Math.Sin(radT) * Math.Sin(radU));
                    float y = (float)(rad * Math.Cos(radT));
                    float z = (float)(rad * Math.Sin(radT) * Math.Cos(radU));

                    vertices.Add(new Vector3(x, y, z));

                }
            }

            return vertices.ToArray();
        }

        //--------------------------------------------------------------------------------

        private static double ToRadians(double angle) {
            return (Math.PI / 180.0) * angle;
        }

        //--------------------------------------------------------------------------------
    }
}