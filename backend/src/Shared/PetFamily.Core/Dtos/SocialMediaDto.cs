namespace PetFamily.Core.Dtos;

public class SocialMediaDto
{
    public SocialMediaDto(string name, string link)
    {
        Name = name;
        Link = link;
    }

    public string Name { get; set; }
    public string Link { get; set; }
}
