using QueryBuilder;
namespace Lab5
{
    public class Program
    {
        static void Main(string[] args)
        {
            var database = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString() + "\\Data\\Lab 5 Database.db";

            List<Author> authors;

            using (var qb = new QueryBuilder(database))
            {
                var ids = 3;
                var sk = new Author(4, "DARE", "USER");
                qb.Create<Author>(sk);

                authors = qb.ReadAll<Author>();
                var readOne = qb.Read<Author>(4);

                sk.FirstName = "Dare";
                sk.SurName = "User";
                qb.Update(sk);


                qb.Delete<Author>(sk);
            }
            foreach (var author in authors)
            {
                Console.WriteLine(author);
            }

        }
    }
}