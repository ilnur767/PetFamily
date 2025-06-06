namespace PetFamily.Core.Dtos;

public class PhotoDto
{
    public PhotoDto(string filename, string filePath, bool isMain)
    {
        FileName = filename;
        FilePath = filePath;
        IsMain = isMain;
    }

    public string FileName { get; set; }
    public string FilePath { get; set; }
    public bool IsMain { get; set; }
}
