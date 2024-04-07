using eKirana.Models.SetUp;

namespace eKirana.ViewModels.SetUp;
public class CategoryIndexVm
{
    public string Item { get; set; } //for field (input from user)
    public List<Category> Categories; //for list of data (output for user)
}
