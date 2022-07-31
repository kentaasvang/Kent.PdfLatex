// See https://aka.ms/new-console-template for more information

using kentaasvang.PdfLatex;
using static System.IO.Path;

PdfLatexBuilder builder = new();

string workingDir  = Directory.GetCurrentDirectory();
string templateDir = Combine(workingDir,  "templates");
string template    = Combine(templateDir, "twentysecondcv.tex");
string outputDir   = Combine(workingDir,  "output");

const string jobName = "testCV";

builder
	.File(template)
	.EnableInstaller()
	.JobName(jobName)
	.IncludeDirectory(templateDir)
	.OutputDirectory(outputDir);

await builder.Run();