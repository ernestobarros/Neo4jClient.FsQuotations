(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "./../../bin/Neo4jClient.FsQuotations"

(**
Basic usage
===========

## Dependencies

Reference this library (dependency on Neo4jClient) and create a Neo4j GraphClient().

*)
#r "Neo4jClient.dll"
#r "Neo4jClient.FsQuotations.dll"

open System
open Neo4jClient
open Neo4jClient.FsQuotations

let client = new GraphClient(Uri("http://localhost:7474/db/data"), "neo4j", "Password123")
client.Connect()

(*** hide ***)
// Clear all data
client.Cypher.Match("n").DetachDelete("n").ExecuteWithoutResults()

(**

## Define your model

Declare nodes & relationships in your model (by inheriting from `INeo4jNode` or
`INeo4jRelationship` interfaces).

We will define users.
*)

[<CLIMutable>]
type UserNode = 
    { FacebookId: string }
    interface INeo4jNode

(**
## Create user nodes
*)

for id in [ "Okay"; "TT"; "Opwal" ] do
    let user = { FacebookId = id }
    <@ createNode user @>
    |> executeWriteQuery client.Cypher

(**
## Get all nodes with specific label
*)

let allUsers =
    <@ let user = declareNode<UserNode>
       matchNode user
       returnResults user @>
    |> executeReadQuery client.Cypher
    |> Seq.toList

(** ### Results *)
(*** include-value: allUsers ***)

(**
## Get specific user
*)
let userTT =
    <@ let user = declareNode<UserNode>
       matchNode user
       where (user.FacebookId = "TT")
       returnResults user @>
    |> executeReadQuery client.Cypher

(** ### Results *)
(*** include-value: userTT ***)
