using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Template.Models{
    [Table("Sastojak")]
    public class Sastojak
    {
        [Key]
        public int ID{get;set;}

        public string Naziv{get;set;}

        public int Cena{get;set;}
       
        [JsonIgnore]
        public List<Prodavnica> Prodavnica{get;set;}
        
        [JsonIgnore]
        public List<ProizvodSastojak> ProizvodSastojak{get;set;}
     
        public Sastojak()
        {
           this.Prodavnica=new List<Prodavnica>();
           this.ProizvodSastojak=new List<ProizvodSastojak>();
        }
    }
}
