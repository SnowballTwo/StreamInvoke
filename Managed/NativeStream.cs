using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Managed
{
	public class NativeStream
	{
		#region Fields

		private readonly Stream _Stream;
		private INativeStream _Interface;

		#endregion Fields

		#region Constructors

		public NativeStream( Stream stream )
		{
			_Interface = new INativeStream
			{
				Read = Read,
				Write = Write,
				Seek = Seek
			};

			_Stream = stream;
		}

		#endregion Constructors

		#region Methods

		public ref INativeStream Get()
		{
			return ref _Interface;
		}

		private int Read( IntPtr ptr, int length )
		{
			var buffer = new byte[ length ];
			var result = _Stream.Read( buffer, 0, length );
			Marshal.Copy( buffer, 0, ptr, result );

			return result;
		}

		private long Seek( long offset, SeekOrigin origin )
		{
			return _Stream.Seek( offset, origin );
		}

		private int Write( IntPtr ptr, int length )
		{
			var buffer = new byte[ length ];
			Marshal.Copy( ptr, buffer, 0, length );
			_Stream.Write( buffer, 0, length );

			return length;
		}

		#endregion Methods
	}
}