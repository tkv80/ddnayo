using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ddnayo.Manager
{
    internal static class HttpManager
    {
        /// <summary>
        /// 조회하기
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string>> GetSite(int campSeq, DateTime dateTime)
        {
            try
            {

                var parameter = string.Format("ctl00%24ctl00%24ctt%24ctt%24sm=ctl00%24ctl00%24ctt%24ctt%24uppcal%7Cctl00%24ctl00%24ctt%24ctt%24cal&__EVENTTARGET=ctl00%24ctl00%24ctt%24ctt%24cal&__EVENTARGUMENT=Select%24{0}&__VIEWSTATE=%2FwEPDwUJOTk3NzU3NTMyDxYGHgdpZF91c2VyBQEwHghpZF9ob3RlbAUEMTcwNx4Jc3VfcnN2ZGF5AjwWAmYPZBYCZg9kFgRmD2QWAgIHD2QWAgIDDxYCHgRocmVmBRkuLi9jc3MvcnN2U3lzL2NscjAwMDAuY3NzZAIBD2QWAgIBD2QWDAIBDxYCHgVzdHlsZQUNZGlzcGxheTpub25lOxYCZg8WAh8DBSUuLi9Sc3ZTeXMvQ2FsZW5kYXIuYXNweD9pZF9ob3RlbD0xNzA3ZAIDDxYEHwMFJS4uL1JzdlN5cy9DYW1wU2l0ZS5hc3B4P2lkX2hvdGVsPTE3MDceBWNsYXNzBQhzZWxlY3RlZGQCBQ8WAh8DBTIuLi9QYWdlL0FwcG1Db25maXJtLmFzcHg%2FaWRfaG90ZWw9MTcwNyZjZF9yZXE9MDAzMGQCBw8WAh8DBSlodHRwczovL3d3dy5kZG5heW8uY29tL0Ftcy8%2FaWRfaG90ZWw9MTcwN2QCCQ9kFgQCCw8PFgIeB1Zpc2libGVoZBYCAgEPFgIeC18hSXRlbUNvdW50ZmQCDw8PFgIfAQUEMTcwN2RkAgsPFgIeBFRleHQFM%2ByLpOyLnOqwhOyYiOyVveyLnOyKpO2FnCDsoIDsnpHqtow6IOuWoOuCmOyalOuLt%2By7tGRkiqosenb5ImH2PB9YBOmRJZzieNg%3D&__EVENTVALIDATION=%2FwEWBQL2kazuCgKK%2FI6nBQKOu6T7DAKxi%2BvGDgK5quPiCRYVdPtbWXeE5vYaV6BvjXtMF4wQ&ctl00%24ctl00%24ctt%24ctt%24id_zone=157&ctl00%24ctl00%24ctt%24ctt%24dt_sel=&ctl00%24ctl00%24ctt%24ctt%24keyRsv=&__ASYNCPOST=true&",
                    dateTime.ToString("yyyy-M-dd"));

                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create("http://www.ddnayo.com/RsvSys/CampSite.aspx?id_hotel=" + campSeq);
                httpWRequest.Accept = "*/*";
                httpWRequest.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
                httpWRequest.Headers.Add("Accept-Language", "en,en-US;q=0.8,ko;q=0.6");
                httpWRequest.Headers.Add("Origin", "http://www.ddnayo.com");
                httpWRequest.Referer =  "http://www.ddnayo.com/RsvSys/CampSite.aspx?id_hotel=" + campSeq;
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                httpWRequest.Method = "Post";
                httpWRequest.ContentLength = parameter.Length;

                var sw = new StreamWriter(httpWRequest.GetRequestStream());
                sw.Write(parameter);
                sw.Close();

                var theResponse = (HttpWebResponse) httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                string resultHtml = sr.ReadToEnd();

                resultHtml = resultHtml.Substring("jsonsites =", ";");
                JObject root = JObject.Parse(resultHtml);

                JArray items = (JArray) root["res"];

                var sites = (from item in items
                    select new Tuple<string, string>(item["nm_room"].ToString(), item["cd_state_nm"].ToString()));
                return sites;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 조회하기
        /// </summary>
        /// <returns></returns>
        public static bool IsPossible(int campSeq, DateTime dateTime, string site)
        {
            try
            {

                var parameter = string.Format("ctl00%24ctl00%24ctt%24ctt%24sm=ctl00%24ctl00%24ctt%24ctt%24uppcal%7Cctl00%24ctl00%24ctt%24ctt%24cal&__EVENTTARGET=ctl00%24ctl00%24ctt%24ctt%24cal&__EVENTARGUMENT=Select%24{0}&__VIEWSTATE=%2FwEPDwUJOTk3NzU3NTMyDxYGHgdpZF91c2VyBQEwHghpZF9ob3RlbAUEMTcwNx4Jc3VfcnN2ZGF5AjwWAmYPZBYCZg9kFgRmD2QWAgIHD2QWAgIDDxYCHgRocmVmBRkuLi9jc3MvcnN2U3lzL2NscjAwMDAuY3NzZAIBD2QWAgIBD2QWDAIBDxYCHgVzdHlsZQUNZGlzcGxheTpub25lOxYCZg8WAh8DBSUuLi9Sc3ZTeXMvQ2FsZW5kYXIuYXNweD9pZF9ob3RlbD0xNzA3ZAIDDxYEHwMFJS4uL1JzdlN5cy9DYW1wU2l0ZS5hc3B4P2lkX2hvdGVsPTE3MDceBWNsYXNzBQhzZWxlY3RlZGQCBQ8WAh8DBTIuLi9QYWdlL0FwcG1Db25maXJtLmFzcHg%2FaWRfaG90ZWw9MTcwNyZjZF9yZXE9MDAzMGQCBw8WAh8DBSlodHRwczovL3d3dy5kZG5heW8uY29tL0Ftcy8%2FaWRfaG90ZWw9MTcwN2QCCQ9kFgQCCw8PFgIeB1Zpc2libGVoZBYCAgEPFgIeC18hSXRlbUNvdW50ZmQCDw8PFgIfAQUEMTcwN2RkAgsPFgIeBFRleHQFM%2ByLpOyLnOqwhOyYiOyVveyLnOyKpO2FnCDsoIDsnpHqtow6IOuWoOuCmOyalOuLt%2By7tGRkiqosenb5ImH2PB9YBOmRJZzieNg%3D&__EVENTVALIDATION=%2FwEWBQL2kazuCgKK%2FI6nBQKOu6T7DAKxi%2BvGDgK5quPiCRYVdPtbWXeE5vYaV6BvjXtMF4wQ&ctl00%24ctl00%24ctt%24ctt%24id_zone=157&ctl00%24ctl00%24ctt%24ctt%24dt_sel=&ctl00%24ctl00%24ctt%24ctt%24keyRsv=&__ASYNCPOST=true&",
                    dateTime.ToString("yyyy-M-dd"));

                var httpWRequest =
                    (HttpWebRequest)
                        WebRequest.Create("http://www.ddnayo.com/RsvSys/CampSite.aspx?id_hotel=" + campSeq);
                httpWRequest.Accept = "*/*";
                httpWRequest.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
                httpWRequest.Headers.Add("Accept-Language", "en,en-US;q=0.8,ko;q=0.6");
                httpWRequest.Headers.Add("Origin", "http://www.ddnayo.com");
                httpWRequest.Referer = "http://www.ddnayo.com/RsvSys/CampSite.aspx?id_hotel=" + campSeq;
                httpWRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                httpWRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                httpWRequest.Method = "Post";
                httpWRequest.ContentLength = parameter.Length;

                var sw = new StreamWriter(httpWRequest.GetRequestStream());
                sw.Write(parameter);
                sw.Close();

                var theResponse = (HttpWebResponse)httpWRequest.GetResponse();
                var sr = new StreamReader(theResponse.GetResponseStream());

                var resultHtml = sr.ReadToEnd();

                resultHtml = resultHtml.Substring("jsonsites =", ";");

                var root = JObject.Parse(resultHtml);
                var sites = (from item in (JArray)root["res"]
                             where (item["nm_room"].ToString() == site && item["cd_state_nm"].ToString() =="예약가능")
                             select new Tuple<string, string>(item["nm_room"].ToString(), item["cd_state_nm"].ToString())
                             );

                return sites.Any();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}