public class LinearProgrammingModel
{
    public int NumConstraints { get; set; }
    public int NumVariables { get; set; }
    public double[] ObjectiveFunctionCoefficients { get; set; }
    public Constraint[] Constraints { get; set; }

    public class Constraint
    {
        public double[] Coefficients { get; set; }
        public double UpperBound { get; set; }
    }
}