using System;
using System.Collections.Generic;
using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class FrameRenderer {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public int ThreadSleepMS => this.frameDefinition.ThreadSleepMS;

        public int FrameWidth => this.frameWidth;
        public int FrameHeight => this.frameHeight;

        //--------------------------------------------------------------------------------
        // Constants
        //--------------------------------------------------------------------------------

        private const char EMPTY_CHAR = '\0';

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private readonly Vector3 viewDirection = new Vector3(0f, 0f, 1f);

        private List<Row> rows = new List<Row>();

        private int frameWidth;
        private int frameHeight;

        private IFrameDefinition frameDefinition;

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public FrameRenderer(IFrameDefinition frameDefinition) {

            this.frameDefinition = frameDefinition;

            this.UpdateFrameSize();
            this.ClearRows();
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void Draw(List<Drawable> drawables) {

            this.UpdateFrameSize();
            this.FillRows(drawables);

            this.frameDefinition.DrawRows(this.rows);
        }

        //--------------------------------------------------------------------------------

        private void UpdateFrameSize() {

            int width = this.frameDefinition.FrameWidth;
            int height = this.frameDefinition.FrameHeight;

            bool changed = false;

            if (this.frameWidth != width) {

                changed = true;
                this.frameWidth = width;
                this.frameDefinition.OnHeightChanged(this.frameWidth);
                this.UpdateRowLengths(this.frameWidth);
            }

            if (this.frameHeight != height) {

                changed = true;
                this.UpdateRowCount(this.frameHeight, height);
                this.frameHeight = height;
                this.frameDefinition.OnWidthChanged(this.frameHeight);         
            }

            if (changed) {
                this.ClearRows();
            }
        }

        //--------------------------------------------------------------------------------

        private void FillRows(List<Drawable> drawables) {

            for (int i = 0; i < drawables.Count; i++) {
                this.DrawObject(drawables[i]);
            }
        }

        //--------------------------------------------------------------------------------

        private void ClearRows() {

            for (int i = 0; i < this.rows.Count; i++) {
                this.rows[i].Clear();
            }
        }

        //--------------------------------------------------------------------------------

        private void UpdateRowCount(int currentRowCount, int newRowCount) {

            if (currentRowCount < newRowCount) {

                for (int i = 0; i < newRowCount - currentRowCount; i++) {
                    this.rows.Add(new Row(this.frameWidth, FrameRenderer.EMPTY_CHAR));
                }
            }
            else {
                this.rows.RemoveRange(newRowCount, Math.Abs(newRowCount - currentRowCount));
            }
        }

        //--------------------------------------------------------------------------------

        private void UpdateRowLengths(int length) {

            for (int i = 0; i < this.rows.Count; i++) {
                this.rows[i].SetMaxLength(length);
            }
        }

        //--------------------------------------------------------------------------------

        private void DrawObject(Drawable drawables) {

            char[][] pixels = drawables.GetPixels(this.viewDirection);

            int sizeX = pixels[0].Length;
            int sizeY = pixels.Length;

            int posX = drawables.PosX - sizeX / 2;
            int posY = drawables.PosY - sizeY / 2;

            for (int i = 0; i < pixels.Length; i++) {

                char[] row = pixels[i];
                string content = "";

                for (int idx = 0; idx < row.Length; idx++) {
                    content += row[idx];
                }

                int contentLength = content.Length;
                int correctedPosX = posX;

                if (correctedPosX < 0) {

                    content = content.Substring(Math.Abs(correctedPosX));
                    contentLength = content.Length;
                    correctedPosX = 0;
                }

                if (correctedPosX + contentLength >= this.frameWidth) {

                    int charactersToCut = correctedPosX + contentLength - this.frameWidth;

                    int length = contentLength - charactersToCut;
                    content = 0 < length && length < contentLength
                        ? content.Substring(0, length)
                        : "";
                }

                if (posY + i > 0 && posY + i < this.frameHeight) {
                    this.rows[posY + i].SetContent(correctedPosX, content);
                }
            }
        }

        //--------------------------------------------------------------------------------
    }
}
