using ASCIIRenderer.Graphics;
using System;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.FrameManagement {

    public abstract class Row<T> where T : Drawable {

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

        private int maxLength;

        private char[] content;
        
        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public Row() { }

        //--------------------------------------------------------------------------------
        // Abstract methods
        //--------------------------------------------------------------------------------

        protected abstract void OnResize(int length);
        protected abstract void OnClear();
        protected abstract void OnCharacterChanged(int position, T drawable);

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void SetMaxLength(int length) {

            if (this.maxLength != length) {

                Array.Resize(ref this.content, length);
                this.OnResize(length);
                
                if (this.maxLength > length) {

                    for (int i = this.maxLength - 1; i < length; i++) {
                        this.content[i] = RendererContants.EMPTY_CHAR;
                    }
                }

                this.maxLength = length;
            }
        }

        //--------------------------------------------------------------------------------

        public void Clear() {

            for (int i = 0; i < this.content.Length; i++) {
                this.content[i] = RendererContants.EMPTY_CHAR;
            }

            this.OnClear();

            this.HasNewContent = true;
        }

        //--------------------------------------------------------------------------------

        public void SetContent(int position, string text, T drawable) {

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

                if (drawable.ForceOverride || newChar != RendererContants.EMPTY_CHAR) {

                    this.content[i + position] = newChar;
                    this.OnCharacterChanged(i + position, drawable);
                }
            }

            this.HasNewContent = true;
        }

        //--------------------------------------------------------------------------------
    }
}