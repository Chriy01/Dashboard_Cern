var comunita_id;
function SwitchCosto() {
    if ($('#check_mercato').is(":checked")) {
        $('#parameters_col').show();
    } else {
        $('#parameters_col').hide();
    }
}

function SwitchParametri() {
    if ($('#check_parametri').is(":checked")) {
        $('#insert_month').show();
        $('#parametri_annuali').hide();
    } else {


        $('#insert_month').hide();
        $('#parametri_annuali').show();
    }
}



function sendSelectedGenerali() {
    var Zona_Mercato = document.getElementById("ZonaMercato").value;
    var Tipo_Conf;
    var nome_Comunita = $('#nome_com').val();
    if ($('#customControlValidation2').is(':checked') == true) {
        Tipo_Conf = 0;
    } else {
        Tipo_Conf = 1;
    }
    var formData = {
        Nome_Comunita: nome_Comunita,
        ZonaMercato: Zona_Mercato,
        Tipo_Conf: Tipo_Conf,
        Inflazione: $('#Inflazione').val(),
        Interesse: $('#Interesse').val(),
        ZonaGeo: $('#ZonaGeo').val(),
        Anno: $('#datepicker').val(),
        prezzo_annuale: $('#pun_annuale').val(),
        prezzo_f1: $('#PUN_F1').val(),
        prezzo_f2: $('#PUN_F2').val(),
        prezzo_f3: $('#PUN_F3').val(),
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
        Dicembre: $('#Consumo_Dic').val()
    };

    $.ajax({
        type: 'POST',
        url: 'DatiGenerali/SaveDatiGenerali',
        data: formData,
        success: function (data) {
            Swal.fire("Salvataggio Effettuato", "", "success");
        },
        error: function () {

            // Gestisci gli errori qui, ad esempio mostrando un messaggio di errore all'utente
        }
    });
}

$(document).ready(function () {

    $('#parameters_col').hide();
    $('#insert_month').hide();
    checkModifica()
})

function checkModifica() {
    $.ajax({
        url: 'datigenerali/ModificaComunita',
        type: 'GET',

        success: function (item) {

            // Itera sui dati e chiama la funzione JavaScript per ogni elemento

            console.log(item);
            $('#nome_com').val(item.nome);
            $('#ZonaMercato').val(item.zona_di_mercato);

            if (item.iscomunita) {
                $('#customControlValidation2').click();
            } else {
                $('#customControlValidation3').click();
            }
            if (item.isPersonalizzato) {
                $("#check_mercato").click();
            }
            $('#Inflazione').val(item.tasso_inflazione_mercato);
            $('#Interesse').val(item.tasso_interesse_mercato);
            $('#ZonaGeo').val(item.zona_geografica);
            $('#datepicker').val(item.anno_di_riferimento);

            $('#pun_annuale').val(item.prezzo_annuale);
            $('#PUN_F1').val(item.prezzo_f1);
            $('#PUN_F2').val(item.prezzo_f2);
            $('#PUN_F3').val(item.prezzo_f3);
            $('#Consumo_Genn').val(item.prezzo_Gennaio);
            $('#Consumo_Feb').val(item.prezzo_Febbraio);
            $('#Consumo_Marzo').val(item.prezzo_Marzo);
            $('#Consumo_Apr').val(item.prezzo_Aprile);
            $('#Consumo_Maggio').val(item.prezzo_Maggio);
            $('#Consumo_Giu').val(item.prezzo_Giugno);
            $('#Consumo_Lug').val(item.prezzo_Luglio);
            $('#Consumo_Ago').val(item.prezzo_Agosto);
            $('#Consumo_Set').val(item.prezzo_Settembre);
            $('#Consumo_Ott').val(item.prezzo_Ottobre);
            $('#Consumo_Nov').val(item.prezzo_Novembre);
            $('#Consumo_Dic').val(item.prezzo_Dicembre);

        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}