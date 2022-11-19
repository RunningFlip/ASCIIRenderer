using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.Graphics {

    public class MeshTransform {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        // Bounds
        public Vector3 BoundingBoxMin => this.boundingMin;
        public Vector3 BoundingBoxMax => this.boundingMax;
        public Vector3 BoundsSize => Vector3.Abs(this.boundingMax - this.boundingMin);

        // Scale
        public int PosX { get; set; }
        public int PosY { get; set; }

        // Scale
        public int ScaleX { get; set; }
        public int ScaleY { get; set; }

        // Rotation
        //public Quaternion Rotation => this.rotation;

        // Mesh
        public Mesh Mesh => this.mesh;

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        // Bounds
        private readonly Vector3 boundsMinDefault = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        private readonly Vector3 boundsMaxDefault = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        private Vector3 boundingMin;
        private Vector3 boundingMax;

        // Rotation
        private Quaternion rotation;

        // Mesh
        private Mesh mesh;

        //--------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------

        public MeshTransform(MeshTransform toCopy) { 
            this.Init(toCopy.mesh, toCopy.rotation, toCopy.PosX, toCopy.PosY, toCopy.ScaleX, toCopy.ScaleY);
        }

        //--------------------------------------------------------------------------------

        public MeshTransform(Mesh mesh, int posX = 0, int posY = 0, int scaleX = 1, int scaleY = 1) {
            this.Init(mesh, Quaternion.Identity, posX, posY, scaleX, scaleY);
        }

        //--------------------------------------------------------------------------------

        public MeshTransform(Mesh mesh, Quaternion rotation, int posX = 0, int posY = 0, int scaleX = 1, int scaleY = 1) { 
            this.Init(mesh, rotation, posX, posY, scaleX, scaleY);
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        private void Init(Mesh mesh, Quaternion rotation, int posX, int posY, int scaleX, int scaleY) {

            this.mesh = mesh;
            this.rotation = rotation;

            this.PosX = posX;
            this.PosY = posY;

            this.ScaleX = scaleX;
            this.ScaleY = scaleY;

            this.SetupBoundingBox();
        }

        //--------------------------------------------------------------------------------

        public void Rotate(ref Vector3 rotationAxis, float angle) {

            this.boundingMin = this.boundsMinDefault;
            this.boundingMax = this.boundsMaxDefault;

            Vector3 rotationVec = Vector3.Normalize(rotationAxis);
            Quaternion newRot = Quaternion.CreateFromAxisAngle(rotationVec, angle);

            for (int i = 0; i < this.mesh.triangles.Length - 2; i += 3) {

                int t1 = this.mesh.triangles[i];
                int t2 = this.mesh.triangles[i + 1];
                int t3 = this.mesh.triangles[i + 2];

                Vector3 vec1 = this.mesh.vertices[t1] = Vector3.Transform(this.mesh.vertices[t1], newRot);
                Vector3 vec2 = this.mesh.vertices[t2] = Vector3.Transform(this.mesh.vertices[t2], newRot);
                Vector3 vec3 = this.mesh.vertices[t3] = Vector3.Transform(this.mesh.vertices[t3], newRot);

                this.UpdateBoundsByVertex(ref vec1);
                this.UpdateBoundsByVertex(ref vec2);
                this.UpdateBoundsByVertex(ref vec3);
            }
        }

        //--------------------------------------------------------------------------------

        private void SetupBoundingBox() {

            this.boundingMin = this.boundsMinDefault;
            this.boundingMax = this.boundsMaxDefault;

            for (int i = 0; i < this.mesh.triangles.Length - 2; i += 3) {

                Vector3 vec1 = this.mesh.vertices[this.mesh.triangles[i]];
                Vector3 vec2 = this.mesh.vertices[this.mesh.triangles[i + 1]];
                Vector3 vec3 = this.mesh.vertices[this.mesh.triangles[i + 2]];

                this.UpdateBoundsByVertex(ref vec1);
                this.UpdateBoundsByVertex(ref vec2);
                this.UpdateBoundsByVertex(ref vec3);
            }
        }

        //--------------------------------------------------------------------------------

        private void UpdateBoundsByVertex(ref Vector3 vec) {

            if (vec.X > this.boundingMax.X) {
                this.boundingMax.X = vec.X;
            }
            if (vec.Y > this.boundingMax.Y) {
                this.boundingMax.Y = vec.Y;
            }
            if (vec.Z > this.boundingMax.Z) {
                this.boundingMax.Z = vec.Z;
            }
            if (vec.X < this.boundingMin.X) {
                this.boundingMin.X = vec.X;
            }
            if (vec.Y < this.boundingMin.Y) {
                this.boundingMin.Y = vec.Y;
            }
            if (vec.Z < this.boundingMin.Z) {
                this.boundingMin.Z = vec.Z;
            }
        }

        //--------------------------------------------------------------------------------
    }
}