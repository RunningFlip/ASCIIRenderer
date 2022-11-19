using ASCIIRenderer.FrameManagement;
using ASCIIRenderer.Graphics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.Definitions {

    public interface IFrameDefinition<T, R> 
        where T : Row<R>, new() 
        where R : Drawable {

        int ThreadSleepMS { get; }

        int FrameWidth { get; }
        int FrameHeight { get; }

        void OnHeightChanged(int height);
        void OnWidthChanged(int width);

        void DrawRows(RowTable<T, R> rowTable);
    }
}