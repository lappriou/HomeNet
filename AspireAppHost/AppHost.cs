var builder = DistributedApplication.CreateBuilder(args);

var blazorAppBuilder = builder.AddProject<Projects.Applications_BlazorApp>("BlazorApp");
builder.Build().Run();
