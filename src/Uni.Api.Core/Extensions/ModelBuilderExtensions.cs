using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Uni.Api.Core.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyAllConfigurationsFromAssemblyContaining<T>([NotNull] this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            var assembly = typeof(T).GetTypeInfo().Assembly;

            ApplyAllConfigurationsFromAssemblyInternal(modelBuilder, assembly);
        }

        private static void ApplyAllConfigurationsFromAssemblyInternal(ModelBuilder modelBuilder, Assembly assembly)
        {
            var modelBuilderType = typeof(ModelBuilder);
            var entityTypeConfigurationInterfaceType = typeof(IEntityTypeConfiguration<>);
            const string applyConfigurationMethodName = nameof(ModelBuilder.ApplyConfiguration);

            var methods = modelBuilderType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            var applyConfigurationMethod = methods
                .Where(methodInfo => methodInfo.Name == applyConfigurationMethodName)
                .Single(
                    methodInfo =>
                    {
                        var parameters = methodInfo.GetParameters();
                        if (parameters.Length != 1)
                        {
                            return false;
                        }


                        var type = parameters[0].ParameterType.GetGenericTypeDefinition();

                        return type == entityTypeConfigurationInterfaceType;
                    }
                );

            var nonAbstractNeitherGenericClassTypes = assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && !x.ContainsGenericParameters);

            foreach (var type in nonAbstractNeitherGenericClassTypes)
            {
                var @class = type.GetInterfaces().SingleOrDefault(
                    x => x.IsConstructedGenericType &&
                         x.GetGenericTypeDefinition() == entityTypeConfigurationInterfaceType
                );

                if (@class == null)
                {
                    continue;
                }

                var classGenericTypeArgument = @class.GenericTypeArguments[0];

                var applyConcreteMethod = applyConfigurationMethod.MakeGenericMethod(classGenericTypeArgument);

                var instance = Activator.CreateInstance(type);

                var parameters = new[] {instance};

                applyConcreteMethod.Invoke(modelBuilder, parameters);
            }
        }
    }
}
