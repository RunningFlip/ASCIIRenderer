using System;
using System.Collections.Generic;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class ConsoleFrameDefinition : IFrameDefinition {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public int ThreadSleepMS => 0;

        public int FrameWidth => Console.WindowWidth;
        public int FrameHeight => Console.WindowHeight + 1;

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void DrawRows(List<Row> rows) {

            for (int i = 0; i < rows.Count; i++) {

                Row row = rows[i];

                if (row.HasNewContent) {

                    Console.CursorVisible = false;

                    if (i > 1) {
                        Console.SetCursorPosition(0, i - 1);
                    }

                    Console.Write(row.Content);
                }
            }

            if (this.FrameHeight > 0) {
                Console.SetCursorPosition(0, 0);
            }
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