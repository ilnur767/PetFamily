using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using static PetFamily.Domain.Common.ValidationMessageConstants;
using static PetFamily.Domain.Common.Errors;

namespace PetFamily.Domain.Volunteers;

public class PetPhotosList : ComparableValueObject
{
    private PetPhotosList()
    {
    }

    private PetPhotosList(List<Photo> photos)
    {
        Photos = photos;
    }

    public IList<Photo> Photos { get; }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        foreach (var photo in Photos)
        {
            yield return photo;
        }
    }

    public static Result<PetPhotosList> Create(IEnumerable<Photo> photos)
    {
        return new PetPhotosList(photos.ToList());
    }
}

public class Photo : ComparableValueObject
{
    private Photo()
    {
    }

    private Photo(string fileName, string filepath)
    {
        FileName = fileName;
        FilePath = filepath;
    }

    public string FileName { get; }

    public string FilePath { get; }

    public static Result<Photo, Error> Create(string fileName, string filePath)
    {
        var errorMessage = new StringBuilder();

        if (string.IsNullOrWhiteSpace(fileName))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, "File name"));
        }

        if (string.IsNullOrWhiteSpace(filePath))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, "File path"));
        }

        if (errorMessage.Length > 0)
        {
            return Error.Validation(InvalidValueCode, errorMessage.ToString());
        }

        return new Photo(fileName, filePath);
    }


    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return FileName;
        yield return FilePath;
    }
}
