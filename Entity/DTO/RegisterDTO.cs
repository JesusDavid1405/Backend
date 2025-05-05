using System;

namespace Entity.DTO;

public class RegisterDTO
{
    //user
    //activo que se llenara automaticamente en true
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate  {get; set;}

    //person
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DocumentType { get; set; }
    public string Document { get; set; }
    public DateTime DateBorn { get; set; }
    public string PhoneNumber { get; set; }
    public string Eps { get; set; }
    public string Genero { get; set; }
    public bool RelatedPerson { get; set; }

    //todos los roles
    public List<string> Roles {get; set;} = new List<string>();

}
