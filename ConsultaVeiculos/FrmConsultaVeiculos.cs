using ConsultaVeiculos.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultaVeiculos
{
    public partial class FrmConsultaVeiculos : Form
    {
        #region Declaração de variáveis 
        string uri, placa = string.Empty;
        List<string> jsonRetornado = new List<string>();
        ModelConsulta veiculo = new ModelConsulta();
        Regex validaPlacaDigitada = new Regex(@"^[A-z]{3}[0-9]{1}[A-z 0-9]{1}[0-9]{2}");
        FrmLoading loging;
        Thread loadingThread;
        #endregion

        #region Inicialização
        public FrmConsultaVeiculos()
        {
            InitializeComponent();
        }

        private void FrmConsultaVeiculos_Load(object sender, EventArgs e)
        {
            picImagemLogo.Visible = false;
            Width = 372;
            Height = 138;
            uri = ConfigurationManager.AppSettings["URI"];
        }
        #endregion

        #region Metodos

        public void Show()
        {
            loadingThread = new Thread(new ThreadStart(LoadingProcess));
            loadingThread.Start();
        }
        public void Show(Form parent) 
        {
            loadingThread = new Thread(new ParameterizedThreadStart(LoadingProcess));
            loadingThread.Start(parent);
        }
        public void Close()
        {
            if (loging != null)
            {
                loging.BeginInvoke(new ThreadStart(loging.CloseWaitForm));
                loging = null;
                loadingThread = null;
            }
        }
        private void LoadingProcess() 
        {
            loging = new FrmLoading();
            loging.ShowDialog();
        }
        private void LoadingProcess(object parent)
        {
            Form parent1 = parent as Form;
            loging = new FrmLoading(parent1);
            loging.ShowDialog();
        }
        private async Task BuscaPlaca(string placa)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("chave", "chavedemo");
                var req = new HttpRequestMessage(HttpMethod.Post, uri);
                var reqBody = JsonConvert.SerializeObject(new { placa = placa });

                req.Content = new StringContent(reqBody, Encoding.UTF8, "application/json");
                var resp = client.SendAsync(req).Result;

                if (resp.IsSuccessStatusCode)
                {
                    var VeiculoJsonString = await resp.Content.ReadAsStringAsync();
                    var dados = JsonConvert.DeserializeObject<ModelConsulta>(VeiculoJsonString);
                    AtribuiDadosAComponentes(dados);
                }
                else
                {
                    Width = 372;
                    Height = 138;
                    MessageBox.Show("Não houve nenhem retorno" + resp.ReasonPhrase);
                }
            }
            catch (Exception ex) { }
        }
        private void AtribuiDadosAComponentes(ModelConsulta dados)
        {
            try
            {
                picImagemLogo.Visible = true;
                lblPlacaAntiga.Text = string.IsNullOrEmpty(dados.Placa) ? "S/ info" : dados.Placa;
                lblPlacaNova.Text = string.IsNullOrEmpty(dados.Placa_alternativa) ? "S/ info" : dados.Placa_alternativa;
                lblVeiculo.Text = string.IsNullOrEmpty(dados.Modelo) ? "S/ info" : dados.Modelo;
                lblMarca.Text = string.IsNullOrEmpty(dados.Marca) ? "S/ info" : dados.Marca;
                lblAno.Text = string.IsNullOrEmpty(dados.Ano) ? "S/ info" : dados.Ano;
                lblAnoModelo.Text = string.IsNullOrEmpty(dados.AnoModelo) ? "S/ info" : dados.AnoModelo;
                lblCor.Text = string.IsNullOrEmpty(dados.Cor) ? "S/ info" : dados.Cor;
                lblCombustivel.Text = string.IsNullOrEmpty(dados.Extra.Combustivel.Combustivel) ? "S/ info" : dados.Extra.Combustivel.Combustivel;
                lblVersao.Text = string.IsNullOrEmpty(dados.Versao) ? "S/ info" : dados.Versao;
                lblMunicipio.Text = string.IsNullOrEmpty(dados.Municipio) ? "S/ info" : dados.Municipio;
                lblEstado.Text = string.IsNullOrEmpty(dados.Uf) ? "S/ info" : dados.Uf;
                lblChassi.Text = string.IsNullOrEmpty(dados.Extra.Chassi) ? "S/ info" : dados.Extra.Chassi;
                picImagemLogo.Load(string.IsNullOrEmpty(dados.Logo) ? null : dados.Logo); ;
            }
            catch (Exception ex) { }
        }

        private void LimpaCampos()
        {
            lblAno.Text = string.Empty;
            lblAnoModelo.Text = string.Empty;
            lblChassi.Text = string.Empty;
            lblCombustivel.Text = string.Empty;
            lblCor.Text = string.Empty;
            lblEstado.Text = string.Empty;
            lblMarca.Text = string.Empty;
            lblMunicipio.Text = string.Empty;
            lblPlacaAntiga.Text = string.Empty;
            lblPlacaNova.Text = string.Empty;
            lblVeiculo.Text = string.Empty;
            lblVersao.Text = string.Empty;
            picImagemLogo.Image = null;

        }

        #endregion

        #region Eventos
        private void button1_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            if (!validaPlacaDigitada.IsMatch(txtPlaca.Text))
            {
                lblAjudaUser.Visible = true;
                return;
            }
            lblAjudaUser.Visible = false;

            placa = txtPlaca.Text;
            Show(this);
            Thread.Sleep(2000);
            BuscaPlaca(placa).Wait(2000);
            Close();
            Width = 650;
            Height = 495;

            TimerHabiliarCampos.Enabled = true;
            txtPlaca.Enabled = false;
            btnConsulta.Enabled = false;
        }
        private void TimerHabiliarCampos_Tick(object sender, EventArgs e)
        {

            if (TimerHabiliarCampos.Interval == 60000)
            {
                txtPlaca.Enabled = true;
                btnConsulta.Enabled = true;
                TimerHabiliarCampos.Stop();
                TimerHabiliarCampos.Enabled = false;
            }
        }
        #endregion
    }
}
