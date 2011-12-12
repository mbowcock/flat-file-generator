using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatFile
{
    [AttributeUsage(AttributeTargets.All)]
    public class FlatFileAttribute : System.Attribute
    {
        public int fieldLength { get; private set; }
        public int startPosition { get; private set; }

        public FlatFileAttribute(int startPosition, int fieldLength)
        {
            this.startPosition = startPosition;
            this.fieldLength = fieldLength;
        }
    }

}
