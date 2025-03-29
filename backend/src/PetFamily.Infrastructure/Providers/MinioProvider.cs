﻿using CSharpFunctionalExtensions;
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
    private const int MaxParallelCount = 3;
    private readonly ILogger<MinioProvider> _logger;
    private readonly IMinioClient _minioClient;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(FileData fileData, string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);

            var bucketExists = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExists == false)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);

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

    public async Task<Result<IReadOnlyCollection<string>, Error>> UploadFiles(IEnumerable<FileData> filesData, string bucketName,
        CancellationToken cancellationToken)
    {
        var semaphore = new SemaphoreSlim(MaxParallelCount);
        var files = filesData.ToList();
        try
        {
            await CreateBucketIfNotExists(cancellationToken, bucketName);

            var tasks = files.Select(async file => await CreateFile(file, bucketName, semaphore, cancellationToken));

            var result = await Task.WhenAll(tasks);

            if (result.Any(r => r.IsFailure))
            {
                return result.First().Error;
            }

            return result.Select(r => r.Value).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload files in minio, files count: {filesData}", files.Count);
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphore.Release();
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

    public async Task<UnitResult<Error>> DeleteFiles(IEnumerable<string> filesPath, string bucketName,
        CancellationToken cancellationToken)
    {
        var semaphore = new SemaphoreSlim(MaxParallelCount);

        try
        {
            var tasks = filesPath.Select(async fileName =>
            {
                await semaphore.WaitAsync(cancellationToken);

                try
                {
                    var removeObjectArgs = new RemoveObjectArgs().WithBucket(bucketName).WithObject(fileName);

                    await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete files from minio");
            return Error.Failure("file.delete", "Fail to delete files from minio");
        }


        return new UnitResult<Error>();
    }

    private async Task<Result<string, Error>> CreateFile(FileData file, string bucketName, SemaphoreSlim semaphore,
        CancellationToken cancellationToken)
    {
        await semaphore.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithStreamData(file.Stream)
            .WithObjectSize(file.Stream.Length)
            .WithObject(file.FileName);

        try
        {
            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return result.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                file.FileName,
                bucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task CreateBucketIfNotExists(CancellationToken cancellationToken, string bucketName)
    {
        var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);

        var bucketExists = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

        if (bucketExists == false)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);

            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }
    }
}
