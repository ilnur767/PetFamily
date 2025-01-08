using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers;

public class SocialMediaList : ComparableValueObject
{
    public SocialMediaList() { }
    
    private SocialMediaList(List<SocialMedia> socialMediaList)
    {
        SocialMedias = socialMediaList;
    }
    public List<SocialMedia> SocialMedias { get; private set; }
    
    
    public static  SocialMediaList Create(IEnumerable<SocialMedia> socialMedias)
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