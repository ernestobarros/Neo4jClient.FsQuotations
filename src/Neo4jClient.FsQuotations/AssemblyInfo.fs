﻿namespace System
open System.Reflection
open System.Runtime.CompilerServices

[<assembly: AssemblyTitleAttribute("Neo4jClient.FsQuotations")>]
[<assembly: AssemblyProductAttribute("Neo4jClient.FsQuotations")>]
[<assembly: AssemblyDescriptionAttribute("Neo4jClient with F# quotations")>]
[<assembly: AssemblyVersionAttribute("0.1.0")>]
[<assembly: AssemblyFileVersionAttribute("0.1.0")>]
[<assembly: InternalsVisibleToAttribute("Neo4jClient.FsQuotations.Tests")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.1.0"
    let [<Literal>] InformationalVersion = "0.1.0"
