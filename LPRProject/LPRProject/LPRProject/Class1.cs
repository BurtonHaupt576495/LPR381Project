
using System;
using System.Collections.Generic;

public class LinearProgrammingModel
{
    public double[] ObjectiveCoefficients { get; set; }
    public List<double[]> Constraints { get; set; }
    public List<double> ConstraintValues { get; set; }
    public List<string> ConstraintTypes { get; set; }

    public LinearProgrammingModel()
    {
        Constraints = new List<double[]>();
        ConstraintValues = new List<double>();
        ConstraintTypes = new List<string>();
    }
}