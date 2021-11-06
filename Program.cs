using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace files_module
{
  class Program
  {
    class SalesData
    {
      public double Total { get; set; }
    }

    static IEnumerable<string> FindFiles(string folderName)
    {
      List<string> salesFiles = new List<string>();

      var foundFiles = Directory.EnumerateFiles(
        folderName,
        "*",
        SearchOption.AllDirectories
      );

      foreach (var file in foundFiles)
      {
        // The file name will contain the full path, so only check the end of it
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
          salesFiles.Add(file);
        }
      }

      return salesFiles;
    }

    static double CalculateSalesTotal(IEnumerable<string> salesFiles)
    {
      double salesTotal = 0;

      // Loop over each file path in salesFiles
      foreach (var file in salesFiles)
      {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData data = JsonConvert.DeserializeObject<SalesData>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data.Total;
      }

      return salesTotal;
    }

    static void Main(string[] args)
    {
      var currentDirectory = Directory.GetCurrentDirectory();
      var storesDirectory = Path.Combine(currentDirectory, "stores");
      var salesTotalsDir = Path.Combine(currentDirectory, "salesTotalDir");

      Directory.CreateDirectory(salesTotalsDir);

      var salesFiles = FindFiles(storesDirectory);

      var salesTotal = CalculateSalesTotal(salesFiles);

      File.WriteAllText(Path.Combine(salesTotalsDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");
    }
  }
}
