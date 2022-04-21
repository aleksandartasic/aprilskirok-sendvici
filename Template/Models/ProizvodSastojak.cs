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
       //AKO SE DODA JSON IGNORE TJ NE VRACA SE JSON ZA SASTOJKE AKO STAVIS [JSONIGNORE] ISPRED SASTOJAK
       /*  "id":1,"kolicina":1,"proizvod":null,"sastojak":{"id":1,"naziv":"SVINJSKO meso","cena":100}},
        {"id":2,"kolicina":1,"proizvod":null,"sastojak":{"id":2,"naziv":"SVINJSKO meso","cena":100}},
        {"id":3,"kolicina":1,"proizvod":null,"sastojak":{"id":3,"naziv":"junece meso meso","cena":100}},
        {"id":4,"kolicina":1,"proizvod":null,"sastojak":{"id":4,"naziv":"junece meso meso","cena":100}},
        {"id":5,"kolicina":1,"proizvod":null,"sastojak":{"id":4,"naziv":"junece meso meso","cena":100}}]*/
        public Sastojak Sastojak{get;set;}
  
        public ProizvodSastojak()
        {
           
        }
    }
}
