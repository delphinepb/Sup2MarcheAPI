using Microsoft.AspNetCore.Identity;

namespace Sup2Marché.Model
{
    public class UserEntity
    {
        public int? id { get; set; }

        public string? email { get; set; }

        public string? password { get; set; }

        public int? role { get; set; }

        public UserEntity() { }

        public UserEntity(int id, string email, string password, int? role)
        {
            id = id;
            email = email;
            password = password;
            role = role;
        }
        public string CreateUser()
        {
            return "Insert Into utilisateur (email, password, role) values (@email, @password, @role); Select @@Identity; ";
        }

        public string ReadUser(string type)
        {
            var typeExtend = (type == "id") ? "user" : "";
            return $"Select * From utilisateur Where {type} = @{type}";

        }
        public string readrole(string email)
        {
            return "select role, nom from utilisateur where email = @email";

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

   
