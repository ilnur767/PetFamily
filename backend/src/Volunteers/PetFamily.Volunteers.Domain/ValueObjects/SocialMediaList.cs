using CSharpFunctionalExtensions;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public class SocialMediaList : ComparableValueObject
{
    public SocialMediaList()
    {
    }

    private SocialMediaList(List<SocialMedia> socialMediaList)
    {
        SocialMedias = socialMediaList;
    }

    public List<SocialMedia> SocialMedias { get; }


    public static SocialMediaList Create(IEnumerable<SocialMedia> socialMedias)
    {
        return new SocialMediaList(socialMedias.ToList());
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        foreach (var socialMedia in SocialMedias)
        {
            yield return socialMedia;
        }
    }
}
