using System.Collections.Concurrent;
using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Abstractions;
using AtendeLogo.Shared.Constants;

namespace AtendeLogo.Shared.Helpers;

public static class LocalizationHelper
{
    private static readonly ConcurrentDictionary<Type, string> _cache = new();
    public static string GetResourceKey<T>()
    {
        if (_cache.TryGetValue(typeof(T), out var resourceKey))
        {
            return resourceKey;
        }
        resourceKey = GetResourceKeyInternal<T>();
        _cache.TryAdd(typeof(T), resourceKey);
        return resourceKey;
    }

    private static string GetResourceKeyInternal<T>()
    {
        var resourceKey = CaseConventionUtils.ToKebabCase(typeof(T).Name);
        var resourcePrefixName = GetResourcePrefix<T>();
        if(resourcePrefixName is null)
        {
            return resourceKey;
        }
        return $"{resourcePrefixName}-{resourceKey}";
    }

    private static string? GetResourcePrefix<T>()
    {
        var type = typeof(T);
        if (type.IsSubclassOf<ValueObjectBase>())
        {
            return "value-objects";
        }

        if (type.IsAssignableTo<IEntityBase>())
        {
            return "entities";
        }

        if (type.Namespace?.Contains(ApplicationNameConstants.TenantPortal) == true)
        {
            return "tenant-portal";
        }
        return null;
    }
}
