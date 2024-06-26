var lat
var lng
// Crea la mappa e impostala su una vista iniziale con una determinata latitudine e longitudine e livello di zoom
var map = L.map('map').setView([41.8903, 12.4922], 13);
var consumer_id;
var prosumer_id;
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

            // Imposta un timer per ricaricare la pagina dopo 5 secondi (5000 millisecondi)
            setTimeout(reloadPage, 150);
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


}

function ShowPopupImpianto() {
    $('#centermodal_impianto').modal();
    $('#div_annuale_Impianto').hide();
    $('#div_mensile_Impianto').hide();
    $('#div_orario_Impianto').hide();
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
        console.log("YAS");

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
        swal("Form Submitted!", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed lorem erat eleifend ex semper, lobortis purus sed.");

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


$(document).ready(function () {
    // Effettua una chiamata AJAX al controller
    GetSelectTipo_Utenza(-1);
    _GetTableConsumer()
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


function saveGeneralProsumer() {


    var formData = {
        forma: $('#Forma_Utenza_Prosumer').val(),
        tipo_utenza: $('#Tipo_Utenza_Prosumer').val(),
        n_utenze: $('#n_utenze_Prosumer').val(),
        modalita_inserimento: $('#modalita_inserimento_Prosumer').val(),
        consumo_annuale: $("#Consumo_Annuale_Prosumer").val(),
        gennaio: $('#Consumo_Genn_Prosumer').val(),
        Febbraio: $('#Consumo_Feb_Prosumer').val(),
        Marzo: $('#Consumo_Marzo_Prosumer').val(),
        Aprile: $('#Consumo_Apr_Prosumer').val(),
        Maggio: $('#Consumo_Maggio_Prosumer').val(),
        Giugno: $('#Consumo_Giu_Prosumer').val(),
        Luglio: $('#Consumo_Lug_Prosumer').val(),
        Agosto: $('#Consumo_Ago_Prosumer').val(),
        Settembre: $('#Consumo_Set_Prosumer').val(),
        Ottobre: $('#Consumo_Ott_Prosumer').val(),
        Novembre: $('#Consumo_Nov_Prosumer').val(),
        Dicembre: $('#Consumo_Dic_Prosumer').val(),
        Descrizione: $('#nome_consumer_prosumer').val(),
        consumer_id: consumer_id
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveGeneralProsumer',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
            Table_Prosumer.clear().draw(true);
            //_GetTableConsumer();

            // Imposta un timer per ricaricare la pagina dopo 5 secondi (5000 millisecondi)
            setTimeout(reloadPage, 150);
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
            //_GetTableConsumer();

            // Imposta un timer per ricaricare la pagina dopo 5 secondi (5000 millisecondi)
            setTimeout(reloadPage, 150);
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
        gennaio: $('#Consumo_Genn_Impianto').val(),
        Febbraio: $('#Consumo_Feb_Impianto').val(),
        Marzo: $('#Consumo_Marzo_Impianto').val(),
        Aprile: $('#Consumo_Apr_Impianto').val(),
        Maggio: $('#Consumo_Maggio_Impianto').val(),
        Giugno: $('#Consumo_Giu_Impianto').val(),
        Luglio: $('#Consumo_Lug_Impianto').val(),
        Agosto: $('#Consumo_Ago_Impianto').val(),
        Settembre: $('#Consumo_Set_Impianto').val(),
        Ottobre: $('#Consumo_Ott_Impianto').val(),
        Novembre: $('#Consumo_Nov_Impianto').val(),
        Dicembre: $('#Consumo_Dic_Impianto').val(),
        Descrizione: $('#nome_consumer_impianto').val(),
        consumer_id: consumer_id
    };

    $.ajax({
        type: 'POST',
        url: 'DatiUtenze/SaveGeneralImpianto',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
            Table_Impianto.clear().draw(true);
            //_GetTableConsumer();

            // Imposta un timer per ricaricare la pagina dopo 5 secondi (5000 millisecondi)
            setTimeout(reloadPage, 150);
        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}
