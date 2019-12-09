open System
open Avro
open Avro.Specific
open System.IO
open Avro.IO
open Avro.Generic

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

let avroSchema =
  let schemaParsed =
    Schema.Parse(schemaJson)
  schemaParsed

let specificDatumWriter : SpecificDatumWriter<HitType> =
  SpecificDatumWriter(avroSchema)

let getAvroMsg msg =
  let memStream = new MemoryStream(256)
  let encoder = BinaryEncoder(memStream)
  specificDatumWriter.Write(msg, encoder)
  encoder.Flush()
  memStream.ToArray()

let hitEmpty =
  {hitHostname = ""; hitMemoryUsed = 0.0; hitPage = ""}

let getJsonFromAvro (avroBytes: byte []) =
  Console.WriteLine("{0}", avroBytes.Length)
  let memStream = new MemoryStream(avroBytes.Length)
  Console.WriteLine("Capacity {0} : Length {1} : {2}", memStream.Capacity, memStream.Length, memStream.Position)
  memStream.Write(avroBytes, 0, avroBytes.Length)
  Console.WriteLine("Capacity {0} : Length {1} : {2}", memStream.Capacity, memStream.Length, memStream.Position)
  memStream.Seek(int64(0), SeekOrigin.Begin) |> ignore
  Console.WriteLine("Capacity {0} : Length {1} : {2}", memStream.Capacity, memStream.Length, memStream.Position)
  let reader = new GenericDatumReader<HitType>(avroSchema,avroSchema)
  let decoder = BinaryDecoder(memStream)
  let ret = reader.Read(hitEmpty,decoder)
  memStream.Close()
  ret

let testAvroFrag =
  File.ReadAllBytes("example.avro")

[<EntryPoint>]
let main argv =
  Console.WriteLine("Hello World from F#!")
  let hit = { hitHostname="google.com"; hitMemoryUsed=0.3455; hitPage="index.html"; }
  // let avroMsg = getAvroMsg hit
  Console.WriteLine hit
  Console.WriteLine (getJsonFromAvro testAvroFrag)
  0
