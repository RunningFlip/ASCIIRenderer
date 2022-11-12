using System;
using System.Collections.Generic;
using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class ConsoleRenderer {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public virtual int ThreadSleepMS => 0;

        public int ConsoleWidth => this.consoleWidth;
        public int ConsoleHeight => this.consoleHeight;

        //--------------------------------------------------------------------------------
        // Constants
        //--------------------------------------------------------------------------------

        private const char EMPTY_CHAR = '\0';

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private readonly Vector3 viewDirection = new Vector3(0f, 0f, 1f);

        private List<Row> rows = new List<Row>();

        private int consoleWidth;
        private int consoleHeight;

        private string emptyLine;

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public ConsoleRenderer() {

            this.GetConsoleSize();
            this.RewriteBackground();
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void Draw(List<Drawable> drawables) {

            this.GetConsoleSize();
            this.FillFrame(drawables);
        }

        //--------------------------------------------------------------------------------

        private void FillFrame(List<Drawable> drawables) {

            for (int i = 0; i < drawables.Count; i++) {
                this.DrawObject(drawables[i]);
            }

            for (int i = 0; i < this.rows.Count; i++) {

                Row row = this.rows[i];

                if (row.HasNewContent) {

                    Console.CursorVisible = false;
                    Console.SetCursorPosition(0, i);

                    Console.Write(row.Content);
                    Console.SetCursorPosition(0, this.consoleHeight - 3);
                }
            }
        }

        //--------------------------------------------------------------------------------

        private void RewriteBackground() {

            if (this.consoleHeight > 0) {
                Console.Clear();
            }

            for (int i = 0; i < this.consoleHeight; i++) {
                Console.WriteLine(this.emptyLine);
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

                if (posX < 0) {

                    content = content.Substring(Math.Abs(posX));
                    posX = 0;
                }

                if (posX + contentLength >= this.consoleWidth) {

                    int charactersToCut = posX + contentLength - this.consoleWidth;

                    int length = contentLength - charactersToCut;
                    content = length > 0
                        ? content.Substring(0, length)
                        : "";
                }

                if (posY + i > 0 && posY + i < this.consoleHeight) {
                    this.rows[posY + i].SetContent(posX, content);
                }
            }
        }

        //--------------------------------------------------------------------------------

        private void GetConsoleSize() {

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            bool changed = false;

            if (this.consoleWidth != width) {

                changed = true;
                this.consoleWidth = width;
                this.emptyLine = new string(ConsoleRenderer.EMPTY_CHAR, this.ConsoleWidth - 1);
                this.UpdateRowLengths(this.consoleWidth);
            }

            if (this.consoleHeight != height) {

                changed = true;
                this.UpdateRowCount(this.consoleHeight, height);
                this.consoleHeight = height;
            }

            if (changed) {
                this.RewriteBackground();
            }
        }

        //--------------------------------------------------------------------------------

        private void UpdateRowCount(int currentRowCount, int newRowCount) {

            if (currentRowCount < newRowCount) {

                for (int i = 0; i < newRowCount - currentRowCount; i++) {
                    this.rows.Add(new Row(this.consoleWidth, ConsoleRenderer.EMPTY_CHAR));
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
    }
}
