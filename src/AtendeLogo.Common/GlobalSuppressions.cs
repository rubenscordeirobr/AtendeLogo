using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "The 'Error' record is only used internally within this project and will not be exposed to other languages.", Scope = "member", Target = "~P:AtendeLogo.Common.IResultValue.Error")]
[assembly: SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "MD5 is used solely for file integrity verification when files are uploaded.", Scope = "member", Target = "~M:AtendeLogo.Common.Helpers.HashHelper.GenerateMd5GuidHash(System.Byte[])~System.Guid")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "The 'Error' record is only used internally within this project and will not be exposed to other languages.", Scope = "type", Target = "~T:AtendeLogo.Common.Error")]

