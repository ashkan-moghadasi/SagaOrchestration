using System.Text;
using System.Text.Json;
using DataModel;

namespace Saga.Orchestrator.Proxies;

public class NotificationProxy : INotificationProxy
{
    private readonly IHttpClientFactory _clientFactory;

    public NotificationProxy(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<(int, bool)> SendNotification(Order order)
    {
        
        
        try
        {
            //Create Notification
            var request = JsonSerializer.Serialize(order);
            var notificationRequest = new HttpRequestMessage(HttpMethod.Post, "api/Notification");
            notificationRequest.Content = new StringContent(request, Encoding.UTF8, "application/JSON");
            var notificationClient = _clientFactory.CreateClient("NotificationService");
            var notificationResponse = await notificationClient.SendAsync(notificationRequest);

            var notificationId = await notificationResponse.Content.ReadAsStringAsync();
            return (Convert.ToInt32(notificationId), true);
        }
        catch (Exception e)
        {
            return (-1,false);
        }
    }
}