module TestingUtils

open Expecto
open System.IO

open ARCtrl
open ARCtrl.ISA
open ARCtrl.ISA.Spreadsheet
open ARCtrl.NET
open ArcCommander
open ArgumentProcessing
open Argu

type ArcInvestigation with

    member this.ContainsStudy(studyIdentifier : string) =
        this.StudyIdentifiers |> Seq.contains studyIdentifier

    member this.TryGetStudy(studyIdentifier : string) =
        if this.ContainsStudy studyIdentifier then 
            Some (this.GetStudy studyIdentifier)
        else
            None

    member this.DeregisterStudy(studyIdentifier : string) =
        this.RegisteredStudyIdentifiers.Remove(studyIdentifier)

type ArcStudy with
    member this.TryGetRegisteredAssayAt(index : int) = 
        this.RegisteredAssays
        |> Seq.tryItem index

    member this.TryGetRegisteredAssay(assayIdentifier : string) =
        this.RegisteredAssayIdentifiers 
        |> Seq.tryFindIndex((=) assayIdentifier)
        |> Option.bind this.TryGetRegisteredAssayAt


    ///
let floatsClose accuracy (seq1:seq<float>) (seq2:seq<float>) = 
    Seq.map2 (fun x1 x2 -> Accuracy.areClose accuracy x1 x2) seq1 seq2
    |> Seq.contains false
    |> not

let createConfigFromDir testListName testCaseName =
    let dir = Path.Combine(__SOURCE_DIRECTORY__, "TestResult", testListName, testCaseName)
    ArcConfiguration.GetDefault()
    |> ArcConfiguration.ofIniData
    |> fun c -> {c with General = (Map.ofList ["workdir", dir; "verbosity", "2"]) }

let standardISAArgs = 
    Map.ofList 
        [
            "investigationfilename","isa.investigation.xlsx";
            "studiesfilename","isa.study.xlsx";
            "assayfilename","isa.assay.xlsx"
        ]

let processCommand (arcConfiguration : ArcConfiguration) (commandF : _ -> ArcParseResults<'T> -> _) (r : 'T list when 'T :> IArgParserTemplate) =
    let g = groupArguments r
    Prompt.deannotateArguments g 
    |> commandF arcConfiguration

let processCommandWoArgs (arcConfiguration : ArcConfiguration) commandF = commandF arcConfiguration


module Result =

    let getMessage res =
        match res with
        | Ok m -> m
        | Error m -> m