using System;
using System.IO;
using System.Collections.Generic;

public class FileHandler
{
    public static LinearProgrammingModel ReadModelFromFile(string filePath)
    {
        var model = new LinearProgrammingModel
        {
            ObjectiveFunctionCoefficients = new double[0], // Initialize with empty array
            Constraints = new LinearProgrammingModel.Constraint[0] // Initialize with empty array
        };

        try
        {
            var coefficientsList = new List<double>();
            var constraintsList = new List<LinearProgrammingModel.Constraint>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                // Read the objective function line
                string objectiveLine = reader.ReadLine().Trim();
                if (objectiveLine != null && objectiveLine.StartsWith("max"))
                {
                    string coefficientsPart = objectiveLine.Substring(objectiveLine.IndexOf('=') + 1).Trim();
                    string[] terms = coefficientsPart.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string term in terms)
                    {
                        if (double.TryParse(term.Trim(), out double coefficient))
                        {
                            coefficientsList.Add(coefficient);
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Unable to parse coefficient '{term.Trim()}'.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error: The first line does not contain a valid objective function.");
                    return null;
                }

                // Read constraints
                string line;
                while ((line = reader.ReadLine()?.Trim()) != null)
                {
                    if (line.Contains("≤"))
                    {
                        var parts = line.Split(new[] { '≤' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 2 && double.TryParse(parts[1].Trim(), out double upperBound))
                        {
                            var constraint = new LinearProgrammingModel.Constraint
                            {
                                UpperBound = upperBound,
                                Coefficients = new double[0] // Initialize with empty array
                            };

                            string constraintsPart = parts[0].Trim();
                            string[] coefficients = constraintsPart.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                            var coefficientsListForConstraint = new List<double>();
                            foreach (string coefficient in coefficients)
                            {
                                if (double.TryParse(coefficient.Trim(), out double termCoefficient))
                                {
                                    coefficientsListForConstraint.Add(termCoefficient);
                                }
                                else
                                {
                                    Console.WriteLine($"Warning: Unable to parse constraint coefficient '{coefficient.Trim()}'.");
                                }
                            }
                            constraint.Coefficients = coefficientsListForConstraint.ToArray();
                            constraintsList.Add(constraint);
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Invalid constraint upper bound '{parts[1].Trim()}'.");
                        }
                    }
                }

                // Set the final values for the model
                model.ObjectiveFunctionCoefficients = coefficientsList.ToArray();
                model.NumVariables = model.ObjectiveFunctionCoefficients.Length;
                model.Constraints = constraintsList.ToArray();
                model.NumConstraints = model.Constraints.Length;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading the file: " + ex.Message);
        }

        return model;
    }
}
