using Requeceptor.Domain;
using System.Xml;


namespace Requeceptor.Services.Parsers;

public class XmlRequestParser : IRequestParser
{
    public RequestFormat Format => RequestFormat.Xml;

    public bool CanParse(string? contentType, string? body)
    {
        if (contentType?.Contains("xml") == true) return true;
        return body?.TrimStart().StartsWith("<") == true;
    }

    public string? GetActionName(HttpRequest request, string body)
    {
        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(body);

            var bodyNode = xmlDoc.GetElementsByTagName("soap:Body").Cast<XmlNode>().FirstOrDefault()
                       ?? xmlDoc.GetElementsByTagName("s:Body").Cast<XmlNode>().FirstOrDefault()
                       ?? xmlDoc.GetElementsByTagName("Body").Cast<XmlNode>().FirstOrDefault();

            return bodyNode?.FirstChild?.LocalName;
        }
        catch
        {
            return null;
        }
    }
}
