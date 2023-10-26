namespace Sup2Marché.Model
{
    public class produitEntity
    {
        public int id { get; set; }
        public string nom { get; set; }

        public int quantite { get; set; }

        public int categorie { get; set; }

        public string CreatedProduit()
        {
            return "INSERT INTO produit(nom, quantite, categorie) values (@nom, @quantite, @categorie)";
        }

        public string ReadProduit()
        {
            return "SELECT id, nom, quantite, categorie from produit where id = @id";
        }

        public string UpdateProduit()
        {
            return "UPDATE produit set quantite = @quantite WHERE id = @id";
        }

        public string DeleteProduit()
        {
            return "DELETE FROM produit WHERE id = @id";
        }

        public string Readall()
        {
            return "SELECT id, nom, quantite, categorie FROM produit";
        }
    }
}
