using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Tsp;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class TestTSAPostaRs
{
    public static void Main()
    {
        Test1();
        Test2();
        Test3();
    }
    public static void Test1()
    {
        try
        {
            // Choose the TSA server URL
            string tsaServerUrl = "http://test-tsa.ca.posta.rs/timestamp"; // Change this to the desired TSA URL

            // Create a TimeStampRequestGenerator
            TimeStampRequestGenerator timeStampRequestGenerator = new TimeStampRequestGenerator();
            timeStampRequestGenerator.SetCertReq(true); // Include certificate request

            // Create the message imprint
            byte[] dataToTimestamp = new byte[] { 0x54, 0x65, 0x73, 0x74, 0x20, 0x44, 0x61, 0x74, 0x61 };
            byte[] messageImprint = DigestUtilities.CalculateDigest("SHA-256", dataToTimestamp);

            // Generate a nonce
            Org.BouncyCastle.Math.BigInteger nonce = Org.BouncyCastle.Math.BigInteger.ValueOf(DateTime.UtcNow.Ticks);

            // Generate the timestamp request
            TimeStampRequest timeStampRequest = timeStampRequestGenerator.Generate(
                TspAlgorithms.Sha256,
                messageImprint,
                nonce
            );

            // Convert the request to bytes
            byte[] requestBytes = timeStampRequest.GetEncoded();

            // Create an HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                // Prepare the request content
                ByteArrayContent content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/timestamp-query");

                // Send the timestamp request to the TSA server
                HttpResponseMessage response = client.PostAsync(tsaServerUrl, content).Result;

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    // Get the response content
                    byte[] responseBytes = response.Content.ReadAsByteArrayAsync().Result;

                    // Parse the response
                    TimeStampResponse tspResponse = new TimeStampResponse(responseBytes);
                    PkiStatus status = (PkiStatus)tspResponse.Status;

                    if (status.Equals(PkiStatus.Granted) || status.Equals(PkiStatus.GrantedWithMods))
                    {
                        TimeStampToken timeStampToken = tspResponse.TimeStampToken;
                        // Process the timestamp token as needed
                        Console.WriteLine("Timestamp token received and verified successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Timestamp request failed with status: {status}");
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    public static void Test2()
    {
        try
        {
            // TSA server URL
            string tsaServerUrl = "http://test-tsa.ca.posta.rs/timestamp1"; // Change this to the desired TSA URL

            // Authentication credentials
            string username = "Test.Korisnik";
            string password = "123456";

            // Create a TimeStampRequestGenerator
            TimeStampRequestGenerator timeStampRequestGenerator = new TimeStampRequestGenerator();
            timeStampRequestGenerator.SetCertReq(true); // Include certificate request

            // Create the message imprint
            byte[] dataToTimestamp = new byte[] { 0x54, 0x65, 0x73, 0x74, 0x20, 0x44, 0x61, 0x74, 0x61 };
            byte[] messageImprint = DigestUtilities.CalculateDigest("SHA-256", dataToTimestamp);

            // Generate a nonce
            Org.BouncyCastle.Math.BigInteger nonce = Org.BouncyCastle.Math.BigInteger.ValueOf(DateTime.UtcNow.Ticks);

            // Generate the timestamp request
            TimeStampRequest timeStampRequest = timeStampRequestGenerator.Generate(
                TspAlgorithms.Sha256,
                messageImprint,
                nonce
            );

            // Convert the request to bytes
            byte[] requestBytes = timeStampRequest.GetEncoded();

            // Create an HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                // Prepare the request content
                ByteArrayContent content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/timestamp-query");

                // Set Basic Authentication header
                string authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

                // Send the timestamp request to the TSA server
                HttpResponseMessage response = client.PostAsync(tsaServerUrl, content).Result;

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    // Get the response content
                    byte[] responseBytes = response.Content.ReadAsByteArrayAsync().Result;

                    // Parse the response
                    TimeStampResponse tspResponse = new TimeStampResponse(responseBytes);
                    PkiStatus status = (PkiStatus)tspResponse.Status;

                    if (status.Equals(PkiStatus.Granted) || status.Equals(PkiStatus.GrantedWithMods))
                    {
                        TimeStampToken timeStampToken = tspResponse.TimeStampToken;
                        // Process the timestamp token as needed
                        Console.WriteLine("Timestamp token received and verified successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Timestamp request failed with status: {status}");
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    public static void Test3()
    {
        try
        {
            // TSA server URL
            string tsaServerUrl = "https://test-tsa.ca.posta.rs/timestamp2"; // Change this to the desired TSA URL

            // Path to the PFX certificate file and its password
            string certificatePath = "certs/TestKorisnik.pfx"; // Update with the actual path
            string certificatePassword = "1234";

            // Load the certificate
            X509Certificate2 certificate = new X509Certificate2(certificatePath, certificatePassword);

            // Create a TimeStampRequestGenerator
            TimeStampRequestGenerator timeStampRequestGenerator = new TimeStampRequestGenerator();
            timeStampRequestGenerator.SetCertReq(true); // Include certificate request

            // Create the message imprint
            byte[] dataToTimestamp = new byte[] { 0x54, 0x65, 0x73, 0x74, 0x20, 0x44, 0x61, 0x74, 0x61 };
            byte[] messageImprint = DigestUtilities.CalculateDigest("SHA-256", dataToTimestamp);

            // Generate a nonce
            Org.BouncyCastle.Math.BigInteger nonce = Org.BouncyCastle.Math.BigInteger.ValueOf(DateTime.UtcNow.Ticks);

            // Generate the timestamp request
            TimeStampRequest timeStampRequest = timeStampRequestGenerator.Generate(
                TspAlgorithms.Sha256,
                messageImprint,
                nonce
            );

            // Convert the request to bytes
            byte[] requestBytes = timeStampRequest.GetEncoded();

            // Create an HttpClient instance with certificate authentication
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ClientCertificates.Add(certificate);
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                using (HttpClient client = new HttpClient(handler))
                {
                    // Prepare the request content
                    ByteArrayContent content = new ByteArrayContent(requestBytes);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/timestamp-query");

                    // Send the timestamp request to the TSA server
                    HttpResponseMessage response = client.PostAsync(tsaServerUrl, content).Result;

                    // Check the response status
                    if (response.IsSuccessStatusCode)
                    {
                        // Get the response content
                        byte[] responseBytes = response.Content.ReadAsByteArrayAsync().Result;

                        // Parse the response
                        TimeStampResponse tspResponse = new TimeStampResponse(responseBytes);
                        PkiStatus status = (PkiStatus)tspResponse.Status;

                        if (status.Equals(PkiStatus.Granted) || status.Equals(PkiStatus.GrantedWithMods))
                        {
                            TimeStampToken timeStampToken = tspResponse.TimeStampToken;
                            // Process the timestamp token as needed
                            Console.WriteLine("Timestamp token received and verified successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Timestamp request failed with status: {status}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
