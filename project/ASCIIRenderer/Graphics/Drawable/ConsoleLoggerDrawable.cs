using ASCIIRenderer.Logging;
using System;
using System.Numerics;

//--------------------------------------------------------------------------------

namespace ASCIIRenderer.Graphics {

    public class ConsoleLoggerDrawable : ConsoleDrawable {

        //--------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------

        public override bool ForceOverride => true;

        public Logger Logger => this.logger;

        //--------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------

        private readonly Logger logger;

        //--------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------

        public ConsoleLoggerDrawable(ConsoleLoggerDrawable toCopy) : base(toCopy) {
            this.logger = toCopy.logger;
        }

        //--------------------------------------------------------------------------------

        public ConsoleLoggerDrawable(ConsoleColor consoleColor, Logger logger) 
            : base(consoleColor, ((int)((logger.SizeX / 2f) + 0.5f)), ((int)((logger.SizeY / 2f) + 0.5f))) {

            this.logger = logger;
        }

        //--------------------------------------------------------------------------------

        public ConsoleLoggerDrawable(ConsoleColor consoleColor, int scaleX, int scaleY)  
            : base(consoleColor, ((int)((scaleX / 2f) + 0.5f)), ((int)((scaleY / 2f) + 0.5f))) {

            this.logger = new Logger(scaleX, scaleY);
        }

        //--------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------

        public override string[] GetContent(ref Vector3 viewDirection) {
            return this.logger.GetContent();
        }
    }
}