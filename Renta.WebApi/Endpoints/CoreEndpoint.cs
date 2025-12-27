using System;
using FastEndpoints;
using Renta.Domain.Settings;

namespace Renta.WebApi.Endpoints;

public class CoreEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse> where TRequest : notnull
{
    private readonly ThrottleSettings? _throttleSettings;

    public CoreEndpoint()
    {
        var configuration = Resolve<IConfiguration>();
        if (configuration != null)
            _throttleSettings = configuration.GetSection("Throttle").Get<ThrottleSettings>()!;
    }

    public override void Configure()
    {
        if (_throttleSettings != null)
            Throttle(_throttleSettings.HitLimit, _throttleSettings.DurationSeconds);
    }
}

public class CoreEndpoint<TRequest> : CoreEndpoint<TRequest, object?> where TRequest : notnull
{
}

public class CoreEndpointWithoutRequest<TResponse> : EndpointWithoutRequest<TResponse>
{
    private readonly ThrottleSettings? _throttleSettings;


    public CoreEndpointWithoutRequest()
    {
        var configuration = Resolve<IConfiguration>();
        if (configuration != null)
            _throttleSettings = configuration.GetSection("Throttle").Get<ThrottleSettings>()!;
    }

    public override void Configure()
    {
        if (_throttleSettings != null)
            Throttle(_throttleSettings.HitLimit, _throttleSettings.DurationSeconds);
    }
}
