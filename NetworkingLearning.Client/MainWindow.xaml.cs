using System.IO;
using System.Linq;
using System.Net;
using System.Windows;


namespace NetworkingLearning.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void request_Click(object sender, RoutedEventArgs e)
        {
            // Создать объект запроса
            WebRequest request = WebRequest.Create(txb_url.Text);

            // Получить ответ с сервера
            WebResponse response = request.GetResponse();

            // Получаем поток данных из ответа
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                // Выводим исходный код страницы
                string line;

                while ((line = stream.ReadLine()) != null)
                {
                    txb_sourceCode.Text += line + "\n";
                }
            }

            // Получаем некоторые данные о сервере
            string messageServer = "Целевой URL: \t" + request.RequestUri + 
                                 "\nМетод запроса: \t" + request.Method +
                                 "\nТип полученных данных: \t" + response.ContentType + 
                                 "\nДлина ответа: \t" + response.ContentLength + 
                                 "\nЗаголовки";

            // Получаем заголовки, используем LINQ
            WebHeaderCollection whc = response.Headers;
            var headers = Enumerable.Range(0, whc.Count)
                                    .Select(p =>
                                    {
                                        return new
                                        {
                                            Key = whc.GetKey(p),
                                            Names = whc.GetValues(p)
                                        };
                                    });

            foreach (var item in headers)
            {
                messageServer += "\n  " + item.Key + ":";
                foreach (string? n in item.Names)
                {
                    messageServer += "\t" + n;
                }
            }

            txb_serverInfo.Text = messageServer;
        }

    }
}
