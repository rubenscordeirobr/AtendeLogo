using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", 
    Justification = "The 'Error' record is only used internally within this project and will not be exposed to other languages.", 
    Scope = "member", 
    Target = "~P:AtendeLogo.Common.IResultValue.Error")]

[assembly: SuppressMessage("Security", 
    "CA5351:Do Not Use Broken Cryptographic Algorithms", 
    Justification = "MD5 is used solely for file integrity verification when files are uploaded.", 
    Scope = "member",
    Target = "~M:AtendeLogo.Common.Helpers.HashHelper.GenerateMd5GuidHash(System.Byte[])~System.Guid")]

[assembly: SuppressMessage("Naming",
    "CA1716:Identifiers should not match keywords", 
    Justification = "The 'Error' record is only used internally within this project and will not be exposed to other languages.", Scope = "type", Target = "~T:AtendeLogo.Common.Error")]

[assembly: SuppressMessage("Performance", "CA1819:Properties should not return arrays", 
    Justification = "This property is metadata only. ",
    Scope = "member",
    Target = "~P:AtendeLogo.Common.Infos.PhoneNumberFormatInfo.AlternateNationalFormats")]

[assembly: SuppressMessage("Design", "CA1024:Use properties where appropriate", 
    Justification = "Property must not throw exception. Why this is method", 
    Scope = "member", 
    Target = "~M:AtendeLogo.Common.Result`1.GetRequiredValue~`0")]

[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", 
    Justification = "<Pending>", 
    Scope = "member", Target = "~M:AtendeLogo.Common.Utils.RandomUtils.GenerateRandomNumber(System.Int32)~System.String")]

[assembly: SuppressMessage("Major Code Smell", 
    "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", 
    Justification = "The goals is set private field", 
    Scope = "member", 
    Target = "~M:AtendeLogo.Common.Utils.ReflectionUtils.SetFiledValue(System.Object,System.String,System.Object)")]
