namespace OnlineCourse.Application.Models.Minio;

public class UploadFileModel
{
    
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public Stream Data { get; set; }
}
