using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MyShift.Notifications.Entitys;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyShift.Notifications.Service
{
    public class ServiceEng : IService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogsRepository _LogsRepository;
        public ServiceEng(IConfiguration configuration, ILogsRepository logsRepository)
        {
            _configuration = configuration;
            _LogsRepository = logsRepository;
        }
        public async Task<string> GetDataToNotifiy()
        {
            var DataTobeNotifyed = await _LogsRepository.GetLogsQueries();

            var UpdateData= await _LogsRepository.UpdateLogsCommand(DataTobeNotifyed);

            var QueueMessage = JsonConvert.SerializeObject(DataTobeNotifyed);
            return QueueMessage;
        }
        public bool SendDataToQueue(string message)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("AzureWebJobsStorage");
                QueueClient queueClient = new QueueClient
                (connectionString, "test4");
                queueClient.CreateIfNotExists();
                if (queueClient.Exists())
                {
                    queueClient.SendMessage(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
      
    }
}
