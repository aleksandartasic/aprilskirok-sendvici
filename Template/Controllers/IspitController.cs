using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;////dodato za toListAsync
using Microsoft.Extensions.Logging;
using Template.Models;

namespace Template.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IspitController : ControllerBase
    {
        IspitDbContext Context { get; set; }

        public IspitController(IspitDbContext context)
        {
            Context = context;
        }

        [Route("PreuzmiProdavnice")]
        [HttpGet]
        public async Task<List<Prodavnica>> PreuzmiProdavnice()
        {
          //var vraceneProdavnice=await Context.Prodavnica.FindAsync(idProdavnice);
          return await Context.Prodavnica.ToListAsync();
        }

        [Route("PreuzmiSastojke/{idProdavnice}")]
        [HttpGet]
        public async Task<List<Sastojak>> PreuzmiSastojke(int idProdavnice)
        {
          var vraceneProdavnice=await Context.Prodavnica.Where(p=>p.ID==idProdavnice).FirstAsync();////posto je n m veza
          var vraceniTipovi=await Context.Sastojak.Where(p=>p.Prodavnica.Contains(vraceneProdavnice)).ToListAsync();
          return vraceniTipovi;
        }
   
        [Route("PreuzmiProizvod/{idProdavnice}")]
        [HttpGet]
        public async Task<List<Proizvod>> PreuzmiProizvod(int idProdavnice)
        {
          ////posto je n m veza
          var vraceniProizvodi=await Context.Proizvod.Where(p=>p.Prodavnica.ID==idProdavnice).ToListAsync();
          return vraceniProizvodi;
        }
    
        [Route("DodavanjeSastojakaUProizvod/{kolicina}/{idProizvoda}/{idSastojka}/{idProd}")]
        [HttpPost]
        public async Task<ActionResult> DodavanjeSastojakaUProizvod(int kolicina,int idProizvoda, int idSastojka,int idProd)
        {
         //////prosledi niz kolicina i sastojaka!!!!!!!!!!!!!!!!!!!!!!!!!*/
        var trazeniSastojak=await Context.Sastojak.Where(p=>p.ID==idSastojka).FirstAsync();
        var trazeniProizvod=await Context.Proizvod.Where(p=>p.ID==idProizvoda).FirstAsync();

          //var nadjeniPodaci = await Context.Proizvod.Where(p=>p.Prodavnica.ID==idProd && p.Tip.ID==idTipa  && p.cena>=nizacena  && p.cena<=visacena).ToListAsync(); ///moras tamo gde je await da imas i async
          
          //if (nadjeniPodaci!=null){

        ProizvodSastojak noviSastProizvod=new ProizvodSastojak();

        noviSastProizvod.Proizvod=trazeniProizvod;
        noviSastProizvod.Sastojak=trazeniSastojak;
        noviSastProizvod.Kolicina=kolicina;
             
        Context.proizvodSastojak.Add(noviSastProizvod);

        await Context.SaveChangesAsync();
        return Ok("uspelo");
        }    
  

        
        [Route("UcitavanjeProizvoda/{idProdavnice}")]
        [HttpGet]
        public async Task<List<Proizvod>> UcitavanjeProizvoda(int idProdavnice)
        {///radi preko contains posto je to jedini nacin da pretrazujes da li je neki manji string u vecem stringu!
        //niz izabranih proizvoda sastoji se od id-eva elemenata i slova a koje ih razdvaja.

          var NadjeniProizvod=await Context.Proizvod.Where(p=>p.Prodavnica.ID==idProdavnice).ToListAsync();
          return NadjeniProizvod;
        }

          
        [Route("UcitavanjeSastojka/{idProizvoda}")]
        [HttpGet]
        public async Task<List<ProizvodSastojak>> UcitavanjeSastojka(int idProizvoda)
        {
        ///Include ti je da ti odredi sta ti se vraca u response-u OBRATI PAZNJU AKO STAVIS JSON IGNORE 
        var NadjeniProizvodiSastojci=await Context.proizvodSastojak.Where(p=>p.Proizvod.ID==idProizvoda).Include(p=>p.Sastojak)/*.Include(p=>p.Proizvod)*/.ToListAsync();
        if (NadjeniProizvodiSastojci!= null)
        {  
        /*List<Sastojak> nadjeniSastojci=null;
        foreach(var element in NadjeniProizvodiSastojci)
        {
        nadjeniSastojci=await Context.Sastojak.Include(p=>p.ProizvodSastojak).Where(p=>p.ID==element.Sastojak.ID).ToListAsync();
        }*/
        /*
        [{"id":1,"kolicina":1,"proizvod":null,"sastojak":{"id":1,"naziv":"SVINJSKO meso","cena":100}},
        {"id":2,"kolicina":1,"proizvod":null,"sastojak":{"id":2,"naziv":"SVINJSKO meso","cena":100}},
        {"id":3,"kolicina":1,"proizvod":null,"sastojak":{"id":3,"naziv":"junece meso meso","cena":100}},
        {"id":4,"kolicina":1,"proizvod":null,"sastojak":{"id":4,"naziv":"junece meso meso","cena":100}},
        {"id":5,"kolicina":1,"proizvod":null,"sastojak":{"id":4,"naziv":"junece meso meso","cena":100}}]
        */


        return NadjeniProizvodiSastojci;
        }
        else return null;
        }

        [Route("CenaPojedinacnogProizvoda/{idProdavnice}")]
        [HttpGet]
        public async Task<int> CenaPojedinacnogProizvoda(int idProizvoda)
        {///radi preko contains posto je to jedini nacin da pretrazujes da li je neki manji string u vecem stringu!
        //niz izabranih proizvoda sastoji se od id-eva elemenata i slova a koje ih razdvaja.
          int CenaPojedinacnog=0;
          var NadjeniProizvodiSastojci=await Context.proizvodSastojak.Where(p=>p.Proizvod.ID==idProizvoda).Include(p=>p.Sastojak).ToListAsync();
          foreach(var element in NadjeniProizvodiSastojci)
          {
            CenaPojedinacnog=CenaPojedinacnog+element.Sastojak.Cena*element.Kolicina;//??????
          }
          return CenaPojedinacnog;
        }



    }
}


//

/*Jedino sto smo dodali u ovom Template-u je ,
  "ConnectionStrings": {
    "IspitCS": "Server=(localdb)\\MSSQLLocalDB;Database=TestBazaPodataka"   
  },
  "AllowedHosts": "*" i index.html */