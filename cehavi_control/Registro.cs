using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cehavi_control
{

    

    class Registro
    {

      
        private string NombreString = "";
        private object CurrentValue;

        public Registro(string Nombre, Object valor )
        {
            this.CurrentValue = valor;
            this.NombreString = Nombre;

        }


        public void SetValue(Object valor)
        {
            this.CurrentValue = valor;
        }
        

        public object getValue()
        {
            return this.CurrentValue;
        }
        

    }
}
