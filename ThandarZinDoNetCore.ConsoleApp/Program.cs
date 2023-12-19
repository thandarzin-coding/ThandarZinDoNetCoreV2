// See https://aka.ms/new-console-template for more information
using ThandarZinDoNetCore.ConsoleApp.AdoDoNetExample;
using ThandarZinDoNetCore.ConsoleApp.DapperExamples;

Console.WriteLine("Hello, World!");


//AdoDoNetExample adoDoNetExample = new AdoDoNetExample();
//adoDoNetExample.Run();

DapperExampler dapper = new DapperExampler();
dapper.Run();
Console.ReadKey();