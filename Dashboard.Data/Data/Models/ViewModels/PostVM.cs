namespace Dashboard.Data.Data.Models.ViewModels;

public class PostVM
{
    public  string Title
    { get; set; }
 
    public  string? ShortDescription
    { get; set; }
 
    public  string Description
    { get; set; }
 
    public  string Body
    { get; set; }
    public  string Author
    { get; set; }
  
    
    public  IList<string> Tags
    { get; set; }
}