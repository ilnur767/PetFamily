// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace PetFamily.Application.Volunteers.DeletePetPhoto;

public record DeletePetPhotoCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> FilesPath);
