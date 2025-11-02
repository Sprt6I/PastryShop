# PastryShop
got to C: and create "certs" folder

# In CMD:
1. setx Gmail__Login "darek26655@gmail.com"
2. setx Gmail__Password "auwryxbqulskiwam"

# In Powershell ( as admin ):
1. ipconfig look for IPv4 address -> ip_address
2. dotnet dev-certs https --clean
3. dotnet dev-certs https --trust
4. New-SelfSignedCertificate -DnsName "<ip_address>" -CertStoreLocation "cert:\LocalMachine\My" -> certificat_name (thumbprint)
5. $pwd = ConvertTo-SecureString -String "yourpassword" -Force -AsPlainText
6. Export-PfxCertificate -Cert "cert:\LocalMachine\My\\<certificat_name>" -FilePath "C:\certs\localcert.pfx" -Password $pwd

# If you don't have python go: "https://www.python.org/downloads/release/python-3133/" and download "Windows installer (64-bit)"
 
# Go to PastryShop folder and run "configurate_server.py"

# run Visual Studio as admin


# In Visual Studio:
click solution in "Solution Explorer" with right click -> "Configurate Startip Project" -> Choose "Multiple startup project" -> set them from top to bottom 1. Pastry Server 2. Pastry Admin 3. Pastry Shop -> Set all their action to "Start"
