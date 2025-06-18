import numpy as np

def Curva_residenziale(XX):
    
    Curva_gen_d = np.array([0.875, 0.750, 0.700, 0.675, 0.675, 0.700, 0.850, 1.075, 1.125, 1.150, 1.150, 1.125, 1.175, 1.175, 1.175, 1.150, 1.175, 1.300, 1.450, 1.550, 1.550, 1.450, 1.275, 1.075])
    Curva_gen_d = Curva_gen_d / np.sum(Curva_gen_d)
    
    Curva_lug_d = np.array([1.025, 0.900, 0.825, 0.800, 0.775, 0.775, 0.825, 0.925, 1.000, 1.050, 1.075, 1.100, 1.175, 1.250, 1.250, 1.200, 1.175, 1.175, 1.225, 1.350, 1.450, 1.475, 1.350, 1.200])
    Curva_lug_d = Curva_lug_d / np.sum(Curva_lug_d)
    
    Curva_feb_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) / 6
    Curva_mar_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 2 / 6
    Curva_apr_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 3 / 6
    Curva_mag_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 4 / 6
    Curva_giu_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 5 / 6
    Curva_ago_d = Curva_giu_d
    Curva_set_d = Curva_mag_d
    Curva_ott_d = Curva_apr_d
    Curva_nov_d = Curva_mar_d
    Curva_dic_d = Curva_feb_d

    # Calcolo aliquote mensili, ancora non sviluppato
    Curva_mens = np.array([1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1])
    Curva_mens = Curva_mens / np.sum(Curva_mens)

    # Risultati
    F = np.zeros((13, 24))
    F[0, :] = Curva_gen_d
    F[1, :] = Curva_feb_d
    F[2, :] = Curva_mar_d
    F[3, :] = Curva_apr_d
    F[4, :] = Curva_mag_d
    F[5, :] = Curva_giu_d
    F[6, :] = Curva_lug_d
    F[7, :] = Curva_ago_d
    F[8, :] = Curva_set_d
    F[9, :] = Curva_ott_d
    F[10, :] = Curva_nov_d
    F[11, :] = Curva_dic_d
    F[12, :12] = Curva_mens

    return F

def Curva_ut_condominio(XX):
    # Calcolo, ancora non sviluppato
    Curva_gen_d = np.array([1.1, 1.2, 1.1, 1, 0.9, 0.97, 1, 1.075, 2, 2.3, 2.1, 2.3, 2.1, 1.97, 2.1, 2.04, 2.08, 1.8, 1.95, 2.3, 2.1, 2.1, 2.05, 1.5])
    Curva_gen_d = Curva_gen_d / np.sum(Curva_gen_d)
    
    Curva_lug_d = Curva_gen_d
    Curva_feb_d = Curva_gen_d
    Curva_mar_d = Curva_gen_d
    Curva_apr_d = Curva_gen_d
    Curva_mag_d = Curva_gen_d
    Curva_giu_d = Curva_gen_d
    Curva_ago_d = Curva_gen_d
    Curva_set_d = Curva_gen_d
    Curva_ott_d = Curva_gen_d
    Curva_nov_d = Curva_gen_d
    Curva_dic_d = Curva_gen_d

    # Calcolo aliquote mensili, ancora non sviluppato
    Curva_mens = np.array([1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1])
    Curva_mens = Curva_mens / np.sum(Curva_mens)

    # Risultati
    F = np.zeros((13, 24))
    F[0, :] = Curva_gen_d
    F[1, :] = Curva_feb_d
    F[2, :] = Curva_mar_d
    F[3, :] = Curva_apr_d
    F[4, :] = Curva_mag_d
    F[5, :] = Curva_giu_d
    F[6, :] = Curva_lug_d
    F[7, :] = Curva_ago_d
    F[8, :] = Curva_set_d
    F[9, :] = Curva_ott_d
    F[10, :] = Curva_nov_d
    F[11, :] = Curva_dic_d
    F[12, :12] = Curva_mens

    return F

def Curva_supermarket(XX):
    # Calcolo, ancora non sviluppato
    Curva_gen_d = np.array([0.027, 0.026, 0.026, 0.028, 0.026, 0.026, 0.031, 0.045, 0.051, 0.051, 0.049, 0.050, 0.051, 0.053, 0.051, 0.050, 0.052, 0.055, 0.055, 0.055, 0.047, 0.034, 0.031, 0.030])
    Curva_gen_d = Curva_gen_d / np.sum(Curva_gen_d)
    
    Curva_lug_d = np.array([0.031, 0.030, 0.030, 0.030, 0.030, 0.028, 0.029, 0.041, 0.047, 0.047, 0.047, 0.047, 0.048, 0.047, 0.047, 0.047, 0.050, 0.052, 0.056, 0.057, 0.055, 0.037, 0.035, 0.034])
    Curva_lug_d = Curva_lug_d / np.sum(Curva_lug_d)
    
    Curva_feb_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) / 6
    Curva_mar_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 2 / 6
    Curva_apr_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 3 / 6
    Curva_mag_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 4 / 6
    Curva_giu_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 5 / 6
    Curva_ago_d = Curva_giu_d
    Curva_set_d = Curva_mag_d
    Curva_ott_d = Curva_apr_d
    Curva_nov_d = Curva_mar_d
    Curva_dic_d = Curva_feb_d

    # Calcolo aliquote mensili, ancora non sviluppato
    Curva_mens = np.array([1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1])
    Curva_mens = Curva_mens / np.sum(Curva_mens)

    # Risultati
    F = np.zeros((13, 24))
    F[0, :] = Curva_gen_d
    F[1, :] = Curva_feb_d
    F[2, :] = Curva_mar_d
    F[3, :] = Curva_apr_d
    F[4, :] = Curva_mag_d
    F[5, :] = Curva_giu_d
    F[6, :] = Curva_lug_d
    F[7, :] = Curva_ago_d
    F[8, :] = Curva_set_d
    F[9, :] = Curva_ott_d
    F[10, :] = Curva_nov_d
    F[11, :] = Curva_dic_d
    F[12, :12] = Curva_mens

    return F


def Curva_industria(XX):
    # Calcolo curve orarie, ancora non sviluppato
    Curva_gen_d = np.array([0.027, 0.026, 0.026, 0.028, 0.026, 0.026, 0.031, 0.045, 0.051, 0.051, 0.049, 0.050, 0.051, 0.053, 0.051, 0.050, 0.052, 0.055, 0.055, 0.055, 0.047, 0.034, 0.031, 0.030])
    Curva_gen_d = Curva_gen_d / np.sum(Curva_gen_d)
    
    Curva_lug_d = np.array([0.031, 0.030, 0.030, 0.030, 0.030, 0.028, 0.029, 0.041, 0.047, 0.047, 0.047, 0.047, 0.048, 0.047, 0.047, 0.047, 0.050, 0.052, 0.056, 0.057, 0.055, 0.037, 0.035, 0.034])
    Curva_lug_d = Curva_lug_d / np.sum(Curva_lug_d)
    
    Curva_feb_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) / 6
    Curva_mar_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 2 / 6
    Curva_apr_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 3 / 6
    Curva_mag_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 4 / 6
    Curva_giu_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 5 / 6
    Curva_ago_d = Curva_giu_d
    Curva_set_d = Curva_mag_d
    Curva_ott_d = Curva_apr_d
    Curva_nov_d = Curva_mar_d
    Curva_dic_d = Curva_feb_d

    # Calcolo aliquote mensili, ancora non sviluppato
    Curva_mens = np.array([1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1])
    Curva_mens = Curva_mens / np.sum(Curva_mens)

    # Risultati
    F = np.zeros((13, 24))
    F[0, :] = Curva_gen_d
    F[1, :] = Curva_feb_d
    F[2, :] = Curva_mar_d
    F[3, :] = Curva_apr_d
    F[4, :] = Curva_mag_d
    F[5, :] = Curva_giu_d
    F[6, :] = Curva_lug_d
    F[7, :] = Curva_ago_d
    F[8, :] = Curva_set_d
    F[9, :] = Curva_ott_d
    F[10, :] = Curva_nov_d
    F[11, :] = Curva_dic_d
    F[12, :12] = Curva_mens

    return F


def Curva_ufficio(XX):
    # Calcolo curve orarie, ancora non sviluppato
    Curva_gen_d = np.array([1.1, 1.12, 1.12, 1.13, 1.44, 2, 3, 4.25, 6.5, 8, 9.5, 9.4, 9.1, 9.6, 10, 9.3, 7, 5, 3.5, 2.5, 2, 1.11, 1.09, 1.08])
    Curva_gen_d = Curva_gen_d / np.sum(Curva_gen_d)
    
    Curva_lug_d = np.array([1.1, 1.12, 1.12, 1.13, 1.44, 2, 3, 4.25, 6.5, 8, 9.5, 9.4, 9.1, 9.6, 10, 9.3, 7, 5, 3.5, 2.5, 2, 1.11, 1.09, 1.08])
    Curva_lug_d = Curva_lug_d / np.sum(Curva_lug_d)
    
    Curva_feb_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) / 6
    Curva_mar_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 2 / 6
    Curva_apr_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 3 / 6
    Curva_mag_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 4 / 6
    Curva_giu_d = Curva_gen_d + (Curva_lug_d - Curva_gen_d) * 5 / 6
    Curva_ago_d = Curva_giu_d
    Curva_set_d = Curva_mag_d
    Curva_ott_d = Curva_apr_d
    Curva_nov_d = Curva_mar_d
    Curva_dic_d = Curva_feb_d

    # Calcolo aliquote mensili, ancora non sviluppato
    Curva_mens = np.array([1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1])
    Curva_mens = Curva_mens / np.sum(Curva_mens)

    # Risultati
    F = np.zeros((13, 24))
    F[0, :] = Curva_gen_d
    F[1, :] = Curva_feb_d
    F[2, :] = Curva_mar_d
    F[3, :] = Curva_apr_d
    F[4, :] = Curva_mag_d
    F[5, :] = Curva_giu_d
    F[6, :] = Curva_lug_d
    F[7, :] = Curva_ago_d
    F[8, :] = Curva_set_d
    F[9, :] = Curva_ott_d
    F[10, :] = Curva_nov_d
    F[11, :] = Curva_dic_d
    F[12, :12] = Curva_mens

    return F

