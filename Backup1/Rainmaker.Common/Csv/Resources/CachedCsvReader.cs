using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Rainmaker.Common.Csv.Resources;

namespace Rainmaker.Common.Csv.Resources
{
	/// <summary>
	/// Represents a reader that provides fast, cached, dynamic access to CSV data.
	/// </summary>
	public class CachedCsvReader : Rainmaker.Common.Csv.CsvReader
	{
		/// <summary>
		/// Contains the cached records.
		/// </summary>
		private List<string[]> _records;

		/// <summary>
		/// Contains the current record index (inside the cached records array).
		/// </summary>
		private int _currentRecordIndex;

		/// <summary>
		/// Indicates if a new record is being read from the CSV stream.
		/// </summary>
		private bool _readingStream;

		/// <summary>
		/// Initializes a new instance of the CsvReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:ArgumentException">
		///		Cannot read from <paramref name="reader"/>.
		/// </exception>
		public CachedCsvReader(TextReader reader, bool hasHeaders)
			: this(reader, hasHeaders, DefaultBufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CsvReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <param name="bufferSize">The buffer size in bytes.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:ArgumentException">
		///		Cannot read from <paramref name="reader"/>.
		/// </exception>
		public CachedCsvReader(TextReader reader, bool hasHeaders, int bufferSize)
			: this(reader, hasHeaders, DefaultDelimiter, DefaultQuote, DefaultEscape, DefaultComment, true, bufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CsvReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
		/// <param name="quote">The quotation character wrapping every field (default is ''').</param>
		/// <param name="escape">
		/// The escape character letting insert quotation characters inside a quoted field (default is '\').
		/// If no escape character, set to '\0' to gain some performance.
		/// </param>
		/// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
		/// <param name="trimSpaces"><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>. Default is <see langword="true"/>.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:ArgumentException">
		///		Cannot read from <paramref name="reader"/>.
		/// </exception>
		public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces)
			: this(reader, hasHeaders, delimiter, quote, escape, comment, trimSpaces, DefaultBufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CsvReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
		/// <param name="quote">The quotation character wrapping every field (default is ''').</param>
		/// <param name="escape">
		/// The escape character letting insert quotation characters inside a quoted field (default is '\').
		/// If no escape character, set to '\0' to gain some performance.
		/// </param>
		/// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
		/// <param name="trimSpaces"><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>. Default is <see langword="true"/>.</param>
		/// <param name="bufferSize">The buffer size in bytes.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="bufferSize"/> must be 1 or more.
		/// </exception>
		public CachedCsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces, int bufferSize)
			: base(reader, hasHeaders, delimiter, quote, escape, comment, trimSpaces, bufferSize)
		{
			_records = new List<string[]>();
			_currentRecordIndex = -1;
			_readingStream = false;
		}

		/// <summary>
		/// Gets the current record index in the CSV file.
		/// </summary>
		/// <value>The current record index in the CSV file.</value>
		/// <exception cref="T:System.ComponentModel.ObjectDisposedException">
		///		The instance has been disposed of.
		/// </exception>
		public override int CurrentRecordIndex
		{
			get
			{
				base.CheckDisposed();

				return _currentRecordIndex;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the current stream position is at the end of the stream.
		/// </summary>
		/// <value><see langword="true"/> if the current stream position is at the end of the stream; otherwise <see langword="false"/>.</value>
		public override bool EndOfStream
		{
			get
			{
				base.CheckDisposed();

				if (_currentRecordIndex < base.CurrentRecordIndex)
					return false;
				else
					return base.EndOfStream;
			}
		}

		/// <summary>
		/// Gets the field at the specified index.
		/// </summary>
		/// <value>The field at the specified index.</value>
		/// <exception cref="T:ArgumentOutOfRangeException">
		///		<paramref name="field"/> must be included in [0, <see cref="M:FieldCount"/>[.
		/// </exception>
		/// <exception cref="T:InvalidOperationException">
		///		No record read yet. Call ReadLine() first.
		/// </exception>
		/// <exception cref="T:MalformedCsvException">
		///		The CSV appears to be corrupt at the current position.
		/// </exception>
		/// <exception cref="T:System.ComponentModel.ObjectDisposedException">
		///		The instance has been disposed of.
		/// </exception>
		public override String this[int field]
		{
			get
			{
				CheckDisposed();

				if (_readingStream)
					return base[field];
				else if (_currentRecordIndex > -1)
				{
					if (field > -1 && field < this.FieldCount)
						return _records[_currentRecordIndex][field];
					else
						throw new ArgumentOutOfRangeException("field", field, string.Format(ExceptionMessage.FieldIndexOutOfRange, field));
				}
				else
					throw new InvalidOperationException(ExceptionMessage.NoCurrentRecord);
			}
		}

		/// <summary>
		/// Reads the CSV stream from the current position to the end of the stream.
		/// </summary>
		public virtual void ReadToEnd()
		{
			CheckDisposed();

			_currentRecordIndex = base.CurrentRecordIndex;

			while (ReadNextRecord()) ;
		}

		/// <summary>
		/// Reads the next record.
		/// </summary>
		/// <returns><see langword="true"/> if a record has been successfully reads; otherwise, <see langword="false"/>.</returns>
		public override bool ReadNextRecord()
		{
			CheckDisposed();

			if (_currentRecordIndex < base.CurrentRecordIndex)
				return true;
			else
			{
				_readingStream = true;

				try
				{
					bool canRead = base.ReadNextRecord();

					if (canRead)
					{
						string[] record = new string[this.FieldCount];
						CopyCurrentRecordTo(record);

						_records.Add(record);

						_currentRecordIndex++;
					}
					else
					{
						// No more records to read, so set array size to only what is needed
						_records.Capacity = _records.Count;
					}

					return canRead;
				}
				finally
				{
					_readingStream = false;
				}
			}
		}

		/// <summary>
		/// Moves before the first record.
		/// </summary>
		public void MoveToStart()
		{
			CheckDisposed();

			_currentRecordIndex = -1;
		}

		/// <summary>
		/// Moves to the last record read so far.
		/// </summary>
		public void MoveToLastCachedRecord()
		{
			CheckDisposed();

			_currentRecordIndex = base.CurrentRecordIndex;
		}

		/// <summary>
		/// Moves to the specified record index.
		/// </summary>
		/// <param name="record">The record index.</param>
		/// <exception cref="T:ArgumentOutOfRangeException">
		///		Record index must be > 0.
		/// </exception>
		/// <exception cref="T:System.ComponentModel.ObjectDisposedException">
		///		The instance has been disposed of.
		/// </exception>
		public override void MoveTo(int record)
		{
			CheckDisposed();

			if (record < -1)
				throw new ArgumentOutOfRangeException("record", record, ExceptionMessage.RecordIndexLessThanZero);

			if (record <= base.CurrentRecordIndex)
				_currentRecordIndex = record;
			else
			{
				_currentRecordIndex = base.CurrentRecordIndex;

				int offset = record - _currentRecordIndex;

				// read to the last record before the one we want
				while (offset-- > 0 && ReadNextRecord()) ;
			}
		}

		/// <summary>
		/// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// 	<see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_records = null;

			base.Dispose(disposing);
		}

	}
}
