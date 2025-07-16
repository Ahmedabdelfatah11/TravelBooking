namespace TravelBooking.EmailBuilderbody
{
    public static class EmailBodyBuilder
    {

        public static string BuildEmailBody(string templete, Dictionary<string,string> templeteModel)
        {  
            var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{templete}.html";

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Template file '{templete}.html' not found in the specified path.");
            }
            var streamReader = new StreamReader(templatePath);
            var emailBody = streamReader.ReadToEnd();
            streamReader.Close();
            foreach (var item in templeteModel)
            {
                emailBody = emailBody.Replace(item.Key, item.Value);
            }
            return emailBody;
        }
    }
}
