using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlueMonkey.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        private const string AppSecretsMountPath = "/mnt/secrets-store";
        public Dictionary<string, string> Secrets { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void OnGet()
        {
            Secrets = ReadSecretMountVolume(AppSecretsMountPath);
        }

        private Dictionary<string, string> ReadSecretMountVolume(string mountPath)
        {
            var strings = new Dictionary<string, string>();
            var secretFolders = Directory.GetDirectories(mountPath);
            foreach (var folder in secretFolders)
            {
                if (folder.Contains("..data"))
                {
                    var allSecretFiles = Directory.GetFiles(folder);
                    foreach (var f in allSecretFiles)
                    {
                        strings.Add(f.Split('/').Last(), System.IO.File.ReadAllText(f));
                    }
                }
            }

            return strings;
        }
    }
}