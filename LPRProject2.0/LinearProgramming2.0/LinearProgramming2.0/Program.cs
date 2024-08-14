
using System;
using System.Reflection;

class Program
{
   
    static void Main(string[] args)
    {
        string filePath = "C:\\Users\\User\\Desktop\\Belgium\\LPR381\\LPRProject2.0\\LinearProgramming2.0\\LinearProgramming2.0\\input.txt"; // Path to your file
        var model = FileHandler.ReadModelFromFile(filePath);

        //BranchAndBoundSolver.Solve(model);
        //SimplexSolver.PrimalSimplex(model);
        BranchAndBoundSolver.Solve(model);
        

        if (model == null)
        {
            Console.WriteLine("Failed to load the model. Exiting.");
            return;
        }

    //    while (true)
    //    {
    //        Console.Clear();
    //        Console.WriteLine("Main Menu:");
    //        Console.WriteLine("1. Display Linear Programming Model");
    //        Console.WriteLine("2. Perform Simplex Algorithm");
    //        Console.WriteLine("3. Exit");

    //        if (Enum.TryParse(Console.ReadLine(), out MainMenuOption mainMenuChoice))
    //        {
    //            Console.WriteLine(mainMenuChoice);
    //            switch (mainMenuChoice)
    //            {
    //                case MainMenuOption.DisplayModel:
    //                    DisplayModelMenu(model);
    //                    break;
    //                case MainMenuOption.PerformSimplex:

    //                    displayalgorithms(model);
    //                    break;
    //                case MainMenuOption.Exit:
    //                    Console.WriteLine("Exiting...");
    //                    return;
    //                default:
    //                    Console.WriteLine("Invalid option. Please try again.");
    //                    break;
    //            }
    //        }
    //        else
    //        {
    //            Console.WriteLine("Invalid input. Please enter a number.");
    //        }
    //    }
    }

    static void DisplayModelMenu(LinearProgrammingModel model)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Display Model Menu:");
            Console.WriteLine("1. Display Canonical Form");
            Console.WriteLine("2. Return to Main Menu");

            if (Enum.TryParse(Console.ReadLine(), out SubMenuOption subMenuChoice))
            {
                switch (subMenuChoice)
                {
                    case SubMenuOption.DisplayCanonicalForm:
                        DisplayCanonicalForm(model);
                        break;
                    case SubMenuOption.ReturnToMainMenu:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }
    static void displayalgorithms(LinearProgrammingModel model)

    {
          //SimplexSolver psimplex = new SimplexSolver();
        try 
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Display Algorithms:");
                Console.WriteLine("1. Display Primal Simplex");
                Console.WriteLine("2. Display DualSimplex");
                Console.WriteLine("3. Display Branch and Bound");
                Console.WriteLine("4. Display Knapsack");

                if (Enum.TryParse(Console.ReadLine(), out algorithms algorithms))
                {

                    switch (algorithms)
                    {
                        case algorithms.primalSimplex:
                            //if (model != null)
                            //{
                            //    if (algorithms == algorithms.primalSimplex)
                            //    {
                            //        SimplexSolver.PrimalSimplex(model);
                            //    }
                            //}
                            
                            return;
                        case algorithms.dualSimplex:
                            break;
                        case algorithms.branchandbound:
                            break;
                        case algorithms.knapsack:
                            break;
                        case algorithms.Exit:
                            Console.WriteLine("Exiting...");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("error" + ex.Message);
        }
    }

    static void DisplayCanonicalForm(LinearProgrammingModel model)
    {
        Console.Clear();
        Console.WriteLine("Canonical Form:");
        Console.WriteLine($"Objective Function: z = {string.Join(" + ", model.ObjectiveFunctionCoefficients)}");

        for (int i = 0; i < model.NumConstraints; i++)
        {
            var constraint = model.Constraints[i];
            Console.WriteLine($"{string.Join(" + ", constraint.Coefficients)} ≤ {constraint.UpperBound}");
        }

        Console.WriteLine("\nPress any key to return to the Display Model Menu...");
        Console.ReadKey();
    }


}
