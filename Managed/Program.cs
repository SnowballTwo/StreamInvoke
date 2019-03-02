using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Managed
{
	internal class Program
	{
		#region Methods

		[DllImport( @"Unmanaged.dll", CallingConvention = CallingConvention.Cdecl )]
		public extern static void Test( INativeStream pFunc );

		private static void Main( string[] args )
		{
			var testData = "Lorem ipsum dolor sit amet";
			var stream = new MemoryStream( Encoding.ASCII.GetBytes( testData ) );

			var nativeStream = new NativeStream( stream );

			Test( nativeStream );

			Console.WriteLine( Encoding.ASCII.GetString( stream.ToArray() ) );

			Console.ReadLine();
		}

		#endregion Methods

	}
}