using System;
using System.IO;

namespace Managed
{
	public delegate Int32 Read( IntPtr ptr, Int32 length );

	public delegate Int64 Seek( Int64 position, SeekOrigin origin );

	public delegate Int32 Write( IntPtr ptr, Int32 length );

	public struct INativeStream
	{
		#region Fields

		public Read Read;
		public Seek Seek;
		public Write Write;

		#endregion Fields
	}
}