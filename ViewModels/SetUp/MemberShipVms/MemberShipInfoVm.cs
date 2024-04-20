namespace eKirana.ViewModels.SetUp.MemberShipVms;
public class MemberShipInfoVm
{
    public long MemberShipId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
    public string MemberShipStatus { get; set; }
    public DateTime RegisteredOn { get; set; }
    public DateTime? LastTransaction { get; set; }

}
