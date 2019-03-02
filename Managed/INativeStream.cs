using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Managed
{
	public delegate Int32 Read( IntPtr ptr, Int32 length );

	public delegate Int64 Seek( Int64 position, SeekOrigin origin );

	public delegate Int32 Write( IntPtr ptr, Int32 length );

	[StructLayout( LayoutKind.Sequential )]
	public abstract class INativeStream
	{
		#region Fields

		private Read _Read;
		private Write _Write;
		private Seek _Seek;

		#endregion Fields

		public INativeStream()
		{
			_Read = Read;
			_Write = Write;
			_Seek = Seek;
		}

		protected abstract int Read( IntPtr ptr, int length );

		protected abstract int Write( IntPtr ptr, int length );

		protected abstract long Seek( long offset, SeekOrigin origin );
	}
}