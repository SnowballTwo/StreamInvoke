# StreamInvoke

This small sample is supposed to remind me of the correct invoke of structs with delegate members. In case someone ever reads this, please feel free to change this to a more elegant solution (besides COM-interfaces).

## Interface definition in native code, typedefs are omittable

```cpp
typedef __int32(__stdcall *Read)(void*, __int32);
typedef __int32(__stdcall *Write)(void*, __int32);
typedef __int64(__stdcall *Seek)(__int64, __int32);

typedef struct {

	Read Read;
	Write Write;
	Seek Seek;

} INativeStream;
```

## Interface definition in managed code. Member order and struct layout attribute are essential.

```csharp
public delegate Int32 Read( IntPtr ptr, Int32 length );

public delegate Int64 Seek( Int64 position, SeekOrigin origin );

public delegate Int32 Write( IntPtr ptr, Int32 length );

[StructLayout( LayoutKind.Sequential )]
public abstract class INativeStream
{
	private Read _Read;
	private Write _Write;
	private Seek _Seek;
	
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
```

## Creating an implementation by wrapping a System.IO.Stream. Struct layout attribute is essential.

```csharp
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
```

## Native function requiring a stream

```cpp
API_EXPORT void Test(INativeStream* nativeStream) {  ... }
```

## Invoke on managed side

```csharp

[DllImport( @"Unmanaged.dll", CallingConvention = CallingConvention.Cdecl )]
public extern static void Test( INativeStream pFunc );

var testData = "Lorem ipsum dolor sit amet";
var stream = new MemoryStream( Encoding.ASCII.GetBytes( testData ) );
var nativeStream = new NativeStream( stream );
Test( nativeStream );

```
