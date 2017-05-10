#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json.dll"

open System.Linq
open System.Net
open System.Net.Http
open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json

type Quark() =
    inherit TableEntity()
    member val Title: string = null with get, set
    member val Speaker: string = null with get, set
    member val Description: string = null with get, set

type QuarkModel = {title: string; speaker: string; description: string}

let Run(req: HttpRequestMessage, inTable: IQueryable<Quark>, log: TraceWriter) =
    log.Info("GETting sessions")
    let sessions = 
        query {
            for quark in inTable do
            select {title = quark.Title; speaker = quark.Speaker; description = quark.Description}
        }
        |> JsonConvert.SerializeObject
    req.CreateResponse(HttpStatusCode.OK, sessions)