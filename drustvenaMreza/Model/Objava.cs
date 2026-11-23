namespace drustvenaMreza.Model
{
    public class Objava
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DatumObjave { get; set; }
        public int? UserId { get; set; }
        public Korisnik? User { get; set; }

        public Objava(int id, string content, DateTime datumObjave,int? userId)
        {
            this.Id = id;
            this.Content = content;
            this.DatumObjave = datumObjave;
            this.UserId = userId;
        }
    }
}
