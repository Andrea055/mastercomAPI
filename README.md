# Mastercom API

IT: 

API del registro elettronico Mastercom create con web scraping.

L'endpoint login esegue la query verso la pagina login usando password_user e user, questo endpoint restituisce uno user_id e un JWT nella pagina HTML, questi vengono presi e restituiti all'utente perchè utilizzati negli altri endpoint.

Il routing di tutte le altre pagine del registro viene regolato da una query in index.php specificando stato_principale=NOME_PAGINA, le informazioni prese con HTML scraping.

Specifica openAPI: 

EN:

Mastercom electronic ledger API created with web scraping.

The login endpoint queries the login page using password_user and user, this endpoint returns a user_id and a JWT in the HTML page, these are taken and returned to the user because they are used in the other endpoints.

The routing of all other registry pages is governed by a query in index.php specifying main_status=PAGE_NAME, the information taken with HTML scraping.

openAPI specification: 