using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Maximus.Library.WebApi
{
  /// <summary>
  /// Authentication type for <seealso cref="SimpleWebApiClient"/>.
  /// </summary>
  public enum WebAuthenticationMethod 
  { 
    /// <summary>
    /// No authentication
    /// </summary>
    Anonymous,
    /// <summary>
    /// Basic authentication
    /// </summary>
    Basic,
    /// <summary>
    /// Use the standard 'Authorization' header with custom value
    /// </summary>
    Header, 
    /// <summary>
    /// Use custom header name and value.
    /// </summary>
    CustomHeader 
  }

  /// <summary>
  /// Simple wrapper around <seealso cref="HttpWebRequest"/> for Web API requests.
  /// </summary>
  public class SimpleWebApiClient
  {
    protected UriBuilder BaseAddress;
    protected WebAuthenticationMethod authenticationMethod;
    protected string AuthorizationHeaderValue, AuthorizationHeaderName;

    /// <summary>
    /// Updates authorization header value within life cycle. Write-only property.
    /// </summary>
    public string AuthorizationValue { set { AuthorizationHeaderValue = value; } }

    public SimpleWebApiClient(Uri baseAddress, WebAuthenticationMethod authMethod, string usernameOrHeaderName, string passwordOrHeaderValue)
    {
      // setup the client
      authenticationMethod = authMethod;
      switch(authMethod)
      {
        case WebAuthenticationMethod.Anonymous:
          break;
        case WebAuthenticationMethod.Basic:
          AuthorizationHeaderValue = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{usernameOrHeaderName}:{passwordOrHeaderValue}"));
          break;
        case WebAuthenticationMethod.Header:
          AuthorizationHeaderValue = passwordOrHeaderValue;
          break;
        case WebAuthenticationMethod.CustomHeader:
          AuthorizationHeaderValue = passwordOrHeaderValue;
          AuthorizationHeaderName = usernameOrHeaderName;
          break;
      }
      
      BaseAddress = new UriBuilder(baseAddress);
    }

    /// <summary>
    /// Creates an instance of <seealso cref="SimpleWebApiClient"/> using default <seealso cref="WebAuthenticationMethod.Anonymous"/> authentication method.
    /// </summary>
    /// <param name="baseAddress"></param>
    public SimpleWebApiClient(Uri baseAddress)
    {
      // setup the client
      authenticationMethod = WebAuthenticationMethod.Anonymous;
      BaseAddress = new UriBuilder(baseAddress);
    }

    /// <summary>
    /// Updates credentials for Basic authentication. Method will fail if current authentication mode is not set to Basic.
    /// </summary>
    /// <param name="username">User name</param>
    /// <param name="password">Password</param>
    /// <exception cref="ArgumentException"/>
    public void UpdateBasicAuthorization(string username, string password)
    {
      if (authenticationMethod == WebAuthenticationMethod.Basic)
        AuthorizationHeaderValue = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
      else
        throw new ArgumentException("Incorrect method call. Current authentication mode is not set to Basic.");
    }

    /// <summary>
    /// Set <seealso cref="ServicePoint.Expect100Continue"/> property to either <code>true</code> or <code>false</code> if not null,
    /// or leave unset by default (when set to null).
    /// </summary>
    public bool? Expect100Continue { get; set; } = null;

    protected HttpWebRequest PrepareHttpWebRequest(string subPath, string query = null, string method = null, NameValueCollection extraHeaders = null, string body = null)
    {
      BaseAddress.Path = subPath;
      BaseAddress.Query = query;
      HttpWebRequest Request = WebRequest.CreateHttp(BaseAddress.Uri);
      if (Expect100Continue != null)
        Request.ServicePoint.Expect100Continue = Expect100Continue ?? false;
      if (!string.IsNullOrWhiteSpace(method))
        Request.Method = method;
      else
        Request.Method = "GET";
      // default headers
      SetHeaders(Request, new NameValueCollection
      {
        { "Host", BaseAddress.Host }
      });
      // user-defined headers
      SetHeaders(Request, extraHeaders);

      #region Authentication Header
      if (authenticationMethod == WebAuthenticationMethod.Basic || authenticationMethod == WebAuthenticationMethod.Header)
      {
        if (!Request.Headers.AllKeys.Any(x => x == "Authorization"))
          Request.Headers.Add("Authorization", AuthorizationHeaderValue);
      }
      if (authenticationMethod == WebAuthenticationMethod.CustomHeader)
      {
        if (!Request.Headers.AllKeys.Any(x => x == AuthorizationHeaderName))
          Request.Headers.Add(AuthorizationHeaderName, AuthorizationHeaderValue);
      }
      #endregion

      if (string.IsNullOrEmpty(Request.Accept))
        Request.Accept = "application/json";
      if (!string.IsNullOrWhiteSpace(body))
      {
        if (string.IsNullOrEmpty(Request.ContentType))
          Request.ContentType = "application/json";
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteBody = encoding.GetBytes(body);
        Request.ContentLength = byteBody.Length;
        using (Stream dataStream = Request.GetRequestStream())
          dataStream.Write(byteBody, 0, byteBody.Length);
      }
      return Request;
    }

    public string GetResponse(string subPath, string query = null, string method = null, NameValueCollection extraHeaders = null, string body = null)
    {
      HttpWebRequest Request = PrepareHttpWebRequest(subPath, query, method, extraHeaders, body);
      using (WebResponse Response = Request.GetResponse())
      {
        using (Stream ResponseStream = Response.GetResponseStream())
        {
          using (StreamReader Reader = new StreamReader(ResponseStream, Encoding.UTF8))
          {
            return Reader.ReadToEnd();
          }
        }
      }
    }

    public async Task<string> GetResponseAsync(string subPath, string query = null, string method = null, NameValueCollection extraHeaders = null, string body = null)
    {
      HttpWebRequest Request = PrepareHttpWebRequest(subPath, query, method, extraHeaders, body);
      using (WebResponse Response = Request.GetResponse())
      {
        using (Stream ResponseStream = Response.GetResponseStream())
        {
          using (StreamReader Reader = new StreamReader(ResponseStream, Encoding.UTF8))
          {
            return await Reader.ReadToEndAsync();
          }
        }
      }
    }

    protected static void SetHeaders(HttpWebRequest request, NameValueCollection headers)
    {
      if (headers == null || headers.Count == 0)
        return;
      foreach (string valueKey in headers.Keys)
        switch (valueKey)
        {
          // https://docs.microsoft.com/en-us/dotnet/api/system.net.httpwebrequest.headers?view=netframework-4.7.2
          case "Accept": request.Accept = headers[valueKey]; break;
          case "Connection": request.Connection = headers[valueKey]; break;
          case "Content-Length": request.ContentLength = int.Parse(headers[valueKey]); break;
          case "Content-Type": request.ContentType = headers[valueKey]; break;
          case "Expect": request.Expect = headers[valueKey]; break;
          case "Date": request.Date = DateTime.Parse(headers[valueKey]); break;
          case "Host": request.Host = headers[valueKey]; break;
          case "If-Modified-Since": request.IfModifiedSince = DateTime.Parse(headers[valueKey]); break;
          case "Range": break; // not supported  request.AddRange= headers[valueKey]; break;
          case "Referer": request.Referer = headers[valueKey]; break;
          case "Transfer-Encoding": request.TransferEncoding = headers[valueKey]; break;
          case "User-Agent": request.UserAgent = headers[valueKey]; break;
          default: request.Headers.Add(valueKey, headers[valueKey]); break;
        }
    }
  }
}