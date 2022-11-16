namespace Dashboard.Data.Data.Models.ViewModels;

public class GetPostVM
{
    public int page { get; set; }
    public List<string>? Tags { get; set; }
    public string? Find { get; set; }
}