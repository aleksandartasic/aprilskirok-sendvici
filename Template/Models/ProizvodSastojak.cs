using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Template.Models{
    [Table("ProizvodSastojak")]
    public class ProizvodSastojak//sastojci ali ajde da ne menjam
    {
        [Key]
        public int ID{get;set;}

        public int Kolicina{get;set;}

      //  [JsonIgnore]  
        public Proizvod Proizvod{get;set;}

       //[JsonIgnore]///POGLEDAJ SLIKU , MENJA SE RESPONSE 
       //AKO SE DODA JSON IGNORE TJ NE VRACA SE JSON ZA SASTOJKE 
        public Sastojak Sastojak{get;set;}
  
        public ProizvodSastojak()
        {
           
        }
    }
}
