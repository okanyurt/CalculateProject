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
    TRDEVİR = 6,
    [Description("GELEN TRANSFER")]
    GELENTRANSFER = 7
}

public enum EnumRole
{
    [Description("AGENT")]
    AGENT = 1,
    [Description("ADMİN")]
    ADMIN = 2,
}