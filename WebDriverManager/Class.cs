using Microsoft.Win32;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO.Compression;

namespace WebDriverManager
{
    public class ChromeDriverManager
    {
        
        public string download_and_install(string DriverPath)
        {
            
            string extract_to = DriverPath;
            object path;
            //get value of chrome from registry
            path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);


            if (path != null)
            {


                //read exact version of chrome browser from registry value
                string chrome_browser_vers;
                chrome_browser_vers = FileVersionInfo.GetVersionInfo(path.ToString()).FileVersion;

                //get major version of chrome browser
                int pos = chrome_browser_vers.LastIndexOf('.') + 1;
                string major_browser = chrome_browser_vers.Substring(0, pos - 1);

                //read version of chrome driver associated with major browser version
                System.Net.WebClient wc = new System.Net.WebClient();
                string associated_driver_vers = wc.DownloadString("https://chromedriver.storage.googleapis.com/LATEST_RELEASE_" + major_browser);

                //check if associated browser version is already installed
                bool download = true;
                if (System.IO.File.Exists(extract_to + "\\chromedriver.exe"))
                {


                    System.IO.File.Delete(extract_to + "\\chromedriver.exe");

                    //downloaded chromedriver has no version info. In the future, save file with version # and compare version of currently installed driver
                    //string current_driver_vers = DriverVersion(extract_to + "\\chromedriver.exe");
                    //if (current_driver_vers == associated_driver_vers)
                    //{
                    //  download = false;
                    //}
                }

                if (download==true)
                {

                    string fname = associated_driver_vers + "/chromedriver_win32.zip";
                    var jsonstr = wc.DownloadString("https://www.googleapis.com/storage/v1/b/chromedriver/o");

                    //get mediaLink from JSON object
                    JObject json = JObject.Parse(jsonstr);
                    var media_list = (from p in json["items"] select (string)p["mediaLink"]).ToList();
                    var names_list = (from p in json["items"] select (string)p["name"]).ToList();
                    int media_index = names_list.FindIndex(a => a == fname);
                    string mediaLink = media_list[media_index];

                    //download associated driver from respective media link
                    string remoteUri = mediaLink;
                    string zipfileName = extract_to + "\\chromedriver_win32.zip";

                    // Create a new WebClient instance.
                    using (System.Net.WebClient myWebClient = new System.Net.WebClient())
                    {
                        string myStringWebResource = remoteUri;
                        // Download the Web resource and save it into the current filesystem folder.
                        myWebClient.DownloadFile(myStringWebResource, zipfileName);
                    }



                    ZipFile.ExtractToDirectory(zipfileName, extract_to);
                    System.IO.File.Delete(zipfileName);
                }
              

                return extract_to;
            }
            else
            {
                throw new System.Exception("Chrome is not installed on this machine.");
            }



        }

        string DriverVersion(string DriverPath) {
            string vers = System.Diagnostics.FileVersionInfo.GetVersionInfo(DriverPath).ToString();

            return vers;
        }
    }
}
