﻿[<AutoOpen>]
module Neo4jClient.FsQuotations.TestDomainModels

open Neo4jClient.FsQuotations
open Neo4jClient

[<CLIMutable>]
type UserNode = 
    { [<Neo4jKey>] FacebookId: string }
    interface INeo4jNode

[<CLIMutable>]
type IsResidentOf =
    { CustomHouseholdName: string }
    interface INeo4jRelationship

[<CLIMutable>]
type IsGuestOf =
    { CustomHouseholdName: string }
    interface INeo4jRelationship

[<CLIMutable>]
type HouseholdNode = 
    { [<Neo4jKey>] Name: string }
    interface INeo4jNode


let clearAllRelations (client: GraphClient) =
    client.Cypher.Match("()-[r]->()").Delete("r").ExecuteWithoutResults()

let clearAllNodes (client: GraphClient) =
    client.Cypher.Match("(n)").Delete("n").ExecuteWithoutResults()

let createNodeAndExecute (client: GraphClient) (node: #INeo4jNode) =
    <@ createNode node @>
    |> executeWriteQuery client.Cypher

let initDbWithTestData (client: GraphClient) =
    // prepare data for tests
    let userDenis = { FacebookId = "Denis" }
    let userTT = { FacebookId = "TT" }
    let userOpwal = { FacebookId = "Opwal" }
    let userChou2 = { FacebookId = "Chouchou" }

    let householdColocJoie = { Name = "Coloc de la Joie" }
    let householdColoCarrouf = { Name = "Colocarouf" }

    let createNode (node: 'T when 'T :> INeo4jNode) =
        let nodeTypeName = typeof<'T>.Name
        client
            .Cypher
            .Create(sprintf "(node:%s {nodeParam})" nodeTypeName)
            .WithParam("nodeParam", node)
            .ExecuteWithoutResults()

    let createRightRelationship (node1: #INeo4jNode) (rel: #INeo4jRelationship) (node2: #INeo4jNode) =
        let nLabel1 = node1.GetType().Name
        let relLabel = rel.GetType().Name
        let nLabel2 = node2.GetType().Name
        
        let keyName1, keyValue1 = Cypher.findNeo4jKey node1
        let keyName2, keyValue2 = Cypher.findNeo4jKey node2

        client
            .Cypher
            .Match(sprintf "(n1:%s), (n2:%s)" nLabel1 nLabel2)
            .Where(sprintf "n1.%s = '%O' AND n2.%s = '%O'" keyName1 keyValue1 keyName2 keyValue2)
            .Create(sprintf "(n1)-[:%s {relParam}]->(n2)" relLabel)
            .WithParam("relParam", rel)
            .ExecuteWithoutResults()

    clearAllRelations client
    clearAllNodes client

    createNode userDenis
    createNode userTT
    createNode userOpwal
    createNode userChou2

    createNode householdColocJoie
    createNode householdColoCarrouf

    createRightRelationship userDenis { IsResidentOf.CustomHouseholdName = "Maison à République" } householdColocJoie
    createRightRelationship userTT { IsResidentOf.CustomHouseholdName = "" } householdColocJoie
    createRightRelationship userOpwal { IsResidentOf.CustomHouseholdName = "Répupu" } householdColocJoie
    createRightRelationship userDenis { IsGuestOf.CustomHouseholdName = "Chez Barbie" } householdColoCarrouf