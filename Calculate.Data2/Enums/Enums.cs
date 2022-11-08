using System.ComponentModel;

namespace Calculate.Data.Enums;


public enum EnumProcessType
{
    [Description("YATIRIM")]
    YATIRIM = 1,
    [Description("ÇEKİM")]
    CEKIM = 2,
    [Description("KOMİSYON")]
    KOMISYON = 3,
    [Description("TRANSFER")]
    TRANSFER = 4,
    [Description("DEVİR")]
    DEVİR = 5,
    [Description("TR DEVİR")]
    TRDEVİR = 6
}
