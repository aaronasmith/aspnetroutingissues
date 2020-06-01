using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.AspNetCore;
using System.Linq;

namespace RoutingIssues {
    public static class NSwagServiceCollectionExtensions {
        public static IServiceCollection AddVersionedSwaggerDocuments(this IServiceCollection services) {
            var apiVersionDescriptionProviderService = services.SingleOrDefault(s => s.ServiceType == typeof(IApiVersionDescriptionProvider));

            if (apiVersionDescriptionProviderService == null) return services;

            // TODO: Figure out a way to access the version description provider without building the service provider here, or figure out how to inject these after the service provider is built.
            using (var serviceProvider = services.BuildServiceProvider()) {
                var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion)) {
                    services.AddSwaggerDocument(document => {
                        document.Title = "Program Specification API";
                        document.Description = "API for integrating program specifications.";
                        document.DocumentName = description.GroupName;
                        document.ApiGroupNames = new[] { description.GroupName };
                        document.Version = description.ApiVersion.ToString();

                        document.AddOperationFilter(genericContext => {
                            // TODO: This should work by default, but doesn't.  Remove this once this is working correctly in the library.
                            var context = genericContext as AspNetCoreOperationProcessorContext;

                            var apiDescription = context.ApiDescription;

                            var operation = context.OperationDescription.Operation;

                            operation.IsDeprecated |= apiDescription.IsDeprecated();

                            // TODO: We can replace this with 'return true' once the default version is no longer aloud.  This prevents the default versions showing up in the specs.
                            var apiVersion = apiDescription.GetApiVersion();
                            var version = (context.Settings as AspNetCoreOpenApiDocumentGeneratorSettings).DocumentName;

                            return version == $"v{apiVersion}" && apiDescription.RelativePath.Contains(version);
                        });
                    });
                }
            }

            return services;
        }
    }
}
