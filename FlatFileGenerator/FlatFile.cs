using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace FlatFile {
	/// <summary>
	/// Outputs a flat file of type T.
	/// </summary>
	/// <typeparam name="T">The type of the file to write, with properties that implement <see cref="FlatFileAttribute"/>.</typeparam>
	public sealed class FlatFile<T> where T : IFlatFile {
		public void WriteFile( List<T> records, string filePath ) {
			// Make sure there are records to write
			if( records.Count == 0 )
				return;
			var t = records[ 0 ].GetType( );
			var pi = t.GetProperties( );

			using( var writer = File.CreateText( filePath ) ) {
				foreach( var line in records.Select( record => createLine( record, pi ) ) ) {
					// write to file
					writer.WriteLine( line );
				}
			}
		}

		public void WriteFile( List<T> records, Stream stream ) {
			// Make sure there are records to write
			if( records.Count == 0 )
				return;
			var t = records[ 0 ].GetType( );
			var pi = t.GetProperties( );

			foreach( var lineBytes in records.Select( record => createLine( record, pi ) ).Select( stringToBytes ) ) {
				// write to file
				stream.Write( lineBytes, 0, lineBytes.Length );
			}
		}

		private static byte[ ] stringToBytes( string text ) {
			var bytes = new ASCIIEncoding( ).GetBytes( text + Environment.NewLine );
			return bytes;
		}

		private static string createLine( T record, IEnumerable<PropertyInfo> pi ) {
			var dataRow = String.Empty;

			foreach( var p in pi ) {
				var attributes = p.GetCustomAttributes( typeof( FlatFileAttribute ), false );
				FlatFileAttribute ffa = null;
				int start = 0, length = 0;

				if( attributes.Length > 0 ) {
					ffa = (FlatFileAttribute) attributes[ 0 ];
				}

				if( ffa != null ) {
					start = ffa.StartPosition;
					length = ffa.FieldLength;
				}

				var outputString = p.GetValue( record, null ).ToString( );
				if( outputString.Length < length ) {
					outputString = outputString.PadRight( length );
				} else if( outputString.Length > length ) {
					outputString = outputString.Substring( 0, length );
				}

				if( dataRow.Length + 1 == start ) {
					dataRow += outputString;
				} else if( dataRow.Length < start ) {
					// need to extend the line out
					dataRow = dataRow.PadRight( start - 1 ) + outputString;
				} else {
					// current field falls inside the middle of the existing output string.
					// split based on start position and then concatenate
					var leftSide = dataRow.Substring( 0, start - 1 );
					var rightSide = dataRow.Substring( start, dataRow.Length );
					dataRow = leftSide + outputString + rightSide;
				}
			}

			if( record.FixedLineWidth > 0 ) {
				// row has a fixed width, trim or extend to specified length
				if( dataRow.Length > record.FixedLineWidth )
					dataRow = dataRow.Substring( 0, record.FixedLineWidth );
				else if( dataRow.Length < record.FixedLineWidth )
					dataRow = dataRow.PadRight( record.FixedLineWidth );
			}

			return dataRow;
		}
	}
}
