﻿namespace Exira.IIS.Domain

module Helpers =
    open System
    open Exira.EventStore
    open Exira.EventStore.EventStore
    open Exira.IIS.Domain.Railway

    let getTypeName o = o.GetType().Name
    let stateTransitionFail event state = Failure (InvalidStateTransition (sprintf "Invalid event %s for state %s" (event |> getTypeName) (state |> getTypeName)))

    // Apply each event on itself to get to the final state
    let evolve evolveOne initState =
        List.fold
            (fun result event ->
                result
                >>= fun (version, state) ->
                    evolveOne state event
                    >>= fun state -> Success (version + 1, state))
            (Success (-1, initState))

    let getState evolveOne initState id es =
        async {
            let! streamEvents =
                readFromStream es id 0 Int32.MaxValue

            let events =
                streamEvents
                |> (fun (e, _, _) -> e)

            return evolveOne initState events
        }

    let toStreamId prefix (id: Guid) = sprintf "%s-%O" prefix id |> StreamId

    let save es (id, version, events) =
        appendToStream es id version events |> Async.RunSynchronously // TODO: Temporary sync, needs to turn into full async
        Success events