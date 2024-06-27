var lat
var lng
// Crea la mappa e impostala su una vista iniziale con una determinata latitudine e longitudine e livello di zoom
var map = L.map('map').setView([41.8903, 12.4922], 13);
var consumer_id;
var prosumer_id;
var impianto_id;
// Aggiungi il layer delle mappe di OpenStreetMap
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

// Aggiungi un marker iniziale (opzionale)
var marker = L.marker([41.8903, 12.4922]).addTo(map);
var geocoder = L.Control.geocoder({
    defaultMarkGeocode: false
})
    .on('markgeocode', function (e) {
        var latlng = e.geocode.center;
        marker.setLatLng(latlng).addTo(map);
        map.setView(latlng, map.getZoom());
        document.getElementById('coordinates').innerText = "Latitudine: " + latlng.lat + ", Longitudine: " + latlng.lng;
    })
    .addTo(map);

// Aggiungi un evento click alla mappa per ottenere le coordinate
map.on('click', function (e) {
    lat = e.latlng.lat;
    lng = e.latlng.lng;

    // Sposta il marker sulla nuova posizione cliccata
    marker.setLatLng([lat, lng]);

    // Mostra le coordinate nella console
    console.log("Latitudine: " + lat + ", Longitudine: " + lng);

    // Aggiorna l'elemento HTML per mostrare le coordinate all'utente
    document.getElementById('coordinates').innerText = "Latitudine: " + lat + ", Longitudine: " + lng;
});

function ShowCol() {
    var sel = $('#modalita_inserimento').val();
    if (sel == 1) {
        $('#div_annuale').show();
        $('#div_mensile').hide();
        $('#div_orario').hide();
    } else if (sel == 2) {
        $('#div_annuale').hide();
        $('#div_mensile').show();
        $('#div_orario').hide();
    } else if (sel == 3) {
        $('#div_annuale').hide();
        $('#div_mensile').hide();
        $('#div_orario').show();

    } else {
        $('#div_annuale').hide();
        $('#div_mensile').hide();
        $('#div_orario').hide();
    }
}

function ShowColProsumer() {
    var sel = $('#modalita_inserimento_Prosumer').val();
    if (sel == 1) {
        $('#div_annuale_Prosumer').show();
        $('#div_mensile_Prosumer').hide();
        $('#div_orario_Prosumer').hide();
    } else if (sel == 2) {
        $('#div_annuale_Prosumer').hide();
        $('#div_mensile_Prosumer').show();
        $('#div_orario_Prosumer').hide();
    } else if (sel == 3) {
        $('#div_annuale_Prosumer').hide();
        $('#div_mensile_Prosumer').hide();
        $('#div_orario_Prosumer').show();

    } else {
        $('#div_annuale_Prosumer').hide();
        $('#div_mensile_Prosumer').hide();
        $('#div_orario_Prosumer').hide();
    }
}

function ShowColImpianto() {
    var sel = $('#modalita_inserimento_Impianto').val();
    if (sel == 1) {
        $('#div_annuale_Impianto').show();
        $('#div_mensile_Impianto').hide();
        $('#div_orario_Impianto').hide();
    } else if (sel == 2) {
        $('#div_annuale_Impianto').hide();
        $('#div_mensile_Impianto').show();
        $('#div_orario_Impianto').hide();
    } else if (sel == 3) {
        $('#div_annuale_Impianto').hide();
        $('#div_mensile_Impianto').hide();
        $('#div_orario_Impianto').show();

    } else {
        $('#div_annuale_Impianto').hide();
        $('#div_mensile_Impianto').hide();
        $('#div_orario_Impianto').hide();
    }
}


function sendMonthsValue() {


    var formData = {
        forma: $('#Forma_Utenza').val(),
        tipo_utenza: $('#Tipo_Utenza').val(),
        n_utenze: $('#n_utenze').val(),
        modalita_inserimento: $('#modalita_inserimento').val(),
        consumo_annuale: $("#Consumo_Annuale").val(),
        gennaio: $('#Consumo_Genn').val(),
        Febbraio: $('#Consumo_Feb').val(),
        Marzo: $('#Consumo_Marzo').val(),
        Aprile: $('#Consumo_Apr').val(),
        Maggio: $('#Consumo_Maggio').val(),
        Giugno: $('#Consumo_Giu').val(),
        Luglio: $('#Consumo_Lug').val(),
        Agosto: $('#Consumo_Ago').val(),
        Settembre: $('#Consumo_Set').val(),
        Ottobre: $('#Consumo_Ott').val(),
        Novembre: $('#Consumo_Nov').val(),
        Dicembre: $('#Consumo_Dic').val(),
        Descrizione: $('#nome_consumer').val(),
        consumer_id: consumer_id
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveData',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
            Table_Consumer.clear().draw(true);
            _GetTableConsumer();
     

           
        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}


function sendSelectedUtenze() {
    var Forma_Utenza = document.getElementById("Forma_Utenza").value;
    var N_Cluster = document.getElementById("n_utenze").value;
    var Tipo_Utenza = document.getElementById("Tipo_Utenza").value;
    var modalita_inserimento = document.getElementById("modalita_inserimento").value;

    var formData = {
        Forma_Utenza: Forma_Utenza,
        N_Cluster: N_Cluster,
        Tipo_Utenza: Tipo_Utenza,
        Modalita_Inserimento: modalita_inserimento
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveDatiUtenze',
        data: formData,
        success: function (data) {
            console.log(data);
            if (data == '1') {
                window.location.href = '/DatiTecnici';
            } else {
                Swal.fire("Configurazione Completata", "", "success");
            }

        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}

var Table_Consumer;
var Table_Prosumer;
var Table_Impianto;


jQuery('.mydatepicker, #datepicker, .input-group.date').datepicker(
    {
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    }
);
jQuery('#datepicker-autoclose').datepicker({
    autoclose: true,
    todayHighlight: true
});
jQuery('#date-range').datepicker({
    toggleActive: true
});
jQuery('#datepicker-inline').datepicker({
    todayHighlight: true
});

$(document).ready(function () {
    Table_Consumer = $('#grd_Consumer').DataTable({
        "language":
        {
            "url": url,
            "decimal": ',',
            "thousands": '.',

        },
        buttons: [
            {
                extend: 'excelHtml5',
                className: 'btn btn-info',
                title: 'Consumer',
                exportOptions: {
                    format: {
                        body: function (data, row, column, node) {
                            data = $('<p>' + data + '</p>').text().replace(' warning', '');
                            return $.isNumeric(data.replace(',', '.').replace('€', '')) ? data.replace(',', '.').replace('€', '') : data;
                        }
                    }


                }


            }
        ],
        "paging": true,
        dom: 'Biflrtp',
        "lengthMenu": [[-1, 10, 25, 50], ['Tutti', 10, 25, 50]],
        "columnDefs":
            [
                /* {
                     "targets": 'no-sort',
                     "orderable": false,
                 },
                 {
                     "targets": 2,
                     "type": "date-eu"
                 },
                 {
                     "targets": 3,
                     "orderable": false
                 },
                 {
                     "targets": 5,
                     "visible": false
                 }
                 */

            ],
        "columns": [
            { data: 'DESCRIZIONE' },
            { data: 'FORMA' },
            { data: 'TIPOLOGIA_UTENZA' },
            { data: 'NUMERO_UTENZE' },
            { data: 'MODALITA_INSERIMENTO' },
            { data: 'BTN' }

        ]
    });
    Table_Prosumer = $('#grd_Prosumer').DataTable({
        "language":
        {
            "url": url,
            "decimal": ',',
            "thousands": '.',

        },
        buttons: [
            {
                extend: 'excelHtml5',
                className: 'btn btn-info',
                title: 'Consumer',
                exportOptions: {
                    format: {
                        body: function (data, row, column, node) {
                            data = $('<p>' + data + '</p>').text().replace(' warning', '');
                            return $.isNumeric(data.replace(',', '.').replace('€', '')) ? data.replace(',', '.').replace('€', '') : data;
                        }
                    }


                }


            }
        ],
        "paging": true,
        dom: 'Biflrtp',
        "lengthMenu": [[-1, 10, 25, 50], ['Tutti', 10, 25, 50]],
        "columnDefs":
            [

            ],
        "columns": [
            { data: 'DESCRIZIONE' },
            { data: 'FORMA' },
            { data: 'TIPOLOGIA_UTENZA' },
            { data: 'NUMERO_UTENZE' },
            { data: 'MODALITA_INSERIMENTO' },
            { data: 'BTN' }

        ]
    })
    Table_Impianto = $('#grd_Impianto').DataTable({
        "language":
        {
            "url": url,
            "decimal": ',',
            "thousands": '.',

        },
        buttons: [
            {
                extend: 'excelHtml5',
                className: 'btn btn-info',
                title: 'Consumer',
                exportOptions: {
                    format: {
                        body: function (data, row, column, node) {
                            data = $('<p>' + data + '</p>').text().replace(' warning', '');
                            return $.isNumeric(data.replace(',', '.').replace('€', '')) ? data.replace(',', '.').replace('€', '') : data;
                        }
                    }


                }


            }
        ],

        "paging": true,
        dom: 'Biflrtp',
        "lengthMenu": [[-1, 10, 25, 50], ['Tutti', 10, 25, 50]],
        "columnDefs":
            [

            ],
        "columns": [
            { data: 'DESCRIZIONE' },
            { data: 'FORMA' },
            { data: 'TIPOLOGIA_UTENZA' },
            { data: 'NUMERO_UTENZE' },
            { data: 'MODALITA_INSERIMENTO' },
            { data: 'BTN' }

        ]
    })



});

function ShowPopupConsumer() {
    $('#centermodal_consumer').modal();
    $('#div_annuale').hide();
    $('#div_mensile').hide();
    $('#div_orario').hide();
    GetSelectTipo_Utenza("-1");
    consumer_id = -1;

}


function ShowPopupProsumer() {
    $('#centermodal_prosumer').modal();
    $('#div_annuale_Prosumer').hide();
    $('#div_mensile_Prosumer').hide();
    $('#div_orario_Prosumer').hide();
    prosumer_id = -1;


}

function ShowPopupImpianto() {
    $('#centermodal_impianto').modal();
    $('#div_annuale_Impianto').hide();
    $('#div_mensile_Impianto').hide();
    $('#div_orario_Impianto').hide();
    impianto_id = -1;
}

function ShowHideFinanziamentoProsumer() {
    if ($('#CheckFinanziamentoProsumer').is(':checked') == false) {

        $('#divFinanziamentoProsumer').hide();
    } else {

        $('#divFinanziamentoProsumer').show();
    }
}
function ShowHideFinanziamentoImpianto() {
    if ($('#CheckFinanziamentoImpianto').is(':checked') == false) {

        $('#divFinanziamentoImpianto').hide();
    } else {

        $('#divFinanziamentoImpianto').show();
    }
}
var type_
function ShowMap(t) {
    type_ = t
    $('#centermodalmap_prosumer').on('transitionend', function () { map.invalidateSize(); });


    $('#centermodalmap_prosumer').modal();

}

function InsertValues() {
    if (type_ == 0) {
        $('#Latitudine_Prosumer').val(lat);
        $('#Longitudine_Prosumer').val(lng);
    } else {
        $('#Latitudine_Impianto').val(lat);
        $('#Longitudine_Impianto').val(lng);
    }
    $('#centermodalmap_prosumer').modal("hide");
}


function ContoChangeProsumer() {
    if ($('#CheckContoProsumer').is(':checked') == false) {

        $('#divContoProsumer').hide();
    } else {

        $('#divContoProsumer').show();
    }
}

function ContoChangeImpianto() {
    if ($('#CheckContoImpianto').is(':checked') == false) {

        $('#divContoImpianto').hide();
    } else {

        $('#divContoImpianto').show();
    }
}

function SwitchCostoVenditaProsumer() {
    if ($('#check_vendita_prosumer').is(':checked') == false) {
        $('#Modalita_libera_prosumer').hide();
    } else {
        $('#Modalita_libera_prosumer').show();
    }


}
function SwitchCostoVenditaImpianto() {
    if ($('#check_vendita_impianto').is(':checked') == false) {
        $('#Modalita_libera_impianto').hide();
    } else {
        $('#Modalita_libera_impianto').show();
    }
}

function SwitchCostoAcquistoProsumer() {

    if ($('#check_acquisto_prosumer').is(':checked') == false) {
        $('#Modalita_acquisto_prosumer').hide();
    } else {
        $('#Modalita_acquisto_prosumer').show();
    }
}

function SwitchCostoAcquistoImpianto() {
    if ($('#check_acquisto_impianto').is(':checked') == false) {
        $('#Modalita_acquisto_impianto').hide();
    } else {
        $('#Modalita_acquisto_impianto').show();
    }
}


$(".tab-wizard").steps({
    headerTag: "h6",
    bodyTag: "section",
    transitionEffect: "fade",
    titleTemplate: '<span class="step">#index#</span> #title#',
    labels: {
        finish: "Salva"
    },
    onFinished: function (event, currentIndex) {
        saveGeneralProsumer();
        saveDatiImpiantoProsumer();
        saveDatiEconomiciProsumer();
        location.reload();

    }
});
$(".tab-wizard2").steps({
    headerTag: "h6",
    bodyTag: "section",
    transitionEffect: "fade",
    titleTemplate: '<span class="step">#index#</span> #title#',
    labels: {
        finish: "Salva"
    },
    onFinished: function (event, currentIndex) {
        saveGeneralImpianto();
        saveDatiImpiantoImpianto();
        saveDatiEconomiciImpianto();
        
    }
});


var rinnovabile_Prosumer = 0;
var sezione2_Prosumer = 0;
var costoTot_Prosumer = 0;

var rinnovabile_Impianto = 0;
var sezione2_Impiantor = 0;
var costoTot_Impianto = 0;

$("#mod_Prosumer").bootstrapSwitch();

$(document).ready(function () {
    $('#div_rinnovabile_Prosumer').hide();
    $('#sezione2_Prosumer').hide();
    $('#divCostoTot_Prosumer').hide();
});

function ShowRinnovabile_Prosumer() {

    if ($('#customCheck1_Prosumer').is(':checked') == false) {
        rinnovabile_Prosumer = 0;
        $('#div_rinnovabile_Prosumer').hide();
    } else {
        rinnovabile_Prosumer = 1;
        $('#div_rinnovabile_Prosumer').show();
    }


}
function ShowSezione2_Prosumer() {

    if ($('#customCheck2_Prosumer').is(':checked') == false) {
        sezione2_Prosumer = 0;
        $('#sezione2_Prosumer').hide();
    } else {
        sezione2_Prosumer = 1;
        $('#sezione2_Prosumer').show();
    }


}

function SwitchCosto_Prosumer() {

    if ($('#mod_Prosumer').is(':checked') == true) {
        costoTot_Prosumer = 0;
        $('#divCostoTot_Prosumer').hide();
        $('#divCostokW_Prosumer').show();
    } else {
        costoTot_Prosumer = 1;
        $('#divCostoTot_Prosumer').show();
        $('#divCostokW_Prosumer').hide();
    }


}


$("#mod_Impianto").bootstrapSwitch();

$(document).ready(function () {
    $('#div_rinnovabile_Impianto').hide();
    $('#sezione2_Impianto').hide();
    $('#divCostoTot_Impianto').hide();
});

function ShowRinnovabile_Impianto() {

    if ($('#customCheck1_Impianto').is(':checked') == false) {
        rinnovabile_Impianto = 0;
        $('#div_rinnovabile_Impianto').hide();
    } else {
        rinnovabile_Impianto = 1;
        $('#div_rinnovabile_Impianto').show();
    }


}
function ShowSezione2_Impianto() {

    if ($('#customCheck2_Impianto').is(':checked') == false) {
        sezione2_Impianto = 0;
        $('#sezione2_Impianto').hide();
    } else {
        sezione2_Impianto = 1;
        $('#sezione2_Impianto').show();
    }


}

function SwitchCosto_Impianto() {

    if ($('#mod_Impianto').is(':checked') == true) {
        costoTot_Impianto = 0;
        $('#divCostoTot_Impianto').hide();
        $('#divCostokW_Impianto').show();
    } else {
        costoTot_Impianto = 1;
        $('#divCostoTot_Impianto').show();
        $('#divCostokW_Impianto').hide();
    }


}

function ShowDettaglio(values) {
    if (values == 0) {
        $('#approfondimento_prosumer').modal('show');
    } else {
        $('#approfondimento_impianto').modal('show');
    }
}
var t;

function GetSelectTipo_Utenza(id) {
    var formData = {
        id: id
    };
    $.ajax({
        url: 'datiutenze/GetTipoUtenza',
        type: 'GET',
        data: formData,
        success: function (response) {
            $("#Tipo_Utenza").empty()
            var newOption = new Option("Seleziona", -1, false, false);
            $('#Tipo_Utenza').append(newOption).trigger('change');
            // Itera sui dati e chiama la funzione JavaScript per ogni elemento
           
            response.forEach(function (item) {
                t = item;
               
                newOption = new Option(t.descrizione.toString(), item.tipo_Utenza_Id, false, false);
                $('#Tipo_Utenza').append(newOption).trigger('change');
             

            });
        
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function _SetIdConsumer(id) {
    var formData = {
        id: id
    };
    consumer_id = id;
    $.ajax({
        url: 'datiutenze/SetIdConsumer',
        type: 'GET',
        data: formData,
        success: function (item) {

        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function _DeleteConsumer() {
    console.log(consumer_id);
    var formData = {
        id: consumer_id.toString()
    };
    $.ajax({
        url: 'datiutenze/DeleteConsumer',
        type: 'GET',
        data: formData,
        success: function (item) {
            _GetTableConsumer();
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function ModiConsumer(id) {
    var formData = {
        id: id       
    };
    consumer_id = id;
    $.ajax({
        url: 'datiutenze/ModificaConsumer',
        type: 'GET',
        data: formData,
        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

                console.log(item);
                $('#Forma_Utenza').val(item.forma_Utenza);
                $('#Tipo_Utenza').val(item.tipo_Utenza_Id)
                $('#n_utenze').val(item.n_Utenze_Cluster);
                console.log("pippo");
                console.log(item.modalita_inserimento);
                $('#modalita_inserimento').val(item.modalita_Inserimento);
                ShowCol();
                $("#Consumo_Annuale").val(item.consumo_Annuale);
                $("#Consumo_Annuale").val(item.consumo_Annuale);
                $('#Consumo_Genn').val(item.consumo_Gennaio);
                $('#Consumo_Feb').val(item.consumo_Febbraio);
                $('#Consumo_Marzo').val(item.consumo_Marzo);
                $('#Consumo_Apr').val(item.consumo_Aprile);
                $('#Consumo_Maggio').val(item.consumo_Maggio);
                $('#Consumo_Giu').val(item.consumo_Giugno);
                $('#Consumo_Lug').val(item.consumo_Luglio);
                $('#Consumo_Ago').val(item.consumo_Agosto);
                $('#Consumo_Set').val(item.consumo_Settembre);
                $('#Consumo_Ott').val(item.consumo_Ottobre);
                $('#Consumo_Nov').val(item.consumo_Novembre);
                $('#Consumo_Dic').val(item.consumo_Dicembre);
                $('#nome_consumer').val(item.descrizione);


            $("#centermodal_consumer").modal("show");
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function AddConsumer(Nome, Forma_Utenza, Tipo_Utenza, N_Utenze_Cluster,Modalita, Btn) {

    console.log(Nome);

    var apo = String.fromCharCode(39);
    myData = [
        {
            "DESCRIZIONE": Nome,
            "FORMA": Forma_Utenza,
            "TIPOLOGIA_UTENZA": Tipo_Utenza,
            "NUMERO_UTENZE": N_Utenze_Cluster,
            "MODALITA_INSERIMENTO": Modalita,
            "BTN": Btn
        }

    ];




    Table_Consumer.rows.add(myData);

}


function ModiProsumer(id) {
    var formData = {
        id: id
    };
    prosumer_id = id;
    $.ajax({
        url: 'datiutenze/ModificaProsumer',
        type: 'GET',
        data: formData,
        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

            console.log(item);
            $('#Forma_Utenza_Prosumer').val(item.forma_Utenza);
            $('#Tipo_Utenza_Prosumer').val(item.tipo_Utenza_Id)
            $('#n_utenze_Prosumer').val(item.n_Utenze_Cluster);
            console.log("pippo");
            console.log(item.modalita_inserimento);
            $('#modalita_inserimento_Prosumer').val(item.modalita_Inserimento);
            ShowCol();
            $("#Consumo_Annuale_Prosumer").val(item.consumo_Annuale);
            $('#Consumo_Genn').val(item.consumo_Gennaio);
            $('#Consumo_Feb').val(item.consumo_Febbraio);
            $('#Consumo_Marzo').val(item.consumo_Marzo);
            $('#Consumo_Apr').val(item.consumo_Aprile);
            $('#Consumo_Maggio').val(item.consumo_Maggio);
            $('#Consumo_Giu').val(item.consumo_Giugno);
            $('#Consumo_Lug').val(item.consumo_Luglio);
            $('#Consumo_Ago').val(item.consumo_Agosto);
            $('#Consumo_Set').val(item.consumo_Settembre);
            $('#Consumo_Ott').val(item.consumo_Ottobre);
            $('#Consumo_Nov').val(item.consumo_Novembre);
            $('#Consumo_Dic').val(item.consumo_Dicembre);
            $('#nome_prosumer').val(item.descrizione);

            ModiImpiantoProsumer(prosumer_id);
            ModiEconomiciProsumer(prosumer_id);
            $("#centermodal_prosumer").modal("show");
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function ModiImpiantoProsumer(id) {
    var formData = {
        id: id
    };
    consumer_id = id;
    $.ajax({
        url: 'datiutenze/ModificaProsumerImpianto',
        type: 'GET',
        data: formData,
        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

            console.log(item);
            $('#Longitudine_Prosumer').val(item.longitudine);
            $('#Latitudine_Prosumer').val(item.latitudine);
            $('#Potenza_impianto_Prosumer').val(item.potenza_Impianto);
            $('#Quota_Potenza_Rinnovabile_Prosumer').val(item.quota_Potenza_Rinnovabile);
            $('#Potenza_inverter_Prosumer').val(item.potenza_Inverter);
            $('#Costo_Impianto_Prosumer').val(item.costo_Impianto);
            $('#Capacita_Batteria_Prosumer').val(item.capacita_Batteria);
            $('#Is_Costo_KW_Prosumer').prop('checked', item.is_Costo_KW);
            $('#Costo_Totale_Prosumer').val(item.costo_Totale);
            $('#Costo_KW_Prosumer').val(item.costo_KW);
            $('#Is_Escluso_Premio_Prosumer').prop('checked', item.is_Escluso_Premio);            
            $('#Area_Impianto_Prosumer').val(item.area_Impianto);
            $('#Tipologia_Impianto_Prosumer').val(item.tipologia_Impianto);
            $('#Data_Esercizio_Prosumer').val(item.data_Esercizio);
            $('#Is_Seconda_Falda_Prosumer').prop('checked', item.is_Seconda_Falda);
            $('#Potenza_Sezione_Prosumer').val(item.potenza_Sezione);
            $('#Angolo_di_tilt_Prosumer').val(item.angolo_di_tilt);
            $('#Angolo_di_Azimut_Prosumer').val(item.angolo_di_Azimut);
            $('#Potenza_Sezione_S_Prosumer').val(item.potenza_Sezione_S);
            $('#Angolo_di_tilt_S_Prosumer').val(item.angolo_di_tilt_S);
            $('#Angolo_di_Azimut_S_Prosumer').val(item.angolo_di_Azimut_S);
            $('#Efficienza_Prosumer').val(item.efficienza);
            $('#Coefficiente_T_Prosumer').val(item.coefficiente_T);
            $('#NOCT_Prosumer').val(item.noct);
            $('#Fattore_Riduzione_Prosumer').val(item.fattore_Riduzione);
            $('#Efficienza_Inverter_Prosumer').val(item.efficienza_Inverter);
            $('#Costo_Ricambio_Batt_Prosumer').val(item.costo_Ricambio_Batt);
            $('#Altre_Perdite_Prosumer').val(item.altre_Perdite);

            $('#checkTariffa_Prosumer').prop('checked', item.is_Escluso_Premio);
            $('#mod_Prosumer').prop('checked', item.is_Costo_KW);
            $('#customCheck1_Prosumer').prop('checked', item.is_Abilitato_Rinnovabile);
            $('#customCheck2_Prosumer').prop('checked', item.is_Seconda_Falda);
            
            ShowRinnovabile_Prosumer();
            SwitchCosto_Prosumer();
            ShowSezione2_Prosumer();
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function ModiEconomiciProsumer(id) {
    var formData = {
        id: id
    };
    consumer_id = id;
    $.ajax({
        url: 'datiutenze/ModificaProsumerEconomici',
        type: 'GET',
        data: formData,
        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

            console.log(item);
            $('#check_vendita_prosumer').prop('checked', item.modalita_Vendita === 1);
            $('#check_acquisto_prosumer').prop('checked', item.modalita_Acquisto === 1);
            $('#DetrazioneProsumer').prop('checked', item.is_Detrazione);
            $('#CheckFinanziamentoProsumer').prop('checked', item.is_Finanziamento);
            $('#CheckContoProsumer').prop('checked', item.is_Conto);
            $('#Tipo_Utenza_Economica_Prosumer').val(item.tipo_Utenza_Id);
            $('#PUN_F1_PRE_PRO').val(item.prezzo_F1);
            $('#PUN_F2_PRE_PRO').val(item.prezzo_F2);
            $('#PUN_F3_PRE_PRO').val(item.prezzo_F3);
            $('#PUN_F1_COSTO_PRO').val(item.costo_F1);
            $('#PUN_F2_COSTO_PRO').val(item.costo_F2);
            $('#PUN_F3_COSTO_PRO').val(item.costo_F3);
            $('#AltreSpeseInvestmentProsumer').val(item.altre_Spese_Investimento);
            $('#AltreSpeseProsumer').val(item.altre_Spese);
            $('#AltreSpeseAnnoProsumer').val(item.spese_Manutenzione);
            $('#FinanziamentoProsumer').val(item.importo_Finanziamento);
            $('#IntersseFinanziamentoProsumer').val(item.interesse_Finanziamento);
            $('#PercentFinanziamentoProsumer').val(item.conto_Capitale);
            SwitchCostoAcquistoProsumer();
            SwitchCostoVenditaProsumer();
            ShowHideFinanziamentoProsumer();
            ContoChangeProsumer();
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}


function AddProsumer(Nome, Forma_Utenza, Tipo_Utenza, N_Utenze_Cluster, Modalita, Btn) {

    console.log(Nome);

    var apo = String.fromCharCode(39);
    myData = [
        {
            "DESCRIZIONE": Nome,
            "FORMA": Forma_Utenza,
            "TIPOLOGIA_UTENZA": Tipo_Utenza,
            "NUMERO_UTENZE": N_Utenze_Cluster,
            "MODALITA_INSERIMENTO": Modalita,
            "BTN": Btn
        }

    ];




    Table_Prosumer.rows.add(myData);

}

function ModiImpianto(id) {
    var formData = {
        id: id
    };
    consumer_id = id;
    $.ajax({
        url: 'datiutenze/ModificaImpianto',
        type: 'GET',
        data: formData,
        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

            console.log(item);
            $('#Forma_Utenza_Impianto').val(item.forma_Utenza);
            $('#Tipo_Utenza_Impianto').val(item.tipo_Utenza_Id)
            $('#n_utenze_Impianto').val(item.n_Utenze_Cluster);
            console.log("pippo");
            console.log(item.modalita_inserimento);
            $('#modalita_inserimento_Impianto').val(item.modalita_Inserimento);
            ShowCol();
            $("#Consumo_Annuale_Impianto").val(item.consumo_Annuale);
            $("#Consumo_Annuale_Impianto").val(item.consumo_Annuale);
            $('#Consumo_Genn').val(item.consumo_Gennaio);
            $('#Consumo_Feb').val(item.consumo_Febbraio);
            $('#Consumo_Marzo').val(item.consumo_Marzo);
            $('#Consumo_Apr').val(item.consumo_Aprile);
            $('#Consumo_Maggio').val(item.consumo_Maggio);
            $('#Consumo_Giu').val(item.consumo_Giugno);
            $('#Consumo_Lug').val(item.consumo_Luglio);
            $('#Consumo_Ago').val(item.consumo_Agosto);
            $('#Consumo_Set').val(item.consumo_Settembre);
            $('#Consumo_Ott').val(item.consumo_Ottobre);
            $('#Consumo_Nov').val(item.consumo_Novembre);
            $('#Consumo_Dic').val(item.consumo_Dicembre);
            $('#nome_impianto').val(item.descrizione);


            $("#centermodal_Impianto").modal("show");
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function AddImpianto(Nome, Forma_Utenza, Tipo_Utenza, N_Utenze_Cluster, Modalita, Btn) {

    console.log(Nome);

    var apo = String.fromCharCode(39);
    myData = [
        {
            "DESCRIZIONE": Nome,
            "FORMA": Forma_Utenza,
            "TIPOLOGIA_UTENZA": Tipo_Utenza,
            "NUMERO_UTENZE": N_Utenze_Cluster,
            "MODALITA_INSERIMENTO": Modalita,
            "BTN": Btn
        }

    ];




    Table_Impianto.rows.add(myData);

}

$(document).ready(function () {
    // Effettua una chiamata AJAX al controller
    GetSelectTipo_Utenza(-1);
    _GetTableConsumer();
    _GetTableProsumer();
    _GetTableImpianto();
});

function _GetTableConsumer() {
    $.ajax({
        url: 'DatiUtenze/GetTableConsumer',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            // Itera sui dati e chiama la funzione JavaScript per ogni elemento
            response.forEach(function (item) {
                console.log(item);
                AddConsumer(item.nome, item.formautenza, item.tipoutenza, item.nutenzecluster, item.modalita, item.btn)
            });
            Table_Consumer.draw(true);
            ! function ($) {
                "use strict";

                var SweetAlert = function () { };

                SweetAlert.prototype.init = function () {

                    $(".btncancelConsumer").click(function () {
                        Swal.fire({
                            title: "Sei sicuro di voler cancellare?",
                            text: "",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#1aa33f',
                            cancelButtonColor: '#d33',
                            confirmButtonText: "Conferma",
                            cancelButtonText: "Annulla"
                        }).then((result) => {
                            if (result.value) {
                                _DeleteConsumer();
                                Table_Consumer.clear().draw(true);
                                
                                Swal.fire(
                                    "Cancellato",
                                    '',
                                    'success'
                                )
                            }
                        })
                    });

                },
                    //init
                    $.SweetAlert = new SweetAlert, $.SweetAlert.Constructor = SweetAlert
            }(window.jQuery),

                //initializing 
                function ($) {
                    "use strict";
                    $.SweetAlert.init()
                }(window.jQuery);
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}
function _GetTableProsumer() {
    $.ajax({
        url: 'DatiUtenze/GetTableProsumer',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            // Itera sui dati e chiama la funzione JavaScript per ogni elemento
            response.forEach(function (item) {
                console.log(item);
                AddProsumer(item.nome, item.formautenza, item.tipoutenza, item.nutenzecluster, item.modalita, item.btn)
            });
            Table_Prosumer.draw(true);
            ! function ($) {
                "use strict";

                var SweetAlert = function () { };

                SweetAlert.prototype.init = function () {

                    $(".btncancelProsumer").click(function () {
                        Swal.fire({
                            title: "Sei sicuro di voler cancellare?",
                            text: "",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#1aa33f',
                            cancelButtonColor: '#d33',
                            confirmButtonText: "Conferma",
                            cancelButtonText: "Annulla"
                        }).then((result) => {
                            if (result.value) {
                                _DeleteProsumer();
                                Table_Prosumer.clear().draw(true);

                                Swal.fire(
                                    "Cancellato",
                                    '',
                                    'success'
                                )
                            }
                        })
                    });

                },
                    //init
                    $.SweetAlert = new SweetAlert, $.SweetAlert.Constructor = SweetAlert
            }(window.jQuery),

                //initializing 
                function ($) {
                    "use strict";
                    $.SweetAlert.init()
                }(window.jQuery);
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}
function _GetTableImpianto() {
    $.ajax({
        url: 'DatiUtenze/GetTableImpianto',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            // Itera sui dati e chiama la funzione JavaScript per ogni elemento
            response.forEach(function (item) {
                console.log(item);
                AddImpianto(item.nome, item.formautenza, item.tipoutenza, item.nutenzecluster, item.modalita, item.btn)
            });
            Table_Impianto.draw(true);
            ! function ($) {
                "use strict";

                var SweetAlert = function () { };

                SweetAlert.prototype.init = function () {

                    $(".btncancelImpianto").click(function () {
                        Swal.fire({
                            title: "Sei sicuro di voler cancellare?",
                            text: "",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#1aa33f',
                            cancelButtonColor: '#d33',
                            confirmButtonText: "Conferma",
                            cancelButtonText: "Annulla"
                        }).then((result) => {
                            if (result.value) {
                                _DeleteImpianto();
                                Table_Impianto.clear().draw(true);

                                Swal.fire(
                                    "Cancellato",
                                    '',
                                    'success'
                                )
                            }
                        })
                    });

                },
                    //init
                    $.SweetAlert = new SweetAlert, $.SweetAlert.Constructor = SweetAlert
            }(window.jQuery),

                //initializing 
                function ($) {
                    "use strict";
                    $.SweetAlert.init()
                }(window.jQuery);
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}
function _SetIdProsumer(id) {
    var formData = {
        id: id
    };
    prosumer_id = id;
    $.ajax({
        url: 'datiutenze/SetIdProsumer',
        type: 'GET',
        data: formData,
        success: function (item) {

        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function _DeleteProsumer() {
    console.log(prosumer_id);
    var formData = {
        id: prosumer_id
    };
    $.ajax({
        url: 'datiutenze/DeleteProsumer',
        type: 'GET',
        data: formData,
        success: function (item) {
            _GetTableProsumer();
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function saveGeneralProsumer() {


    var formData = {
        forma: $('#Forma_Utenza_Prosumer').val(),
        tipo_utenza: $('#Tipo_Utenza_Prosumer').val(),
        n_utenze: $('#n_utenze_Prosumer').val(),
        modalita_inserimento: $('#modalita_inserimento_Prosumer').val(),
        consumo_annuale: $("#Consumo_Annuale_Prosumer").val(),
        gennaio: $('#Consumo_Genn').val(),
        Febbraio: $('#Consumo_Feb').val(),
        Marzo: $('#Consumo_Marzo').val(),
        Aprile: $('#Consumo_Apr').val(),
        Maggio: $('#Consumo_Maggio').val(),
        Giugno: $('#Consumo_Giu').val(),
        Luglio: $('#Consumo_Lug').val(),
        Agosto: $('#Consumo_Ago').val(),
        Settembre: $('#Consumo_Set').val(),
        Ottobre: $('#Consumo_Ott').val(),
        Novembre: $('#Consumo_Nov').val(),
        Dicembre: $('#Consumo_Dic').val(),
        Descrizione: $('#nome_prosumer').val(),
        prosumer_id: prosumer_id
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveGeneralProsumer',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
            Table_Prosumer.clear().draw(true);
            _GetTableProsumer();

            
        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}

function saveDatiImpiantoProsumer() {
    var checkQuota;
    var modProd;
    var isescluso;
    var is_secondafalda;
    if ($('#customCheck1_Prosumer').is(':checked') == false) {
        checkQuota = 0;
    } else {
        checkQuota = 1;
    }
    if ($('#mod_Prosumer').is(':checked') == false) {
        modProd = 0;
    } else {
        modProd = 1;
    }
    if ($('#checkTariffa_Prosumer').is(':checked') == false) {
        isescluso = 0;
    } else {
        isescluso = 1;
    }
    if ($('#customCheck2_Prosumer').is(':checked') == false) {
        is_secondafalda = 0;
    } else {
        is_secondafalda = 1;
    }
    


    mod_Prosumer
    var formData = {
        longitudine: $('#Longitudine_Prosumer').val(),
        latitudine: $('#Latitudine_Prosumer').val(),
        potenza_impianto: $('#Potenza_impianto_Prosumer').val(),
        checkquota: checkQuota,
        potenza_rinnovabile: $("#Potenza_rinnovabile_Prosumer").val(),
        potenza_inverter: $('#potenza_inverter_Prosumer').val(),
        capacita_batteria: $('#capacita_batteria_Prosumer').val(),
        ismodProd: modProd,
        costokw: $('#costokw_Prosumer').val(),
        costotot: $('#costotot_Prosumer').val(),
        escluso_premio: isescluso,
        area_impianto: $('#Area_Impianto_Prosumer').val(),
        tipo_impianto: $('#Tipo_Impianto_Prosumer').val(),
        data_potenza_inverter: $('#data_potenza_inverter_Prosumer').val(),
        is_secondafalda: is_secondafalda,
        potenza_sezione: $('#Potenza_sezione1_Prosumer').val(),
        angolo_tilt: $('#angolo_di_tilt1_Prosumer').val(),
        angolo_azimut: $('#angolo_di_azimut1_Prosumer').val(),
        potenza_sezione_s: $('#Potenza_sezione2_Prosumer').val(),
        angolo_tilt_s: $('#angolo_di_tilt2_Prosumer').val(),
        angolo_azimut_s: $('#angolo_di_azimut2_Prosumer').val(),
        efficienza_pannello: $("#efficienzapannello_Prosumer").val(),
        coefficiente_t: $("#coef_temp_Prosumer").val(),
        prestazioni: $("#prestazioni_Prosumer").val(),
        efficienza_inverter: $("#efficienzainverter_Prosumer").val(),
        costo_ric_batt: $("#costo_Prosumer").val(),
        perdite: $("#perdite_Prosumer").val(),        
        prosumer_id: prosumer_id
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveImpiantoProsumer',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
            Table_Prosumer.clear().draw(true);
            _GetTableProsumer();

            // Imposta un timer per ricaricare la pagina dopo 5 secondi (5000 millisecondi)
            
        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}

function saveDatiEconomiciProsumer() {
    var checkVendita;
    var checkAcquisto;
    var isDetrazione;
    var is_finanziamento;
    var is_conto;
    if ($('#check_vendita_prosumer').is(':checked') == false) {
        checkVendita = 0;
    } else {
        checkVendita = 1;
    }
    if ($('#check_acquisto_prosumer').is(':checked') == false) {
        checkAcquisto = 0;
    } else {
        checkAcquisto = 1;
    }
    if ($('#DetrazioneProsumer').is(':checked') == false) {
        isDetrazione = 0;
    } else {
        isDetrazione = 1;
    }
    if ($('#CheckFinanziamentoProsumer').is(':checked') == false) {
        is_finanziamento = 0;
    } else {
        is_finanziamento = 1;
    }
    if ($('#CheckContoProsumer').is(':checked') == false) {
        is_conto = 0;
    } else {
        is_conto = 1;
    }
    var formData = {
        isvendita: checkVendita,
        isacquisto: checkAcquisto,
        is_detrazione: isDetrazione,
        isfinanziamento: is_finanziamento,
        isconto: is_conto,
        tipo_utente: $('#Tipo_Utenza_Economica_Prosumer').val(),
        prezzo_f1: $('#PUN_F1_PRE_PRO').val(),
        prezzo_f2: $('#PUN_F2_PRE_PRO').val(),
        prezzo_f3: $('#PUN_F3_PRE_PRO').val(),
        costo_f1: $('#PUN_F1_COSTO_PRO').val(),
        costo_f2: $('#PUN_F2_COSTO_PRO').val(),
        costo_f3: $('#PUN_F3_COSTO_PRO').val(),
        altre_spese_inv: $('#AltreSpeseInvestmentProsumer').val(),
        altre_spese: $('#AltreSpeseProsumer').val(),
        spese_man: $('#AltreSpeseAnnoProsumer').val(),
        finanziamento: $('#FinanziamentoProsumer').val(),
        interesse: $('#IntersseFinanziamentoProsumer').val(),
        perce_finanziamento: $('#PercentFinanziamentoProsumer').val(),
        prosumer_id: prosumer_id
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveEconomiciProsumer',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
            Table_Prosumer.clear().draw(true);
            _GetTableProsumer();

            // Imposta un timer per ricaricare la pagina dopo 5 secondi (5000 millisecondi)
         
        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}

function saveGeneralImpianto() {


    var formData = {
        forma: $('#Forma_Utenza_Impianto').val(),
        tipo_utenza: $('#Tipo_Utenza_Impianto').val(),
        n_utenze: $('#n_utenze_Impianto').val(),
        modalita_inserimento: $('#modalita_inserimento_Impianto').val(),
        consumo_annuale: $("#Consumo_Annuale_Impianto").val(),
        gennaio: $('#Consumo_Genn').val(),
        Febbraio: $('#Consumo_Feb').val(),
        Marzo: $('#Consumo_Marzo').val(),
        Aprile: $('#Consumo_Apr').val(),
        Maggio: $('#Consumo_Maggio').val(),
        Giugno: $('#Consumo_Giu').val(),
        Luglio: $('#Consumo_Lug').val(),
        Agosto: $('#Consumo_Ago').val(),
        Settembre: $('#Consumo_Set').val(),
        Ottobre: $('#Consumo_Ott').val(),
        Novembre: $('#Consumo_Nov').val(),
        Dicembre: $('#Consumo_Dic').val(),
        Descrizione: $('#nome_impianto').val(),
        impianto_id: impianto_id
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveGeneralImpianto',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
            Table_Impianto.clear().draw(true);
            _GetTableImpianto();

        
        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}
function saveDatiEconomiciImpianto() {
    var checkVendita;
    var checkAcquisto;
    var isDetrazione;
    var is_finanziamento;
    var is_conto;
    if ($('#check_vendita_impianto').is(':checked') == false) {
        checkVendita = 0;
    } else {
        checkVendita = 1;
    }
    if ($('#check_acquisto_impianto').is(':checked') == false) {
        checkAcquisto = 0;
    } else {
        checkAcquisto = 1;
    }
    if ($('#DetrazioneImpianto').is(':checked') == false) {
        isDetrazione = 0;
    } else {
        isDetrazione = 1;
    }
    if ($('#CheckFinanziamentoImpianto').is(':checked') == false) {
        is_finanziamento = 0;
    } else {
        is_finanziamento = 1;
    }
    if ($('#CheckContoImpianto').is(':checked') == false) {
        is_conto = 0;
    } else {
        is_conto = 1;
    }
    var formData = {
        isvendita: checkVendita,
        isacquisto: checkAcquisto,
        is_detrazione: isDetrazione,
        isfinanziamento: is_finanziamento,
        isconto: is_conto,
        tipo_utente: $('#Tipo_Utenza_Economica_Impianto').val(),
        prezzo_f1: $('#PUN_F1_PRE_IMP').val(),
        prezzo_f2: $('#PUN_F2_PRE_IMP').val(),
        prezzo_f3: $('#PUN_F3_PRE_IMP').val(),
        costo_f1: $('#PUN_F1_COSTO_IMP').val(),
        costo_f2: $('#PUN_F2_COSTO_IMP').val(),
        costo_f3: $('#PUN_F3_COSTO_IMP').val(),
        altre_spese_inv: $('#AltreSpeseInvestmentProsumer').val(),
        altre_spese: $('#AltreSpeseProsumer').val(),
        spese_man: $('#AltreSpeseAnnoProsumer').val(),
        finanziamento: $('#FinanziamentoProsumer').val(),
        interesse: $('#IntersseFinanziamentoProsumer').val(),
        perce_finanziamento: $('#PercentFinanziamentoProsumer').val(),
        impianto_id: impianto_id
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveEconomiciProsumer',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
            Table_Prosumer.clear().draw(true);
            _GetTableProsumer();
            location.reload();
            // Imposta un timer per ricaricare la pagina dopo 5 secondi (5000 millisecondi)
            
        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}
    function _SetIdImpianto(id) {
        var formData = {
            id: id
        };
        impianto_id = id;
        $.ajax({
            url: 'datiutenze/SetIdImpianto',
            type: 'GET',
            data: formData,
            success: function (item) {

            },
            error: function (xhr, status, error) {
                console.log(error);
                console.error("Errore durante la chiamata AJAX: " + error);
            }
        });
    }


function _DeleteImpianto() {
    console.log(impianto_id);
    var formData = {
        id: impianto_id
    };
    $.ajax({
        url: 'datiutenze/DeleteImpianto',
        type: 'GET',
        data: formData,
        success: function (item) {
            _GetTableImpianto();
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}
function ModiImpianto(id) {
    var formData = {
        id: id
    };
    impianto_id = id;
    $.ajax({
        url: 'datiutenze/ModificaImpianto',
        type: 'GET',
        data: formData,
        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

            console.log(item);
            $('#Forma_Utenza_Impianto').val(item.forma_Utenza);
            $('#Tipo_Utenza_Impianto').val(item.tipo_Utenza_Id)
            $('#n_utenze_Impianto').val(item.n_Utenze_Cluster);
            console.log("pippo");
            console.log(item.modalita_inserimento);
            $('#modalita_inserimento_Impianto').val(item.modalita_Inserimento);
            ShowCol();
            $("#Consumo_Annuale_Impianto").val(item.consumo_Annuale);
            $('#Consumo_Genn').val(item.consumo_Gennaio);
            $('#Consumo_Feb').val(item.consumo_Febbraio);
            $('#Consumo_Marzo').val(item.consumo_Marzo);
            $('#Consumo_Apr').val(item.consumo_Aprile);
            $('#Consumo_Maggio').val(item.consumo_Maggio);
            $('#Consumo_Giu').val(item.consumo_Giugno);
            $('#Consumo_Lug').val(item.consumo_Luglio);
            $('#Consumo_Ago').val(item.consumo_Agosto);
            $('#Consumo_Set').val(item.consumo_Settembre);
            $('#Consumo_Ott').val(item.consumo_Ottobre);
            $('#Consumo_Nov').val(item.consumo_Novembre);
            $('#Consumo_Dic').val(item.consumo_Dicembre);
            $('#nome_impianto').val(item.descrizione);

            ModiImpiantoImpianto(Impianto_id);
            ModiEconomiciImpianto(Impianto_id);
            $("#centermodal_impianto").modal("show");
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function ModiImpiantoImpianto(id) {
    var formData = {
        id: id
    };
    consumer_id = id;
    $.ajax({
        url: 'datiutenze/ModificaImpiantoImpianto',
        type: 'GET',
        data: formData,
        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

            console.log(item);
            $('#Longitudine_Impianto').val(item.longitudine);
            $('#Latitudine_Impianto').val(item.latitudine);
            $('#Potenza_impianto_Impianto').val(item.potenza_Impianto);
            $('#Quota_Potenza_Rinnovabile_Impianto').val(item.quota_Potenza_Rinnovabile);
            $('#Potenza_inverter_Impianto').val(item.potenza_Inverter);
            $('#Costo_Impianto_Impianto').val(item.costo_Impianto);
            $('#Capacita_Batteria_Impianto').val(item.capacita_Batteria);
            $('#Is_Costo_KW_Impianto').prop('checked', item.is_Costo_KW);
            $('#Costo_Totale_Impianto').val(item.costo_Totale);
            $('#Costo_KW_Impianto').val(item.costo_KW);
            $('#Is_Escluso_Premio_Impianto').prop('checked', item.is_Escluso_Premio);
            $('#Area_Impianto_Impianto').val(item.area_Impianto);
            $('#Tipologia_Impianto_Impianto').val(item.tipologia_Impianto);
            $('#Data_Esercizio_Impianto').val(item.data_Esercizio);
            $('#Is_Seconda_Falda_Impianto').prop('checked', item.is_Seconda_Falda);
            $('#Potenza_Sezione_Impianto').val(item.potenza_Sezione);
            $('#Angolo_di_tilt_Impianto').val(item.angolo_di_tilt);
            $('#Angolo_di_Azimut_Impianto').val(item.angolo_di_Azimut);
            $('#Potenza_Sezione_S_Impianto').val(item.potenza_Sezione_S);
            $('#Angolo_di_tilt_S_Impianto').val(item.angolo_di_tilt_S);
            $('#Angolo_di_Azimut_S_Impianto').val(item.angolo_di_Azimut_S);
            $('#Efficienza_Impianto').val(item.efficienza);
            $('#Coefficiente_T_Impianto').val(item.coefficiente_T);
            $('#NOCT_Impianto').val(item.noct);
            $('#Fattore_Riduzione_Impianto').val(item.fattore_Riduzione);
            $('#Efficienza_Inverter_Impianto').val(item.efficienza_Inverter);
            $('#Costo_Ricambio_Batt_Impianto').val(item.costo_Ricambio_Batt);
            $('#Altre_Perdite_Impianto').val(item.altre_Perdite);

            $('#checkTariffa_Impianto').prop('checked', item.is_Escluso_Premio);
            $('#mod_Impianto').prop('checked', item.is_Costo_KW);
            $('#customCheck1_Impianto').prop('checked', item.is_Abilitato_Rinnovabile);
            $('#customCheck2_Impianto').prop('checked', item.is_Seconda_Falda);

            ShowRinnovabile_Impianto();
            SwitchCosto_Impianto();
            ShowSezione2_Impianto();


        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function ModiEconomiciImpianto(id) {
    var formData = {
        id: id
    };
    consumer_id = id;
    $.ajax({
        url: 'datiutenze/ModificaImpiantoImpianto',
        type: 'GET',
        data: formData,
        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

            console.log(item);
            $('#check_vendita_impianto').prop('checked', item.modalita_Vendita === 1);
            $('#check_acquisto_impianto').prop('checked', item.modalita_Acquisto === 1);
            $('#DetrazioneImpianto').prop('checked', item.is_Detrazione);
            $('#CheckFinanziamentoImpianto').prop('checked', item.is_Finanziamento);
            $('#CheckContoImpinato').prop('checked', item.is_Conto);
            $('#Tipo_Utenza_Economica_Impianto').val(item.tipo_Utenza_Id);
            $('#PUN_F1_PRE_IMP').val(item.prezzo_F1);
            $('#PUN_F2_PRE_IMP').val(item.prezzo_F2);
            $('#PUN_F3_PRE_IMP').val(item.prezzo_F3);
            $('#PUN_F1_COSTO_IMP').val(item.costo_F1);
            $('#PUN_F2_COSTO_IMP').val(item.costo_F2);
            $('#PUN_F3_COSTO_IMP').val(item.costo_F3);
            $('#AltreSpeseInvestmentImpianto').val(item.altre_Spese_Investimento);
            $('#AltreSpeseImpianto').val(item.altre_Spese);
            $('#AltreSpeseAnnoImpianto').val(item.spese_Manutenzione);
            $('#FinanziamentoImpianto').val(item.importo_Finanziamento);
            $('#IntersseFinanziamentoImpianto').val(item.interesse_Finanziamento);
            $('#PercentFinanziamentoImpianto').val(item.conto_Capitale);


            SwitchCostoAcquistoImpianto();
            SwitchCostoVenditaImpianto();
            ShowHideFinanziamentoImpianto();
            ContoChangeImpianto();

        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}