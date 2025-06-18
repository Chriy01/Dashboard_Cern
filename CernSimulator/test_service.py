from flask import Flask, request, jsonify
import pandas as pd

app = Flask(__name__)

# Load system data
system_data = {
    "PUN": pd.read_excel("0_Dati di sistema.xlsx", sheet_name="PUN"),
    "Oneri": pd.read_excel("0_Dati di sistema.xlsx", sheet_name="Oneri"),
    "Fasce": pd.read_excel("0_Dati di sistema.xlsx", sheet_name="Fasce"),
    "Date": pd.read_excel("0_Dati di sistema.xlsx", sheet_name="Date")
}


@app.route('/api/python/receiveData', methods=['POST'])
def receive_data():

    technical_data = request.get_json()
    print(technical_data)
    configurazione = 1
    zona_di_mercato = 0
    zona_geografica = 1
    tasso_inflazione = technical_data["tasso_inflazione_mercato"]
    tasso_interesse = technical_data["tasso_interesse_mercato"]
    prezzo_annuale = technical_data["Prezzo_annuale"]
    gennaio = technical_data["Prezzo_Gennaio"]
    febbraio = technical_data["Prezzo_Febbraio"]
    marzo = technical_data["Prezzo_Marzo"]
    aprile = technical_data["Prezzo_Aprile"]
    maggio = technical_data["Prezzo_Maggio"]
    giugno = technical_data["Prezzo_Giugno"]
    luglio = technical_data["Prezzo_Luglio"]
    agosto = technical_data["Prezzo_Agosto"]
    settembre = technical_data["Prezzo_Settembre"]
    ottobre = technical_data["Prezzo_Ottobre"]
    novembre = technical_data["Prezzo_Novembre"]
    dicembre = technical_data["Prezzo_Dicembre"]
    prezzo_F1 = technical_data["Prezzo_F1"]
    prezzo_F2 = technical_data["Prezzo_F2"]
    prezzo_F3 = technical_data["Prezzo_F3"]


    match technical_data["Zona_di_mercato"]:
        case "CNOR":
            zona_di_mercato = 1
        case "CSUD":
            zona_di_mercato = 2
        case "NORD":
            zona_di_mercato = 3
        case "SARD":
            zona_di_mercato = 4
        case "SICI":
            zona_di_mercato = 5
        case "SUD":
            zona_di_mercato = 6
        case "CALA":
            zona_di_mercato = 7
        case _:
            zona_di_mercato = 1

    if technical_data["iscomunita"] == "false":
        configurazione = 2
    else:
        configurazione = 1

    match technical_data["zona_geografica"]:
        case "NORD":
            zona_geografica = 1
        case "CENTRO":
            zona_geografica = 2
        case "SUD":
            zona_geografica = 3

    print(configurazione)
    print(zona_di_mercato)
    print(zona_geografica)
    print(tasso_inflazione)
    print(tasso_interesse)
    print(prezzo_annuale)
    print(gennaio)
    print(febbraio)
    print(marzo)
    print(aprile)
    print(maggio)
    print(giugno)
    print(luglio)
    print(agosto)
    print(settembre)
    print(ottobre)
    print(novembre)
    print(dicembre)
    print(prezzo_F1)
    print(prezzo_F2)
    print(prezzo_F3)

    return jsonify({"status": "success"}), 200

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5001)