namespace Shop.Paging
{
    public class PageInfo
    {
       
        public int GetTotalPage(int TotalItem,int PageSize)
        { 
            return (int)Math.Ceiling((decimal)TotalItem / PageSize);
        }
        public int GetPage(int page)
        {
            return 1;
        }
    }
}
