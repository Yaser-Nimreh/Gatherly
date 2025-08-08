using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Options.JwtToken;

public sealed class ConfigureJwtTokenOptions(IConfiguration configuration) : IConfigureOptions<JwtTokenOptions>
{
    private readonly IConfiguration _configuration = configuration;
    private const string SectionName = nameof(JwtTokenOptions);

    public void Configure(JwtTokenOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}