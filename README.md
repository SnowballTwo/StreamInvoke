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

## Interface definition in managed code.

```csharp
public delegate Int32 Read( IntPtr ptr, Int32 length );
public delegate Int32 Write( IntPtr ptr, Int32 length );
public delegate Int64 Seek( Int64 position, SeekOrigin origin );

public struct INativeStream
{
	public Read Read;		
	public Write Write;
	public Seek Seek;	
}
```

## Creating a struct from a stream-wrapping class which can be obtained and invoked

```csharp
public class NativeStream
{  
  private readonly Stream _Stream;
  private INativeStream _Interface;

  public NativeStream( Stream stream )
  {
    _Stream = stream;
    _Interface = new INativeStream
    {
      Read = Read,
      Write = Write,
      Seek = Seek
    };
  }
 
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
}
```

## Native function requiring a stream

```cpp
API_EXPORT void Test(INativeStream* nativeStream) {  ... }
```

## Invoke on managed side

```csharp

[DllImport( @"Unmanaged.dll", CallingConvention = CallingConvention.Cdecl )]
public extern static void Test( ref INativeStream pFunc );

var testData = "Lorem ipsum dolor sit amet";
var stream = new MemoryStream( Encoding.ASCII.GetBytes( testData ) );
var nativeStream = new NativeStream( stream );
Test( ref nativeStream.Get() );

```
