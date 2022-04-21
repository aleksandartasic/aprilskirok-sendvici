
import {Proizvod} from "./Proizvod.js"
export class Prodavnica {
   
    constructor(id, naziv,brojStolova,zarada) {
        this.id = id;
        this.naziv = naziv;
        this.kontejner = null;
        this.brojStolova = brojStolova;
        this.zarada=zarada;
    }

    crtajPK(host) {

        if (!host) {
            throw new Exception("Roditeljski element ne postoji");
        }
        this.kontejner = document.createElement("div");
        this.kontejner.classList.add("kontejner");
        host.appendChild(this.kontejner);
        this.crtajFormu(this.kontejner);
    }
    crtajFormu(host) {
        const kontForma = document.createElement("div");
        kontForma.className = "kontForma";
        host.appendChild(kontForma);

        const kontForma1 = document.createElement("div");
        kontForma1.className = "kontForma1";
        kontForma.appendChild(kontForma1);

        var divZaIzborKategorije = document.createElement("div");

        let labela = document.createElement("label");
        labela.innerHTML = "Redni broj stola odnosno proizvoda";
        divZaIzborKategorije.appendChild(labela);

        var sel = document.createElement("select");
        sel.name = "selectStolovi";
        divZaIzborKategorije.appendChild(sel);
        kontForma1.appendChild(divZaIzborKategorije);

        fetch("https://localhost:5001/Ispit/PreuzmiProizvod/" + this.id, {
            method: "GET",
        }).then(p => {
            p.json().then(data => {
                data.forEach((dataa) => {
                    var opcija = document.createElement("option");
                    opcija.innerHTML = dataa.naziv;
                    opcija.value = dataa.id;
                    sel.appendChild(opcija);
                })
            })
        })


        var divZaIzborKategorije2  = document.createElement("div");
        labela = document.createElement("label");
        labela.innerHTML = " Sastojak "
        divZaIzborKategorije2.appendChild(labela);
        kontForma1.appendChild(divZaIzborKategorije2);

        var selTip = document.createElement("select");
        selTip.name = "selectsastojak";
        divZaIzborKategorije2.appendChild(selTip);
        kontForma1.appendChild(divZaIzborKategorije2);

        fetch("https://localhost:5001/Ispit/PreuzmiSastojke/" + this.id, {
            method: "GET",
        }).then(p => {
            p.json().then(data => {
                data.forEach((dataa) => {
                    var opcija = document.createElement("option");
                    opcija.innerHTML = dataa.naziv;
                    opcija.value = dataa.id;
                    selTip.appendChild(opcija);
                })
            })
        })
        
        var elLabela = document.createElement("label");
        elLabela.innerHTML = "Kolicina u stotinama grama";
        kontForma1.appendChild(elLabela);

        var inputKolicina = document.createElement("input");
        inputKolicina.className = "kolicina";
        kontForma1.appendChild(inputKolicina);

        const dugme = document.createElement("button");
        dugme.innerHTML = "Dodaj ";
        kontForma1.appendChild(dugme);

        const kontPrikaz = document.createElement("div");
        kontPrikaz.className = "kontPrikaz";
        kontForma.appendChild(kontPrikaz);

        const kontPrikaz2 = document.createElement("div");
        kontPrikaz2.className = "kontPrikaz2";
        kontPrikaz.appendChild(kontPrikaz2);

        var cenaa=0;
        dugme.onclick = (ev) => {
            var kolicina = parseInt(this.kontejner.querySelector(".kolicina").value);
           
            var idProizvoda=this.kontejner.querySelector('select[name="selectStolovi"]').value;
            var idSastojka = this.kontejner.querySelector('select[name="selectsastojak"]').value;//BIRANJE SASTOJKA i proizvoda, nista specijalno do ovde
            cenaa=0;

            fetch("https://localhost:5001/Ispit/DodavanjeSastojakaUProizvod/" + kolicina + "/" + idProizvoda + "/" + idSastojka + "/" + this.id , {
            method: "POST",
            headers: 
            {
                    "Content-Type": "application/json"
            },
            /*body:{ 
            },*/
            }).then(p => {
                if(p.ok)
                {
                    fetch("https://localhost:5001/Ispit/UcitavanjeProizvoda/"+ this.id+"/"+idProizvoda,{
                    method:"GET",
                    }).then(p=>{
                        p.json().then(data=>
                            
                        data.forEach(proiz=>{
                            var proizvodElementKlase=new Proizvod();
                            proizvodElementKlase.id=proiz.id;
                            proizvodElementKlase.cena=proiz.cena;
                            proizvodElementKlase.naziv=proiz.naziv;

                            var kontPrikazZaSendvic = document.createElement("div");
                            var divPretraga=this.kontejner.getElementsByClassName('kontPrikazZaSendvic'+proizvodElementKlase.id);
                            
                            if (divPretraga.length!=0){/////AKO NE NADJE NISTA,TJ AKO ZA TAJ STO VEC NIJE PRIKAZAN SADRZAJ, MORA OVAKO DA SE TO PROVERI, NE SA NULL!!!!!
                            
                                                /*kontPrikazZaSendvic = divPretraga[0];
                                                (divPretraga[0].childNodes).forEach(element => {///ovo je neki drugi nacin, ovde ga necu koristiti ali neka ostane
                                                divPretraga[0].removeChild(element);  });*/
                                (divPretraga[0].parentNode).removeChild(divPretraga[0]);
                                }

                                kontPrikazZaSendvic.className = "kontPrikazZaSendvic"+proizvodElementKlase.id;
                                kontPrikazZaSendvic.innerHTML=proizvodElementKlase.naziv;
                                kontPrikaz2.appendChild(kontPrikazZaSendvic); 

                                var spoljniDeoSendvica=document.createElement("div");
                                spoljniDeoSendvica.className="spoljniDeoSendvica";
                                kontPrikazZaSendvic.appendChild(spoljniDeoSendvica);

                                var unutrasnjiDeoSendvica = document.createElement("div");
                                unutrasnjiDeoSendvica.className ="unutrasnjiDeoSendvica";
                                kontPrikazZaSendvic.appendChild(unutrasnjiDeoSendvica);
                                proizvodElementKlase.miniKontejner1=unutrasnjiDeoSendvica;//

                                var cenaProizvoda=document.createElement("label");

                                fetch("https://localhost:5001/Ispit/UcitavanjeSastojka/"+ proizvodElementKlase.id,{
                                method:"GET",
                                }).then(p=>{
                                    p.json().then(data1=>
                                        
                                        data1.forEach(data2=>{///on tu vrati ovako nesto
                /*"id":1,"kolicina":1,"proizvod":null,"sastojak":{"id":1,"naziv":"SVINJSKO meso","cena":100}},
                {"id":2,"kolicina":1,"proizvod":null,"sastojak":{"id":2,"naziv":"SVINJSKO meso","cena":100}},
                {"id":3,"kolicina":1,"proizvod":null,"sastojak":{"id":3,"naziv":"junece meso meso","cena":100}},
                {"id":4,"kolicina":1,"proizvod":null,"sastojak":{"id":4,"naziv":"junece meso meso","cena":100}},
                {"id":5,"kolicina":1,"proizvod":null,"sastojak":{"id":4,"naziv":"junece meso meso","cena":100}}]*/
                                        var label1=document.createElement("label");

                                        cenaa=cenaa+data2.sastojak.cena*data2.kolicina;
                                        label1.innerHTML=data2.sastojak.naziv+ " "+ data2.kolicina+"00 grama";
                                        unutrasnjiDeoSendvica.appendChild(label1);
                                                                                    ///ovde se desava nesto cudno , vrednost cenaa nije moguce uvecavati u fetchu
                                                                                    // a zatim ga odstampati na kraju u parent funkciji : CLOSURE
                                        cenaProizvoda.innerHTML=cenaa+" dinara";
                                        kontPrikazZaSendvic.appendChild(cenaProizvoda)
                                        
                                        })
                                    )}
                                )
                           
                                var spoljniDeoSendvica1=document.createElement("div");
                                spoljniDeoSendvica1.className="spoljniDeoSendvica1";
                                kontPrikazZaSendvic.appendChild(spoljniDeoSendvica1);

                                var dugmeProdato= document.createElement("button");
                                dugmeProdato.innerHTML = "Prodato";
                                dugmeProdato.className="dugmeProdato"+proizvodElementKlase.id;
                                dugmeProdato.value = proizvodElementKlase.id ;
                                kontPrikazZaSendvic.appendChild(dugmeProdato);
                            
                            /* var dugmee=document.createElement("button");
                            dugmee=document.getElementsByClassName('dugmeProdato'+1)[0];//kada se mora se naci kontejner gde je dugme, TO SE RADI OVAKO,
                            ovde medjutim dugmetu se moze pristupiti odmah u narednom redu i vrednost te promenjive ce se dobiti iz konteksta promenjivih,
                             skapirace da je to dugme iz tog diva gde sam kliknu*/
                                dugmeProdato.onclick=(ev)=>{
                               
                                    fetch("https://localhost:5001/Ispit/ProdajaProizvoda/" + dugmeProdato.value + "/" + this.id , {
                                    method: "DELETE",
                                    
                                    }).then(p => {
                                        if(p.ok)
                                        {
        
                                            alert("Proizvod prodat "+ dugmeProdato.value )
                                            kontPrikazZaSendvic.style.display="none";
                                        }
                                    })
                                }
                            })
                        )
                    })
                    alert("uspelooo");
                }
                
            })
        }
        
    }
}



