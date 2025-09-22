using k8s.Models;


var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Cerberus_Api>("cerberus");

builder.AddNpmApp(name: "vue", workingDirectory: "../Cerberus.UI")
.WithReference(api)
.WaitFor(api)
.WithHttpEndpoint(env: "PORT")
.WithExternalHttpEndpoints();


builder.Build().Run();
