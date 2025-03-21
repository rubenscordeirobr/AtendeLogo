namespace AtendeLogo.Shared.Enums;
 
public enum AuthenticationType
{
    [UndefinedValue]
    Unknown,
    System,
    Anonymous,
    Credentials,  
    Google,      
    Facebook,
    Microsoft,        
    WhatsApp,
    SMS
}
