using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Common;

namespace PetFamily.Infrastructure.Providers;

[UsedImplicitly]
public class MinioProvider : IFileProvider
{
    private readonly ILogger<MinioProvider> _logger;
    private readonly IMinioClient _minioClient;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(fileData.BucketName);

            var bucketExists = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExists == false)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(fileData.BucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var path = $"{Guid.NewGuid()}_{fileData.FileName}";

            var putObjectArgs = new PutObjectArgs()
                .WithBucket("photos")
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(path);

            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return result.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }

    public async Task<Result<string, Error>> GetPresignedUrl(string fileName, string bucketName,
        CancellationToken cancellationToken)
    {
        try
        {
            var getObjectArgs = new PresignedGetObjectArgs()
                .WithObject(fileName)
                .WithBucket(bucketName)
                .WithExpiry(604800);

            var result = await _minioClient.PresignedGetObjectAsync(getObjectArgs);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to get file link from minio");
            return Error.Failure("file.getLink", "Fail to get file link from minio");
        }
    }

    public async Task<UnitResult<Error>> DeleteFile(string fileName, string bucketName,
        CancellationToken cancellationToken)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs().WithBucket(bucketName).WithObject(fileName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return new UnitResult<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete file from minio");
            return Error.Failure("file.delete", "Fail to delete file from minio");
        }
    }
}