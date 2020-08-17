using System;
using System.Threading.Tasks;

namespace AnagramSolver.SOAP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var calculator = new CalculatorService();

            while (true)
            {
                Console.WriteLine("What action do you want to perform? (+ - / *) x - to exit");
                var action = Console.ReadLine();
                if (action == "x")
                    break;
                Console.WriteLine("Enter 2 numbers");
                var num1 = int.Parse(Console.ReadLine());
                var num2 = int.Parse(Console.ReadLine());

                switch (action)
                {
                    case "+":
                        Console.WriteLine($"{num1} + {num2} = {await calculator.Add(num1, num2)}");
                        break;
                    case "-":
                        Console.WriteLine($"{num1} - {num2} = {await calculator.Substract(num1, num2)}");
                        break;
                    case "/":
                        Console.WriteLine($"{num1} / {num2} = {await calculator.Divide(num1, num2)}");
                        break;
                    case "*":
                        Console.WriteLine($"{num1} * {num2} = {await calculator.Multiply(num1, num2)}");
                        break;
                    default:
                        Console.WriteLine("You must enter one of the following actions: + - / *");
                        break;
                }
            }
        }
    }
}
