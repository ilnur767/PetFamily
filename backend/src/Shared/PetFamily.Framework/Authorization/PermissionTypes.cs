namespace PetFamily.Framework.Authorization;

public static class PermissionTypes
{
    public static class Volunteers
    {
        public const string Create = "volunteers.create";
        public const string Update = "volunteers.update";
        public const string Delete = "volunteers.delete";
        public const string Read = "volunteers.read";
    }

    public static class Pet
    {
        public const string Create = "pet.create";
        public const string Update = "pet.update";
        public const string Delete = "pet.delete";
        public const string Read = "pet.read";
    }

    public static class Species
    {
        public const string Create = "species.create";
        public const string Update = "species.update";
        public const string Delete = "species.delete";
        public const string Read = "species.read";
    }
}
