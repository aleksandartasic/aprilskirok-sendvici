
import {Proizvod} from "./Proizvod.js"
export class Prodavnica {
   
    constructor(id, naziv,brojStolova,zarada) {
        this.id = id;
        this.naziv = naziv;
        this.kontejner = null;
        this.brojStolova = brojStolova;
        this.zarada=zarada;
        this.nizIdIzabranihProizvoda="";//definicija stringa jer ne Saljes Niz kroz fetch
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
        labela.innerHTML = "Stolovi";
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
        selTip.name = "selecttip";
        divZaIzborKategorije2.appendChild(selTip);
        kontForma1.appendChild(divZaIzborKategorije2);

        fetch("https://localhost:5001/Ispit/PreuzmiSastojke/" + this.id, {///sastojci
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
        elLabela.innerHTML = "kolicina";
        kontForma1.appendChild(elLabela);

        var inputKolicina = document.createElement("input");
        inputKolicina.className = "kolicina";
        kontForma1.appendChild(inputKolicina);

        const dugme = document.createElement("button");
        dugme.innerHTML = "Dodaj";
        kontForma1.appendChild(dugme);

        const kontPrikaz = document.createElement("div");
        kontPrikaz.className = "kontPrikaz";
        kontForma.appendChild(kontPrikaz);

        const kontPrikaz2 = document.createElement("div");
        kontPrikaz2.className = "kontPrikaz2";
        kontPrikaz.appendChild(kontPrikaz2);

        dugme.onclick = (ev) => {
            var kolicina = parseInt(this.kontejner.querySelector(".kolicina").value);
           
            var idProizvoda=this.kontejner.querySelector('select[name="selectStolovi"]').value;
            var idSastojka = this.kontejner.querySelector('select[name="selecttip"]').value;//BIRANJE SASTOJKA
            var cenaa=0;

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
                    fetch("https://localhost:5001/Ispit/UcitavanjeProizvoda/"+ this.id,{
                    method:"GET",
                    }).then(p=>{
                        p.json().then(data=>
                            
                            data.forEach(d=>{
                                
                            const kontPrikazZaSendvice = document.createElement("div");
                            kontPrikazZaSendvice.className = "kontPrikazZaSendvice";
                            kontPrikaz2.appendChild(kontPrikazZaSendvice);

                            var spoljniDeoSendvica=document.createElement("div");
                            spoljniDeoSendvica.className="spoljniDeoSendvica";
                            kontPrikazZaSendvice.appendChild(spoljniDeoSendvica);

                            var unutrasnjiDeoSendvica = document.createElement("div");
                            unutrasnjiDeoSendvica.className ="unutrasnjiDeoSendvica";
                            kontPrikazZaSendvice.appendChild(unutrasnjiDeoSendvica);

                           
                                                                  
                            fetch("https://localhost:5001/Ispit/UcitavanjeSastojka/"+ d.id,{
                                method:"GET",
                                }).then(p=>{
                                    p.json().then(data1=>
                                        
                                        
                                        data1.forEach(data2=>{
                                        var label1=document.createElement("label");
                                        label1.innerHTML=data2.sastojak.naziv;
                                        unutrasnjiDeoSendvica.appendChild(label1);

                                        cenaa+=data2.sastojak.cena;
                                                                                                             
                                        })
                                )}
                            )
                           
                            var spoljniDeoSendvica1=document.createElement("div");
                            spoljniDeoSendvica1.className="spoljniDeoSendvica1";
                            kontPrikazZaSendvice.appendChild(spoljniDeoSendvica1);

                            var cenaProizvoda=document.createElement("label");
                            cenaProizvoda.innerHTML=cenaa;
                            kontPrikazZaSendvice.appendChild(cenaProizvoda);
                            
                            })
                        )}
                    )
                    alert("uspelooo")
                }
            })
        }
    }
}