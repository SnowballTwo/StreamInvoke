using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Managed
{
	[StructLayout( LayoutKind.Sequential )]
	public class NativeStream : INativeStream
	{
		private readonly Stream _Stream;

		public NativeStream( Stream stream ) : base( )
		{
			_Stream = stream;
		}
			
		protected override int Read( IntPtr ptr, int length )
		{
			var buffer = new byte[ length ];
			var result = _Stream.Read( buffer, 0, length );
			Marshal.Copy( buffer, 0, ptr, result );

			return result;
		}

		protected override int Write( IntPtr ptr, int length )
		{
			var buffer = new byte[ length ];
			Marshal.Copy( ptr, buffer, 0, length );
			_Stream.Write( buffer, 0, length );

			return length;
		}

		protected override long Seek( long offset, SeekOrigin origin )
		{
			return _Stream.Seek( offset, origin );
		}
	}
}
