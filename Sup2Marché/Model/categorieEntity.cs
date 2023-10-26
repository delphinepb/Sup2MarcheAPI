namespace Sup2Marché.Model
{
    public class categorieEntity
    {
        public int id {  get; set; }
        public string nom { get; set; }

        public string CreatedCategorie()
        {
            return "INSERT INTO categorie(nom) values (@nom)";
        }

        public string ReadCategorie()
        {
            return "SELECT * from categorie";
        }

        public string UpdateCategorie()
        {
            return "UPDATE categorie set nom = @nom WHERE id = @id";
        }

        public string DeleteCategorie()
        {
            return "DELETE FROM categorie WHERE id = @id";
        }

        public string ReadallCategorie()
        {
            return "SELECT id, nom FROM categorie";
        }
    }
}
