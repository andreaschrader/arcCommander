﻿module InvestigationTests

open Argu
open Expecto
open ARCtrl
open ARCtrl.ISA
open ARCtrl.ISA.Spreadsheet
open ARCtrl.NET
open ArcCommander
open ArgumentProcessing
open ArcCommander.CLIArguments
open ArcCommander.APIs
open System
open System.IO

let standardISAArgs = 
    Map.ofList 
        [
            "investigationfilename","isa.investigation.xlsx";
            "studiesfilename","isa.study.xlsx";
            "assayfilename","isa.assay.xlsx"
        ]

let processCommand (arcConfiguration : ArcConfiguration) commandF (r : 'T list when 'T :> IArgParserTemplate) =
    let g = groupArguments r
    Prompt.deannotateArguments g 
    |> commandF arcConfiguration

let processCommandWoArgs (arcConfiguration : ArcConfiguration) commandF = commandF arcConfiguration

let setupArc (arcConfiguration : ArcConfiguration) =

    let arcArgs : ArcInitArgs list =  [] 

    processCommand arcConfiguration ArcAPI.init             arcArgs


//[<Tests>]
//let testInvestigationReading = 

//    let testDirectory = __SOURCE_DIRECTORY__ + @"/TestFiles/"
//    let investigationFileName = "isa.investigation.xlsx"
//    let investigationFilePath = Path.Combine(testDirectory,investigationFileName)

//    testCase "MatchesInvestigation" (fun () -> 
//        let testInvestigation = 
//            ISADotNet.XLSX.Investigation.InvestigationInfo.create
//                "BII-I-1"
//                "Growth control of the eukaryote cell: a systems biology study in yeast"
//                "Background Cell growth underlies many key cellular and developmental processes, yet a limited number of studies have been carried out on cell-growth regulation. Comprehensive studies at the transcriptional, proteomic and metabolic levels under defined controlled conditions are currently lacking. Results Metabolic control analysis is being exploited in a systems biology study of the eukaryotic cell. Using chemostat culture, we have measured the impact of changes in flux (growth rate) on the transcriptome, proteome, endometabolome and exometabolome of the yeast Saccharomyces cerevisiae. Each functional genomic level shows clear growth-rate-associated trends and discriminates between carbon-sufficient and carbon-limited conditions. Genes consistently and significantly upregulated with increasing growth rate are frequently essential and encode evolutionarily conserved proteins of known function that participate in many protein-protein interactions. In contrast, more unknown, and fewer essential, genes are downregulated with increasing growth rate; their protein products rarely interact with one another. A large proportion of yeast genes under positive growth-rate control share orthologs with other eukaryotes, including humans. Significantly, transcription of genes encoding components of the TOR complex (a major controller of eukaryotic cell growth) is not subject to growth-rate regulation. Moreover, integrative studies reveal the extent and importance of post-transcriptional control, patterns of control of metabolic fluxes at the level of enzyme synthesis, and the relevance of specific enzymatic reactions in the control of metabolic fluxes during cell growth. Conclusion This work constitutes a first comprehensive systems biology study on growth-rate control in the eukaryotic cell. The results have direct implications for advanced studies on cell growth, in vivo regulation of metabolic fluxes for comprehensive metabolic engineering, and for the design of genome-scale systems biology models of the eukaryotic cell."
//                "4/30/2007"
//                "3/10/2009"
//                [ISADotNet.XLSX.Comment.fromString "Created With Configuration" ""; ISADotNet.XLSX.Comment.fromString "Last Opened With Configuration" "isaconfig-default_v2013-02-13"]
                
//        let investigation = ISADotNet.XLSX.Investigation.fromFile investigationFilePath

//        Expect.equal investigation.Contacts.Value.Length 3 "There should be 3 Persons in the investigation"
//        Expect.equal investigation.Studies.Value.Head.Assays.Value.Length 3 "There should be 3 Assays in the first study"
//        Expect.equal investigation.Studies.Value.Length 2 "There should be two studies in the investigation"
//        Expect.equal investigation.Identifier.Value testInvestigation.Identifier "Investigation Identifier does not match"
//        Expect.sequenceEqual investigation.Comments.Value testInvestigation.Comments "Investigation Comments do not match"
//    )

//[<Tests>]
//let testInvestigationCreatedByInit = 

//    let testDirectory = __SOURCE_DIRECTORY__ + @"/TestResult/investigationCreateTest"

//    let configuration = 
//        ArcConfiguration.create 
//            (Map.ofList ["workdir",testDirectory;"verbosity","2"]) 
//            (standardISAArgs)
//            Map.empty Map.empty Map.empty Map.empty
//    testList "InvestigationCreateTests" [
//        testCase "MatchesInvestigationValues" (fun () -> 
//            let investigationName = "TestInvestigation"
//            let submissionDate = "FirstOctillember"
//            let investigationArgs = [InvestigationCreateArgs.Identifier investigationName;InvestigationCreateArgs.SubmissionDate submissionDate]

//            let testInvestigation = 
//                ISADotNet.XLSX.Investigation.fromParts 
//                    (ISADotNet.XLSX.Investigation.InvestigationInfo.create investigationName "" "" submissionDate "" [])
//                    [] [] [] [] []
   
//            setupArc configuration
//            processCommand configuration InvestigationAPI.create investigationArgs

//            let investigation = ISADotNet.XLSX.Investigation.fromFile (IsaModelConfiguration.getInvestigationFilePath configuration)
//            Expect.equal investigation testInvestigation "The assay in the file should match the one created per hand but did not"
//        )
//        testCase "ShouldNotOverwrite" (fun () -> 
//            let investigationName = "OverwriteInvestigation"
//            let investigationArgs = [InvestigationCreateArgs.Identifier investigationName]

//            let testInvestigation = ISADotNet.XLSX.Investigation.fromFile (IsaModelConfiguration.getInvestigationFilePath configuration)

//            processCommand configuration InvestigationAPI.create investigationArgs

//            let investigation = ISADotNet.XLSX.Investigation.fromFile (IsaModelConfiguration.getInvestigationFilePath configuration)
//            Expect.equal investigation testInvestigation "Investigation file was overwritten even if it should not have been"
//        )
//    ]
//    |> testSequenced


//[<Tests>]
//let testInvestigationUpdate = 

//    let testDirectory = __SOURCE_DIRECTORY__ + @"/TestResult/investigationUpdateTest"
//    let investigationFileName = "isa.investigation.xlsx"
//    let source = __SOURCE_DIRECTORY__
//    let investigationToCopy = Path.Combine([|source;"TestFiles";investigationFileName|])

//    let configuration = 
//        ArcConfiguration.create 
//            (Map.ofList ["workdir",testDirectory;"verbosity","2"]) 
//            (standardISAArgs)
//            Map.empty Map.empty Map.empty Map.empty

//    testList "InvestigationUpdateTests" [
//        testCase "UpdateStandard" (fun () -> 
//            let newTitle = "newTitle"
//            let investigationArgs = [InvestigationUpdateArgs.Identifier "BII-I-1";InvestigationUpdateArgs.Title newTitle]
//            let investigationFilePath = (IsaModelConfiguration.getInvestigationFilePath configuration)

//            let investigationBeforeChangingIt = ISADotNet.XLSX.Investigation.fromFile investigationToCopy
//            setupArc configuration
//            //Copy testInvestigation
//            File.Copy(investigationToCopy,investigationFilePath)
//            processCommand configuration InvestigationAPI.update investigationArgs
            
//            let investigation = ISADotNet.XLSX.Investigation.fromFile investigationFilePath
//            // Updated with given value
//            Expect.equal investigation.Title.Value newTitle "The value which should be updated was not updated"
//            // Updated with given value
//            Expect.equal investigation.Description investigationBeforeChangingIt.Description "No update value was given for description and the \"ReplaceWithEmptyValues\" flag was not given but the description was still changed"
//            // Updated with given value
//            Expect.sequenceEqual  investigation.Studies.Value investigationBeforeChangingIt.Studies.Value "Studies should not have been affected by update function"
//            // Updated with given value
//            Expect.sequenceEqual  investigation.Comments.Value investigationBeforeChangingIt.Comments.Value "Comments should not have been affected by update function"      
//        )
//        testCase "UpdateReplaceWithEmpty" (fun () -> 
//            let newIdentifier = "BestInvestigation"
//            let investigationArgs = [InvestigationUpdateArgs.Identifier newIdentifier;InvestigationUpdateArgs.ReplaceWithEmptyValues]
//            let investigationFilePath = (IsaModelConfiguration.getInvestigationFilePath configuration)

//            let investigationBeforeChangingIt = ISADotNet.XLSX.Investigation.fromFile investigationFilePath
   
//            processCommand configuration InvestigationAPI.update investigationArgs

//            let investigation = ISADotNet.XLSX.Investigation.fromFile investigationFilePath
//            // Updated with given value
//            Expect.equal investigation.Identifier.Value newIdentifier "The value which should be updated was not updated"
//            // Updated with given value
//            Expect.isNone investigation.SubmissionDate "No update value was given for submission date and the \"ReplaceWithEmptyValues\" flag was given but the value was not removed"
//            // Updated with given value
//            Expect.sequenceEqual  investigation.Studies.Value investigationBeforeChangingIt.Studies.Value "Studies should not have been affected by update function"
//            // Updated with given value
//            Expect.sequenceEqual  investigation.Comments.Value investigationBeforeChangingIt.Comments.Value "Comments should not have been affected by update function"      
//        )
//        |> testSequenced
//    ]

//[<Tests>]
//let testInvestigationContacts = 
//    let testDirectory = __SOURCE_DIRECTORY__ + @"/TestResult/investigationContactTest"
//    let investigationFileName = "isa.investigation.xlsx"
//    let source = __SOURCE_DIRECTORY__
//    let investigationToCopy = Path.Combine([|source;"TestFiles";investigationFileName|])

//    let configuration = 
//        ArcConfiguration.create 
//            (Map.ofList ["workdir",testDirectory;"verbosity","2"]) 
//            (standardISAArgs)
//            Map.empty Map.empty Map.empty Map.empty

//    let investigationFilePath = (IsaModelConfiguration.getInvestigationFilePath configuration)
    
//    let investigationBeforeChangingIt = ISADotNet.XLSX.Investigation.fromFile investigationToCopy
//    setupArc configuration
//    //Copy testInvestigation
//    File.Copy(investigationToCopy,investigationFilePath)

//    testList "InvestigationContactTests" [
//        testCase "Update" (fun () -> 
//            let newAddress = "FunStreet"

//            let firstName = "Oliver"
//            let midInitials = "G"
//            let lastName = "Stephen"

//            let contactArgs = 
//                [
//                    InvestigationContacts.PersonUpdateArgs.FirstName firstName;
//                    InvestigationContacts.PersonUpdateArgs.MidInitials midInitials;
//                    InvestigationContacts.PersonUpdateArgs.LastName lastName;
//                    InvestigationContacts.PersonUpdateArgs.Address newAddress;
//                ]

//            let personBeforeUpdating = 
//                investigationBeforeChangingIt.Contacts.Value
//                |> API.Person.tryGetByFullName firstName midInitials lastName
//                |> Option.get
            
//            processCommand configuration InvestigationAPI.Contacts.update contactArgs
            
//            let investigation = ISADotNet.XLSX.Investigation.fromFile investigationFilePath
//            Expect.isSome investigation.Contacts "Investigation Contacts missing after updating one person"

//            let person = API.Person.tryGetByFullName firstName midInitials lastName investigation.Contacts.Value

//            Expect.isSome person "Person missing after updating person"

//            let adress = person.Value.Address

//            Expect.isSome adress "Adress missing after updating person"
//            Expect.equal adress.Value newAddress "Adress was not updated with new value"
//            Expect.equal person.Value {personBeforeUpdating with Address = Some newAddress} "Other values of person were changed even though only the Address should have been updated"

//        )

//    ]
//    |> testSequenced

// currently not valid anymore since Expecto does not trigger NLog's console loggings. Did not find any workarounds
//[<Tests>]
//let testInvestigationShow =
    
//    let setupArc (arcConfiguration : ArcConfiguration) = processCommandWoArgs arcConfiguration ArcAPI.init (Map [])
    
//    let createConfigFromDir testListName testCaseName =
//        let dir = Path.Combine(__SOURCE_DIRECTORY__, "TestResult", testListName, testCaseName)
//        ArcConfiguration.GetDefault()
//        |> ArcConfiguration.ofIniData
//        |> fun c -> {c with General = (Map.ofList ["workdir", dir; "verbosity", "2"]) }

//    testList "InvestigationShowTests" [
//        testCase "ShowsCorrectly" (fun () -> 
   
//            let configuration = createConfigFromDir "InvestigationShowTests" "ShowsCorrectly"
//            setupArc configuration

//            let investigationName = "TestInvestigation"
//            let submissionDate = "FirstOctillember"
//            let investigationArgs = [InvestigationCreateArgs.Identifier investigationName; InvestigationCreateArgs.SubmissionDate submissionDate]
//            processCommand configuration InvestigationAPI.create investigationArgs

//            let writer = new StringWriter()
//            let stdout = Console.Out // standard Console output
//            Console.SetOut(writer) // reads everything that the console prints

//            processCommandWoArgs configuration InvestigationAPI.show

//            let consoleOutput = writer.ToString().Replace("\013","") // get rid of stupid carriage return char

//            Console.SetOut(stdout) // reset Console output to stdout

//            let expectedOutput = 
//                "Start Investigation Show\nInvestigation Identifier:TestInvestigation\nInvestigation Title:\nInvestigation Description:\nInvestigation Submission Date:FirstOctillember\nInvestigation Public Release Date:\n"

//            Expect.equal consoleOutput expectedOutput "The showed output differed from the expected output"

//        )

//    ]
//    |> testSequenced