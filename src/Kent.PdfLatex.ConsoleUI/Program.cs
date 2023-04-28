// See https://aka.ms/new-console-template for more information

using Kent.PdfLatex;

Console.WriteLine("Hello, World!");

var outputDirectory = "../../../../../external/Output";
var filePath = "../../../../../external/Templates/test.tex";
var templateDirectory = "../../../../../external/Templates";

var pdfLatexBuilder = new PdfLatexBuilder()
    .OutputDirectory(outputDirectory)
    .File(filePath)
    .EnableInstaller()
    .IncludeDirectory(templateDirectory)
    .NonStopMode();
    
await pdfLatexBuilder.Run();