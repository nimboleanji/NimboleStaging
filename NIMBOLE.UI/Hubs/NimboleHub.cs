using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using NIMBOLE.UI.Controllers;
using System.Net.Http;
using System.Configuration;

namespace NIMBOLE.UI.Hubs
{
    public class NimboleHub : Hub
    {
        HttpClient client;
        Uri contactUri = null;
        string strAPIURL = string.Empty;

        public List<string> BindValues(string empId)
        {
            strAPIURL = ConfigurationManager.AppSettings["ServiceUrl"].ToString();
            contactUri = new Uri(strAPIURL);
            client = new HttpClient();
            client.BaseAddress = contactUri;

            var response = client.GetAsync("api/Dashboard/GetAllCountsByEmpId?iEmpId=" + empId).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<List<string>>().Result;
                Update(result);
                return result;
            }            
            return null;
        }
        public void Update(List<string> result)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NimboleHub>();
            context.Clients.All.update(result);
        }
    }
}