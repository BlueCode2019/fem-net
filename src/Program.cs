﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace FEM_NET
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "femnet";
            app.HelpOption("-?|-h|--help");

            var meshArg = app.Argument("mesh", "Path to the mesh");

            var elementTypeOption = app.Option("-t|--type", "Finite element type", CommandOptionType.SingleValue);
            var accuracyOption = app.Option("-a|-accuracy", "Accuracy", CommandOptionType.SingleValue);
            
            app.OnExecute(() => {
                if (meshArg.Value == null)
                {
                    Console.WriteLine("ERROR: Missing mesh.");
                    return 1;
                }
                var meshPath = meshArg.Value;
                if (meshPath.EndsWith(".mesh"))
                    meshPath = meshPath.Substring(0, meshPath.Length - 5);

                var feType = elementTypeOption.HasValue() ? elementTypeOption.Value().ToLowerInvariant() : "p1";
                double accuracy = accuracyOption.HasValue() ? double.Parse(accuracyOption.Value()) : 1e-6;
                // TODO: Format error.

                Console.WriteLine("\nSolving...\n");
                FEM2D.HeatProgram.Run(meshPath, feType, accuracy);

                Console.WriteLine("\nPress ENTER to exit...");
                Console.ReadLine();
                return 0;
            });

            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine($"FILE NOT FOUND: {exception.Message}");
            }
            catch (DirectoryNotFoundException exception)
            {
                Console.WriteLine($"DIRECTORY NOT FOUND: {exception.Message}");
            }
        }
    }
}
