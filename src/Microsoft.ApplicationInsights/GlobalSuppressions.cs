// <copyright file="GlobalSuppressions.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click "In Suppression File".
// You may also add suppressions to this file manually.

using System.Diagnostics.CodeAnalysis;

[assembly:SuppressMessage(
            category:       "Microsoft.Usage",
            checkId:        "CA2213: Disposable Fields Should Be Disposed",
            MessageId =     "startRunnerEvent",
            Scope =         "member",
            Target =        "Microsoft.ApplicationInsights.Channel.InMemoryTransmitter.#Dispose(System.Boolean)", 
            Justification = "startRunnerEvent is being disposed.")]

[assembly:SuppressMessage(
            category:       "Microsoft.Usage",
            checkId:        "CA2213: Disposable Fields Should Be Disposed", 
            MessageId =     "telemetryChannel", 
            Scope =         "member", 
            Target =        "Microsoft.ApplicationInsights.Extensibility.TelemetrySink.#Dispose()", 
            Justification = "telemetryChannel is being disposed using ?. syntax")]

[assembly:SuppressMessage(
            category:       "StyleCop.CSharp.DocumentationRules",
            checkId:        "SA1005: Single line comment must begin with a space.",
            Justification = "Aligning comments lines with spaces can help readability."
                          + " Critical from the accessibility standpoint for people with dyslexia.")]

[assembly:SuppressMessage(
            category:       "StyleCop.CSharp.DocumentationRules",
            checkId:        "SA1025: Code Must Not Contain Multiple Whitespace In A Row",
            Justification = "Aligning elements across lines with spaces can help readability."
                          + " Critical from the accessibility standpoint for people with dyslexia.")]

[assembly:SuppressMessage(
            category:       "StyleCop.CSharp.MaintainabilityRules",
            checkId:        "SA1119: Statement Must Not Use Unnecessary Parenthesis",
            Justification = "Parentheses are important for making long expressions readable."
                          + " Critical from the accessibility standpoint for people with dyslexia.")]

[assembly:SuppressMessage(
            category:       "StyleCop.CSharp.DocumentationRules",
            checkId:        "SA1623: Property Summary Documentation Must Match Accessors",
            Justification = "Forcing to start an element description with a specific word is counter-productive."
                          + " Focus should be on quality formulations in the docs, not on blindly starting the sentences with the same word.")]

[assembly:SuppressMessage(
            category:       "StyleCop.CSharp.DocumentationRules",
            checkId:        "SA1625: Element Documentation Must Not Be Copied And Pasted",
            Justification = "Specifically works around the remark 'ToDo: Complete documentation before stable release.'."
                          + " Needs to be re-enabled when all those are addressed.")]

[assembly:SuppressMessage(
            category:       "StyleCop.CSharp.DocumentationRules",
            checkId:        "SA1643: Destructor Summary Documentation Must Begin With Standard Text",
            Justification = "Useless rule. It is not even clear what this standard text should be.")]