// <copyright file="Program.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace POQ.CodingChallenge.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}