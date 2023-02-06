namespace miranaSolution.Data.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public int Name { get; set; }

        public List<Book> Books {get; set;}
    }
}