using System;
using System.Collections.Generic;
using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class ConsoleRenderer {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public virtual int ThreadSleepDuration => 0;

        public int ConsoleWidth => this.consoleWidth;
        public int ConsoleHeight => this.consoleHeight;

        //--------------------------------------------------------------------------------
        // Constants
        //--------------------------------------------------------------------------------

        private const char EMPTY_CHAR = '.';

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private readonly Vector3 viewDirection = new Vector3(0f, 0f, 1f);

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
        }

        //--------------------------------------------------------------------------------

        private void RewriteBackground() {

            Console.Clear();
            
            for (int i = 0; i < this.consoleHeight; i++) {
                Console.WriteLine(this.emptyLine);
            }
        }

        //--------------------------------------------------------------------------------

        private void DrawObject(Drawable drawables) {

            char[][] pixels = drawables.GetPixels(this.viewDirection);

            int sizeX = pixels[0].Length;
            int sizeY = pixels.Length;

            int posX = drawables.posX - sizeX / 2;
            int posY = drawables.posY - sizeY / 2;

            for (int i = 0; i < pixels.Length; i++) {

                char[] row = pixels[i];
                string newLine = "";
                
                for (int idx = 0; idx < row.Length; idx++) {
                    newLine += row[idx];
                }

                this.RewriteLine(posY + i, posX, newLine);
            }
        }

        //--------------------------------------------------------------------------------

        private void GetConsoleSize() {

            int x = Console.WindowWidth;
            int y = Console.WindowHeight;

            if (this.consoleWidth != x) {

                this.consoleWidth = x;
                this.consoleHeight = y;

                this.emptyLine = new string(EMPTY_CHAR, this.ConsoleWidth - 1);
                this.RewriteBackground();
            }
        }

        //--------------------------------------------------------------------------------

        public void RewriteLine(int lineNumber, int xPosition, string newText) {

            int size = newText.Length;

            if (0 < lineNumber && lineNumber < this.consoleHeight && 0 < xPosition + size && xPosition < this.consoleWidth) {

                if (xPosition < 0) {

                    newText = newText.Substring(Math.Abs(xPosition));
                    xPosition = 0;
                }

                if (xPosition + size >= this.consoleWidth) {

                    int charactersToCut = xPosition + size - this.consoleWidth;
                    newText = newText.Substring(0, newText.Length - charactersToCut / 2);
                }

                Console.CursorVisible = false;
                Console.SetCursorPosition(0, lineNumber);

                string s = this.emptyLine;
                string pre = s.Substring(0, xPosition);
                string suf = s.Substring(xPosition, this.emptyLine.Length - xPosition - newText.Length);
                string tmp = pre + newText + suf;

                Console.Write(tmp);
                Console.SetCursorPosition(0, this.consoleHeight - 3);
            }
        }

        //--------------------------------------------------------------------------------
    }
}
