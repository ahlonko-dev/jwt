# Mise en place de JWT

Dans le fichier Startup.cs, il faut ajouter dans le ConfigureServices: services.AddCors()
et dans le Configure: app.UseCors 

Dans le repertoire Helpers, il y a 3 classes à ajouter:
- AppSettings.cs: permet de définir la classe appsettings qui contient le "secret string" qui sera utilisée pour la génération du token.
- AuthorizeAttributes.cs: hérite de la classe Attribute, methode OnAuthorization qui valide l'authentification, AspNetCore.Mvc.Filters permet l'utilisation du context,
  ce context (HttpContext) contient entre autre les infos sur le User.
- JwtMiddleware.cs: on passe dans le constructeur de cette classe comme parametre, entre autre, l'appSettings qui contient la "secret phrase"
  qui peut être modifiée par le développeur. 
* Cette phrase "secrète" est utilisée lors de la génération du Token pour s'assurer qu'il n'y
  a pas "d'infiltrations" ou intervenants non autorisés.
* La méthode Invoke recoit en paramètres, le context et le service. C'est dans le header du context.request que sont
  stockées les autorisations et donc le Token. 
* La méthode attachUserToContext recoit en parametre le context, le service et le token.
  On créé un objet JwtSecurityTokenHandler qui va permettre la validation du token
  On appelle la méthode ValidateToken en passant en parametre le Token ainsi qu'un ensemble de paramètres utilisés pour la validation du token.
  ValidateIssuerSigningKey: booleen qui spécifie si on utilise ou pas la "secret string" lors de la génération du token.
  Cette "secret string" est définie dans le fichier appsettings.json
  IssuerSigningKey: permet de prendre en compte la "secret string" (qui se trouve dans AppSettings)
  ex: "AppSettings": {
      "Secret": "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING"
        }
  ClockSkew: permet de remettre à zéro le début de l'intervalle de temps pour le compteur de validité du Token 
  Cette méthode ValidateToken renvoit en paramètre de sortie le Token validé
  Le Token validé est casté en JwtSecurityToken et cette classe contient, entre autre, une liste Ienumerable de Claims.
  Un claim est une information sous forme de clé/valeur (dictionnaire) associée au token qui pourra être utilisée 
  plus tard dans la partie client (front-end) ex: userid
  Dans le cas d'une validation réussie du token, on ajoute au context des infos sur l'utilisateur authentifié.

- Dans le service "UserService", c'est dans la méthode Authenticate que l'on fait appel à la méthode generateJwtToken en passant 
  en paramètre l'utilisateur authentifié
  La méthode generateJwtToken recoit en parametre le User. Cette méthode génère un gestionnaire de Token (JwtSecurityTokenHandler)
  Ce tokenHandler va générer le token sur base des paramètres passé dans le tokenDescriptor
  à savoir: Subject, Durée de vie du token, l'algorithme utilisé pour le hashage de la secret-string.
  C'est la propriété Expires (de la classe SecurityTokenDescriptor) qui va définir la durée de vie du token.
  ex: Expires = DateTime.UtcNow.AddMinutes(5)
      Expires = DateTime.UtcNow.AddSeconds(45)
      Expires = DateTime.UtcNow.AddHours(1)

- Le token généré est récupéré via la classe AuthenticateResponse qui contient une propriété Token destinée à stocker le token généré.



