using CreatTransferTransactiom.DLL;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Xml;
using System.Xml.Linq;
namespace CreatTransferTransactiom
{
    public class DDLCreateTransfere
    {
        DAL DalCode = new DAL();
        public string CheckChannel(String ChannelName, string username, string ServiceName)
        {
            string ChannelIP = "";
            string statusChannel = "";
            string EnableChannel = "";
            DataTable dt_Channel = DalCode.IMALChannelstatus(ChannelName, username, ChannelIP, ServiceName);
            DDLCreateTransfere[] BR_Channel = new DDLCreateTransfere[dt_Channel.Rows.Count];
            if (BR_Channel.Length != 0)
            {
                int ii;
                for (ii = 0; ii < dt_Channel.Rows.Count; ii++)
                {

                    EnableChannel = dt_Channel.Rows[ii]["EnableChannel"].ToString().Trim();
                }
            }
            if (EnableChannel == "1")
            {
                statusChannel = "Enabled";
            }
            else
            {
                statusChannel = "Disabled";
            }
            return statusChannel;
        }

        public string CreatTr(string transactionType, string To_additionalRef, string from_additionalRef, string transactionPurpose, string transactionAmount, string currencyIso, string transactionDate,
          string valueDate, string userID, string password)
        {

            string soapResult = string.Empty;
            string StatusDesc = string.Empty;
            string StatusCode = string.Empty;
            string TransactionAmount = string.Empty;
            string TransactionDate = string.Empty;
            string TransactionNumber = string.Empty;
            string TransactionPurpose = string.Empty;
            string TransactionType = string.Empty;
           

            List<CsCreatTransferRequest> logrequest = new List<CsCreatTransferRequest>();
            List<CsCreateTransferResponse> logresponse = new List<CsCreateTransferResponse>();
           // DateTime curent = DateTime.ParseExact(dateRequested, "dd-MM-yyyy", null);
            //dateRequested = Convert.ToDateTime(curent).ToString("yyyy-MM-dd");
            string RequestID = "MW-CHQREQ-" + transactionType + "-" + DateTime.Now.ToString("ddMMyyyyHHmmssff");
            string requesterTimeStamp = System.DateTime.Now.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss");

           /* try
            {*/
                logrequest.Add(new CsCreatTransferRequest
                {
                    transactionType = transactionType,
                    To_additionalRef = To_additionalRef,
                    from_additionalRef = from_additionalRef,
                    transactionPurpose = transactionPurpose,
                    transactionAmount = transactionAmount,
                    currencyIso = currencyIso,
                    transactionDate = transactionDate,
                    valueDate = valueDate,
                    userID = userID,
                    password = "******",
                    


                });
                string ClientRequest = JsonConvert.SerializeObject(logrequest, Newtonsoft.Json.Formatting.Indented);
               // DalCode.InsertLog("CheqBookCreate", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")), ClientRequest, "Pending", ChannelName, RequestID);
               // string status = CheckChannel(ChannelName, userID, "CheqBookCreate");
               // if (status == "Enabled")
               // {
                    HttpWebRequest request = HTTPl.CreateWebRequestTransfer();
                    XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tran=""transferWs"">
   <soapenv:Header/>
   <soapenv:Body>
      <tran:createTransfer>
         <!--You may enter the following 68 items in any order-->
         <!--Optional:-->
         <serviceContext>
            <!--You may enter the following 6 items in any order-->
            <!--Optional:-->
            <businessArea>Retail</businessArea>
            <!--Optional:-->
            <businessDomain>PaymentsOperationsManagement</businessDomain>
            <!--Optional:-->
            <operationName>createTransfer</operationName>
            <!--Optional:-->
            <serviceDomain>Transfer</serviceDomain>
            <!--Optional:-->
            <serviceID>4801</serviceID>
            <!--Optional:-->
            <version>1.0</version>
         </serviceContext>

         
         <companyCode>1</companyCode>
         <branchCode>5599</branchCode>
         <transactionType>" + transactionType + @"</transactionType>
         
         <fromAccount>
         		<additionalRef>" + from_additionalRef + @"</additionalRef>
         </fromAccount>
         
         <toAccounts>
         		<multiAccount>
         			<account>
         				<additionalRef>" + To_additionalRef + @"</additionalRef>
         			</account>
         		</multiAccount>
         </toAccounts>

         
         <transactionPurpose>" + transactionPurpose + @"</transactionPurpose>
         <transactionAmount>" + transactionAmount + @"</transactionAmount>
         <currencyIso>" + currencyIso + @"</currencyIso> 
         <transactionDate>" + transactionDate + @"</transactionDate>  <!-- Transaction Date -->
         <valueDate>" + valueDate + @"</valueDate>   <!-- Transaction Date -->
         <useDate>0</useDate>       <!-- Default -->
<!--         <differentTradeValueDate>1</differentTradeValueDate>-->
         
         <useAccount>1</useAccount> <!-- Default -->



        <requestContext>
           <requestID>" + RequestID+ @"</requestID>
           <coreRequestTimeStamp>" + requesterTimeStamp + @"</coreRequestTimeStamp>
         </requestContext>
         
         <requesterContext>
         		<channelID>1</channelID>
         		<hashKey>1</hashKey>
         		<langId>EN</langId>
         		<password>" + password + @"</password>
         		<requesterTimeStamp>" + requesterTimeStamp + @"</requesterTimeStamp>
         		<userID>" + userID + @"</userID>
         </requesterContext>



         <!--Optional:-->
         <vendorContext>
            <!--You may enter the following 3 items in any order-->
            <!--Optional:-->
            <license>Copyright 2018 Path Solutions. All Rights Reserved</license>
            <!--Optional:-->
            <providerCompanyName>Path Solutions</providerCompanyName>
            <!--Optional:-->
            <providerID>IMAL</providerID>
         </vendorContext>
      </tran:createTransfer>
   </soapenv:Body>
</soapenv:Envelope>
");

            using (Stream stream = request.GetRequestStream())
                    {
                        soapEnvelopeXml.Save(stream);
                    }


                    using (WebResponse response = request.GetResponse())
                    {
                        
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            soapResult = rd.ReadToEnd();
                            //Console.WriteLine(soapResult);
                            var str = XElement.Parse(soapResult);
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(soapResult);
                            XmlNodeList elemStatusCode = xmlDoc.GetElementsByTagName("statusCode");
                            StatusCode = elemStatusCode[0].InnerXml;
                            XmlNodeList elemStatusCodeDes = xmlDoc.GetElementsByTagName("statusDesc");
                            StatusDesc = elemStatusCodeDes[0].InnerXml;


                    if (StatusCode == "0")
                        {
                            XmlNodeList elemtransactionAmount = xmlDoc.GetElementsByTagName("transactionAmount");
                            transactionAmount = elemtransactionAmount[0].InnerXml;

                            XmlNodeList elemtransactionDate = xmlDoc.GetElementsByTagName("transactionDate");
                            transactionDate = elemtransactionDate[0].InnerXml;

                            XmlNodeList elemtransactionNumber = xmlDoc.GetElementsByTagName("transactionNumber");
                            TransactionNumber= elemtransactionNumber[0].InnerXml;

                            XmlNodeList elemtransactionPurpose = xmlDoc.GetElementsByTagName("transactionPurpose");
                            transactionPurpose = elemtransactionPurpose[0].InnerXml;

                            XmlNodeList elemtransactionType = xmlDoc.GetElementsByTagName("transactionType");
                            transactionType = elemtransactionType[0].InnerXml;

                            logresponse.Add(new CsCreateTransferResponse
                            {
                                transactionNumber=TransactionNumber,
                                transactionAmount=TransactionAmount,
                                transactionDate=TransactionDate,
                                transactionPurpose=TransactionPurpose,
                                transactionType=TransactionType,
                                StatusCode=StatusCode,
                                StatusDesc=StatusDesc,

                            });
                        }
                        else
                        {
                            logresponse.Add(new CsCreateTransferResponse
                            {

                                
                                StatusCode=StatusCode,
                                StatusDesc=StatusDesc,

                            }) ;
                        }


                        }
                }
            return JsonConvert.SerializeObject(logresponse, Newtonsoft.Json.Formatting.Indented);


        }
    }
        
        }
   


    

            

