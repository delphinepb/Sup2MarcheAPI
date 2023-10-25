using Microsoft.AspNetCore.Identity;

namespace Sup2Marché.Model
{
    public class UserEntity
    {
        public int? id { get; set; }

        public string? email { get; set; }

        public string? password { get; set; }

        public UserEntity() { }

        public UserEntity(int id, string email, string password)
        {
            id = id;
            email = email;
            password = password;
        }
        public string CreateUser()
        {
            return "Insert Into utlisateur (email, password) values (@email, @password); Select @@Identity; ";
        }

        public string ReadUser(string type)
        {
            var typeExtend = (type == "id") ? "user" : "";
            return $"Select * From utilisateur Where {type} = @{type}";

        }

        public string UpdateUser()
        {
            return "UPDATE Utilisateur SET email = @email, password = @password WHERE id = @id";
        }

        public string DeleteUser()
        {
            return "DELETE FROM Utilisateur WHERE id = @id";
        }

    }
}

   
