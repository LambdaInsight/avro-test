open System
open System.IO
open PeterO.Cbor
open MsgPack.Serialization

type HitType = {
  hitHostname                                   : string;
  hitMemoryUsed                                 : float;
  hitPage                                       : string;
}

let exampleHit = { hitHostname = "google.com"; hitMemoryUsed = 0.1234; hitPage = "index.html"}

//DOESNTWORK
let cborTest() =
  // No support for HitType -> CBOR
  Console.WriteLine("{0}", "Cbor test")
  let cbor = CBORObject.NewMap().Add("hitHostname", "google.com").Add("hitMemoryUsed", 0.123).Add("hitPage", "index.html")
  let encoded = cbor.EncodeToBytes()
  let buffer = new MemoryStream()
  buffer.Write(encoded, 0, encoded.Length)
  buffer.Seek(int64(0), SeekOrigin.Begin) |> ignore
  let decoded = CBORObject.Read(buffer);
  Console.WriteLine("{0}", decoded)

//WORKS
let msgpackTest() =
  Console.WriteLine("{0}", "msgpack test")
  let serializer = MessagePackSerializer.Get<HitType>();
  let buffer = new MemoryStream()
  serializer.Pack(buffer, exampleHit)
  buffer.Seek(int64(0), SeekOrigin.Begin) |> ignore
  let deser = serializer.Unpack(buffer)
  Console.WriteLine("{0}", deser)

// ?
let capnprotoTest =
  Console.WriteLine("{0}", "")  

[<EntryPoint>]
let main argv =
  cborTest()
  msgpackTest()
  0
