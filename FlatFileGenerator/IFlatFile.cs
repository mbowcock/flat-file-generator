namespace FlatFile {
	public interface IFlatFile {
		/// <summary>
		/// Defines the line length, when it is a fixed length. Should only be necessary when there's not a field at the very end of the line.
		/// </summary>
		/// <remarks>When zero (0), this setting should be ignored.</remarks>
		int FixedLineWidth { get; }
	}
}
