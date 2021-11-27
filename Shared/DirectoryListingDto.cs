namespace BlazorApp.Shared
{
    public class DirectoryListingDto
    {
        public string? FullName { get; set; }
        public List<string>? Directories { get; set; }
        public List<FileDto>? Files { get; set; }
    }

    public class FileDto
    {
        public string? Name { get; set; }
        public long Size { get; set; }
    }
}