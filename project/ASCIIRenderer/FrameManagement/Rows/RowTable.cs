using ASCIIRenderer.Graphics;
using System;
using System.Collections.Generic;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.FrameManagement {

    public class RowTable<T, R> 
        where T : Row<R>, new() 
        where R : Drawable {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public int Count => this.rows.Count;

        public T this[int index] => this.rows[index];

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private List<T> rows = new List<T>();

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void SetRowContent(int index, int posX, ref string content, R drawable) {
            this.rows[index].SetContent(posX, content, drawable);
        }

        //--------------------------------------------------------------------------------

        public void ClearRows() {

            for (int i = 0; i < this.rows.Count; i++) {
                this.rows[i].Clear();
            }
        }

        //--------------------------------------------------------------------------------

        public void UpdateRowCount(int length, int count) {

            int currentCount = this.rows.Count;

            if (currentCount < count) {

                for (int i = 0; i < count - currentCount; i++) {

                    T row = new T();
                    row.SetMaxLength(length);

                    this.rows.Add(row);
                }
            }
            else {
                this.rows.RemoveRange(count, Math.Abs(count - currentCount));
            }
        }

        //--------------------------------------------------------------------------------

        public void UpdateRowLengths(int length) {

            for (int i = 0; i < this.rows.Count; i++) {
                this.rows[i].SetMaxLength(length);
            }
        }

        //--------------------------------------------------------------------------------
    }
}
