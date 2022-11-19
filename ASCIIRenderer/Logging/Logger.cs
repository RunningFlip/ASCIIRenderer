using System;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.Logging {

    public class Logger {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public int SizeX => this.sizeX;
        public int SizeY => this.sizeY;

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private int sizeX;
        private int sizeY;

        private char[][] content;
        private string[] logs;

        //--------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------

        public Logger(int sizeX, int sizeY) {

            this.content = new char[1][];
            this.logs = new string[1];

            this.UpdateSize(sizeX, sizeY);
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public void UpdateSize(int x, int y) {

            this.sizeX = x;
            this.sizeY = y;
            this.content = new char[this.sizeY + 1][];

            this.UpdateMessagesSize(this.sizeY);
        }

        //--------------------------------------------------------------------------------

        private void UpdateMessagesSize(int count) {

            int oldLength = this.logs.Length;

            if (oldLength != count) {

                Array.Resize(ref this.logs, count);

                if (oldLength < count) {

                    for (int i = oldLength - 1; i < count; i++) {
                        this.logs[i] = string.Empty;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------

        public void Log(string message) {

            for (int i = 0; i < this.logs.Length - 1; i++) {
                this.logs[i] = this.logs[i + 1];
            }

            this.logs[this.logs.Length - 1] = message;
        }

        //--------------------------------------------------------------------------------

        public char[][] GetContent() {

            for (int i = 0; i < this.content.Length & i < this.logs.Length; i++) {

                string currentMessage = this.logs[i];

                currentMessage = this.sizeX >= currentMessage.Length
                    ? currentMessage + new string(RendererContants.EMPTY_CHAR, this.sizeX - currentMessage.Length)
                    : this.logs[i].Substring(0, this.sizeX);

                this.content[i] = (currentMessage + RendererContants.EMPTY_CHAR + RendererContants.EMPTY_CHAR + "|").ToCharArray(); 
            }

            this.content[this.content.Length - 1] = (new string('_', this.sizeX + 1) + "/").ToCharArray();

            return this.content;
        }

        //--------------------------------------------------------------------------------
    }
}