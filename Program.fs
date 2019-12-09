open System
open Avro
open Avro.Specific
open System.IO
open Avro.IO

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
  let memStream =
    new MemoryStream(256)
  let encoder =
    BinaryEncoder(memStream)
  specificDatumWriter.Write(msg, encoder)
  encoder.Flush()
  memStream.ToArray()

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let hit = {hitHostname=""; hitMemoryUsed=0.3455; hitPage=""; }
    let avroMsg = getAvroMsg hit
    0
