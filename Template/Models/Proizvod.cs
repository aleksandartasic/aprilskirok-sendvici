using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Template.Models
{
    [Table("Proizvod")]
    public class Proizvod
    {
        [Key]
        [Column("ID")]    
        public int ID{get;set;}

        [Column("Naziv")]
        public string naziv{get;set;}

        [Column("Cena")]
        public int cena{get;set;}

        [JsonIgnore]
        public virtual Prodavnica Prodavnica {get;set;}
        
        [JsonIgnore]
        public List<ProizvodSastojak> ProizvodSastojak {get;set;}

        public Proizvod()
        {
        }
    }
}