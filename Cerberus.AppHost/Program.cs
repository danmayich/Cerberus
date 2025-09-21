var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Cerberus_Api>("cerberus");

builder.Build().Run();
