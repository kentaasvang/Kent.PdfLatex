using System.Diagnostics;
using System.Text;

namespace Kent.PdfLatex;

public class PdfLatexBuilder
{
	private       PdfLatexArguments         Arguments     { get; } = new();
	private       DataReceivedEventHandler? OutputHandler { get; set; }

	public async Task Run()
	{
		StringBuilder output = new();

		OutputHandler = OutputDataReceived;

		// call executable
		var process = new Process().InitiateProcess();
		process.StartInfo.Arguments = Arguments.ToString();

		process.Start();

		// This raises OutputDataReceived events for each line of output.
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();

		if (OutputHandler is not null)
			process.OutputDataReceived += OutputHandler;

		await process.WaitForExitAsync();

		// Write the redirected output to this application's window.
		Console.WriteLine(output);

		await process.WaitForExitAsync();
		process.Close();
	}

	/// <summary>
	/// diagnostic message appear on the terminal but as in batch mode there is no user interaction
	/// </summary>
	public PdfLatexBuilder NonStopMode()
	{
		Arguments.AddArgument("-interaction=nonstopmode");
		return this;
	}

	/// <summary>
	/// Enable the package installer.  Missing files will be installed.
	/// </summary>
	public PdfLatexBuilder EnableInstaller()
	{
		Arguments.AddArgument("-enable-installer");
		return this;
	}

	/// <summary>
	/// Suppress all output (except errors). 
	/// </summary>
	public PdfLatexBuilder Quiet()
	{
		Arguments.AddArgument("-quiet");
		return this;
	}

	/// <summary>
	/// Path to LateX-file
	/// </summary>
	public PdfLatexBuilder File(string file)
	{
		Arguments.AddArgument(file);
		return this;
	}

	/// <summary>
	/// LateX-document as string
	/// </summary>
	public PdfLatexBuilder LatexDocument(string document)
	{
		throw new NotImplementedException("Use PdfLatexBuilder.File instead");
	}

	/// <summary>
	/// Set the job name and hence the name(s) of the output file(s).  
	/// </summary>
	public PdfLatexBuilder JobName(string jobName)
	{
		Arguments.AddArgument($"-job-name={jobName}");
		return this;
	}

	/// <summary>
	/// Prefix DIR to the input search path. 
	/// </summary>
	public PdfLatexBuilder IncludeDirectory(string directory)
	{
		Arguments.AddArgument($"-include-directory={directory}");
		return this;
	}

	/// <summary>
	/// Use DIR as the directory to write output files to. 
	/// </summary>
	public PdfLatexBuilder OutputDirectory(string directory)
	{
		Arguments.AddArgument($"-output-directory={directory}");
		return this;
	}

	private static void OutputDataReceived(object sender, DataReceivedEventArgs @event)
	{
		if (!string.IsNullOrEmpty(@event.Data))
			Console.WriteLine(@event.Data);
	}
}

internal class PdfLatexArguments
{
	private readonly StringBuilder Arguments = new();

	public override string ToString()
		=> Arguments.ToString();

	public void AddArgument(string argument)
		=> Arguments.Append(" " + argument);
}

public static class Extensions
{
	private const string EXECUTABLE = "pdflatex";

	public static Process InitiateProcess(this Process process)
	{
		process.StartInfo.FileName               = EXECUTABLE;
		process.StartInfo.UseShellExecute        = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError  = true;
		return process;
	}
}