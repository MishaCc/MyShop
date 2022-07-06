namespace Shop.Models
{
    public class Requests
    {
		 public int Id{get;set;}
	     public string UserId{get;set;}
	     public int ProductId{get;set;}
         public string Description{get;set;}
	     public string? Adventages{get;set;}
	     public string? DisAdventages{ get; set;}
	     public DateTime PostTime{get;set;}
    }
}
