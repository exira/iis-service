﻿namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Exira.IIS.Domain")>]
[<assembly: AssemblyProductAttribute("Exira.IIS")>]
[<assembly: AssemblyDescriptionAttribute("Manage IIS through a REST API.")>]
[<assembly: AssemblyVersionAttribute("0.0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.1"
