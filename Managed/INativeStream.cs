﻿using System;
using System.IO;

namespace Managed
{
	public delegate Int32 Read( IntPtr ptr, Int32 length );
	public delegate Int32 Write( IntPtr ptr, Int32 length );
	public delegate Int64 Seek( Int64 position, SeekOrigin origin );

	public struct INativeStream
	{
		#region Fields

		public Read Read;		
		public Write Write;
		public Seek Seek;

		#endregion Fields
	}
}