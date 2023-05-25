using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Tomat.Tetron.Avalonia.Services;

/// <summary>
///     Handles saving and loading configuration files.
/// </summary>
public sealed class ConfigurationService {
    // TODO: Comply with XDG and other stuff.
    private static readonly string save_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "tetron");

    private static string GetConfigPath(MemberInfo type) {
        var path = Path.Combine(save_path, $"{type.Name}.json");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        return path;
    }

    private readonly Dictionary<Type, object?> configs = new();

    public void SaveConfig<T>() {
        SaveConfig(typeof(T));
    }

    public void SaveConfig(Type type) {
        if (!configs.ContainsKey(type))
            throw new InvalidOperationException($"Configuration for {type.Name} is null.");

        var path = GetConfigPath(type);
        File.WriteAllText(path, JsonConvert.SerializeObject(configs[type]));
    }

    public void SaveConfigs() {
        foreach (var configType in configs.Keys)
            SaveConfig(configType);
    }

    public void RegisterConfig<T>(T config) {
        configs[typeof(T)] = config;
    }

    public void RegisterConfig(object config, Type type) {
        configs[type] = config;
    }

    public T? GetConfig<T>() {
        return (T?)GetConfig(typeof(T));
    }

    public object? GetConfig(Type type) {
        if (configs.TryGetValue(type, out var value))
            return value;

        var path = GetConfigPath(type);
        if (!File.Exists(path))
            return default;

        var config = JsonConvert.DeserializeObject(File.ReadAllText(path), type);
        if (config is null)
            return default;

        RegisterConfig(config, type);
        return config;
    }

    public T RequireConfig<T>() {
        return GetConfig<T>() ?? throw new InvalidOperationException($"Configuration for {typeof(T).Name} is null.");
    }
}
