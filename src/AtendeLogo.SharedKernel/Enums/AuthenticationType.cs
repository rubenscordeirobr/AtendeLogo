using AtendeLogo.Common.Attributes;

namespace AtendeLogo.Shared.Enums;
 
public enum AuthenticationType
{
    [UndefinedValue]
    Unknown,
    System,
    Anonymous,
    Email_Password,  
    Google,      
    Facebook,
    Microsoft,        
    WhatsApp,
    SMS
}
