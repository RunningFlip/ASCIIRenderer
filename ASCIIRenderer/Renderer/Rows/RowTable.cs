using System;
using System.Collections.Generic;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class RowTable {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public List<Row> Rows => this.rows;

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private List<Row> rows = new List<Row>();

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void SetRowContent(int index, int posX, ref string content, bool force = false) {
            this.rows[index].SetContent(posX, content, force);
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
                    this.rows.Add(new Row(length, RendererContants.EMPTY_CHAR));
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
