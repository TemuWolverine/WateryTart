using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WateryTart.Core.ViewModels;

namespace WateryTart.Core;

public class ViewLocator : IDataTemplate
{
    private static readonly Dictionary<string, Type?> TypeCache = new();
    private static IEnumerable<Assembly>? _searchAssemblies;

    public ViewLocator()
    {
        // Cache assemblies to search on first instantiation
        _searchAssemblies ??= GetSearchAssemblies();
    }

    public Control Build(object? data)
    {
        if (data is null)
        {
            return new TextBlock { Text = "data was null" };
        }

        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = FindType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }
        else
        {
            return new TextBlock { Text = "Not Found: " + name };
        }
    }

    public bool Match(object? data)
    {
        return data is IViewModelBase;
    }

    private static Type? FindType(string fullTypeName)
    {
        // Check cache first
        if (TypeCache.TryGetValue(fullTypeName, out var cachedType))
        {
            return cachedType;
        }

        // Try direct lookup (same assembly)
        var type = Type.GetType(fullTypeName);
        if (type != null)
        {
            TypeCache[fullTypeName] = type;
            return type;
        }

        // Search through other assemblies
        if (_searchAssemblies != null)
        {
            foreach (var assembly in _searchAssemblies)
            {
                try
                {
                    type = assembly.GetType(fullTypeName, false);
                    if (type != null)
                    {
                        TypeCache[fullTypeName] = type;
                        return type;
                    }
                }
                catch
                {
                    // Continue searching if this assembly fails
                }
            }
        }

        // Cache miss
        TypeCache[fullTypeName] = null;
        return null;
    }

    private static IEnumerable<Assembly> GetSearchAssemblies()
    {
        var assemblies = new HashSet<Assembly>();

        // Add all currently loaded assemblies
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Filter to your project assemblies to avoid searching everything
            if (assembly.GetName().Name?.StartsWith("WateryTart") == true)
            {
                assemblies.Add(assembly);
            }
        }

        return assemblies;
    }

    /// <summary>
    /// Register additional assemblies to search for views.
    /// Call this during application startup if needed.
    /// </summary>
    public static void RegisterAssembly(Assembly assembly)
    {
        if (_searchAssemblies is HashSet<Assembly> set)
        {
            set.Add(assembly);
        }
    }

    /// <summary>
    /// Clear the type cache. Useful for testing or dynamic reloading.
    /// </summary>
    public static void ClearCache()
    {
        TypeCache.Clear();
    }
}