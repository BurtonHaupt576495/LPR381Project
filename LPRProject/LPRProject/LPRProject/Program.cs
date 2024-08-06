using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string filePath = "@\"C:\\Desktop\\Belgium\\LPR381\\LPRProject\\input.txt";
        try
        {
            LinearProgrammingModel model = ReadLinearProgrammingModel(filePath);
            DisplayModel(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }


    static LinearProgrammingModel ReadLinearProgrammingModel(string filePath)
    {
        var model = new LinearProgrammingModel();
        try
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(filePath);

            if (lines.Length < 4) // Basic check for file length (adjust based on format)
            {
                throw new FormatException("The file format is incorrect or the file is too short.");
            }

            // Parse Objective Coefficients
            var objectiveLine = lines[0].Split(':')[1].Trim();
            model.ObjectiveCoefficients = Array.ConvertAll(objectiveLine.Split(' '), double.Parse);

            // Parse Constraints
            for (int i = 1; i < lines.Length - 2; i++)
            {
                var parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 3)
                {
                    throw new FormatException($"Constraint line format is incorrect: {lines[i]}");
                }

                var constraintCoefficients = Array.ConvertAll(parts.Take(parts.Length - 2).ToArray(), double.Parse);
                var constraintValue = double.Parse(parts[parts.Length - 1]);
                model.Constraints.Add(constraintCoefficients);
                model.ConstraintValues.Add(constraintValue);
            }

            // Parse Constraint Types
            var typesLine = lines[lines.Length - 2].Split(':')[1].Trim();
            model.ConstraintTypes = new List<string>(typesLine.Split(' '));

            return model;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException("The specified file was not found.");
        }
        catch (FormatException ex)
        {
            throw new FormatException("There was an error parsing the file. Please check the file format.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while reading the file.", ex);
        }
    }

    static void DisplayModel(LinearProgrammingModel model)
    {
        Console.WriteLine("Objective Coefficients:");
        foreach (var coef in model.ObjectiveCoefficients)
        {
            Console.Write(coef + " ");
        }
        Console.WriteLine();

        Console.WriteLine("Constraints:");
        for (int i = 0; i < model.Constraints.Count; i++)
        {
            Console.Write("Constraint " + i + ": ");
            foreach (var val in model.Constraints[i])
            {
                Console.Write(val + " ");
            }
            Console.WriteLine(" <= " + model.ConstraintValues[i] + " (" + model.ConstraintTypes[i] + ")");
        }
    }
}

