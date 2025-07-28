
namespace TravelBooking.EmailBuilderbody
{
    public static class EmailBodyBuilder
    {
        public static string BuildEmailBody(string templete, Dictionary<string, string> templeteModel)
        {
            var rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", $"{templete}.html");
             

            if (!File.Exists(rootPath))
            {
                throw new FileNotFoundException($"Template file '{templete}.html' not found in path: {rootPath}");
            }

            string emailBody;
            using (var streamReader = new StreamReader(rootPath))
            {
                emailBody = streamReader.ReadToEnd();
            }

            foreach (var item in templeteModel)
            {
                emailBody = emailBody.Replace(item.Key, item.Value);
            }

            return emailBody;
        }

    }
}
