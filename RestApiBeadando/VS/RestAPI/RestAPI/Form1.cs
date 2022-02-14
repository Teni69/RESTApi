using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;

namespace RestAPI
{
    public partial class Form1 : Form
    {
        String URL = "http://localhost:8080/PHP/";
        String ROUTE = "index.php";
        String AUTH = "E76IKD|admin";

        public Form1()
        { InitializeComponent(); }    
                

        private void BtnGet_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var client = new RestClient(URL);
            var request = new RestRequest(ROUTE + "?A=" + AUTH, Method.GET);
            IRestResponse<List<Game>> response = client.Execute<List<Game>>(request);
            if (response.Content.Substring(1, response.Content.Length - 2) != "WRONG AUTH INFO")
            {
                foreach (Game game in response.Data)
                    listBox1.Items.Add("Id: " + game.id + " Név: " + game.name + " Műfaj: " + game.genre + " Ár: " + game.price + "$ Megjelenés éve: " + game.releaseYear);
            }
            else MessageBox.Show(response.Content);
        }

        private void BtnGetId_Click(object sender, EventArgs e)
        {
            var client = new RestClient(URL);
            String idRoute = ROUTE + "?A=" + AUTH + "&id=" + textBox1.Text;
            var request = new RestRequest(idRoute, Method.GET);
            IRestResponse<Game> response = client.Execute<Game>(request);
            if (response.Content.Substring(1, response.Content.Length - 2) != "WRONG AUTH INFO")
            {
                if (response.Content != "[]")
                {
                    var content = response.Content.Split(',')[2].Split(':')[1].ToString();
                    textBox2.Text = content.Substring(1, content.Length - 2);
                }
                else MessageBox.Show("Wrong ID");
            }
            else MessageBox.Show(response.Content);
        }

        private void BtnPost_Click(object sender, EventArgs e)
        {
            var client = new RestClient(URL);
            var request = new RestRequest(ROUTE + "?A=" + AUTH, Method.POST);            
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new Game
            {
                name = textBox2.Text,
                genre = textBox3.Text,
                price = int.Parse(textBox4.Text),
                releaseYear = int.Parse(textBox5.Text)
            });
            IRestResponse response = client.Execute(request);
            if (response.Content.Substring(1, response.Content.Length - 2) == "WRONG AUTH INFO")
                MessageBox.Show(response.Content);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var client = new RestClient(URL);
            String idRoute = ROUTE + "?A=" + AUTH + "&id=" + textBox1.Text;
            var request = new RestRequest(idRoute, Method.PUT);            
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new Game
            {
                name = textBox2.Text,
                genre = textBox3.Text,
                price = int.Parse(textBox4.Text),
                releaseYear = int.Parse(textBox5.Text)
            });
            IRestResponse response = client.Execute(request);
            if (response.Content.Substring(1, response.Content.Length - 2) == "WRONG AUTH INFO")
                MessageBox.Show(response.Content);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var client = new RestClient(URL);
            String idRoute = ROUTE + "?A=" + AUTH + "&id=" + textBox1.Text;
            var request = new RestRequest(idRoute, Method.DELETE);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(AUTH);
            IRestResponse response = client.Execute(request);
            if (response.Content.Substring(1, response.Content.Length - 2) == "WRONG AUTH INFO")
                MessageBox.Show(response.Content);
        }

        public class Game
        {
            public int id { get; set; }
            public string name { get; set; }
            public string genre { get; set; }
            public decimal price { get; set; }
            public decimal releaseYear { get; set; }
        }

    }
}
