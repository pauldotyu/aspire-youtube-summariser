var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");
var storage = builder.AddAzureStorage("storage");
var queue = storage.AddQueues("queue");
var table = storage.AddTables("table");

var apiapp = builder.AddProject<Projects.AspireYouTubeSummariser_ApiApp>("apiapp")
                    .WithReference(table);

builder.AddProject<Projects.AspireYouTubeSummariser_WebApp>("webapp")
       .WithReference(cache)
       .WithReference(queue)
       .WithReference(apiapp);

builder.AddProject<Projects.AspireYouTubeSummariser_Worker>("worker")
       .WithReference(queue)
       .WithReference(table);

builder.Build().Run();
