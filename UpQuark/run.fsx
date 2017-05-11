#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json.dll"

open System
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
    member val StartTime: Nullable<DateTime> = null with get, set
    member val Endtime: Nullable<DateTime> = null with get, set
    member val Room: string = null with get, set
    member val Tags: string = null with get, set

type QuarkModel = {title: string; speaker: string; description: string; startTime: DateTime; endTime: DateTime; room: string; tags: string}

let Run(req: HttpRequestMessage, inTable: IQueryable<Quark>, log: TraceWriter) =
    log.Info("GETting sessions")
    let sessions = 
        query {
            for quark in inTable do
            select {
                title = quark.Title; 
                speaker = quark.Speaker;
                description = quark.Description;
                startTime = quark.StartTime;
                endTime = quark.EndTime;
                room = quark.Room;
                tags = quark.Tags
            }
        }
        |> JsonConvert.SerializeObject
    req.CreateResponse(HttpStatusCode.OK, sessions)