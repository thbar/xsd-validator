open System
open System.IO
open System.Xml
open System.Xml.Schema

let xsdPath = fsi.CommandLineArgs.[1]
let xmlFolder = fsi.CommandLineArgs.[2]

let schemas = XmlSchemaSet()
schemas.XmlResolver <- XmlUrlResolver()
schemas.Add(null, Path.GetFullPath(xsdPath)) |> ignore
schemas.Compile()

// TODO: allow specific file validation, instead of full folder

for xmlPath in Directory.GetFiles(xmlFolder, "*.xml", SearchOption.AllDirectories) do
    printfn "\n=== %s ===" (Path.GetFileName(xmlPath))
    let mutable errorCount = 0

    let settings = XmlReaderSettings()
    settings.ValidationType <- ValidationType.Schema
    settings.Schemas <- schemas
    settings.ValidationEventHandler.AddHandler(fun _ args ->
        errorCount <- errorCount + 1
        printfn "[%A] Line %d: %s" args.Severity args.Exception.LineNumber args.Message
    )

    try
        use reader = XmlReader.Create(xmlPath, settings)
        while reader.Read() do ()
        if errorCount = 0 then printfn "✓ Valid"
    with ex -> printfn "Error: %s" ex.Message
