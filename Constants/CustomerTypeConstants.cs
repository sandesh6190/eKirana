namespace eKirana.Constants;
public class CustomerTypeConstants
{
    public const string NormalCustomer = "NormalCustomer";
    public const string MemberShipCustomer = "MemberShipCustomer";
    public static List<string> CustomerTypeList = new List<string>{
        NormalCustomer, MemberShipCustomer
    };
}
