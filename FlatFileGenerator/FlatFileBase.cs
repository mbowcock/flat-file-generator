namespace FlatFile {
	/// <summary>
	/// Abstract class for flat file definitions so that you don't have to implement FixedLineWidth where it's not used.
	/// </summary>
	public abstract class FlatFileBase : IFlatFile {
		/// <summary>
		/// Defines the line length, when it is a fixed length. Should only be necessary when there's not a field at the very end of the line.
		/// </summary>
		/// <remarks>When zero (0), this setting should be ignored.</remarks>
		public virtual int FixedLineWidth { get { return 0; } }
	}
}
