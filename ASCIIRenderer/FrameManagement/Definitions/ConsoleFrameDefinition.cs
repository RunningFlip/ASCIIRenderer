using System;
using ASCIIRenderer.FrameManagement;
using ASCIIRenderer.Graphics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.Definitions {

    public class ConsoleFrameDefinition : IFrameDefinition<ConsoleRow, ConsoleDrawable> {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public bool UseColor { get; set } = true;

        public int ThreadSleepMS => 0;

        public int FrameWidth => Console.WindowWidth;
        public int FrameHeight => Console.WindowHeight + 1;

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void DrawRows(RowTable<ConsoleRow, ConsoleDrawable> rowTable) {

            for (int i = 0; i < rowTable.Count; i++) {

                ConsoleRow row = rowTable[i];

                if (row.HasNewContent) {

                    Console.CursorVisible = false;

                    if (i > 0) {
                        Console.SetCursorPosition(0, i);
                    }

                    if (this.UseColor) {
                        this.ColorizeRowContent(row);
                    }
                    else {
                        Console.Write(row.Content);
                    }
                }
            }

            if (this.FrameHeight > 0) {
                Console.SetCursorPosition(0, 0);
            }
        }

        //--------------------------------------------------------------------------------

        private void ColorizeRowContent(ConsoleRow row) {

            string content = row.Content;

            ConsoleColor cachedForeground = Console.ForegroundColor;
            ConsoleColor[] colors = row.ContentColors;

            int currentColorStart = 0;
            ConsoleColor color = colors[currentColorStart];

            for (int i = 0; i < colors.Length; i++) {

                ConsoleColor current = colors[i];

                if (current != color) {

                    Console.ForegroundColor = color;

                    string coloredContent = content.Substring(currentColorStart, i - currentColorStart);
                    Console.Write(coloredContent);

                    currentColorStart = i;
                    color = current;
                }
            }

            Console.ForegroundColor = cachedForeground;
        }

        //--------------------------------------------------------------------------------

        public void OnHeightChanged(int height) { }

        //--------------------------------------------------------------------------------

        public void OnWidthChanged(int width) {

            if (width > 1) {

                Console.Clear();
                Console.SetCursorPosition(0, 0);
            }
        }

        //--------------------------------------------------------------------------------
    }
}