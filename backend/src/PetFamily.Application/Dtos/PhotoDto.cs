namespace PetFamily.Application.Dtos;

public class PhotoDto
{
    public PhotoDto(string filename, string filePath)
    {
        FileName = filename;
        FilePath = filePath;
    }

    public string FileName { get; set; }
    public string FilePath { get; set; }
}
