using Fclp;
using Fclp.Internals.Extensions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Text.RegularExpressions;



public static class Program
{
    internal class ApplicationArguments
    {
        public string OptimizationType { get; set; }

        public void ShowAllvalues()
        {
            Console.WriteLine("\nCommand : " + OptimizationType);

        }
    }


    private const string HelpString =
          "\n                                Available Combinations : " +
          "\n                                buildOptimize -c size" +
          "\n                                buildOptimize -c speed" +
          "\n                                buildOptimize -c balanced";

    public const string directory = "C:\\Users\\utkarshmohan\\work\\monorepoLates\\MonoRepo\\private\\packages";
    public const string projectName = "BasicHttpServer.Library";
    public const string finalDirectory = directory + "\\" + projectName;

    internal static void PerformOperation(FluentCommandLineParser<ApplicationArguments> argParser)
    {
        var arguments = argParser.Object;

        //Console.WriteLine("Processing for "+arguments.OptimizationType);
        //To be taken from Command Line Later
       
        switch (arguments.OptimizationType)
        {

            case "size":
                //< WholeProgramOptimization > False </ WholeProgramOptimization >
                WholeProgramOptimization("size");
                OptimizationLevel("size");
                Optimization("size");
                Console.WriteLine("Processed Proj file for Size");

                //var searchString = "<PropertyGroup Label=\\\"Globals\\\" Condition=\\\"'$(RedboxRoot)'==''\\\">\";\r\n";
                break;
            case "speed":
                WholeProgramOptimization("speed");
                OptimizationLevel("speed");
                Optimization("speed");
                Console.WriteLine("Processed Proj file for Speed");
                break;

            case "balanced":
                WholeProgramOptimization("balanced");
                OptimizationLevel("balanced");
                Optimization("balanced");
                Console.WriteLine("Processed Proj file for Optimizataion");
                break; 
                
            default:
                Console.WriteLine("Usage Step .. \n");
                argParser.HelpOption.ShowHelp(argParser.Options);
                return;
        }
    }


    public static void WholeProgramOptimization(string type)
    {
        //Remove if already exists
        String WPO = "";
        var allProjFile = Directory.EnumerateFiles(finalDirectory, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".vcxproj"));
        foreach (var item in allProjFile)
        {
            var fileContent = File.ReadAllText(item);
            var patternForSearch = "<WholeProgramOptimization>[a-z,A-Z]*</WholeProgramOptimization>";
            var regSearch = Regex.Match(fileContent, patternForSearch, RegexOptions.IgnoreCase);
            if (regSearch.Success)
            {
                fileContent = fileContent.Replace(regSearch.Value, "", StringComparison.OrdinalIgnoreCase);
                File.WriteAllText(item, fileContent);
            }
        }


        if (type.Equals("size", StringComparison.OrdinalIgnoreCase))
        {
            WPO = "<WholeProgramOptimization>False</WholeProgramOptimization>";
        }
        else if (type.Equals("speed", StringComparison.OrdinalIgnoreCase))
        {
            WPO = "<WholeProgramOptimization>True</WholeProgramOptimization>";
        }
        else if (type.Equals("balanced", StringComparison.OrdinalIgnoreCase))
        {
            WPO = "";
        }
        if(!WPO.Equals(""))
        {
            String contentToWrite = "<PropertyGroup>"+ "\n" + "    "+WPO + "\n" +"</PropertyGroup>" + "\n" ;
            foreach (var item in allProjFile)
            {
                var fileContent = File.ReadAllText(item);
                //Start Writting Where Last PropertyGroup ends
                string afterWrite = "</PropertyGroup>";
                var index = fileContent.LastIndexOf(afterWrite, StringComparison.OrdinalIgnoreCase)+afterWrite.Length + 2;
                fileContent = fileContent.Insert(index, contentToWrite);
                File.WriteAllText(item, fileContent);
            }
        }

        Console.WriteLine("WholeProgramOptimization");

    }

    public static void OptimizationLevel(string type)
    {
        //Remove if already exists;
        //ItemGroupDefination-> ClCompile
        String WPO = "";
        var allProjFile = Directory.EnumerateFiles(finalDirectory, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".vcxproj"));

        foreach (var item in allProjFile)
        {
            var fileContent = File.ReadAllText(item);
            var patternForSearch = "<OptimizationLevel>[a-z,A-Z-+]*</OptimizationLevel>";
            var regSearch = Regex.Match(fileContent, patternForSearch, RegexOptions.IgnoreCase);
            if (regSearch.Success)
            {
                fileContent = fileContent.Replace(regSearch.Value, "", StringComparison.OrdinalIgnoreCase);
                File.WriteAllText(item, fileContent);
            }
        }

        if (type.Equals("speed", StringComparison.OrdinalIgnoreCase))
        {
            WPO = "<OptimizationLevel>O2</OptimizationLevel>";
        }
        else if (type.Equals("size", StringComparison.OrdinalIgnoreCase))
        {
            WPO = "<OptimizationLevel>O3</OptimizationLevel>";
        }
        else if (type.Equals("balanced", StringComparison.OrdinalIgnoreCase))
        {
            // WPO = "<OptimizationLevel>O1</OptimizationLevel>";
            WPO = "";
        }

        if (!WPO.Equals(""))
        {
            String contentToWrite = "<ItemDefinitionGroup> " + "\n" + "    <ClCompile>" + "        "+ "\n" +"        " +WPO + "\n" + "    </ClCompile> " + "\n" + "</ItemDefinitionGroup>" +  "\n";
            foreach (var item in allProjFile)
            {
                var fileContent = File.ReadAllText(item);
                //Start Writting Where Last PropertyGroup ends
                string afterWrite = "</PropertyGroup>";
                var index = fileContent.LastIndexOf(afterWrite, StringComparison.OrdinalIgnoreCase) + afterWrite.Length + 2;
                fileContent = fileContent.Insert(index, contentToWrite);
                File.WriteAllText(item, fileContent);
            }
        }
    }

    public static void Optimization(string type)
    {
       //  < ClCompile >
       //      < Optimization > MinSpace </ Optimization >
       // </ ClCompile >

        String WPO = "";
        var allProjFile = Directory.EnumerateFiles(finalDirectory, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".vcxproj"));

        foreach (var item in allProjFile)
        {
            var fileContent = File.ReadAllText(item);
            var patternForSearch = "<Optimization>[a-z,A-Z-+]*</Optimization>";
            var regSearch = Regex.Match(fileContent, patternForSearch, RegexOptions.IgnoreCase);
            if (regSearch.Success)
            {
                fileContent = fileContent.Replace(regSearch.Value, "", StringComparison.OrdinalIgnoreCase);
                File.WriteAllText(item, fileContent);
            }
        }


        if (type.Equals("speed", StringComparison.OrdinalIgnoreCase))
        {
            WPO = "<Optimization>MinSpeed</Optimization >";
        }
        else if (type.Equals("size", StringComparison.OrdinalIgnoreCase))
        {
            WPO = "<Optimization>MinSpace</Optimization>";
        }
        else if (type.Equals("balanced", StringComparison.OrdinalIgnoreCase))
        {
            WPO = "";
        }
        if(!WPO.Equals(""))
        {
            String contentToWrite = "<ItemDefinitionGroup> " + "\n" + "    <ClCompile>" + "\n" + "        "+WPO + "\n" + "    </ClCompile>" + "\n"+ "</ItemDefinitionGroup>" + "\n";
            foreach (var item in allProjFile)
            {
                var fileContent = File.ReadAllText(item);
                //Start Writting Where Last PropertyGroup ends
                string afterWrite = "</PropertyGroup>";
                var index = fileContent.LastIndexOf(afterWrite, StringComparison.OrdinalIgnoreCase) + afterWrite.Length + 2;
                fileContent = fileContent.Insert(index, contentToWrite);
                File.WriteAllText(item, fileContent);
            }
        }
        Console.WriteLine("Optimization");
    }



    public static void Main(string[] args)
    {

        var cmdArgParser = new FluentCommandLineParser<ApplicationArguments>();
        cmdArgParser.Setup(arg => arg.OptimizationType).As('c', "command").WithDescription("Compulsory. Specify operation that you wish " + HelpString);
        cmdArgParser.SetupHelp("?", "help").Callback(text => Console.WriteLine(text));
        var result = cmdArgParser.Parse(args);
        if (result.HasErrors)
        {
            Console.WriteLine("Invalid command .. \n");
            cmdArgParser.HelpOption.ShowHelp(cmdArgParser.Options);
            return;
        }
        PerformOperation(cmdArgParser);
        return;
    }
}
