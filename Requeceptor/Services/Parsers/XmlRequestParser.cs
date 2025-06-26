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
        string? actionName;

        if (request.Headers.TryGetValue("SOAPAction", out var soapAction))
        {
            actionName = soapAction.FirstOrDefault()?.Trim('"');
        }
        else
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(body);

                var bodyNode = xmlDoc.GetElementsByTagName("soap:Body").Cast<XmlNode>().FirstOrDefault()
                           ?? xmlDoc.GetElementsByTagName("s:Body").Cast<XmlNode>().FirstOrDefault()
                           ?? xmlDoc.GetElementsByTagName("Body").Cast<XmlNode>().FirstOrDefault();

                actionName = bodyNode?.FirstChild?.LocalName;
            }
            catch
            {
                actionName = null;
            }
        }

        if (actionName is null)
        {
            return null;
        }

        // Handle action names with namespace prefixes (e.g., "ns:ActionName")
        return actionName.Contains(":") ? actionName.Split(':').Last() : actionName;
    }
}
