using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Common;
using static PetFamily.SharedKernel.Common.ValidationMessageConstants;
using static PetFamily.SharedKernel.Common.Errors;

namespace PetFamily.Volunteers.Domain.ValueObjects;

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

    private Photo(string fileName, string filepath, bool isMain)
    {
        FileName = fileName;
        FilePath = filepath;
        IsMain = isMain;
    }

    public string FileName { get; }

    public string FilePath { get; }
    public bool IsMain { get; }

    public static Result<Photo, Error> Create(string fileName, string filePath, bool isMain = false)
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

        return new Photo(fileName, filePath, isMain);
    }


    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return FileName;
        yield return FilePath;
        yield return IsMain;
    }
}
