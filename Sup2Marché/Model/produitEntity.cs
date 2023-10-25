namespace Sup2Marché.Model
{
    public class produitEntity
    {
        public int id { get; set; }
        public string nom { get; set; }

        public int quantite { get; set; }

        public string CreatedProduit()
        {
            return "INSERT INTO produit(nom, quantite) values (@nom, @quantite)";
        }

        public string ReadProduit()
        {
            return "SELECT id, nom, quantite from produit where id = @id";
        }

        public string UpdateProduit()
        {
            return "UPDATE produit SET nom = @nom, quantite = @quantite WHERE id = @id";
        }

        public string DeleteProduit()
        {
            return "DELETE FROM produit WHERE id = @id";
        }

        public string Readall()
        {
            return "SELECT id, nom, quantite FROM produit";
        }
    }
}
