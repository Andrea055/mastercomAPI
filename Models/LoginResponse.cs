namespace mastercom_api.Models {
    public class LoginResponse {
        public int currentUser { get; set; } = 0;
        public string currentKey { get; set; } = "";
        public string dbKey { get; set; }  = "";
        public string tipoUtente { get; set; }  = "";
    }
}