using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaVeiculos.Model
{
    public class ModelConsulta
    {
        private ModeloExtra extra;
        public ModelConsulta()
        {
            extra = new ModeloExtra();
        }

        private string placa, placa_alternativa, modelo, marca, ano, anoModelo, cor, versao, municipio, uf, logo;
        
        public string Placa { get => placa; set => placa = value; }
        public string Placa_alternativa { get => placa_alternativa; set => placa_alternativa = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Ano { get => ano; set => ano = value; }
        public string AnoModelo { get => anoModelo; set => anoModelo = value; }
        public string Cor { get => cor; set => cor = value; }
        public string Versao { get => versao; set => versao = value; }
        public string Municipio { get => municipio; set => municipio = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Logo { get => logo; set => logo = value; }
        public ModeloExtra Extra { get => extra; set => extra = value; }
    }
}
