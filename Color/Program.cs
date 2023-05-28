// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Color;
using System.Drawing;
using System.Security.Cryptography;

//BenchmarkRunner.Run<Benchmark>();
Console.WriteLine("Hello, World!");
Benchmark Benchmark = new Benchmark();
Benchmark.MainForm(out var col0);
Console.WriteLine(col0);
//MainFce(out var col01, out var col11, out var col21);
//MainFceJava(out var col32);
//MainNew(out var col0, out var col1, out var col2, out var col3);
//
//Console.WriteLine(col0);
//Console.WriteLine(col1);
//Console.WriteLine(col2);
//Console.WriteLine(col3);
//
//Console.WriteLine(col01);
//Console.WriteLine(col11);
//Console.WriteLine(col21);
//
//Console.WriteLine(col32);