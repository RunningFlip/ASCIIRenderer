using System.Collections.Generic;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public interface IFrameDefinition {

        int ThreadSleepMS { get; }

        int FrameWidth { get; }
        int FrameHeight { get; }

        void OnHeightChanged(int height);
        void OnWidthChanged(int width);

        void DrawRows(List<Row> row);
    }
}