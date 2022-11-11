using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaVeiculos.Model
{
   public  class ModeloExtra
    {
        private string chassi;

        private ModelCombustivel combustivel;
        public ModeloExtra()
        {
            combustivel = new ModelCombustivel();
        }
        public string Chassi { get => chassi; set => chassi = value; }
        public ModelCombustivel Combustivel { get => combustivel; set => combustivel = value; }
    }
}
