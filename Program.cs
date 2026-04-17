using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace 拉片pro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8848/");
            listener.Start();
            Console.WriteLine("Listening on port 8848...");
            //打开网页
            Process.Start("http://localhost:8848/");
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                response.ContentEncoding = Encoding.UTF8;

                // 添加CORS头
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                try
                {
                    string v = request.QueryString["v"];

                    if (request.HttpMethod == "GET" && v == "data")
                    {
                        HandleGetRequest(request, response);
                    }
                    else if (request.HttpMethod == "POST" && v == "add")
                    {
                        HandlePostRequest(request, response);
                    }
                    else if (request.HttpMethod == "GET" && v == "del")
                    {
                        HandleGetRequestdel(request, response);
                    }
                    else
                    {
                        string filePath = @"lapianpage.htmll";

                        try
                        {
                            // 检查文件是否存在
                            if (File.Exists(filePath))
                            {
                                // 读取文件所有内容
                                string content = File.ReadAllText(filePath);
                                SendResponsehtml(response, content, 200);
                                //Console.WriteLine(content);
                            }
                            else
                            {
                                Console.WriteLine($"文件 {filePath} 不存在。");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"读取文件时出错: {ex.Message}");
                        }


                        //SendResponse(response, "{\"error\":\"Invalid request\"}", 400);
                    }
                }
                catch (Exception ex)
                {
                    SendResponse(response, $"{{\"error\":\"Server error: {ex.Message}\"}}", 500);
                }
            }
            //读取详细

            //入库存储
        }
        static void HandleGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            string md5 = request.QueryString["md5"];

            if (string.IsNullOrEmpty(md5))
            {
                SendResponse(response, "{\"error\":\"Missing 'md5' parameter\"}", 400);
                return;
            }

            string caseFolder = Path.Combine("case", md5);
            string jsonFile = Path.Combine(caseFolder, "index.json");

            if (File.Exists(jsonFile))
            {
                string jsonContent = File.ReadAllText(jsonFile);
                SendResponse(response, jsonContent, 200);
            }
            else
            {
                SendResponse(response, "{\"error\":\"Request failed, index.json not found\"}", 404);
            }
        }
        static void HandleGetRequestdel(HttpListenerRequest request, HttpListenerResponse response)
        {
            string md5 = request.QueryString["md5"];
            string id = request.QueryString["id"];

            if (string.IsNullOrEmpty(md5))
            {
                SendResponse(response, "{\"error\":\"Missing 'md5' parameter\"}", 400);
                return;
            }

            if (string.IsNullOrEmpty(id))
            {
                SendResponse(response, "{\"error\":\"Missing 'id' parameter\"}", 400);
                return;
            }

            string caseFolder = Path.Combine("case", md5);
            string jsonFile = Path.Combine(caseFolder, "index.json");

            if (!File.Exists(jsonFile))
            {
                SendResponse(response, "{\"error\":\"Request failed, index.json not found\"}", 404);
                return;
            }

            try
            {
                // 读取JSON文件
                string jsonContent = File.ReadAllText(jsonFile);
                var items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonContent);

                if (items == null)
                {
                    SendResponse(response, "{\"error\":\"Invalid JSON format\"}", 400);
                    return;
                }

                // 查找并删除匹配id的项
                int originalCount = items.Count;
                items.RemoveAll(item => item.ContainsKey("id") && item["id"] == id);

                if (items.Count == originalCount)
                {
                    SendResponse(response, "{\"error\":\"ID not found\"}", 404);
                    return;
                }

                // 保存修改后的JSON
                string updatedJson = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(jsonFile, updatedJson);

                SendResponse(response, "{\"success\":\"Item deleted successfully\"}", 200);
            }
            catch (Exception ex)
            {
                SendResponse(response, $"{{\"error\":\"An error occurred: {ex.Message}\"}}", 500);
            }
        }
        static void HandlePostRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (!request.HasEntityBody)
            {
                SendResponse(response, "{\"error\":\"No data received\"}", 400);
                return;
            }
           
            //先删除一次，再添加
           

            using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                string jsonData = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
                #region 先删除在添加
                try
                {
                    string md5 = data["videoMd5"].ToString(); 
                    string id = data["id"].ToString();

                    string caseFolder = Path.Combine("case", md5);
                    string jsonFile = Path.Combine(caseFolder, "index.json");

                    try
                    {
                        // 读取JSON文件
                        string jsonContent = File.ReadAllText(jsonFile);
                        var items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonContent);


                        // 查找并删除匹配id的项
                        int originalCount = items.Count;
                        items.RemoveAll(item => item.ContainsKey("id") && item["id"] == id);


                        // 保存修改后的JSON
                        string updatedJson = JsonConvert.SerializeObject(items, Formatting.Indented);
                        File.WriteAllText(jsonFile, updatedJson);

                    }
                    catch (Exception ex)
                    {
                        //  SendResponse(response, $"{{\"error\":\"An error occurred: {ex.Message}\"}}", 500);
                    }
                }
                catch { }
                #endregion
                try
                {
                   
                    if (!data.ContainsKey("videoMd5"))
                    {
                        SendResponse(response, "{\"error\":\"Missing videoMd5 in data\"}", 400);
                        return;
                    }

                    string videoMd5 = data["videoMd5"].ToString();
                    string caseFolder = Path.Combine("case", videoMd5);
                    string jsonFile = Path.Combine(caseFolder, "index.json");

                    Directory.CreateDirectory(caseFolder);

                    List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
                    if (File.Exists(jsonFile))
                    {
                        string existingJson = File.ReadAllText(jsonFile);
                        items = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(existingJson);
                    }

                    items.Add(data);

                    string updatedJson = JsonConvert.SerializeObject(items, Formatting.Indented);
                    File.WriteAllText(jsonFile, updatedJson);

                    SendResponse(response, "{\"success\":\"Data added successfully\"}", 200);
                }
                catch (Exception err)
                {
                    string aaa = err.Message;
                    SendResponse(response, "{\"error\":\"Invalid JSON data\"}", 400);
                }
            }
        }

        static void SendResponse(HttpListenerResponse response, string content, int statusCode)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = buffer.Length;
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
        static void SendResponsehtml(HttpListenerResponse response, string content, int statusCode)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = buffer.Length;
            response.StatusCode = statusCode;
            response.ContentType = "text/html; charset=utf-8";
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

    }
}
