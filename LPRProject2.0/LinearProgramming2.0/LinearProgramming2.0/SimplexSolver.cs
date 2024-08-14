using System;
using System.Collections.Generic;
using System.Linq;

public class SimplexSolver
{
    public static void PrimalSimplex(LinearProgrammingModel model)
    {
        int numVariables = model.NumVariables;
        int numConstraints = model.NumConstraints;

        // Initialize tableau
        int numRows = numConstraints + 1;
        int numCols = numVariables + numConstraints + 1;
        double[,] tableau = new double[numRows, numCols];

        // Set objective function row (last row)
        for (int i = 0; i < numVariables; i++)
        {
            tableau[numConstraints, i] = -model.ObjectiveFunctionCoefficients[i];
        }

        // Set constraints
        for (int i = 0; i < numConstraints; i++)
        {
            var constraint = model.Constraints[i];
            for (int j = 0; j < numVariables; j++)
            {
                if (j < constraint.Coefficients.Length)
                {
                    tableau[i, j] = constraint.Coefficients[j];
                }
            }
            tableau[i, numVariables + i] = 1; // Identity matrix for slack variables
            tableau[i, numCols - 1] = constraint.UpperBound; // Right-hand side
        }

        Console.WriteLine("Initial Tableau:");
        DisplayTableau(tableau);

        // Perform simplex algorithm
        while (true)
        {
            int pivotColumn = -1;
            for (int j = 0; j < numCols - 1; j++)
            {
                if (tableau[numConstraints, j] < 0)
                {
                    pivotColumn = j;
                    break;
                }
            }

            if (pivotColumn == -1)
            {
                Console.WriteLine("Optimal solution found.");
                break;
            }

            int pivotRow = -1;
            double minRatio = double.MaxValue;
            for (int i = 0; i < numConstraints; i++)
            {
                if (tableau[i, pivotColumn] > 0)
                {
                    double ratio = tableau[i, numCols - 1] / tableau[i, pivotColumn];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }

            if (pivotRow == -1)
            {
                Console.WriteLine("Unbounded solution.");
                return;
            }

            // Perform pivoting
            double pivotValue = tableau[pivotRow, pivotColumn];
            for (int j = 0; j < numCols; j++)
            {
                tableau[pivotRow, j] /= pivotValue;
            }

            for (int i = 0; i < numRows; i++)
            {
                if (i != pivotRow)
                {
                    double ratio = tableau[i, pivotColumn];
                    for (int j = 0; j < numCols; j++)
                    {
                        tableau[i, j] -= ratio * tableau[pivotRow, j];
                    }
                }
            }

            Console.WriteLine("\nTableau after pivot:");
            DisplayTableau(tableau);
        }

        // Output results
        Console.WriteLine("\nOptimal Solution:");
        for (int i = 0; i < numVariables; i++)
        {
            double value = 0;
            for (int j = 0; j < numConstraints; j++)
            {
                if (tableau[j, i] != 0)
                {
                    value = tableau[j, numCols - 1];
                    break;
                }
            }
            Console.WriteLine($"x{i + 1} = {value}");
        }
        Console.WriteLine($"Objective value = {Math.Abs(-tableau[numConstraints, numCols - 1])}");
    }

    public static Tuple<Dictionary<int, double>, double> PrimalSimplexSolver(LinearProgrammingModel model)
    {
        int numVariables = model.NumVariables;
        int numConstraints = model.NumConstraints;

        // Initialize tableau
        double[,] tableau = new double[numConstraints + 1, numVariables + numConstraints + 1];
        for (int i = 0; i < numConstraints; i++)
        {
            for (int j = 0; j < numVariables; j++)
            {
                tableau[i, j] = model.Constraints[i].Coefficients[j];
            }
            tableau[i, numVariables + i] = 1;
            tableau[i, tableau.GetLength(1) - 1] = model.Constraints[i].UpperBound;
        }
        for (int i = 0; i < numVariables; i++)
        {
            tableau[numConstraints, i] = -model.ObjectiveFunctionCoefficients[i];
        }

        while (true)
        {
            int pivotColumn = -1;
            for (int j = 0; j < numVariables + numConstraints; j++)
            {
                if (tableau[numConstraints, j] < 0)
                {
                    pivotColumn = j;
                    break;
                }
            }

            if (pivotColumn == -1)
            {
                break;
            }

            int pivotRow = -1;
            double minRatio = double.MaxValue;
            for (int i = 0; i < numConstraints; i++)
            {
                if (tableau[i, pivotColumn] > 0)
                {
                    double ratio = tableau[i, tableau.GetLength(1) - 1] / tableau[i, pivotColumn];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }

            if (pivotRow == -1)
            {
                throw new InvalidOperationException("Unbounded solution.");
            }

            double pivotValue = tableau[pivotRow, pivotColumn];
            for (int j = 0; j < tableau.GetLength(1); j++)
            {
                tableau[pivotRow, j] /= pivotValue;
            }
            for (int i = 0; i < tableau.GetLength(0); i++)
            {
                if (i == pivotRow) continue;
                double factor = tableau[i, pivotColumn];
                for (int j = 0; j < tableau.GetLength(1); j++)
                {
                    tableau[i, j] -= factor * tableau[pivotRow, j];
                }
            }
        }

        var solution = new Dictionary<int, double>();
        for (int i = 0; i < numVariables; i++)
        {
            solution[i] = 0;
            for (int j = 0; j < numConstraints; j++)
            {
                if (tableau[j, i] == 1)
                {
                    solution[i] = tableau[j, tableau.GetLength(1) - 1];
                }
            }
        }
        double objectiveValue = -tableau[numConstraints, tableau.GetLength(1) - 1];

        return Tuple.Create(solution, objectiveValue);
    }

    private static void DisplayTableau(double[,] tableau)
    {
        int numRows = tableau.GetLength(0);
        int numCols = tableau.GetLength(1);

        // Print header
        Console.Write("        ");
        for (int j = 0; j < numCols - 1; j++)
        {
            Console.Write($"x{j + 1,6} ");
        }
        Console.WriteLine(" RHS ");

        // Print rows
        for (int i = 0; i < numRows; i++)
        {
            Console.Write($"Row{i + 1,2}: ");
            for (int j = 0; j < numCols; j++)
            {
                Console.Write($"{tableau[i, j],6:F2} ");
            }
            Console.WriteLine();
        }
    }
}