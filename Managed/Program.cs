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
		public extern static void Test( ref INativeStream pFunc );

		private static void Main( string[] args )
		{
			var testData = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam";
			var stream = new MemoryStream( Encoding.ASCII.GetBytes( testData ) );

			var nativeStream = new NativeStream( stream );

			Test( ref nativeStream.Get() );

			Console.ReadLine();
		}

		#endregion Methods

	}
}