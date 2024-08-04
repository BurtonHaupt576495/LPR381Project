# LPR381Project
Outline
Tip: do the assignment first.
For the project, create a program that solves Linear Programming and Integer Programming Models
and then analyses how the changes in an LP’s parameters change the optimal solution.
Supply the source code as a visual studio project. Any .NET programming language may be used. The
project should build an executable (solve.exe) that is menu driven with the following:
The program should be able to accept an input text file with the mathematical model and export all
results to an output text file.

Minimum Requirements Criteria
• Program should accept a random amount of decision variables.
• Program should accept a random amount of constraints.
• Use comments with programming.
• Programming Best Practices should be implemented.
Input Text File Criteria
The first line contains the following, seperated by spaces:
• The word max or min, to indicate whether it is a maximization or a minimization problem.
• For each decision variable, a operator to represent wheter the objective function coefficient is a
negative or positive.
• For each decision variable, a number to represent its objective function coefficient.
A line for each constraint:
• The operator of the technological coefficients for the decision variables, in the same order as in
the specification of the objective function in line 1, that represents whether the technological
coefficient is negative or positive.
• The technological coefficients for the decision variables, in the same order as in the specification
of the objective function in line 1.
• The relation used in the constraint, with =,<=, or >=, to indicate respectively, an inequality to
constraint the constaint right-hand-side.
• The right-hand-side of the constraint.
Sign Restrictions
• Sign restriction to be below all the constraints, seperated by a space, +, -, urs, int, bin, in the
same order as in the specification of the objective function in line 1.

Note: The Linear Programming Model or the Integer Programming Model should be entered into the
file. Not the canonical forms of the different algorithms respectivly or the Relaxed Linear
Programming Model.
