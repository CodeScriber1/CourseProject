using System.IO;

namespace CourseProject.Service.Helpers;

public class EnvironmentHelper
{
	public static string WebRootPath { get; set; }
	public static string BookFilePath => Path.Combine(WebRootPath, "books");
    public static string FilePath => "books";
}
