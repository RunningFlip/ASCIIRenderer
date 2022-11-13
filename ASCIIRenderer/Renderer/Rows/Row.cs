using System;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer {

    public class Row {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public string Content {

            get {

                this.HasNewContent = false;
                return new string(this.content);
            }
        }

        public bool HasNewContent { get; private set; }

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private readonly char emptyChar;

        private int maxLength;
        private char[] content;

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public Row(int length, char emptyChar) {

            this.emptyChar = emptyChar;
            this.SetMaxLength(length);
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void SetMaxLength(int length) {

            if (this.maxLength != length) {

                Array.Resize(ref this.content, length);

                if (this.maxLength > length) {

                    for (int i = this.maxLength - 1; i < length; i++) {
                        this.content[i] = this.emptyChar;
                    }
                }

                this.maxLength = length;
            }
        }

        //--------------------------------------------------------------------------------

        public void Clear() {

            for (int i = 0; i < this.content.Length; i++) {
                this.content[i] = this.emptyChar;
            }

            this.HasNewContent = true;
        }

        //--------------------------------------------------------------------------------

        public void SetContent(int position, string text) {

            if (string.IsNullOrEmpty(text)) {
                return;
            }

            if (position + text.Length > this.maxLength) {
                throw new Exception("Text is too long to match the content size!");
            }

            if (!this.HasNewContent) {
                this.Clear();
            }

            for (int i = 0; i < text.Length; i++) {

                char newChar = text[i];

                if (newChar != this.emptyChar) {
                    this.content[i + position] = newChar;              
                }
            }

            this.HasNewContent = true;
        }

        //--------------------------------------------------------------------------------
    }
}