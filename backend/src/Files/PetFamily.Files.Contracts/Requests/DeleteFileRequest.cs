using PetFamily.Core.Dtos;

namespace PetFamily.Files.Contracts.Requests;

public record DeleteFileRequest(IEnumerable<string> FileName, string BucketName);

public record UploadFilesRequest(IEnumerable<FileDataDto> FilesData, string BucketName);
