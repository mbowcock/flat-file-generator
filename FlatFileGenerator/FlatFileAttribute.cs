using System;

namespace FlatFile
{
	/// <summary>
	/// Attribute for defining a field in a flat file.
	/// </summary>
	[AttributeUsage( AttributeTargets.All )]
	public sealed class FlatFileAttribute : Attribute {
		public int FieldLength { get; private set; }
		public int StartPosition { get; private set; }

		public FlatFileAttribute( int startPosition, int fieldLength ) {
			this.StartPosition = startPosition;
			this.FieldLength = fieldLength;
		}
	}
}
