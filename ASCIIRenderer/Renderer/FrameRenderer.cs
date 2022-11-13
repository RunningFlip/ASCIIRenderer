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
        // Fields
        //--------------------------------------------------------------------------------

        private Vector3 viewDirection = new Vector3(0f, 0f, 1f);

        private int frameWidth;
        private int frameHeight;

        private RowTable rowTable;
        private IFrameDefinition frameDefinition;

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public FrameRenderer(IFrameDefinition frameDefinition) {

            this.rowTable = new RowTable();
            this.frameDefinition = frameDefinition;

            this.UpdateFrameSize();
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void Draw(List<Drawable> drawables) {

            this.UpdateFrameSize();
            this.UpdateContent(drawables);

            this.frameDefinition.DrawRows(this.rowTable.Rows);
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
                this.rowTable.UpdateRowLengths(this.frameWidth);
            }

            if (this.frameHeight != height) {

                changed = true;
                this.frameHeight = height;
                this.rowTable.UpdateRowCount(this.frameWidth, this.frameHeight);
                this.frameDefinition.OnWidthChanged(this.frameHeight);         
            }

            if (changed) {
                this.rowTable.ClearRows();
            }
        }

        //--------------------------------------------------------------------------------

        private void UpdateContent(List<Drawable> drawables) {

            for (int i = 0; i < drawables.Count; i++) {
                this.DrawableToRows(drawables[i]);
            }
        }

        //--------------------------------------------------------------------------------

        private void DrawableToRows(Drawable drawables) {

            char[][] pixels = drawables.GetPixels(ref this.viewDirection);

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
                    this.rowTable.SetRowContent(posY + i, correctedPosX, ref content);
                }
            }
        }

        //--------------------------------------------------------------------------------
    }
}
