using OnlineCourse.Application.Models.Minio;

namespace OnlineCourse.Application.ServiceContracts;

public interface IMinioService
{
    Task UploadFileAsync(UploadFileModel model);
    Task<Stream> DownloadFileAsync(string fileName);
}
