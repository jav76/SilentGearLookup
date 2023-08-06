using Octokit;

namespace SilentGearLookup.Data
{
    internal class MaterialsDataRepo
    {

        private const string VERSION = "1.19.4";
        private const string MATERIALS_PATH = "/src/generated/resources/data/silentgear/silentgear_materials";
        private const string REPO_OWNER = "SilentChaos512";
        private const string REPO_NAME = "Silent-Gear";
        private const string FULL_PATH = @"https://github.com/SilentChaos512/Silent-Gear/tree/{version}/src/generated/resources/data/silentgear/silentgear_materials";
        internal const string LOCAL_MATERIALS_PATH = "silentgear_materials";
        internal static async Task UpdateMaterialsFiles()
        {
            if (!Directory.Exists(LOCAL_MATERIALS_PATH))
            {
                Directory.CreateDirectory(LOCAL_MATERIALS_PATH);
                List<Tuple<string, string>> JSONs = await GetMaterialsData();

                foreach (Tuple<string, string> data in JSONs)
                {
                    File.WriteAllText(Path.Combine(LOCAL_MATERIALS_PATH, data.Item2), data.Item1);
                }
            }
        }

        internal static async Task<List<Tuple<string, string>> > GetMaterialsData()
        {
            List<Tuple<string, string>> URLs = new List<Tuple<string, string>>();
            List<Tuple<string, string>> JSONs = new List<Tuple<string, string>>();

            await GetDownloadURLs(URLs);

            HttpClient httpClient = new HttpClient();

            foreach (Tuple<string, string> url in URLs)
            {
                JSONs.Add(new Tuple<string, string>
                (
                    await httpClient.GetStringAsync(url.Item1),
                    url.Item2
                ));
            }

            return JSONs;
        }

        internal static async Task GetDownloadURLs(List<Tuple<string, string>> URLs, string path = MATERIALS_PATH)
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("SilentGearLookup"));
            string currentPath = Directory.GetCurrentDirectory();
#if DEBUG // Non-authed users can still use the API, but have lower limits. It should be fine to pull the files once.
            string tokenPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Secrets/GithubToken.txt");
            if (File.Exists(tokenPath))
            {
                client.Credentials = new Credentials(new StreamReader(tokenPath).ReadLine());
            }
#endif

            try
            {
                var repoConents = await client.Repository.Content.GetAllContentsByRef(REPO_OWNER, REPO_NAME, path, $"{VERSION}");

                foreach (RepositoryContent content in repoConents)
                {
                    if (content.Type == ContentType.File)
                    {
                        URLs.Add(new Tuple<string, string>(content.DownloadUrl, content.Name));
                    }
                    else if (content.Type == ContentType.Dir)
                    {
                        await GetDownloadURLs(URLs, content.Path);
                    }
                }

            }
            catch (RateLimitExceededException ex)
            {
                Console.WriteLine($"Github API rate limited. Wait a while and try again or manually download config files from {FULL_PATH} to {LOCAL_MATERIALS_PATH}");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
