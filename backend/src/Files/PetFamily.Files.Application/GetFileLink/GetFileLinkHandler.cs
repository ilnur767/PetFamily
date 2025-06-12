﻿using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Files.Application.FileProvider;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Files.Application.GetFileLink;

public class GetFileLinkHandler : ICommandHandler<string, GetFileLinkCommand>
{
    private readonly IFileProvider _fileProvider;

    public GetFileLinkHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(GetFileLinkCommand fileCommand, CancellationToken cancellationToken)
    {
        var result =
            await _fileProvider.GetPresignedUrl(fileCommand.FileName, fileCommand.BucketName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        return result.Value;
    }
}

public record GetFileLinkCommand(string FileName, string BucketName) : ICommand;
