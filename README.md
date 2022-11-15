# OpenTelemetry ASP.Net Core 6  Web API Example

## Run the application
1. Configure and run a local collector

    Then run the docker command to acquire and run the collector based on this configuration:

    ```shell
    docker run -p 4318:4318 \
    -v ./otel-collector-config.yaml:/etc/otel-collector-config.yaml \
    otel/opentelemetry-collector:latest \
    --config=/etc/otel-collector-config.yaml
    ```

2. Run the application

    Run the application like before:

    ```shell
    dotnet restore
    ```
    ```shell
    dotnet build
    ```
    ```shell
    dotnet run
    ```