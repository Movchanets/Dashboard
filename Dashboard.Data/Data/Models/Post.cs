using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Data.Data.Models.ViewModels;
public class Post
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public  int Id
    { get; set; }
 
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
    
    public  DateTime PostedOn
    { get; set; }
 
    public  List<Tag> Tags
    { get; set; }
}

public class Tag
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public  int Id
    { get; set; }

    public string Name { get; set; }
}