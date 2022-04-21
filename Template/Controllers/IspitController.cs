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
          var vraceneProdavnice=await Context.Prodavnica.Where(p=>p.ID==idProdavnice).FirstAsync();
          var vraceniTipovi=await Context.Sastojak.Where(p=>p.Prodavnica.Contains(vraceneProdavnice)).ToListAsync();///posto je n m veza
          ///radi preko contains posto je to jedini nacin da pretrazujes da li je neki manji string(json) u vecem jsonu ili stringu!
         
          return vraceniTipovi;
        }
   
        [Route("PreuzmiProizvod/{idProdavnice}")]
        [HttpGet]
        public async Task<List<Proizvod>> PreuzmiProizvod(int idProdavnice)
        {
          
          var vraceniProizvodi=await Context.Proizvod.Where(p=>p.Prodavnica.ID==idProdavnice).ToListAsync();
          return vraceniProizvodi;
        }
    
        [Route("DodavanjeSastojakaUProizvod/{kolicina}/{idProizvoda}/{idSastojka}/{idProd}")]
        [HttpPost]
        public async Task<ActionResult> DodavanjeSastojakaUProizvod(int kolicina,int idProizvoda, int idSastojka,int idProd)
        {
         
        var trazeniSastojak=await Context.Sastojak.Where(p=>p.ID==idSastojka).FirstAsync();
        var trazeniProizvod=await Context.Proizvod.Where(p=>p.ID==idProizvoda).FirstAsync();

        //Ono sto si ovde radio je da si u tu medjuklasu dodavao svaku instancu kada se doda novi sastojak za neki proizvod
        //to znaci da se dodaje u bazi u tabeli ProizvodSastojak npr [{"id":1,"kolicina":1,"proizvod":null,"sastojak":{"id":1,"naziv":"SVINJSKO meso","cena":100},"proizvod":...},

        ProizvodSastojak noviSastProizvod=new ProizvodSastojak();

        noviSastProizvod.Proizvod=trazeniProizvod;
        noviSastProizvod.Sastojak=trazeniSastojak;
        noviSastProizvod.Kolicina=kolicina;
             
        Context.proizvodSastojak.Add(noviSastProizvod);

        await Context.SaveChangesAsync();
        return Ok("uspelo");
        }    
        
        [Route("UcitavanjeProizvoda/{idProdavnice}/{idProizvoda}")]
        [HttpGet]
        public async Task<List<Proizvod>> UcitavanjeProizvoda(int idProdavnice,int idProizvoda)
        {

          var NadjeniProizvod=await Context.Proizvod.Where(p=>p.Prodavnica.ID==idProdavnice && p.ID==idProizvoda).ToListAsync();
          return NadjeniProizvod;
        }

          
        [Route("UcitavanjeSastojka/{idProizvoda}")]
        [HttpGet]
        public async Task<List<ProizvodSastojak>> UcitavanjeSastojka(int idProizvoda)
        {
        ///Include ti je da ti odredi sta ti se vraca u response-u OBRATI PAZNJU AKO STAVIS JSON IGNORE to se NE vraca
        var NadjeniProizvodiSastojci=await Context.proizvodSastojak.Where(p=>p.Proizvod.ID==idProizvoda)
        .Include(p=>p.Sastojak)/*.Include(p=>p.Proizvod)*/.ToListAsync();
        if (NadjeniProizvodiSastojci!= null)
        {  
        /*List<Sastojak> nadjeniSastojci=null;
        foreach(var element in NadjeniProizvodiSastojci)
        {
        nadjeniSastojci=await Context.Sastojak.Include(p=>p.ProizvodSastojak).Where(p=>p.ID==element.Sastojak.ID).ToListAsync();
        }
        [{"id":1,"kolicina":1,"proizvod":null,"sastojak":{"id":1,"naziv":"SVINJSKO meso","cena":100}},
        {"id":2,"kolicina":1,"proizvod":null,"sastojak":{"id":2,"naziv":"SVINJSKO meso","cena":100}},
        {"id":3,"kolicina":1,"proizvod":null,"sastojak":{"id":3,"naziv":"junece meso meso","cena":100}},
        {"id":4,"kolicina":1,"proizvod":null,"sastojak":{"id":4,"naziv":"junece meso meso","cena":100}},
        {"id":5,"kolicina":1,"proizvod":null,"sastojak":{"id":4,"naziv":"junece meso meso","cena":100}}]*/
        return NadjeniProizvodiSastojci;
        }
        else return null;
        }

        [Route("CenaPojedinacnogProizvoda/{idProizvoda}")]
        [HttpGet]
        public async Task<int> CenaPojedinacnogProizvoda(int idProizvoda)
        {
          int CenaPojedinacnog=0;
          var NadjeniProizvodiSastojci=await Context.proizvodSastojak.Where(p=>p.Proizvod.ID==idProizvoda).Include(p=>p.Sastojak).ToListAsync();
          foreach(var element in NadjeniProizvodiSastojci)
          {
            CenaPojedinacnog+=element.Sastojak.Cena*element.Kolicina;//??????
          }
          return CenaPojedinacnog;
        }

        
        [Route("ProdajaProizvoda/{idProizvoda}/{idProd}")]
        [HttpDelete]
        public async Task<ActionResult> ProdajaProizvoda(int idProizvoda,int idProd)
        {
        var NadjeniProizvodiSastojci=await Context.proizvodSastojak.Where(p=>p.Proizvod.ID==idProizvoda).Include(p=>p.Sastojak).ToListAsync();
        var NadjeniProizvod=await Context.Proizvod.Where(p=>p.Prodavnica.ID==idProd).ToListAsync();
            for (int i = 0; i < NadjeniProizvodiSastojci.Count; i++)
          {
                ProizvodSastojak element = NadjeniProizvodiSastojci[i];
                //element.Kolicina=0;
                Context.proizvodSastojak.Remove(element);
          }
         
        await Context.SaveChangesAsync();
        return Ok("uspelo");
        }    
    }
}

/*
[Route("DodavanjeSastojakaUProizvod/{kolicina}/{idProizvoda}/{idSastojka}/{idProd}")]
[HttpPost]
public async Task<ActionResult> DodavanjeSastojakaUProizvod(int kolicina,int idProizvoda, int idSastojka,int idProd)
{

 var NadjeniProizvodSastojak=await Context.proizvodSastojak.Where(p=>p.Proizvod.ID==idProizvoda && p.Sastojak.ID==idSastojka)
.Include(p=>p.Sastojak).Include(p=>p.Proizvod).FirstAsync();
if (NadjeniProizvodSastojak!=null){
  NadjeniProizvodSastojak.Kolicina+=kolicina;
  Context.proizvodSastojak.Update(NadjeniProizvodSastojak);  
  
  await Context.SaveChangesAsync();
  return Ok("uspelo AZURIRANJE");
}
else{

 
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
  return Ok("uspelo DODAVANJE");
}


} 
*/
//
/*Jedino sto smo dodali u ovom Template-u je ,
  "ConnectionStrings": {
    "IspitCS": "Server=(localdb)\\MSSQLLocalDB;Database=TestBazaPodataka"   
  },
  "AllowedHosts": "*" i index.html */