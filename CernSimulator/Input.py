import numpy as np
import pandas as pd
import requests
from flask import Flask, request, jsonify
import pandas as pd

from datetime import datetime

from openpyxl.reader.excel import load_workbook

from A_Curve import Curva_residenziale, Curva_ut_condominio, Curva_supermarket, Curva_industria, Curva_ufficio


app = Flask(__name__)





@app.route('/api/python/receiveData', methods=['POST'])
def receive_data():
    ## ================
    # Lettura dei dati dai file Excel
    ## ================
    Dati_sist = r'C:\Python\CernSimulator\0_Dati di sistema.xlsx'
    Dati_sist_PUN = pd.read_excel(Dati_sist, sheet_name='PUN').to_numpy()
    Dati_sist_Oneri = pd.read_excel(Dati_sist, sheet_name='Oneri', header=None).to_numpy()
    Dati_sist_Fasce = pd.read_excel(Dati_sist, sheet_name='Fasce', header=None).to_numpy()
    Dati_sist_Date = pd.read_excel(Dati_sist, sheet_name='Date', header=None).to_numpy()


    aggregated_data = request.get_json()
    technical_data = aggregated_data.get("Comunita", {})
    configurazione = 1
    zona_di_mercato = 0
    zona_geografica = 1
    anno_rif = technical_data.get("Anno_di_riferimento", 2019)
    tasso_inflazione = technical_data.get("tasso_inflazione_mercato", 0.0)
    tasso_interesse = technical_data.get("tasso_interesse_mercato", 0.0)
    prezzo_annuale = technical_data.get("Prezzo_annuale", 0.0)
    gennaio = technical_data.get("Prezzo_Gennaio", 0.0)
    febbraio = technical_data.get("Prezzo_Febbraio", 0.0)
    marzo = technical_data.get("Prezzo_Marzo", 0.0)
    aprile = technical_data.get("Prezzo_Aprile", 0.0)
    maggio = technical_data.get("Prezzo_Maggio", 0.0)
    giugno = technical_data.get("Prezzo_Giugno", 0.0)
    luglio = technical_data.get("Prezzo_Luglio", 0.0)
    agosto = technical_data.get("Prezzo_Agosto", 0.0)
    settembre = technical_data.get("Prezzo_Settembre", 0.0)
    ottobre = technical_data.get("Prezzo_Ottobre", 0.0)
    novembre = technical_data.get("Prezzo_Novembre", 0.0)
    dicembre = technical_data.get("Prezzo_Dicembre", 0.0)
    prezzo_F1 = technical_data.get("Prezzo_F1", 0.0)
    prezzo_F2 = technical_data.get("Prezzo_F2", 0.0)
    prezzo_F3 = technical_data.get("Prezzo_F3", 0.0)
    if prezzo_F1 is None:
        prezzo_F1 = 0.0

    if prezzo_F2 is None:
        prezzo_F2 = 0.0

    if prezzo_F3 is None:
        prezzo_F3 = 0.0
    pun_modalita = 1
    if(gennaio > 0):
        pun_modalita = 1
    else:
        pun_modalita = 2

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

    # Estrai le liste di dati
    impianti = aggregated_data['Impianti']
    prosumers = aggregated_data['Prosumers']
    consumers = aggregated_data['Consumers']
    dati_imp = aggregated_data['Dati_Impianto']
    print(dati_imp)
    # Funzione per convertire una lista di dizionari in una lista di liste
    def list_to_columns(item_list):
        columns = []
        for item in item_list:
            column = []
            for value in item.values():
                # Se il valore è una stringa che rappresenta un numero, converti in float
                if isinstance(value, str):
                    try:
                        value = float(value)
                    except ValueError:
                        value = 0
                # Aggiungi valore se è int o float, altrimenti aggiungi 0
                if isinstance(value, (int, float)):
                    column.append(value)
                else:
                    column.append(0)
            columns.append(column)
        return columns

    # Converti le liste di dizionari in liste di colonne
    impianti_columns = list_to_columns(impianti)
    prosumers_columns = list_to_columns(prosumers)
    consumers_columns = list_to_columns(consumers)
    print(dati_imp)
    dati_imp_columns = list_to_columns(dati_imp)

    # Trova la lunghezza massima delle colonne per riempire con NaN i valori mancanti
    max_length = max(len(col) for col in impianti_columns + prosumers_columns + consumers_columns)

    # Riempi le colonne più corte con 0
    for col in impianti_columns + prosumers_columns + consumers_columns:
        while len(col) < max_length:
            col.append(0)

    # Ripeti gli ultimi 12 valori tre volte
    def repeat_last_12_values(columns):
        for col in columns:
            if len(col) >= 12:
                last_12_values = col[-12:]
                col.extend(last_12_values * 2)  # Aggiungiamo altre 2 volte per un totale di 3 volte
        return columns

    # Applica la funzione di ripetizione agli ultimi 12 valori
    impianti_columns = repeat_last_12_values(impianti_columns)
    prosumers_columns = repeat_last_12_values(prosumers_columns)
    consumers_columns = repeat_last_12_values(consumers_columns)

    # Combina tutte le colonne in un unico DataFrame
    combined_columns = impianti_columns + prosumers_columns + consumers_columns
    combined_df = pd.DataFrame(combined_columns).T
    dati_imp_dataframe = pd.DataFrame(dati_imp_columns).T

    # Salva il DataFrame in un file Excel senza intestazioni e senza indici
    output_file_path = 'dati.xlsx'
    combined_df.to_excel(output_file_path, index=False, header=False)

    print(f"File Excel salvato in: {output_file_path}")
    Dati_utenze = combined_df

    # Combina i DataFrame in un unico DataFrame
    #Dati_utenze = pd.concat([prosumers_df, consumers_df, impianti_df], axis=0, ignore_index=True)

    Dati_economici = pd.read_excel(r'C:\Python\CernSimulator\1_Dati.xlsx', sheet_name='Economici', header=None).to_numpy()


    output_file_path = 'dati_tec.xlsx'
    dati_imp_dataframe.to_excel(output_file_path, index=False, header=False)

    Dati_tecn = pd.read_excel('dati_tec.xlsx', sheet_name='Sheet1', header=None).to_numpy()
    print("DATI TECNICI")
    print(Dati_tecn)
    #Dati_utenze = pd.read_excel('1_Dati.xlsx', sheet_name='Utenze', header=None).to_numpy()

    ## ================
    # Dati iniziali
    ## ================

    ## Dati per Tecnici

    # Conta dei giorni totali negli anni considerati (così conta pure i bisestili)
    Start_y = int(Dati_sist_Date[0, 0])
    End_y = int(Dati_sist_Date[1, 0])
    Date1 = pd.date_range(start=datetime(Start_y, 1, 1), end=datetime(End_y, 12, 31), freq='D')

    ## Dati per Utenze

    # Anno per il database dei prezzi
    Anno = anno_rif
    Fasce = Dati_sist_Fasce[:, Anno - 2019]
    print(Dati_utenze)
    # Carica il file Excel esistente
    file_path = '1_Dati_test.xlsx'

    # Carica il foglio "Utenze"
    writer = pd.ExcelWriter(file_path, engine='openpyxl')
    # Sostituisce il foglio "Utenze" con i nuovi dati, senza intestazioni di colonna
    Dati_utenze.to_excel(writer, sheet_name='Utenze', index=False, header=False)
    Dati_utenze = pd.read_excel('dati.xlsx', sheet_name='Sheet1', header=None).to_numpy()
    Numero_di_cluster = int(Dati_utenze[0, -1])
    Numero_di_utenze = int(np.sum(Dati_utenze[3, :]))

    ## Dati per Economici

    # Zona di mercato
    Zona = zona_di_mercato

    # PUN: Prezzo unico nazionale
    PUN_mod = pun_modalita
    PUN = np.zeros(8760)
    # Ciclo per valutare il PUN in base all'inserimento dei dati
    # 1: Anno di riferimento, 2: Annuale con fasce, 3: Mensile con fasce
    if PUN_mod == 0:
        PUN = Dati_sist_PUN[:, (Anno-2019)*8]                                      #[€/MWh]
        PUN = PUN * 100 / 1000                                                     #[c€/kWh]

    elif PUN_mod == 1:
        PUN_y = prezzo_annuale                                                     #[€/MWh]

        indice1PUN = np.where(Dati_sist_Fasce[:, Anno-2019] == 1)
        PUN[indice1PUN] = PUN_y * 100 / 1000 * (1 + prezzo_F1 / 100)    #[c€/kWh]

        indice2PUN = np.where(Dati_sist_Fasce[:, Anno-2019] == 2)
        PUN[indice2PUN] = PUN_y * 100 / 1000 * (1 + prezzo_F2 / 100)    #[c€/kWh]

        indice3PUN = np.where(Dati_sist_Fasce[:, Anno-2019] == 3)
        PUN[indice3PUN] = PUN_y * 100 / 1000 * (1 + prezzo_F3 / 100)    #[c€/kWh]

    elif PUN_mod == 2:
        PUN_gen = gennaio                                                          #[€/MWh]
        PUN_feb = febbraio                                                         #[€/MWh]
        PUN_mar = marzo                                                            #[€/MWh]
        PUN_apr = aprile                                                           #[€/MWh]
        PUN_mag = maggio                                                           #[€/MWh]
        PUN_giu = giugno                                                           #[€/MWh]
        PUN_lug = luglio                                                           #[€/MWh]
        PUN_ago = agosto                                                           #[€/MWh]
        PUN_set = settembre                                                        #[€/MWh]
        PUN_ott = ottobre                                                          #[€/MWh]
        PUN_nov = novembre                                                         #[€/MWh]
        PUN_dic = dicembre                                                         #[€/MWh]

        PUN_molt = np.zeros(8760)
        PUN_m = np.zeros(8760)

        # Vettore che tiene conto degli spostamenti percentuali nelle fasce orarie
        indice1PUN = np.where(Dati_sist_Fasce[:, Anno-2019] == 1)
        PUN_molt[indice1PUN] = 1 + prezzo_F1 / 100                                 #[%]

        indice2PUN = np.where(Dati_sist_Fasce[:, Anno-2019] == 2)
        PUN_molt[indice2PUN] = 1 + prezzo_F2 / 100                                 #[%]

        indice3PUN = np.where(Dati_sist_Fasce[:, Anno-2019] == 3)
        PUN_molt[indice3PUN] = 1 + prezzo_F3 / 100                                 #[%]

        PUN_m[:744] = PUN_gen
        PUN_m[744:1416] = PUN_feb
        PUN_m[1417:2160] = PUN_mar
        PUN_m[2161:2880] = PUN_apr
        PUN_m[2881:3624] = PUN_mag
        PUN_m[3625:4344] = PUN_giu
        PUN_m[4345:5088] = PUN_lug
        PUN_m[5089:5832] = PUN_ago
        PUN_m[5833:6552] = PUN_set
        PUN_m[6553:7296] = PUN_ott
        PUN_m[7297:8016] = PUN_nov
        PUN_m[8017:8760] = PUN_dic

        PUN = PUN_m * 100 / 1000 * PUN_molt                                        #[c€/kWh]

    ## ================
    # Inizializzazione dei vettori vuoti
    ## ================

    G_i_tot = np.zeros((len(Date1)*24, Dati_tecn.shape[1]))
    T2m_tot = np.zeros((len(Date1)*24, Dati_tecn.shape[1]))
    Matrice_G_t = np.zeros((8761, Dati_tecn.shape[1]))
    Matrice_Ta = np.zeros((8761, Dati_tecn.shape[1]))
    G_i_temp = np.zeros((len(Date1)*24, 2))
    T2m_temp = np.zeros((len(Date1)*24, 2))
    Matrice_tecn = np.zeros((17, Dati_tecn.shape[1]))

    Load_y = np.zeros(Dati_utenze.shape[1])
    Load_y1 = np.zeros(Dati_utenze.shape[1])
    Load_y2 = np.zeros(Dati_utenze.shape[1])
    Load_y3 = np.zeros(Dati_utenze.shape[1])
    Load_m = np.zeros((Dati_utenze.shape[1], 12))
    Load_m1 = np.zeros((Dati_utenze.shape[1], 12))
    Load_m2 = np.zeros((Dati_utenze.shape[1], 12))
    Load_m3 = np.zeros((Dati_utenze.shape[1], 12))
    Load_h = np.zeros((Dati_utenze.shape[1], 8760))
    Load_h1 = np.zeros((Dati_utenze.shape[1], 8760))
    Load_h2 = np.zeros((Dati_utenze.shape[1], 8760))
    Load_h3 = np.zeros((Dati_utenze.shape[1], 8760))
    k = 1
    Matrice_El_h = np.zeros((Numero_di_utenze, 8763))
    # Creazione di array per le curve mensili
    ore_mese = [744, 672, 744, 720, 744, 720, 744, 744, 720, 744, 720, 744]
    Curva_gen = np.zeros((Dati_utenze.shape[1], ore_mese[0]))
    Curva_feb = np.zeros((Dati_utenze.shape[1], ore_mese[1]))
    Curva_mar = np.zeros((Dati_utenze.shape[1], ore_mese[2]))
    Curva_apr = np.zeros((Dati_utenze.shape[1], ore_mese[3]))
    Curva_mag = np.zeros((Dati_utenze.shape[1], ore_mese[4]))
    Curva_giu = np.zeros((Dati_utenze.shape[1], ore_mese[5]))
    Curva_lug = np.zeros((Dati_utenze.shape[1], ore_mese[6]))
    Curva_ago = np.zeros((Dati_utenze.shape[1], ore_mese[7]))
    Curva_set = np.zeros((Dati_utenze.shape[1], ore_mese[8]))
    Curva_ott = np.zeros((Dati_utenze.shape[1], ore_mese[9]))
    Curva_nov = np.zeros((Dati_utenze.shape[1], ore_mese[10]))
    Curva_dic = np.zeros((Dati_utenze.shape[1], ore_mese[11]))
    Curva_gen1 = np.zeros((Dati_utenze.shape[1], ore_mese[0]))
    Curva_feb1 = np.zeros((Dati_utenze.shape[1], ore_mese[1]))
    Curva_mar1 = np.zeros((Dati_utenze.shape[1], ore_mese[2]))
    Curva_apr1 = np.zeros((Dati_utenze.shape[1], ore_mese[3]))
    Curva_mag1 = np.zeros((Dati_utenze.shape[1], ore_mese[4]))
    Curva_giu1 = np.zeros((Dati_utenze.shape[1], ore_mese[5]))
    Curva_lug1 = np.zeros((Dati_utenze.shape[1], ore_mese[6]))
    Curva_ago1 = np.zeros((Dati_utenze.shape[1], ore_mese[7]))
    Curva_set1 = np.zeros((Dati_utenze.shape[1], ore_mese[8]))
    Curva_ott1 = np.zeros((Dati_utenze.shape[1], ore_mese[9]))
    Curva_nov1 = np.zeros((Dati_utenze.shape[1], ore_mese[10]))
    Curva_dic1 = np.zeros((Dati_utenze.shape[1], ore_mese[11]))
    Curva_gen2 = np.zeros((Dati_utenze.shape[1], ore_mese[0]))
    Curva_feb2 = np.zeros((Dati_utenze.shape[1], ore_mese[1]))
    Curva_mar2 = np.zeros((Dati_utenze.shape[1], ore_mese[2]))
    Curva_apr2 = np.zeros((Dati_utenze.shape[1], ore_mese[3]))
    Curva_mag2 = np.zeros((Dati_utenze.shape[1], ore_mese[4]))
    Curva_giu2 = np.zeros((Dati_utenze.shape[1], ore_mese[5]))
    Curva_lug2 = np.zeros((Dati_utenze.shape[1], ore_mese[6]))
    Curva_ago2 = np.zeros((Dati_utenze.shape[1], ore_mese[7]))
    Curva_set2 = np.zeros((Dati_utenze.shape[1], ore_mese[8]))
    Curva_ott2 = np.zeros((Dati_utenze.shape[1], ore_mese[9]))
    Curva_nov2 = np.zeros((Dati_utenze.shape[1], ore_mese[10]))
    Curva_dic2 = np.zeros((Dati_utenze.shape[1], ore_mese[11]))
    Curva_gen3 = np.zeros((Dati_utenze.shape[1], ore_mese[0]))
    Curva_feb3 = np.zeros((Dati_utenze.shape[1], ore_mese[1]))
    Curva_mar3 = np.zeros((Dati_utenze.shape[1], ore_mese[2]))
    Curva_apr3 = np.zeros((Dati_utenze.shape[1], ore_mese[3]))
    Curva_mag3 = np.zeros((Dati_utenze.shape[1], ore_mese[4]))
    Curva_giu3 = np.zeros((Dati_utenze.shape[1], ore_mese[5]))
    Curva_lug3 = np.zeros((Dati_utenze.shape[1], ore_mese[6]))
    Curva_ago3 = np.zeros((Dati_utenze.shape[1], ore_mese[7]))
    Curva_set3 = np.zeros((Dati_utenze.shape[1], ore_mese[8]))
    Curva_ott3 = np.zeros((Dati_utenze.shape[1], ore_mese[9]))
    Curva_nov3 = np.zeros((Dati_utenze.shape[1], ore_mese[10]))
    Curva_dic3 = np.zeros((Dati_utenze.shape[1], ore_mese[11]))
    Curva_mensile = np.zeros((Dati_utenze.shape[1], 12))

    c_sist_trim = np.zeros((4, Dati_economici.shape[1]))
    c_sist = np.zeros((8760, Dati_economici.shape[1]))
    BTAU = np.zeros(4)
    Tras_e = np.zeros(4)
    P_r_mod = np.zeros(Dati_economici.shape[1])
    P_rF1 = np.zeros(Dati_economici.shape[1])
    P_rF2 = np.zeros(Dati_economici.shape[1])
    P_rF3 = np.zeros(Dati_economici.shape[1])
    c_uel_mod = np.zeros(Dati_economici.shape[1])
    Fee = np.zeros(Dati_economici.shape[1])
    c_uelF1 = np.zeros(Dati_economici.shape[1])
    c_uelF2 = np.zeros(Dati_economici.shape[1])
    c_uelF3 = np.zeros(Dati_economici.shape[1])
    c_uelF = np.zeros((8760, Dati_economici.shape[1]))
    Matrice_c_uel1 = np.zeros((8760, Dati_economici.shape[1]))
    Utenza = np.zeros(Dati_economici.shape[1])
    Lost_EE = np.zeros(Dati_economici.shape[1])
    Matrice_P_r = np.zeros((8761, Dati_economici.shape[1]))
    Matrice_c_uel = np.zeros((8761, Dati_economici.shape[1]))
    Matrice_econo = np.zeros((11, Dati_economici.shape[1]))

    ## ================
    # Inizio del calcolo Tecnici
    ## ================

    Matrice_G_t[0, :] = Dati_tecn[0, :]
    Matrice_Ta[0, :] = Dati_tecn[0, :]
    Matrice_tecn[0, :] = Dati_tecn[0, :]

    ## Recepimento dati da PVgis

    # Aumento il tempo di timeout per la connessione
    timeout = 30

    # Url API per PVgis
    url_base = 'https://re.jrc.ec.europa.eu/api/v5_2/seriescalc?lat={0}&lon={1}&angle={2}&aspect={3}&startyear={4}&endyear={5}&trackingtype={6}&raddatabase=PVGIS-SARAH2&outputformat=json'

    # Creazione curve orarie dell'irraggiamento
    for i in range(Dati_tecn.shape[1]):

        if Dati_tecn[7, i] == 2:
            for j in range(int(Dati_tecn[7, i])):

                # Dati della località
                lon = Dati_tecn[1, i]
                lat = Dati_tecn[2, i]

                # Dati di posizionamento del pannello
                beta = Dati_tecn[9+(j*3), i]                                       # Angolo di tilt della superficie [°]
                Z_s = Dati_tecn[10+(j*3), i]                                       # Angolo di azimut della superficie [°]

                # Modalità di inseguimento dei pannelli. 0:fisso, 2:two-axis, 3:vertical axis, 5:inclined axis
                tracking_map = {1:0, 2:2, 3:3, 4:5}
                tracking = tracking_map.get(Dati_tecn[6, i], 0)

                url = url_base.format(lat, lon, beta, Z_s, Start_y, End_y, tracking)
                response = requests.get(url)
                data = response.json()['outputs']['hourly']

                # Estrazione dati output temporanei
                G_i_temp[:, j] = [Dati_tecn[8+(j*3), i] * item['G(i)'] for item in data]  #[W/m2]
                T2m_temp[:, j] = [Dati_tecn[8+(j*3), i] * item['T2m'] for item in data]  #[°C]

            # Dati output finali (media delle potenze)
            G_i_tot[:, i] = np.sum(G_i_temp, axis=1) / Dati_tecn[3, i]             #[W/m2]
            T2m_tot[:, i] = np.sum(T2m_temp, axis=1) / Dati_tecn[3, i]             #[°C]

        else:
            # Dati della località
            lon = Dati_tecn[1, i]
            lat = Dati_tecn[2, i]

            # Dati di posizionamento del pannello
            beta = Dati_tecn[9, i]                                                 # Angolo di tilt della superficie [°]
            Z_s = Dati_tecn[10, i]                                                 # Angolo di azimut della superficie [°]

            # Modalità di inseguimento dei pannelli. 0:fisso, 2:two-axis, 3:vertical axis, 5:inclined axis
            tracking_map = {1: 0, 2: 2, 3: 3, 4: 5}
            #tracking = tracking_map.get(Dati_tecn[6, i]+1, 0) DA VERIFICARE CON DAVIDE
            tracking = tracking_map.get(3)
            url = url_base.format(lat, lon, beta, Z_s, Start_y, End_y, tracking)
            print(url)
            response = requests.get(url)
            print(response)
            data = response.json()['outputs']['hourly']
            print("OK")
            return jsonify(data)


            # Estrazione dati output
            G_i_tot[:, i] = [item['G(i)'] for item in data]                        #[W/m2]
            T2m_tot[:, i] = [item['T2m'] for item in data]                         #[°C]

    # Rimozione 29 febbraio anni bisestili dai dati output
    Date2 = np.repeat(Date1, 24)
    Date3 = (Date2.month == 2) & (Date2.day == 29)                                 # Indici in cui il vettore è il 29 febbraio
    G_i_tot = G_i_tot[~Date3, :]
    T2m_tot = T2m_tot[~Date3, :]

    for i in range(Dati_tecn.shape[1]):

        # Matrici per riordinare il vettore orario e facilitare il calcolo della media
        G_i_h = np.reshape(G_i_tot[:, i], (8760, -1), order='F')                   #[W/m2]
        T2m_h = np.reshape(T2m_tot[:, i], (8760, -1), order='F')                   #[°C]

        # Media dei vari anni per ottenere un vettore rappresentativo annuale
        Matrice_G_t[1:, i] = np.mean(G_i_h, axis=1)                                #[W/m2]
        Matrice_Ta[1:, i] = np.mean(T2m_h, axis=1)                                 #[°C]

    ## Dati tecnici

    # Costruzione matrice dei dati tecnici
    for i in range(Dati_tecn.shape[1]):

        Matrice_tecn[1, i] = Dati_tecn[3, i]                                       # Potenza totale impianto [kW]
        Matrice_tecn[2, i] = Dati_tecn[4, i]                                       # Potenza obbligo edifici nuovi [kW]
        Matrice_tecn[3, i] = Dati_tecn[5, i]                                       # Area totale impianto (superficie captante) [m2]
        Matrice_tecn[4, i] = Dati_tecn[14, i]                                      # Efficienza del pannello [-]
        Matrice_tecn[5, i] = Dati_tecn[15, i]                                      # Coefficiente di temperatura [%/K]
        Matrice_tecn[6, i] = Dati_tecn[16, i]                                      # NOCT [°C]
        Matrice_tecn[7, i] = Dati_tecn[17, i]                                      # Fattore di riduzione prestazioni [%]
        Matrice_tecn[8, i] = Dati_tecn[18, i]                                      # Efficienza dell'inverter [-]
        Matrice_tecn[9, i] = Dati_tecn[19, i]                                      # Potenza dell'inverter [kW]
        Matrice_tecn[10, i] = Dati_tecn[20, i]                                     # Capacità accumulo [kWh]
        Matrice_tecn[11, i] = Dati_tecn[21, i]                                     # Costo accumulo [€]
        Matrice_tecn[12, i] = Dati_tecn[22, i]                                     # Altre perdite [%]
        Matrice_tecn[13, i] = Dati_tecn[23, i].timestamp()                         # Data entrata in esercizio
        Matrice_tecn[14, i] = Dati_tecn[24, i]                                     # Modalità di inserimento del costo impianto [-]

        if Dati_tecn[24, i] == 1:
            Matrice_tecn[15, i] = Dati_tecn[25, i]                                 # Costo del pannello [€/kW]
        else:
            Matrice_tecn[15, i] = Dati_tecn[26, i]                                 # Costo dell'impianto [€]

        Matrice_tecn[16, i] = Dati_tecn[27, i]                                     # Esclusione dalla TP

    ## ================
    # Inizio del calcolo Utenze
    ## ================

    # Creazione curve orarie standard per tipologia di utenza
    for i in range(Dati_utenze.shape[1]):
        XX = 0
        # XX = Dati_utenze[6:8..., i]
        # Modulo da attivare quando saranno valide le funzioni per creare curve
        # di carico dettagliate. Sono i dati descrittivi di ogni cluster per
        # determinare le curve orarie e rappresentano gli input della funzione

        if Dati_utenze[4, i] == 1:                                                 # Residenziale
            FF = Curva_residenziale(XX)
        elif Dati_utenze[4, i] == 2:                                               # Utenze condominiali
            FF = Curva_ut_condominio(XX)
        elif Dati_utenze[4, i] == 3:                                               # Supermarket (7 su 7)
            FF = Curva_supermarket(XX)
        elif Dati_utenze[4, i] == 4:                                               # Industria
            FF = Curva_industria(XX)
        elif Dati_utenze[4, i] == 5:                                               # Ufficio
            FF = Curva_ufficio(XX)
        print(FF)

        Curva_gen[i, :] = np.tile(FF[0, :], 31) / 31
        Curva_feb[i, :] = np.tile(FF[1, :], 28) / 28
        Curva_mar[i, :] = np.tile(FF[2, :], 31) / 31
        Curva_apr[i, :] = np.tile(FF[3, :], 30) / 30
        Curva_mag[i, :] = np.tile(FF[4, :], 31) / 31
        Curva_giu[i, :] = np.tile(FF[5, :], 30) / 30
        Curva_lug[i, :] = np.tile(FF[6, :], 31) / 31
        Curva_ago[i, :] = np.tile(FF[7, :], 31) / 31
        Curva_set[i, :] = np.tile(FF[8, :], 30) / 30
        Curva_ott[i, :] = np.tile(FF[9, :], 31) / 31
        Curva_nov[i, :] = np.tile(FF[10, :], 30) / 30
        Curva_dic[i, :] = np.tile(FF[11, :], 31) / 31
        Curva_mensile[i, :] = FF[12, :12]


    for i in range(Dati_utenze.shape[1]):

        ## Inserimento energia annuale per valore totale

        if Dati_utenze[5, i] == 1 and Dati_utenze[6, i] == 0:
            Load_y[i] = Dati_utenze[7, i]                                          # Energia annuale [kWh]
            Load_m[i, 0:12] = Load_y[i] * Curva_mensile[i, :]                      # Energia mensile [kWh]

            # Curva di carico oraria per ogni tipologia di utenza
            Load_h[i, :] = np.concatenate((
                Curva_gen[i, :] * Load_m[i, 0], Curva_feb[i, :] * Load_m[i, 1], Curva_mar[i, :] * Load_m[i, 2],
                Curva_apr[i, :] * Load_m[i, 3], Curva_mag[i, :] * Load_m[i, 4], Curva_giu[i, :] * Load_m[i, 5],
                Curva_lug[i, :] * Load_m[i, 6], Curva_ago[i, :] * Load_m[i, 7], Curva_set[i, :] * Load_m[i, 8],
                Curva_ott[i, :] * Load_m[i, 9], Curva_nov[i, :] * Load_m[i, 10], Curva_dic[i, :] * Load_m[i, 11]
            ))

        ## Inserimento energia annuale per fasce

        elif Dati_utenze[5, i] == 1 and Dati_utenze[6, i] == 1:
            Load_y1[i] = Dati_utenze[8, i]                                         # Energia annuale fascia F1 [kWh]
            Load_y2[i] = Dati_utenze[9, i]                                         # Energia annuale fascia F2 [kWh]
            Load_y3[i] = Dati_utenze[10, i]                                        # Energia annuale fascia F3 [kWh]
            Load_y[i] = Load_y1[i] + Load_y2[i] + Load_y3[i]                       # Energia annuale [kWh]
            Load_m1[i, 0:12] = Load_y1[i] * Curva_mensile[i, :]                    # Energia mensile fascia F1 [kWh]
            Load_m2[i, 0:12] = Load_y2[i] * Curva_mensile[i, :]                    # Energia mensile fascia F2 [kWh]
            Load_m3[i, 0:12] = Load_y3[i] * Curva_mensile[i, :]                    # Energia mensile fascia F3 [kWh]
            Load_m[i, 0:12] = Load_m1[i, 0:12] + Load_m2[i, 0:12] + Load_m3[i, 0:12] # Energia mensile [kWh]

            # Curva di carico oraria per ogni tipologia di utenza suddivisa per fasce
            # F1
            Curva_gen1[i, :] = Curva_gen[i, :] * (Fasce[0:744] == 1) / np.sum(Curva_gen[i, :] * (Fasce[0:744] == 1))
            Curva_feb1[i, :] = Curva_feb[i, :] * (Fasce[744:1416] == 1) / np.sum(Curva_feb[i, :] * (Fasce[744:1416] == 1))
            Curva_mar1[i, :] = Curva_mar[i, :] * (Fasce[1416:2160] == 1) / np.sum(Curva_mar[i, :] * (Fasce[1416:2160] == 1))
            Curva_apr1[i, :] = Curva_apr[i, :] * (Fasce[2160:2880] == 1) / np.sum(Curva_apr[i, :] * (Fasce[2160:2880] == 1))
            Curva_mag1[i, :] = Curva_mag[i, :] * (Fasce[2880:3624] == 1) / np.sum(Curva_mag[i, :] * (Fasce[2880:3624] == 1))
            Curva_giu1[i, :] = Curva_giu[i, :] * (Fasce[3624:4344] == 1) / np.sum(Curva_giu[i, :] * (Fasce[3624:4344] == 1))
            Curva_lug1[i, :] = Curva_lug[i, :] * (Fasce[4344:5088] == 1) / np.sum(Curva_lug[i, :] * (Fasce[4344:5088] == 1))
            Curva_ago1[i, :] = Curva_ago[i, :] * (Fasce[5088:5832] == 1) / np.sum(Curva_ago[i, :] * (Fasce[5088:5832] == 1))
            Curva_set1[i, :] = Curva_set[i, :] * (Fasce[5832:6552] == 1) / np.sum(Curva_set[i, :] * (Fasce[5832:6552] == 1))
            Curva_ott1[i, :] = Curva_ott[i, :] * (Fasce[6552:7296] == 1) / np.sum(Curva_ott[i, :] * (Fasce[6552:7296] == 1))
            Curva_nov1[i, :] = Curva_nov[i, :] * (Fasce[7296:8016] == 1) / np.sum(Curva_nov[i, :] * (Fasce[7296:8016] == 1))
            Curva_dic1[i, :] = Curva_dic[i, :] * (Fasce[8016:8760] == 1) / np.sum(Curva_dic[i, :] * (Fasce[8016:8760] == 1))
            # F2
            Curva_gen2[i, :] = Curva_gen[i, :] * (Fasce[0:744] == 2) / np.sum(Curva_gen[i, :] * (Fasce[0:744] == 2))
            Curva_feb2[i, :] = Curva_feb[i, :] * (Fasce[744:1416] == 2) / np.sum(Curva_feb[i, :] * (Fasce[744:1416] == 2))
            Curva_mar2[i, :] = Curva_mar[i, :] * (Fasce[1416:2160] == 2) / np.sum(Curva_mar[i, :] * (Fasce[1416:2160] == 2))
            Curva_apr2[i, :] = Curva_apr[i, :] * (Fasce[2160:2880] == 2) / np.sum(Curva_apr[i, :] * (Fasce[2160:2880] == 2))
            Curva_mag2[i, :] = Curva_mag[i, :] * (Fasce[2880:3624] == 2) / np.sum(Curva_mag[i, :] * (Fasce[2880:3624] == 2))
            Curva_giu2[i, :] = Curva_giu[i, :] * (Fasce[3624:4344] == 2) / np.sum(Curva_giu[i, :] * (Fasce[3624:4344] == 2))
            Curva_lug2[i, :] = Curva_lug[i, :] * (Fasce[4344:5088] == 2) / np.sum(Curva_lug[i, :] * (Fasce[4344:5088] == 2))
            Curva_ago2[i, :] = Curva_ago[i, :] * (Fasce[5088:5832] == 2) / np.sum(Curva_ago[i, :] * (Fasce[5088:5832] == 2))
            Curva_set2[i, :] = Curva_set[i, :] * (Fasce[5832:6552] == 2) / np.sum(Curva_set[i, :] * (Fasce[5832:6552] == 2))
            Curva_ott2[i, :] = Curva_ott[i, :] * (Fasce[6552:7296] == 2) / np.sum(Curva_ott[i, :] * (Fasce[6552:7296] == 2))
            Curva_nov2[i, :] = Curva_nov[i, :] * (Fasce[7296:8016] == 2) / np.sum(Curva_nov[i, :] * (Fasce[7296:8016] == 2))
            Curva_dic2[i, :] = Curva_dic[i, :] * (Fasce[8016:8760] == 2) / np.sum(Curva_dic[i, :] * (Fasce[8016:8760] == 2))
            # F3
            Curva_gen3[i, :] = Curva_gen[i, :] * (Fasce[0:744] == 3) / np.sum(Curva_gen[i, :] * (Fasce[0:744] == 3))
            Curva_feb3[i, :] = Curva_feb[i, :] * (Fasce[744:1416] == 3) / np.sum(Curva_feb[i, :] * (Fasce[744:1416] == 3))
            Curva_mar3[i, :] = Curva_mar[i, :] * (Fasce[1416:2160] == 3) / np.sum(Curva_mar[i, :] * (Fasce[1416:2160] == 3))
            Curva_apr3[i, :] = Curva_apr[i, :] * (Fasce[2160:2880] == 3) / np.sum(Curva_apr[i, :] * (Fasce[2160:2880] == 3))
            Curva_mag3[i, :] = Curva_mag[i, :] * (Fasce[2880:3624] == 3) / np.sum(Curva_mag[i, :] * (Fasce[2880:3624] == 3))
            Curva_giu3[i, :] = Curva_giu[i, :] * (Fasce[3624:4344] == 3) / np.sum(Curva_giu[i, :] * (Fasce[3624:4344] == 3))
            Curva_lug3[i, :] = Curva_lug[i, :] * (Fasce[4344:5088] == 3) / np.sum(Curva_lug[i, :] * (Fasce[4344:5088] == 3))
            Curva_ago3[i, :] = Curva_ago[i, :] * (Fasce[5088:5832] == 3) / np.sum(Curva_ago[i, :] * (Fasce[5088:5832] == 3))
            Curva_set3[i, :] = Curva_set[i, :] * (Fasce[5832:6552] == 3) / np.sum(Curva_set[i, :] * (Fasce[5832:6552] == 3))
            Curva_ott3[i, :] = Curva_ott[i, :] * (Fasce[6552:7296] == 3) / np.sum(Curva_ott[i, :] * (Fasce[6552:7296] == 3))
            Curva_nov3[i, :] = Curva_nov[i, :] * (Fasce[7296:8016] == 3) / np.sum(Curva_nov[i, :] * (Fasce[7296:8016] == 3))
            Curva_dic3[i, :] = Curva_dic[i, :] * (Fasce[8016:8760] == 3) / np.sum(Curva_dic[i, :] * (Fasce[8016:8760] == 3))

            # Curva di carico oraria per fasce
            Load_h1[i, :] = np.concatenate((
                Curva_gen1[i, :] * Load_m1[i, 0], Curva_feb1[i, :] * Load_m1[i, 1], Curva_mar1[i, :] * Load_m1[i, 2],
                Curva_apr1[i, :] * Load_m1[i, 3], Curva_mag1[i, :] * Load_m1[i, 4], Curva_giu1[i, :] * Load_m1[i, 5],
                Curva_lug1[i, :] * Load_m1[i, 6], Curva_ago1[i, :] * Load_m1[i, 7], Curva_set1[i, :] * Load_m1[i, 8],
                Curva_ott1[i, :] * Load_m1[i, 9], Curva_nov1[i, :] * Load_m1[i, 10], Curva_dic1[i, :] * Load_m1[i, 11]
            ))
            Load_h2[i, :] = np.concatenate((
                Curva_gen2[i, :] * Load_m2[i, 0], Curva_feb2[i, :] * Load_m2[i, 1], Curva_mar2[i, :] * Load_m2[i, 2],
                Curva_apr2[i, :] * Load_m2[i, 3], Curva_mag2[i, :] * Load_m2[i, 4], Curva_giu2[i, :] * Load_m2[i, 5],
                Curva_lug2[i, :] * Load_m2[i, 6], Curva_ago2[i, :] * Load_m2[i, 7], Curva_set2[i, :] * Load_m2[i, 8],
                Curva_ott2[i, :] * Load_m2[i, 9], Curva_nov2[i, :] * Load_m2[i, 10], Curva_dic2[i, :] * Load_m2[i, 11]
            ))
            Load_h3[i, :] = np.concatenate((
                Curva_gen3[i, :] * Load_m3[i, 0], Curva_feb3[i, :] * Load_m3[i, 1], Curva_mar3[i, :] * Load_m3[i, 2],
                Curva_apr3[i, :] * Load_m3[i, 3], Curva_mag3[i, :] * Load_m3[i, 4], Curva_giu3[i, :] * Load_m3[i, 5],
                Curva_lug3[i, :] * Load_m3[i, 6], Curva_ago3[i, :] * Load_m3[i, 7], Curva_set3[i, :] * Load_m3[i, 8],
                Curva_ott3[i, :] * Load_m3[i, 9], Curva_nov3[i, :] * Load_m3[i, 10], Curva_dic3[i, :] * Load_m3[i, 11]
            ))

            # Curva di carico oraria
            Load_h[i, :] = Load_h1[i, :] + Load_h2[i, :] + Load_h3[i, :]


        ## Inserimento energia mensile per valore totale

        elif Dati_utenze[5, i] == 2 and Dati_utenze[6, i] == 0:
            Load_m[i, :] = Dati_utenze[11:23, i].T                                 # Energia mensile [kWh]
            Load_y[i] = np.sum(Load_m[i, :])                                       # Energia annuale [kWh]

            # Curva di carico oraria per ogni tipologia di utenza
            Load_h[i, :] = np.concatenate((
                Curva_gen[i, :] * Load_m[i, 0], Curva_feb[i, :] * Load_m[i, 1], Curva_mar[i, :] * Load_m[i, 2],
                Curva_apr[i, :] * Load_m[i, 3], Curva_mag[i, :] * Load_m[i, 4], Curva_giu[i, :] * Load_m[i, 5],
                Curva_lug[i, :] * Load_m[i, 6], Curva_ago[i, :] * Load_m[i, 7], Curva_set[i, :] * Load_m[i, 8],
                Curva_ott[i, :] * Load_m[i, 9], Curva_nov[i, :] * Load_m[i, 10], Curva_dic[i, :] * Load_m[i, 11]
            ))

        ## Inserimento energia mensile per fasce

        elif Dati_utenze[5, i] == 2 and Dati_utenze[6, i] == 1:
            Load_m1[i, 0:12] = Dati_utenze[23:35, i].T                             # Energia mensile fascia F1 [kWh]
            Load_m2[i, 0:12] = Dati_utenze[35:47, i].T                             # Energia mensile fascia F2 [kWh]
            Load_m3[i, 0:12] = Dati_utenze[47:59, i].T                             # Energia mensile fascia F3 [kWh]
            Load_m[i, 0:12] = Load_m1[i, 0:12] + Load_m2[i, 0:12] + Load_m3[i, 0:12] # Energia mensile [kWh]
            Load_y[i] = np.sum(Load_m[i, :])                                       # Energia annuale [kWh]

            # Curva di carico oraria per ogni tipologia di utenza suddivisa per fasce
            # F1
            Curva_gen1[i, :] = Curva_gen[i, :] * (Fasce[0:744] == 1) / np.sum(Curva_gen[i, :] * (Fasce[0:744] == 1))
            Curva_feb1[i, :] = Curva_feb[i, :] * (Fasce[744:1416] == 1) / np.sum(Curva_feb[i, :] * (Fasce[744:1416] == 1))
            Curva_mar1[i, :] = Curva_mar[i, :] * (Fasce[1416:2160] == 1) / np.sum(Curva_mar[i, :] * (Fasce[1416:2160] == 1))
            Curva_apr1[i, :] = Curva_apr[i, :] * (Fasce[2160:2880] == 1) / np.sum(Curva_apr[i, :] * (Fasce[2160:2880] == 1))
            Curva_mag1[i, :] = Curva_mag[i, :] * (Fasce[2880:3624] == 1) / np.sum(Curva_mag[i, :] * (Fasce[2880:3624] == 1))
            Curva_giu1[i, :] = Curva_giu[i, :] * (Fasce[3624:4344] == 1) / np.sum(Curva_giu[i, :] * (Fasce[3624:4344] == 1))
            Curva_lug1[i, :] = Curva_lug[i, :] * (Fasce[4344:5088] == 1) / np.sum(Curva_lug[i, :] * (Fasce[4344:5088] == 1))
            Curva_ago1[i, :] = Curva_ago[i, :] * (Fasce[5088:5832] == 1) / np.sum(Curva_ago[i, :] * (Fasce[5088:5832] == 1))
            Curva_set1[i, :] = Curva_set[i, :] * (Fasce[5832:6552] == 1) / np.sum(Curva_set[i, :] * (Fasce[5832:6552] == 1))
            Curva_ott1[i, :] = Curva_ott[i, :] * (Fasce[6552:7296] == 1) / np.sum(Curva_ott[i, :] * (Fasce[6552:7296] == 1))
            Curva_nov1[i, :] = Curva_nov[i, :] * (Fasce[7296:8016] == 1) / np.sum(Curva_nov[i, :] * (Fasce[7296:8016] == 1))
            Curva_dic1[i, :] = Curva_dic[i, :] * (Fasce[8016:8760] == 1) / np.sum(Curva_dic[i, :] * (Fasce[8016:8760] == 1))
            # F2
            Curva_gen2[i, :] = Curva_gen[i, :] * (Fasce[0:744] == 2) / np.sum(Curva_gen[i, :] * (Fasce[0:744] == 2))
            Curva_feb2[i, :] = Curva_feb[i, :] * (Fasce[744:1416] == 2) / np.sum(Curva_feb[i, :] * (Fasce[744:1416] == 2))
            Curva_mar2[i, :] = Curva_mar[i, :] * (Fasce[1416:2160] == 2) / np.sum(Curva_mar[i, :] * (Fasce[1416:2160] == 2))
            Curva_apr2[i, :] = Curva_apr[i, :] * (Fasce[2160:2880] == 2) / np.sum(Curva_apr[i, :] * (Fasce[2160:2880] == 2))
            Curva_mag2[i, :] = Curva_mag[i, :] * (Fasce[2880:3624] == 2) / np.sum(Curva_mag[i, :] * (Fasce[2880:3624] == 2))
            Curva_giu2[i, :] = Curva_giu[i, :] * (Fasce[3624:4344] == 2) / np.sum(Curva_giu[i, :] * (Fasce[3624:4344] == 2))
            Curva_lug2[i, :] = Curva_lug[i, :] * (Fasce[4344:5088] == 2) / np.sum(Curva_lug[i, :] * (Fasce[4344:5088] == 2))
            Curva_ago2[i, :] = Curva_ago[i, :] * (Fasce[5088:5832] == 2) / np.sum(Curva_ago[i, :] * (Fasce[5088:5832] == 2))
            Curva_set2[i, :] = Curva_set[i, :] * (Fasce[5832:6552] == 2) / np.sum(Curva_set[i, :] * (Fasce[5832:6552] == 2))
            Curva_ott2[i, :] = Curva_ott[i, :] * (Fasce[6552:7296] == 2) / np.sum(Curva_ott[i, :] * (Fasce[6552:7296] == 2))
            Curva_nov2[i, :] = Curva_nov[i, :] * (Fasce[7296:8016] == 2) / np.sum(Curva_nov[i, :] * (Fasce[7296:8016] == 2))
            Curva_dic2[i, :] = Curva_dic[i, :] * (Fasce[8016:8760] == 2) / np.sum(Curva_dic[i, :] * (Fasce[8016:8760] == 2))
            # F3
            Curva_gen3[i, :] = Curva_gen[i, :] * (Fasce[0:744] == 3) / np.sum(Curva_gen[i, :] * (Fasce[0:744] == 3))
            Curva_feb3[i, :] = Curva_feb[i, :] * (Fasce[744:1416] == 3) / np.sum(Curva_feb[i, :] * (Fasce[744:1416] == 3))
            Curva_mar3[i, :] = Curva_mar[i, :] * (Fasce[1416:2160] == 3) / np.sum(Curva_mar[i, :] * (Fasce[1416:2160] == 3))
            Curva_apr3[i, :] = Curva_apr[i, :] * (Fasce[2160:2880] == 3) / np.sum(Curva_apr[i, :] * (Fasce[2160:2880] == 3))
            Curva_mag3[i, :] = Curva_mag[i, :] * (Fasce[2880:3624] == 3) / np.sum(Curva_mag[i, :] * (Fasce[2880:3624] == 3))
            Curva_giu3[i, :] = Curva_giu[i, :] * (Fasce[3624:4344] == 3) / np.sum(Curva_giu[i, :] * (Fasce[3624:4344] == 3))
            Curva_lug3[i, :] = Curva_lug[i, :] * (Fasce[4344:5088] == 3) / np.sum(Curva_lug[i, :] * (Fasce[4344:5088] == 3))
            Curva_ago3[i, :] = Curva_ago[i, :] * (Fasce[5088:5832] == 3) / np.sum(Curva_ago[i, :] * (Fasce[5088:5832] == 3))
            Curva_set3[i, :] = Curva_set[i, :] * (Fasce[5832:6552] == 3) / np.sum(Curva_set[i, :] * (Fasce[5832:6552] == 3))
            Curva_ott3[i, :] = Curva_ott[i, :] * (Fasce[6552:7296] == 3) / np.sum(Curva_ott[i, :] * (Fasce[6552:7296] == 3))
            Curva_nov3[i, :] = Curva_nov[i, :] * (Fasce[7296:8016] == 3) / np.sum(Curva_nov[i, :] * (Fasce[7296:8016] == 3))
            Curva_dic3[i, :] = Curva_dic[i, :] * (Fasce[8016:8760] == 3) / np.sum(Curva_dic[i, :] * (Fasce[8016:8760] == 3))

            # Curva di carico oraria per fasce
            Load_h1[i, :] = np.concatenate((
                Curva_gen1[i, :] * Load_m1[i, 0], Curva_feb1[i, :] * Load_m1[i, 1], Curva_mar1[i, :] * Load_m1[i, 2],
                Curva_apr1[i, :] * Load_m1[i, 3], Curva_mag1[i, :] * Load_m1[i, 4], Curva_giu1[i, :] * Load_m1[i, 5],
                Curva_lug1[i, :] * Load_m1[i, 6], Curva_ago1[i, :] * Load_m1[i, 7], Curva_set1[i, :] * Load_m1[i, 8],
                Curva_ott1[i, :] * Load_m1[i, 9], Curva_nov1[i, :] * Load_m1[i, 10], Curva_dic1[i, :] * Load_m1[i, 11]
            ))
            Load_h2[i, :] = np.concatenate((
                Curva_gen2[i, :] * Load_m2[i, 0], Curva_feb2[i, :] * Load_m2[i, 1], Curva_mar2[i, :] * Load_m2[i, 2],
                Curva_apr2[i, :] * Load_m2[i, 3], Curva_mag2[i, :] * Load_m2[i, 4], Curva_giu2[i, :] * Load_m2[i, 5],
                Curva_lug2[i, :] * Load_m2[i, 6], Curva_ago2[i, :] * Load_m2[i, 7], Curva_set2[i, :] * Load_m2[i, 8],
                Curva_ott2[i, :] * Load_m2[i, 9], Curva_nov2[i, :] * Load_m2[i, 10], Curva_dic2[i, :] * Load_m2[i, 11]
            ))
            Load_h3[i, :] = np.concatenate((
                Curva_gen3[i, :] * Load_m3[i, 0], Curva_feb3[i, :] * Load_m3[i, 1], Curva_mar3[i, :] * Load_m3[i, 2],
                Curva_apr3[i, :] * Load_m3[i, 3], Curva_mag3[i, :] * Load_m3[i, 4], Curva_giu3[i, :] * Load_m3[i, 5],
                Curva_lug3[i, :] * Load_m3[i, 6], Curva_ago3[i, :] * Load_m3[i, 7], Curva_set3[i, :] * Load_m3[i, 8],
                Curva_ott3[i, :] * Load_m3[i, 9], Curva_nov3[i, :] * Load_m3[i, 10], Curva_dic3[i, :] * Load_m3[i, 11]
            ))

            # Curva di carico oraria
            Load_h[i, :] = Load_h1[i, :] + Load_h2[i, :] + Load_h3[i, :]

        ## Inserimento dell'energia oraria
        elif Dati_utenze[5,i] == 3:

            # Curva di carico oraria per ogni tipologia di utenza
            Load_h[i,:] = Dati_utenze[59:8819,i]                                   # Curva oraria [kWh]

            Load_m[i, 0] = sum(Load_h[i, 0:744])                                   # Energia mensile [kWh]
            Load_m[i, 1] = sum(Load_h[i, 744:1416])
            Load_m[i, 2] = sum(Load_h[i, 1416:2160])
            Load_m[i, 3] = sum(Load_h[i, 2160:2880])
            Load_m[i, 4] = sum(Load_h[i, 2880:3624])
            Load_m[i, 5] = sum(Load_h[i, 3624:4344])
            Load_m[i, 6] = sum(Load_h[i, 4344:5088])
            Load_m[i, 7] = sum(Load_h[i, 5088:5832])
            Load_m[i, 8] = sum(Load_h[i, 5832:6552])
            Load_m[i, 9] = sum(Load_h[i, 6552:7296])
            Load_m[i, 10] = sum(Load_h[i, 7296:8016])
            Load_m[i, 11] = sum(Load_h[i, 8016:8760])
            Load_y[i] = sum(Load_m[i,:])                                           # Energia annuale [kWh]

        # Ciclo per creare n righe per le n utenze
        while k <= sum(Dati_utenze[3, 0:i+1]):
            Matrice_El_h[k-1, 0] = Dati_utenze[0, i]                               # Matrice delle curve orarie di ogni singola utenza
            Matrice_El_h[k-1, 1] = Dati_utenze[1, i]
            Matrice_El_h[k-1, 2] = Dati_utenze[2, i]
            Matrice_El_h[k-1, 3:8763] = Load_h[i, :]
            k = k+1

     # Trasposizione della matrice delle curve orarie
    Matrice_El_h = Matrice_El_h.T

    ## ================
    # Inizio del calcolo Economici
    ## ================

    Matrice_P_r[0, :] = Dati_economici[0, :]
    Matrice_c_uel[0, :] = Dati_economici[0, :]
    Matrice_econo[0, :] = Dati_economici[0, :]

    ## Calcolo vendita energia

    for i in range(Dati_economici.shape[1]):
        P_r_mod[i] = Dati_economici[1, i]                                          # Modalità inserimento vendita
        P_rF1[i] = Dati_economici[2, i]                                            # Prezzo in F1 [€/kWh]
        P_rF2[i] = Dati_economici[3, i]                                            # Prezzo in F2 [€/kWh]
        P_rF3[i] = Dati_economici[4, i]                                            # Prezzo in F3 [€/kWh]

        if P_r_mod[i] == 1:
            if PUN_mod == 0:
                Matrice_P_r[1:8761, i] = Dati_sist_PUN[:, (Anno-2019)*8+Zona] * 100 / 1000 #[c€/kWh]
            else:
                Matrice_P_r[1:8761, i] = PUN                                       #[c€/kWh]

        elif P_r_mod[i] == 2:
            indice1 = np.where(Dati_sist_Fasce[:, Anno-2019] == 1)[0]
            Matrice_P_r[indice1+1, i] = P_rF1[i] * 100                             #[c€/kWh]

            indice2 = np.where(Dati_sist_Fasce[:, Anno-2019] == 2)[0]
            Matrice_P_r[indice2+1, i] = P_rF2[i] * 100                             #[c€/kWh]

            indice3 = np.where(Dati_sist_Fasce[:, Anno-2019] == 3)[0]
            Matrice_P_r[indice3+1, i] = P_rF3[i] * 100                             #[c€/kWh]

    ## Costo energia elettrica

    for i in range(Dati_economici.shape[1]):
        c_uel_mod[i] = Dati_economici[5, i]                                        # Modalità inserimento acquisto
        Fee[i] = Dati_economici[6, i]                                              # Spread sull'acquisto [€/kWh]
        c_uelF1[i] = Dati_economici[7, i]                                          # Costo in F1 [€/kWh]
        c_uelF2[i] = Dati_economici[8, i]                                          # Costo in F2 [€/kWh]
        c_uelF3[i] = Dati_economici[9, i]                                          # Costo in F3 [€/kWh]
        Utenza[i] = Dati_economici[10, i]                                          # Utenza elettrica

        if Dati_economici[10, i] in [1, 2, 3, 4, 5]:
            Lost_EE[i] = 1.1                                                       # Fattore moltiplicativo dovuto alle perdite
        else:
            Lost_EE[i] = 1.038

        # Calcolo oneri di sistema e componenti trasporto, distribuzione, misura
        for j in range(4):
            c_sist_trim[j, i] = np.sum(Dati_sist_Oneri[(int(Utenza[i])-1)*4+j, ((Anno-2019)*7):((Anno-2019)*7+7)]) #[c€/KkWh]

        c_sist[:2160, i] = c_sist_trim[0, i]
        c_sist[2160:4344, i] = c_sist_trim[1, i]
        c_sist[4344:6552, i] = c_sist_trim[2, i]
        c_sist[6552:8760, i] = c_sist_trim[3, i]

        # Costo dell'energia si basa sul PUN = PUN*1.1 + Spread + Oneri di sistema
        if c_uel_mod[i] == 1:
            Matrice_c_uel1[0:8760, i] = PUN * Lost_EE[i] + Fee[i] * 100 + c_sist[:, i] #[c€/kWh]

            if Utenza[i] == 1:
                Matrice_c_uel[1:8761, i] = Matrice_c_uel1[0:8760, i] * 1.1
            else:
                Matrice_c_uel[1:8761, i] = Matrice_c_uel1[0:8760, i] * 1.22

        # Costo dell'energia = costo indicato dell'utente e diviso per fasce + Spread
        elif c_uel_mod[i] == 2:
            indice1 = np.where(Dati_sist_Fasce[:, Anno-2019] == 1)
            c_uelF[indice1, i] = c_uelF1[i] * 100                                  #[c€/kWh]

            indice2 = np.where(Dati_sist_Fasce[:, Anno-2019] == 2)
            c_uelF[indice2, i] = c_uelF2[i] * 100                                  #[c€/kWh]

            indice3 = np.where(Dati_sist_Fasce[:, Anno-2019] == 3)
            c_uelF[indice3, i] = c_uelF3[i] * 100                                  #[c€/kWh]

            Matrice_c_uel[1:8761, i] = c_uelF[:, i] + Fee[i] * 100                 #[c€/kWh]

    ## Tariffe di autoconsumo

    # Componente variabile di distribuzione per le utenze per altri usi in bassa tensione BTAU
    for i in range(4):
        BTAU[i] = max(Dati_sist_Oneri[12+i, (Anno-2019)*7+1], Dati_sist_Oneri[16+i, (Anno-2019)*7+1]) #[c€/kWh]

    Matrice_BTAU = np.zeros(8760)
    Matrice_BTAU[:2160] = BTAU[0]
    Matrice_BTAU[2160:4344] = BTAU[1]
    Matrice_BTAU[4344:6552] = BTAU[2]
    Matrice_BTAU[6552:8760] = BTAU[3]

    # Tariffa di trasmissione Tras_e
    for i in range(4):
        Tras_e[i] = Dati_sist_Oneri[12+i, (Anno-2019)*7]                           #[c€/kWh]

    Matrice_Tras_e = np.zeros(8760)
    Matrice_Tras_e[:2160] = Tras_e[0]
    Matrice_Tras_e[2160:4344] = Tras_e[1]
    Matrice_Tras_e[4344:6552] = Tras_e[2]
    Matrice_Tras_e[6552:8760] = Tras_e[3]

    ## Altri dati

    # Coefficiente delle perdite evitate c_pr
    for i in range(Dati_economici.shape[1]):
        if Utenza[i] in [1, 2, 3, 4, 5]:
            Matrice_econo[1, i] = 2.6 / 100                                        #[%]
        else:
            Matrice_econo[1, i] = 1.2 / 100                                        #[%]

    # Altre spese
    for i in range(Dati_economici.shape[1]):
        Matrice_econo[2, i] = Dati_economici[11, i]                                #[€/anno]
        Matrice_econo[3, i] = Dati_economici[12, i]                                #[€/anno]
        Matrice_econo[4, i] = Dati_economici[13, i]                                #[€/anno]

    # Fa il ritiro dedicato? 1:si, 2:no
    for i in range(Dati_economici.shape[1]):
        if P_r_mod[i] == 1:
            Matrice_econo[5, i] = 1
        elif P_r_mod[i] == 2:
            Matrice_econo[5, i] = 0

    # Incentivi e finanziamenti
    for i in range(Dati_economici.shape[1]):
        Matrice_econo[6, i] = Dati_economici[14, i]                                # Fa detrazione 50%?
        Matrice_econo[7, i] = Dati_economici[15, i]                                # Valore finanziamento [€]
        Matrice_econo[8, i] = Dati_economici[16, i]                                # Interesse finanziamento [%]
        Matrice_econo[9, i] = Dati_economici[17, i]                                # Durata finanziamento [anni]
        Matrice_econo[10, i] = Dati_economici[18, i]                               # Conto capitale 40% [%]


    ## ================
    # Traduttore Input
    ## ================

    Numero_di_cluster = np.unique(Matrice_El_h[0,:])
    Prosumer = np.zeros(len(Numero_di_cluster))

    k = 0
    while k < len(Numero_di_cluster):
        index = np.where(Matrice_El_h[0,:] == Numero_di_cluster[k])[0][0]
        Prosumer[k] = Matrice_El_h[1,index]
        k += 1

    Input = {}

    for i in Numero_di_cluster:

        Sottocampo = f'Cluster_{int(i)}'

        mask = Matrice_El_h[0,:] == i

        Input[Sottocampo] = {}
        Input[Sottocampo]['El_h'] = Matrice_El_h[3:, mask]                         #[kWh/h]
        Input[Sottocampo]['Utenza'] = int(np.unique(Matrice_El_h[2, mask]))        #[kWh/h]

        if Prosumer[int(i)-1] == 1:

            Input[Sottocampo]['G_t'] = Matrice_G_t[1:, np.where(Matrice_G_t[0,:] == i)[0]] #[W/m2]
            Input[Sottocampo]['Ta'] = Matrice_Ta[1:, np.where(Matrice_Ta[0,:] == i)[0]] #[°C]
            Input[Sottocampo]['Tecnici'] = Matrice_tecn[1:, np.where(Matrice_tecn[0,:] == i)[0]]
            Input[Sottocampo]['P_r'] = Matrice_P_r[1:, np.where(Matrice_P_r[0,:] == i)[0]] #[c€/kWh]
            Input[Sottocampo]['c_uel'] = Matrice_c_uel[1:, np.where(Matrice_c_uel[0,:] == i)[0]] #[c€/kWh]

            Sottomatrice_econo = Matrice_econo[1:, np.where(Matrice_econo[0,:] == i)[0]]
            Input[Sottocampo]['c_pr'] = Sottomatrice_econo[0]                      #[%]
            Input[Sottocampo]['I_altro'] = Sottomatrice_econo[1]                   #[€]
            Input[Sottocampo]['C_altro'] = Sottomatrice_econo[2]                   #[€]
            Input[Sottocampo]['C_man'] = Sottomatrice_econo[3]                     #[€]
            Input[Sottocampo]['RID'] = Sottomatrice_econo[4]                       #Vende con RID?
            Input[Sottocampo]['f_50'] = Sottomatrice_econo[5]                      #Fa detrazione 50%?
            Input[Sottocampo]['f_fin'] = Sottomatrice_econo[6]                     #Valore finanziamento [€]
            Input[Sottocampo]['i_fin'] = Sottomatrice_econo[7]                     #Interesse finanziamento [%]
            Input[Sottocampo]['a_fin'] = Sottomatrice_econo[8]                     #Durata finanziamento [anni]
            Input[Sottocampo]['i_40'] = Sottomatrice_econo[9]                      #Conto capitale 40% [%]

            Input[Sottocampo]['Prosumer'] = 1

        else:

            Input[Sottocampo]['Prosumer'] = 0

    Input['R'] = tasso_inflazione                                                  #[%]
    Input['Ri'] = tasso_interesse                                                  #[%]
    Input['BTAU'] = Matrice_BTAU                                                   #[c€/kWh]
    Input['Tras_e'] = Matrice_Tras_e                                               #[c€/kWh]
    Input['N_clust'] = Numero_di_cluster
    Input['Configurazione'] = configurazione                                    # 1:comunità energetica, 2:autoconsumo collettivo
    Input['Zona'] = zona_di_mercato
    Input['Zona_it'] = zona_geografica
    Input['N_part'] = Numero_di_utenze
    Input['N_part'] = Numero_di_utenze

    if PUN_mod == 0:
        Input['Pz'] = Dati_sist_PUN[:,(Anno-2019)*8+Zona]*100/1000                 #[c€/kWh]
    elif PUN_mod == 1 or PUN_mod == 2:
        Input['Pz'] = PUN                                                          #[c€/kWh]


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5001)
    receive_data()