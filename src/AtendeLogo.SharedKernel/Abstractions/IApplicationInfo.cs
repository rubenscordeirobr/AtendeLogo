﻿namespace AtendeLogo.Shared.Abstractions;

public interface IApplicationInfo
{
    string ApplicationName { get; }
    string Title { get; }
    Version ApplicationVersion { get; }
    string Environment { get; }
}
