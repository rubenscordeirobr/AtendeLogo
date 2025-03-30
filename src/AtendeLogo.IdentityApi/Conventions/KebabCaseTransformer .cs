﻿namespace AtendeLogo.IdentityApi.Conventions;

public partial class KebabCaseTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        var result = value?.ToString();
        if (string.IsNullOrEmpty(result))
            return null;

        return CaseConventionUtils.ToKebabCase(result);
    }
}
