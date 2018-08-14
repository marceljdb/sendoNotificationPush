using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SendNotification
{
    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FCMResult> results { get; set; }
    }
    public class FCMResult
    {
        public string message_id { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SendNotification();
        }

        static void SendNotification()
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var objNotification = new
            {                
                to = "fI_xwXmUPWU:APA91bFzOtoUaqfgfyi3KM66e1SQ6dUSn30I33aVBqlYZKAqrmQpVvUCY36wVzFjOGuqRKoE59mrtpdP0BFfPQz1PPH8IAn_FBFLHTPMalNvytmn0i8hClzLTPy-HPoDtfM0FNT_GaL9",
                data = new
                {
                    title = "Teste",
                    body = "teste"
                }
            };
            string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);

            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAYiWi8Oo:APA91bEGwqA5d-HenDFwIW8DMOYbTYy2IyDOItjVPCLLvFH9jcgfjmvNiRBjC-UrdY2JOZPaDu780c8YXtOiLHIBPkk9YXEe27XnEHctoGYdVERt_oG3s-nmP3KK5UNQEQhHN6at4V-q"));
            tRequest.Headers.Add(string.Format("Sender: id={0}", "421538230506 "));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();

                            FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            if (response.success == 1)
                            {
                                Console.Write("Enviado");
                                //new NotificationBLL().InsertNotificationLog(dayNumber, notification, true);
                            }
                            else if (response.failure == 1)
                            {
                                Console.Write("Não Foi");

                            }
                            Console.Read();

                        }
                    }

                }
            }

        }
    }
}
