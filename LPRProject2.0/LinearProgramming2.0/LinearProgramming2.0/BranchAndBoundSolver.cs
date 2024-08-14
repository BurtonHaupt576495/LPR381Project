using System.Collections.Generic;
using System.Linq;
using System;

public class BranchAndBoundSolver
{
    private static List<(Dictionary<int, double> Solution, double ObjectiveValue)> candidateSolutions = new List<(Dictionary<int, double> Solution, double ObjectiveValue)>();
    private static List<LinearProgrammingModel> activeSubproblems = new List<LinearProgrammingModel>();
    private static HashSet<string> processedSubproblems = new HashSet<string>();

    public static void Solve(LinearProgrammingModel model)
    {
        if (model == null)
        {
            Console.WriteLine("Error: The model is null.");
            return;
        }

        // Initialize the first node
        activeSubproblems.Add(model);
        ProcessSubproblems();
        DisplayCandidateSolutions();
    }

    private static void ProcessSubproblems()
    {
        while (activeSubproblems.Count > 0)
        {
            var currentSubproblem = activeSubproblems[0];
            activeSubproblems.RemoveAt(0);

            var subproblemId = GenerateSubproblemId(currentSubproblem);
            if (processedSubproblems.Contains(subproblemId))
            {
                Console.WriteLine($"Skipping previously processed subproblem with ID: {subproblemId}");
                continue;
            }

            Console.WriteLine("Processing a new subproblem...");
            PrintModel(currentSubproblem);

            // Solve the current subproblem
            var result = SimplexSolver.PrimalSimplexSolver(currentSubproblem);
            if (result != null)
            {
                var (solution, objectiveValue) = result;
                Console.WriteLine($"Solution found with objective value: {objectiveValue}");

                if (IsIntegerSolution(solution))
                {
                    candidateSolutions.Add((solution, objectiveValue));
                    Console.WriteLine("Integer solution found and added to candidates.");
                }
                else
                {
                    // Generate new subproblems with updated constraints
                    GenerateSubproblems(currentSubproblem, solution);
                }
            }
            else
            {
                Console.WriteLine("No solution found for the current subproblem.");
            }

            processedSubproblems.Add(subproblemId);
        }
    }

    private static void GenerateSubproblems(LinearProgrammingModel model, Dictionary<int, double> solution)
    {
        var fractionalVariable = solution.FirstOrDefault(x => Math.Abs(x.Value - Math.Round(x.Value)) > 1e-6);
        if (fractionalVariable.Equals(default(KeyValuePair<int, double>)))
        {
            Console.WriteLine("No fractional variable found. Branching complete.");
            return;
        }

        int varIndex = fractionalVariable.Key;
        double fractionalValue = fractionalVariable.Value;

        Console.WriteLine($"Branching on variable x{varIndex + 1} with value {fractionalValue}");

        var lowerBoundModel = CloneModel(model);
        var upperBoundModel = CloneModel(model);

        AddConstraint(lowerBoundModel, varIndex, Math.Floor(fractionalValue), true);
        AddConstraint(upperBoundModel, varIndex, Math.Ceiling(fractionalValue), false);

        Console.WriteLine($"Lower bound constraint added: x{varIndex + 1} <= {Math.Floor(fractionalValue)}");
        Console.WriteLine($"Upper bound constraint added: x{varIndex + 1} >= {Math.Ceiling(fractionalValue)}");

        // Instead of directly adding to the list, recursively process the new subproblems
        activeSubproblems.Add(lowerBoundModel);
        activeSubproblems.Add(upperBoundModel);

        ProcessSubproblems();
    }

    private static LinearProgrammingModel CloneModel(LinearProgrammingModel model)
    {
        return new LinearProgrammingModel
        {
            NumVariables = model.NumVariables,
            NumConstraints = model.NumConstraints,
            ObjectiveFunctionCoefficients = (double[])model.ObjectiveFunctionCoefficients.Clone(),
            Constraints = model.Constraints.Select(c => new LinearProgrammingModel.Constraint
            {
                Coefficients = (double[])c.Coefficients.Clone(),
                UpperBound = c.UpperBound
            }).ToArray()
        };
    }

    private static void AddConstraint(LinearProgrammingModel model, int varIndex, double value, bool isUpperBound)
    {
        var newConstraint = new LinearProgrammingModel.Constraint
        {
            Coefficients = new double[model.NumVariables],
            UpperBound = isUpperBound ? value : -value
        };

        newConstraint.Coefficients[varIndex] = isUpperBound ? 1.0 : -1.0;

        // Add the new constraint to the model
        model.Constraints = model.Constraints.Append(newConstraint).ToArray();
    }

    private static bool IsIntegerSolution(Dictionary<int, double> solution)
    {
        // Check if all variables in the solution are integers
        return solution.All(x => Math.Abs(x.Value - Math.Round(x.Value)) < 1e-6);
    }

    private static void DisplayCandidateSolutions()
    {
        if (candidateSolutions.Count == 0)
        {
            Console.WriteLine("No candidate solutions found.");
            return;
        }

        // Sort the solutions by the objective value in descending order
        var sortedSolutions = candidateSolutions
            .OrderByDescending(candidate => candidate.ObjectiveValue)
            .ToList();

        Console.WriteLine("\nAll Candidate Solutions:");
        foreach (var candidate in sortedSolutions)
        {
            Console.WriteLine("Solution:");
            foreach (var var in candidate.Solution)
            {
                Console.WriteLine($"x{var.Key + 1} = {var.Value}");
            }
            Console.WriteLine($"Objective Value = {candidate.ObjectiveValue:F2}");
            Console.WriteLine();
        }

        // Display the optimal solution
        var optimalSolution = sortedSolutions.First();
        Console.WriteLine("Optimal Solution:");
        foreach (var var in optimalSolution.Solution)
        {
            Console.WriteLine($"x{var.Key + 1} = {var.Value}");
        }
        Console.WriteLine($"Objective Value = {optimalSolution.ObjectiveValue:F2}");
        Console.WriteLine();
        Console.WriteLine();
    }

    private static string GenerateSubproblemId(LinearProgrammingModel model)
    {
        // Generate a unique ID based on the constraints of the model
        return string.Join("|", model.Constraints.Select(c => string.Join(",", c.Coefficients) + ":" + c.UpperBound));
    }

    private static void PrintModel(LinearProgrammingModel model)
    {
        Console.WriteLine("Model Details:");
        Console.WriteLine($"Objective Function Coefficients: {string.Join(", ", model.ObjectiveFunctionCoefficients)}");
        foreach (var constraint in model.Constraints)
        {
            Console.WriteLine($"Constraint: {string.Join(", ", constraint.Coefficients)} <= {constraint.UpperBound}");
        }
    }
}
