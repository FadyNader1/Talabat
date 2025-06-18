namespace Talabat.DTO
{
    public class PagginationDTO<T>
    {
        public int PageIndex {  get; set; }
        public int PageSize { get; set; }
        public int Count {  get; set; }
        public IReadOnlyList<T> Data {  get; set; }
        public PagginationDTO(int _pageindex,int _pagesize,int _count,IReadOnlyList<T> _data)
        {
             Data = _data;
            PageIndex = _pageindex;
            PageSize = _pagesize;
            Count = _count;

        }
    }
}
