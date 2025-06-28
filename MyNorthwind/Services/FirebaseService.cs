using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace MyNorthwind.Services;

public class FirebaseService
{
    private static bool _initialized = false;
    private static FirebaseApp? _firebaseApp;

    public static FirebaseApp GetFirebaseApp()
    {
        if (!_initialized)
        {
            var pathToServiceAccountKey = Environment.GetEnvironmentVariable("SERVICE_ACCOUNT_PATH");
            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(pathToServiceAccountKey)
            });
            _initialized = true;
        }

        return _firebaseApp;
    }

    public static async Task SendPushNotificationAsync(List<string> tokens, string title, string body,
        String customerId)
    {
        if (tokens.Count == 0)
        {
            return;
        }
        var message = new MulticastMessage()
        {
            Tokens = tokens, // FCM device token of the recipient
            Notification = new Notification
            {
                Title = title,
                Body = body
            },
            Data = new Dictionary<string, string>() { { "customerId", customerId } }
        };

        // Send the message
        var messaging = FirebaseMessaging.GetMessaging(GetFirebaseApp());
        var response = await messaging.SendEachForMulticastAsync(message);
        if (response.FailureCount > 0)
        {
            var failedTokens = new List<string>();
            for (var i = 0; i < response.Responses.Count; i++)
            {
                if (!response.Responses[i].IsSuccess)
                {
                    // The order of responses corresponds to the order of the registration tokens.
                    failedTokens.Add(tokens[i]);
                }
            }

            Console.WriteLine($"List of tokens that caused failures: {failedTokens}");
        }
        // Handle the response or log it
    }
}