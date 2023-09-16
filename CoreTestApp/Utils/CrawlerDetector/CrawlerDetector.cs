using System.Linq;

namespace Imobiliare.UI.Utils
{
  using System.Text.RegularExpressions;
  using System.Web;

  public static class CrawlerDetector
  {
    //public static bool Iscrawler(this HttpRequest httpRequest)
    //{
    //  if (httpRequest.UserAgent != null)
    //  {
    //    return IsCrawler(httpRequest.UserAgent);
    //  }

    //  return false;
    //}

    //public static bool Iscrawler(this HttpRequestBase httpRequest)
    //{
    //  if (httpRequest.UserAgent != null)
    //  {
    //    return IsCrawler(httpRequest.UserAgent);
    //  }

    //  return false;
    //}

    //public static DeviceType GetDeviceType(this HttpRequest httpRequest)
    //{
    //  //https://briancaos.wordpress.com/2012/04/12/identifying-mobile-devices-in-sitecore/
    //  //FIRST TRY BUILT IN ASP.NET CHECK
    //  if (httpRequest.Browser.IsMobileDevice)
    //  {
    //    return DeviceType.Mobile;
    //  }

    //  //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
    //  if (httpRequest.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
    //  {
    //    return DeviceType.Mobile;
    //  }

    //  //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
    //  if (httpRequest.ServerVariables["HTTP_ACCEPT"] != null &&
    //      httpRequest.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
    //  {
    //    return DeviceType.Mobile;
    //  }

    //  //AND FINALLY CHECK THE HTTP_USER_AGENT
    //  //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
    //  if (httpRequest.ServerVariables["HTTP_USER_AGENT"] != null)
    //  {
    //    var userAgent = httpRequest.ServerVariables["HTTP_USER_AGENT"].ToLower();

    //    var tablets =
    //      new[]
    //      {
    //        "ipad", "android 3", "xoom", "sch-i800", "tablet", "kindle", "playbook"
    //      };

    //    //Loop through each item in the list created above
    //    //and check if the header contains that text
    //    if (tablets.Any(userAgent.Contains) || (userAgent.Contains("android") && !userAgent.Contains("mobile")))
    //    {
    //      return DeviceType.Tablet;
    //    }

    //    //Create a list of all mobile types
    //    var mobiles =
    //      new[]
    //      {
    //        "midp", "j2me", "avant", "docomo",
    //        "novarra", "palmos", "palmsource",
    //        "240x320", "opwv", "chtml",
    //        "pda", "windows ce", "mmp/",
    //        "blackberry", "mib/", "symbian",
    //        "wireless", "nokia", "hand", "mobi",
    //        "phone", "cdm", "up.b", "audio",
    //        "SIE-", "SEC-", "samsung", "HTC",
    //        "mot-", "mitsu", "sagem", "sony"
    //        , "alcatel", "lg", "eric", "vx",
    //        "NEC", "philips", "mmm", "xx",
    //        "panasonic", "sharp", "wap", "sch",
    //        "rover", "pocket", "benq", "java",
    //        "pt", "pg", "vox", "amoi",
    //        "bird", "compal", "kg", "voda",
    //        "sany", "kdd", "dbt", "sendo",
    //        "sgh", "gradi", "jb", "dddi",
    //        "moto", "iphone", "Opera Mini"
    //      };

    //    //Loop through each item in the list created above
    //    //and check if the header contains that text
    //    if (mobiles.Any(userAgent.Contains))
    //    {
    //      return DeviceType.Mobile;
    //    }
    //  }
    //  return DeviceType.Default;
    //}

    private static bool IsCrawler(string userAgent)
    {
      return Regex.IsMatch(
        userAgent,
        @"bot|crawler|baiduspider|80legs|ia_archiver|voyager|curl|wget|yahoo! slurp|mediapartners-google",
        RegexOptions.IgnoreCase);
    }

//Maybe consider full list if the short version is not enough
//    List<string> Crawlers3 = new List<string>()
//{
//    "bot","crawler","spider","80legs","baidu","yahoo! slurp","ia_archiver","mediapartners-google",
//    "lwp-trivial","nederland.zoek","ahoy","anthill","appie","arale","araneo","ariadne",            
//    "atn_worldwide","atomz","bjaaland","ukonline","calif","combine","cosmos","cusco",
//    "cyberspyder","digger","grabber","downloadexpress","ecollector","ebiness","esculapio",
//    "esther","felix ide","hamahakki","kit-fireball","fouineur","freecrawl","desertrealm",
//    "gcreep","golem","griffon","gromit","gulliver","gulper","whowhere","havindex","hotwired",
//    "htdig","ingrid","informant","inspectorwww","iron33","teoma","ask jeeves","jeeves",
//    "image.kapsi.net","kdd-explorer","label-grabber","larbin","linkidator","linkwalker",
//    "lockon","marvin","mattie","mediafox","merzscope","nec-meshexplorer","udmsearch","moget",
//    "motor","muncher","muninn","muscatferret","mwdsearch","sharp-info-agent","webmechanic",
//    "netscoop","newscan-online","objectssearch","orbsearch","packrat","pageboy","parasite",
//    "patric","pegasus","phpdig","piltdownman","pimptrain","plumtreewebaccessor","getterrobo-plus",
//    "raven","roadrunner","robbie","robocrawl","robofox","webbandit","scooter","search-au",
//    "searchprocess","senrigan","shagseeker","site valet","skymob","slurp","snooper","speedy",
//    "curl_image_client","suke","www.sygol.com","tach_bw","templeton","titin","topiclink","udmsearch",
//    "urlck","valkyrie libwww-perl","verticrawl","victoria","webscout","voyager","crawlpaper",
//    "webcatcher","t-h-u-n-d-e-r-s-t-o-n-e","webmoose","pagesinventory","webquest","webreaper",
//    "webwalker","winona","occam","robi","fdse","jobo","rhcs","gazz","dwcp","yeti","fido","wlm",
//    "wolp","wwwc","xget","legs","curl","webs","wget","sift","cmc"
//};
//    string ua = Request.UserAgent.ToLower();
//    bool iscrawler = Crawlers3.Exists(x => ua.Contains(x));
  }
}
