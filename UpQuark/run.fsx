#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"

open System.Linq
open System.Net
open System.Net.Http
open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json

type Quark() =
    inherit TableEntity()
    member val Title: string = null with get, set
    member val Speaker: string = null with get, set
    member val Abstract: string = null with get, set

let XRun(req: HttpRequestMessage, inTable: IQueryable<Quark>, log: TraceWriter) =
    log.Info("GETting conference schedule")
    req.CreateResponse(HttpStatusCode.OK, "{message:\"Hello\"}")

let Run(req: HttpRequestMessage, inTable: IQueryable<Quark>, log: TraceWriter) =
    let sessions = 
        query {
            for quark in inTable do
            select (quark.Title, quark.Speaker, quark.Abstract)
        }
        |> JsonConvert.SerializeObject
    req.CreateResponse(HttpStatusCode.OK, sessions)
    (*
    let people =
        query {
            for person in inTable do
            select person
        }
        |> Seq.map (fun person -> sprintf "\"Name\": \"%s\"" person.Title)
        |> String.concat ","

    req.CreateResponse(HttpStatusCode.OK, sprintf "{%s}" people)
    *)
