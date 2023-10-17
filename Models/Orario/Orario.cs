public class Orario
{
    /*
        {
            "id_orario":"603053",
            "id_professore":"3000167",
            "id_classe":"1000278",
            "id_materia":"1000073",
            "id_locazione":"-1",
            "data_inizio":"1694499301",
            "data_fine":"1694502600",
            "calcola_assenze":"y",
            "id_progetto_alternanza":"-1",
            "cognome_professore":"BUSSOLINO",
            "nome_professore":"UGO GIUSEPPE",
            "nome_materia_sito":"ANIMAZIONE",
            "descrizione":"ANIMAZIONE",
            "tipo_materia":"NORMALE",
            "descrizione_materia":"ANIMAZIONE",
            "data_inizio_tradotta":"12\/09\/2023",
            "data_inizio_tradotta_iso":"2023-09-12",
            "ora_inizio_tradotta":"08:15",
            "data_fine_tradotta":"12\/09\/2023",
            "data_fine_tradotta_iso":"2023-09-12",
            "inizio_scheduler":"2023-09-12T08:15:01+02:00",
            "fine_scheduler":"2023-09-12T09:10:00+02:00",
            "ora_fine_tradotta":"09:10",
            "giorno_tradotto":"Marted\u00ec"
        }
    */
        public string id_orario { get; set; } = "";
        public string id_professore { get; set; } = "";
        public string id_classe { get; set; } = "";
        public string id_materia { get; set; } = "";
        public string id_locazione { get; set; } = "";
        public string data_inizio { get; set; } = "";
        public string data_fine { get; set; } = "";
        public string calcola_assenze { get; set; } = "";
        public string id_progetto_alternanza { get; set; } = "";
        public string cognome_professore { get; set; } = "";
        public string nome_professore { get; set; } = "";
        public string nome_materia_sito { get; set; } = "";
        public string descrizione { get; set; } = "";
        public string tipo_materia { get; set; } = "";
        public string descrizione_materia { get; set; } = "";
        public string data_inizio_tradotta { get; set; } = "";
        public string data_inizio_tradotta_iso { get; set; } = "";
        public string ora_inizio_tradotta { get; set; } = "";
        public string data_fine_tradotta { get; set; } = "";
        public string data_fine_tradotta_iso { get; set; } = "";
        public DateTime inizio_scheduler { get; set; }
        public DateTime fine_scheduler { get; set; }
        public string ora_fine_tradotta { get; set; } = "";
        public string giorno_tradotto { get; set; } = "";
}