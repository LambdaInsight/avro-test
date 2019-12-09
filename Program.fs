open System
open System.IO
open Microsoft.Hadoop.Avro.Container
open Microsoft.Hadoop.Avro.Utils
open Microsoft.Hadoop.Avro

type HitType = {
  hitHostname                                   : string;
  hitMemoryUsed                                 : float;
  hitPage                                       : string;
}

let schemaJson =
  let streamReader =
    new StreamReader @"avsc.json"
  let fileContent =
    streamReader.ReadToEnd().ToString()
  fileContent

// let avroSchema =
//   let schemaParsed =
//     Schema.Parse(schemaJson)
//   schemaParsed

// let specificDatumWriter : SpecificDatumWriter<HitType> =
//   SpecificDatumWriter(avroSchema)

// let getAvroMsg msg =
//   let memStream = new MemoryStream(256)
//   let encoder = BinaryEncoder(memStream)
//   specificDatumWriter.Write(msg, encoder)
//   encoder.Flush()
//   memStream.ToArray()

// let hitEmpty =
//   HitType()

// let getJsonFromAvro (avroBytes: byte []) =
//   Console.WriteLine("{0}", avroBytes.Length)
//   let memStream = new MemoryStream(avroBytes.Length)
//   Console.WriteLine("Capacity {0} : Length {1} : {2}", memStream.Capacity, memStream.Length, memStream.Position)
//   memStream.Write(avroBytes, 0, avroBytes.Length)
//   Console.WriteLine("Capacity {0} : Length {1} : {2}", memStream.Capacity, memStream.Length, memStream.Position)
//   memStream.Seek(int64(0), SeekOrigin.Begin) |> ignore
//   Console.WriteLine("Capacity {0} : Length {1} : {2}", memStream.Capacity, memStream.Length, memStream.Position)
//   let reader = new SpecificDatumReader<HitType>(avroSchema,avroSchema)
//   let decoder = BinaryDecoder(memStream)
//   let ret = reader.Read(hitEmpty,decoder)
//   memStream.Close()
//   ret

let testAvroFrag =
  File.ReadAllBytes("example.avro")

let avroSerde = AvroSerializer.Create<HitType>()

let exampleHit = { hitHostname="google.com"; hitMemoryUsed=0.123; hitPage="index.html" }

[<EntryPoint>]
let main argv =
  let buffer = new MemoryStream()
  Console.WriteLine("Capacity {0} : Length {1} : {2}", buffer.Capacity, buffer.Length, buffer.Position)
  avroSerde.Serialize(buffer, exampleHit)
  Console.WriteLine("Capacity {0} : Length {1} : {2}", buffer.Capacity, buffer.Length, buffer.Position)
  buffer.Seek(int64(0), SeekOrigin.Begin) |> ignore
  Console.WriteLine("Capacity {0} : Length {1} : {2}", buffer.Capacity, buffer.Length, buffer.Position)
  // buffer.Write(testAvroFrag, 0, testAvroFrag.Length)
  let frag = avroSerde.Deserialize(buffer)
  Console.WriteLine(frag)
  0
