using ASCIIRenderer.Definitions;
using ASCIIRenderer.FrameManagement;
using ASCIIRenderer.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class FrameRenderer<T, R> 
        where T : Row<R>, new() 
        where R : Drawable {

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

        private RowTable<T, R> rowTable;
        private IFrameDefinition<T, R> frameDefinition;

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public FrameRenderer(IFrameDefinition<T, R> frameDefinition) {

            this.rowTable = new RowTable<T, R>();
            this.frameDefinition = frameDefinition;

            this.UpdateFrameSize();
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void Draw(List<R> drawables) {

            this.rowTable.ClearRows();

            this.UpdateFrameSize();
            this.UpdateContent(drawables);

            this.frameDefinition.DrawRows(this.rowTable);
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

        private void UpdateContent(List<R> drawables) {

            for (int i = 0; i < drawables.Count; i++) {
                this.DrawableToRows(drawables[i]);
            }
        }

        //--------------------------------------------------------------------------------

        private void DrawableToRows(R drawable) {

            char[][] pixels = drawable.GetPixels(ref this.viewDirection);

            int sizeX = pixels[0].Length;
            int sizeY = pixels.Length;

            int posX = drawable.PosX - sizeX / 2;
            int posY = drawable.PosY - sizeY / 2;

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

                if (0 <= posY + i && posY + i < this.frameHeight) {
                    this.rowTable.SetRowContent(posY + i, correctedPosX, ref content, drawable);
                }
            }
        }

        //--------------------------------------------------------------------------------

        //private void LoggerToRows() {
        //
        //    string[] content = this.logger.GetContent();
        //
        //    for (int i = 0; i < content.Length; i++) {
        //
        //        if (i >= this.frameHeight) {
        //            return;
        //        }
        //
        //        string c = content[i];
        //
        //        if (string.IsNullOrEmpty(c)) {
        //            continue;
        //        }
        //
        //        int contentLength = c.Length;
        //
        //        if (contentLength >= this.frameWidth) {
        //
        //            int charactersToCut = contentLength - this.frameWidth;
        //
        //            int length = contentLength - charactersToCut;
        //            c = 0 < length && length < contentLength
        //                ? c.Substring(0, length)
        //                : "";
        //        }
        //
        //        this.rowTable.SetRowContent(i, 0, ref c, ConsoleColor.White, true);
        //    }
        //}

        //--------------------------------------------------------------------------------
    }
}
