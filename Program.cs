// <copyright file="Program.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;

// Define some important constants to initialize tracing with
var serviceName = "MyCompany.MyProduct.MyService";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

// Configure to send data via the OTLP exporter.
// By default, it will send to port 4318, which the collector is listening on.
builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
    .AddOtlpExporter(opt =>
    {
        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
    })
    .AddSource(serviceName)
    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddSqlClientInstrumentation();
});

var app = builder.Build();

var MyActivitySource = new ActivitySource(serviceName);

app.MapGet("/hello", () =>
{
    // Track work inside of the request
    using var activity = MyActivitySource.StartActivity("SayHello");
    activity?.SetTag("foo", 1);
    activity?.SetTag("bar", "Hello, World!");
    activity?.SetTag("baz", new int[] { 1, 2, 3 });

    return "Hello, World!";
});

app.Run();
