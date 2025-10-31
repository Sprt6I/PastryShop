# PastryShop


In CMD:

setx Gmail__Login ""

setx Gmail__Password ""


Configurate Server:
open cmd -> run "ipconfig" -> look for IPv4 address -> ip_address

got to C: and create "certs" folder

dotnet dev-certs https --clean
dotnet dev-certs https --trust

New-SelfSignedCertificate -DnsName "<ip_address>" -CertStoreLocation "cert:\LocalMachine\My" -> certificat_name

$pwd = ConvertTo-SecureString -String "yourpassword" -Force -AsPlainText
Export-PfxCertificate -Cert "cert:\LocalMachine\My\<certificat_name>" -FilePath "C:\certs\localcert.pfx" -Password $pwd

run Visual Studio as admin


In Visual Studio:
click solution in "Solution Explorer" with right click -> "Configurate Startip Project" -> Choose "Multiple startup project" -> set them from top to bottom 1. Pastry Server 2. Pastry Admin 3. Pastry Shop -> Set all their action to "Start"
